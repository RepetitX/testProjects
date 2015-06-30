// Настройка ajax и культуры

$.ajaxSetup({
    cache: false
});

Globalize.culture('ru-RU');

$.validator.methods.date = function (value, element) {
    return this.optional(element) ||
        !isNaN(Globalize.parseDate(value));
};

$.validator.methods.number = function (value, element) {
    return this.optional(element) ||
        !isNaN(Globalize.parseFloat(value));
};

$(function () {
    // Ajax лоадер

    $(document).ajaxStart(function () {
        $('body').addClass('loading');
    });

    $(document).ajaxStop(function () {
        $('body').removeClass('loading');
    });

    $(document).ajaxError(function (e, xhr) {
        var result;

        $('body').removeClass('loading');

        try {
            result = $.parseJSON(xhr.responseText);

            if (result && result.Errors)
                showGlobalErrors(result);
        }
        catch (e) {
            showNoty('500 Внутренняя ошибка сервера.', 'error');
        }
    });

    $(document).ajaxComplete(function () {
        $('body').removeClass('loading');
    });

    $(window).scroll(function () {
        var sidebar = $('.sidebar');

        if (sidebar.length > 0) {
            if ($(window).scrollTop() !== 0)
                sidebar.css('top', '0');
            else
                sidebar.css('top', '');
        }
    });

    // Модальные окна

    $('body').on('click', '.modal-form', function (event) {
        var self = $(this);

        event.preventDefault();

        $.ajax({
            url: $(this).attr('href'),
            success: function (response) {
                var modalDialog = $('<div class="modal fade" tabindex="-1" role="dialog" aria-labelledby="ModalLabel" aria-hidden="true">'),
                    modalContent = $(response);

                modalContent.find('form').attr('autocomplete', 'off');

                $.validator.unobtrusive.parse(modalContent);

                modalDialog.attr('data-init-link', '#' + self.attr('id'));
                modalDialog.append(modalContent);

                modalDialog.modal({ backdrop: 'static' });
            }
        });

        return false;
    });

    $('body').on('submit', '.modal form', function (event) {
        var self = $(this),
            modal = self.closest('.modal'),
            initLink = $(modal.attr('data-init-link')),
            updateTarget = initLink.attr('data-target-update') ? $(initLink.attr('data-target-update')) : initLink.closest('.updatepanel');

        event.preventDefault();

        self.ajaxSubmit({
            target: modal,
            beforeSubmit: function (data, form) {
                if (!form.valid())
                    return false;

                return true;
            },
            success: function (response) {
                if (!$(response).find('#ModelStateErrors').length|| $(response).find('#ModelStateErrors').val() == 0) {
                    var postMessage = $(response).find('#PostMessage');

                    if (postMessage.length > 0)
                        showNoty(postMessage.val(), postMessage.attr('data-noty-type'));

                    if (updateTarget.length > 0)
                        reloadPanel(updateTarget);

                    modal.modal('hide');
                }
            }
        });

        return false;
    });

    $('body').on('hidden.bs.modal', function (e) {
        $(e.target).remove();
    });

    // Masked input

    $('body').on('focus', '.phone-input', function () {
        $(this).mask('+7 (999) 999-99-99');
    });

    // Дата пикеры

    $('body').on('focus', '.date-ru-input', function () {
        $(this).datetimepicker({
            language: 'ru',
            pickTime: false
        });
    });

    $('body').on('focus', '.time-ru-input', function () {
        $(this).datetimepicker({
            language: 'ru',
            pickDate: false
        });
    });

    $('body').on('focus', '.datetime-ru-input', function () {
        $(this).datetimepicker({
            language: 'ru'
        });
    });

    // Загрузка updatepanel по ссылке

    $('body').on('click', '.subcontent-loader', function (event) {
        var self = $(this);
        var target = self.attr('data-target-update') ? $(self.attr('data-target-update')) : self.closest('.updatepanel');

        event.preventDefault();

        if (!target.length)
            return false;

        target.load(self.attr('href'), function () {
            target.trigger('reload');
        });

        return false;
    });

    // Ajax формы

    $('body').on('focus mouseenter', 'form.ajax-form', function () {
        var self = $(this),
            target = self.attr('data-target-update') ? $(self.attr('data-target-update')) : self.closest('.updatepanel');

        self.attr('autocomplete', 'off');

        if (self.data('data-initialized'))
            return;

        self.ajaxForm({
            beforeSubmit: function (data, form) {
                if (!form.valid())
                    return false;

                return true;
            },
            success: function (response) {
                self.trigger('ajaxFormSuccess');

                try {
                    var result = $.parseJSON(response);

                    if (result && result.Errors)
                        showGlobalErrors(result);
                } catch (e) {
                    target.html(response);
                    target.trigger('ajaxFormTargetUpdated');
                }
            }
        });

        self.data('data-initialized', true);
    });

    // Формы в табах

    $('body').on('submit', '.tab-pane:not(#main) form', function (event) {
        var self = $(this),
            tab = self.closest('.tab-pane');

        event.preventDefault();

        self.ajaxSubmit({
            target: tab,
            beforeSubmit: function (data, form) {
                if (!form.valid())
                    return false;

                return true;
            },
            success: function (response) {
                if ($(response).find('#ModelStateErrors').val() == 0) {
                    var postMessage = $(response).find('#PostMessage');

                    if (postMessage.length > 0)
                        showNoty(postMessage.val(), postMessage.attr('data-noty-type'));
                }
            }
        });

        return false;
    });
});

function showNoty(text, type) {
    noty({ text: text, type: type, timeout: 5000 });
}

function showGlobalErrors(jsonCommandResult) {
    if (jsonCommandResult.Errors['Global'])
        showNoty(jsonCommandResult.Errors['Global'], 'error');
};

function changeAddress(element, attribute, param, value) {
    var address = element.attr(attribute),
        first = address.indexOf('?' + param + '=') > -1,
        nofirst = address.indexOf('&' + param + '=') > -1;

    if (nofirst || first) {
        var params = address.split('?')[1].split('&'),
            oldValue;

        for (var p in params) {
            if (params[p].substring(0, param.length + 1) == param + '=') {
                oldValue = params[p];
                break;
            }
        }

        if (value == '') {
            if (first) {
                if (address.indexOf('&') > -1)
                    element.attr(attribute, address.replace(oldValue, ''));
                else
                    element.attr(attribute, address.replace('?' + oldValue, ''));
            }

            if (nofirst)
                element.attr(attribute, address.replace('&' + oldValue, ''));
        } else {
            element.attr(attribute, address.replace(oldValue, param + '=' + value));
        }
    }
    else if (value != '') {
        if (address.indexOf('?') > -1)
            element.attr(attribute, address + '&' + param + '=' + value);
        else
            element.attr(attribute, address + '?' + param + '=' + value);
    }
}

function reloadPanel(panel, callback) {
    panel = $(panel);

    panel.load(panel.attr('data-action'), function () {
        panel.data('loaded', true);

        if (callback)
            callback();

        panel.trigger('reload');
    });
}