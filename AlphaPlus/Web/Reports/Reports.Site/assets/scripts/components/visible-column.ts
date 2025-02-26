interface VisibleColumnOption {
    listColumn: any;
    onDropdownHidden?: Function;
    visibleColumns?: string;
    selectorId?: string;
    minWidth?: number;
}

class VisibleColumnComponent {
    private $visibleColumn: JQuery;

    render(options: VisibleColumnOption) {
        let isCheckAll = false;
        const $dropdown = $('#' + (options.selectorId || 'dropdown-visible-column'));

        const visibleColumnObject = {};
        const visibleColumns = options.visibleColumns || '';
        if (visibleColumns.length) {
            const visibleColumnArray = visibleColumns.split(',');
            visibleColumnArray.forEach(function (item) {
                visibleColumnObject[item] = true;
            });
            isCheckAll = options.listColumn.length === visibleColumnArray.length;
        } else {
            options.listColumn.forEach(function (item) {
                if (item.checked !== false) {
                    visibleColumnObject[item.value] = true;
                }
            });
        }

        this.$visibleColumn = $dropdown.fanexMultiSelect({
            selectAllText: $dropdown.data('label-selectall'),
            selectedText: $dropdown.data('label-selected'),
            noneSelectedText: $dropdown.data('label-noneselected'),
            customSelectAllText: $dropdown.data('label-all') + ' (#)',
            showFilter: true,
            filterPlaceHolder: $dropdown.data('label-filterplaceholder'),
            noItemFoundText: $dropdown.data('label-noitemfound'),
            isCheckAll: isCheckAll,
            minWidth: options.minWidth || 220,
            maxHeight: 250,
            showTooltip: false,
            listData: options.listColumn.map(function (item) {
                return {
                    value: item.value,
                    text: item.text,
                    checked: isCheckAll || visibleColumnObject.hasOwnProperty(item.value)
                };
            }),
            onLeave: () => options.onDropdownHidden && options.onDropdownHidden(this.getColumns())
        });
    }

    getColumns() {
        return this.$visibleColumn.fanexMultiSelect('getAllValues');
    }

    getShowColumns() {
        return this.$visibleColumn.fanexMultiSelect('getCheckedValues');
    }
}
