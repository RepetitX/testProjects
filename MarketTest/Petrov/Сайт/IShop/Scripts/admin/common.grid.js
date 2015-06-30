$(function () {
    var gridResources = {
        ConfirmDeletion: 'Вы уверены, что хотите удалить эту запись?',
        ConfirmBatchDeletion: 'Вы уверены, что хотите удалить выбранные записи?',
        ConfirmFilterDeletion: 'Вы уверены, что хотите удалить этот фильтр?',
        NoRowsSelected: 'Не выбрано ни одной строки.'
    };

    // Drag'n'Drop в таблице

    createSortableTable();

    $('body').on('reload', '.mvcgridpanel', function () {
        createSortableTable();
    });

    $('body').on('click', 'a.batch-export', function () {
        var url = $(this).attr('href'),
            queryPosition = url.indexOf('?'),
            grid = $(this).attr('data-grid'),
            rowCheckboxes = $(grid).find('.row-checkbox:checked');

        if (rowCheckboxes.length === 0) {
            showNoty(gridResources.NoRowsSelected, 'alert');
            return false;
        }

        var selectedValues = rowCheckboxes.map(function () { return $(this).val(); });

        if (url.indexOf('?') != -1)
            url = url.substring(0, queryPosition);

        url += '?';

        $.each(selectedValues, function (index, value) {
            if (index !== 0)
                url += '&';

            url += 'ids[' + index + ']=' + value;
        });

        $(this).attr('href', url);

        $(this).trigger('batchexportstart');
    });
    
    $('body').on('click', '.mvcgrid .mvcgrid-delete-link', function () {
        var self = $(this),
            panel = $(this).closest('.updatepanel');
        
        noty({
            layout: 'center',
            text: gridResources.ConfirmDeletion,
            type: 'warning',
            modal: true,
            buttons: [
                {
                    addClass: 'btn btn-primary', text: 'Да', onClick: function ($noty) {
                        $noty.close();

                        $.ajax({
                            type: 'POST',
                            url: self.attr('href'),
                            data: { __RequestVerificationToken: self.siblings('input[name=__RequestVerificationToken]').attr('value') },
                            dataType: 'json',
                            success: function (json) {
                                if (json.Successful)
                                    reloadPanel(panel);
                                else
                                    showGlobalErrors(json);
                            }
                        });
                    }
                }, {
                    addClass: 'btn btn-danger', text: 'Нет', onClick: function ($noty) {
                        $noty.close();
                    }
                }
            ]
        });

        return false;
    });

    $('body').on('click', 'a.batch-delete', function () {
        var self = $(this),
            targetUpdate = $(this).attr('data-target-update'),
            rowCheckboxes = $(targetUpdate).find('.row-checkbox:checked'),
            antiForgeryToken = $(this).siblings('input[name=__RequestVerificationToken]').val();

        if (rowCheckboxes.length === 0) {
            showNoty(gridResources.NoRowsSelected, 'alert');
            return false;
        }

        noty({
            layout: 'center',
            text: gridResources.ConfirmBatchDeletion,
            type: 'warning',
            modal: true,
            buttons: [
                {
                    addClass: 'btn btn-primary', text: 'Да', onClick: function ($noty) {
                        $noty.close();
                      
                        var selectedValues = rowCheckboxes.map(function () { return $(this).val(); });

                        $.ajax({
                            type: 'DELETE',
                            url: self.attr('href'),
                            data: { ids: $.makeArray(selectedValues), __RequestVerificationToken: antiForgeryToken },
                            traditional: true,
                            dataType: 'json',
                            success: function (json) {
                                if (json.Successful)
                                    reloadPanel(targetUpdate);
                                else
                                    showGlobalErrors(json);
                            }
                        });
                    }
                }, {
                    addClass: 'btn btn-danger', text: 'Нет', onClick: function ($noty) {
                        $noty.close();
                    }
                }
            ]
        });

        return false;
    });
    
    $('body').on('click', '.mvcgrid .select-all-checkbox', function () {
        var rowCheckboxes = $(this).closest('.mvcgrid').find('.row-checkbox');

        rowCheckboxes.prop('checked', $(this).prop('checked'));
    });

    $('body').on('mouseenter focus', 'table.mvcgrid th > a', function () {
        $(this).addClass('subcontent-loader');
    });

    $('body').on('mouseenter focus', '.mvcgrid-pagination a', function () {
        $(this).addClass('subcontent-loader');
    });

    // Строка поиска в таблице

    $('body').on('keypress', '.mvcgrid-search [name=SearchString]', function (event) {
        var code = event.keyCode || event.which;

        if (code === 13) {
            var panel = $(this).closest('.updatepanel');

            panel.load(panel.attr('data-action'), { SearchString: $(this).val() }, function () {
                panel.trigger('reload');
            });

            return false;
        }
    });

    $('body').on('click', '.grid-search-btn', function () {
        var input = $('.mvcgrid-search [name=SearchString]'),
            panel = input.closest('.updatepanel');

        panel.load(panel.attr('data-action'), { SearchString: input.val() }, function () {
            panel.trigger('reload');
        });
    });

    $('body').on('click', '.grid-clear-search-btn', function () {
        var input = $('.mvcgrid-search [name=SearchString]'),
            panel = input.closest('.updatepanel');

        panel.load(panel.attr('data-action'), { SearchString: input.val('').val() }, function () {
            panel.trigger('reload');
        });
    });

    // Фильтры

    createMultiselectFilters();

    $('body').on('ajaxFormSuccess', '.filter-panel form', function () {
        var filterUpdatePanel = $(this).closest('.updatepanel');

        reloadPanel(filterUpdatePanel, createMultiselectFilters);
    });

    $('body').on('click', 'div.filter .toggle-save-filter-form', function (event) {
        event.preventDefault();

        $('div.save-filter', $(this).closest('.filter')).slideToggle(300);

        return false;
    });
    
    $('body').on('change', 'div.filter #FilterEdit_Id', function () {
        var filterName = $(this).closest('form').find('#FilterEdit_Name');

        if ($(this).val() == '0') {
            filterName.val('');
        } else {
            filterName.val($(this).find('option:selected').text());
        }
    });
    
    $('body').on('click', '.delete-saved-filter', function () {
        var self = $(this),
            targetUpdate = $(this).closest('.updatepanel.filterpanel');
        
        noty({
            layout: 'center',
            text: gridResources.ConfirmFilterDeletion,
            type: 'warning',
            modal: true,
            buttons: [
              {
                  addClass: 'btn btn-primary', text: 'Да', onClick: function ($noty) {
                      $noty.close();

                      $.ajax({
                          type: 'DELETE',
                          url: self.attr('href'),
                          dataType: 'json',
                          success: function (json) {
                              if (json.Successful)
                                  reloadPanel(targetUpdate);
                              else
                                  showGlobalErrors(json);
                          }
                      });
                  }
              }, {
                  addClass: 'btn btn-danger', text: 'Нет', onClick: function ($noty) {
                      $noty.close();
                  }
              }
            ]
        });

        return false;
    });
});

function createMultiselectFilters() {
    $('.select-filter-condition').each(function () {
        var showFilter = $(this).find('option').length >= 20;

        $(this).multiselect({
            numberDisplayed: 0,
            nSelectedText: 'выбрано',
            buttonWidth: '100%',
            nonSelectedText: 'Не выбрано',
            enableCaseInsensitiveFiltering: showFilter,
            filterBehavior: 'text',
            filterPlaceholder: 'Найти',
            buttonClass: 'btn btn-default btn-sm'
        });
    });
}

function createSortableTable() {
    $('table[data-table-sortable=true]').tableDnD({
        onDrop: function (table, row) {
            var rows = $(table).find('tbody tr'),
                sortedIds = rows.map(function () { return $(this).attr('data-item-id'); });

            $.ajax({
                type: 'GET',
                url: $(table).attr('data-url-sortable'),
                data: { ids: $.makeArray(sortedIds) },
                traditional: true,
                dataType: 'json'
            });
        },
        dragHandle: 'drag-handle'
    });
};
