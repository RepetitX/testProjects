$(function () {
    var indicator = $('<div/>').attr('id', 'loading-indicator');

    $('body').append(indicator);

    $(document).ajaxStart(function () { showLoadingIndicator(); });
    $(document).ajaxComplete(function () { hideLoadingIndicator(); });
    $(document).ajaxError(function () { hideLoadingIndicator(); });

    $('body').on('click', 'a, [type=submit], .totalEstimation input', function () {
        var self = $(this);

        if (self.is('a') && (self.attr('href') === undefined || self.attr('href') === null || self.attr('href') === '' || self.attr('href') === '#'))
            return;

        showLoadingIndicator();
    });

    function hideLoadingIndicator() {
        $('#loading-indicator').hide();
    }

    function showLoadingIndicator() {
        $('#loading-indicator').show();
    }
})