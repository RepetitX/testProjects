$(function() {
    $('body').on('hidden.bs.modal', function(e) {
        if (e.target !== undefined && e.target !== null && $(e.target).find('.modal-update-grid').length > 0) {
            var initLink = $($(e.target).attr('data-init-link')),
                updateTarget = initLink.attr('data-target-update') ? $(initLink.attr('data-target-update')) : initLink.closest('.updatepanel');

            if (updateTarget.length > 0)
                reloadPanel(updateTarget);
        }
    });
});

