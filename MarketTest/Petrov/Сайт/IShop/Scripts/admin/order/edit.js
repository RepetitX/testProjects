$(function () {
    $('div.form-jquery-tabs').tabs({
        cache: true,
        beforeLoad: function (event, ui) {
            ui.panel.addClass('preload');
        },
        load: function (event, ui) {
            $(ui.panel).removeClass('preload');
            $('div#' + ui.panel.id).trigger('loadtab');
            //tabajaxForm(ui.panel[0].id);
        },
        beforeActivate: function (event, ui) {

        },
        activate: function (event, ui) {
            $(ui.newPanel).removeClass('preload');
        },
        create: function (event, ui) {

        }
    });

    $('body').on('change', '#ProductTypeId', function (event) {
        var element = $(this);
        $.ajax({
            type: 'POST',
            url: $('#UrlOrderTypeOptions').val(),
            data: { orderTypeId: element.find("option:selected")[0].value }
        }).done(function (data) {
            $('#ProductOptionList').html(data);
        }).fail(function () {
            alert("Ошибка");
        });
        return false;
    });


})

function tabajaxForm(uipanelid) {
    var form = $('div#' + uipanelid).find('form');
    form.attr('autocomplete', 'off');
    if (form.length > 0) {
        form.ajaxForm({
            target: 'div#' + uipanelid,
            type: 'POST',
            beforeSubmit: function (data, form) { if (!form.valid()) { return false; } $('div.form-jquery-tabs').addClass('preload'); },
            error: function (xhr, s) { BlockUIPage('Произошла ошибка!'); },
            success: function (r, s, x, form) {
                if (r.Successful == true || $(r).find('input#modelStateCount').val() == 0) {
                    $('div.form-jquery-tabs').removeClass('preload');
                }

                //tabajaxForm(uipanelid);
            }
        });
    }
}

