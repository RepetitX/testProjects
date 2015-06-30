$(function() {
    $('body').on('click', '.toggle-file-link', function () {
        var self = $(this),
            panel = $(this).closest('.updatepanel');

        $.ajax({
            type: 'POST',
            url: self.attr('href'),
            dataType: 'json',
            success: function (json) {
                if (json.Successful)
                    reloadPanel(panel);
                else
                    showGlobalErrors(json);
            }
        });

        return false;
    });
})