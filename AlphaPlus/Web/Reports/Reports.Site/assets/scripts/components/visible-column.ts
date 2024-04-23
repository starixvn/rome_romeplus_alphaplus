class VisibleColumnComponent {
    private $visibleColumn: JQuery;

    render(visibleColumns: string, listColumn, onDropdownHidden: Function) {
        let isCheckAll = false;
        const $dropdown = $('#dropdown-visible-column');

        const visibleColumnObject = {};
        if (visibleColumns.length) {
            const visibleColumnArray = visibleColumns.split(',');
            visibleColumnArray.forEach(function (item) {
                visibleColumnObject[item] = true;
            });
            isCheckAll = listColumn.length === visibleColumnArray.length;
        } else {
            listColumn.forEach(function (item) {
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
            minWidth: 220,
            maxHeight: 250,
            showTooltip: false,
            listData: listColumn.map(function (item) {
                return {
                    value: item.value,
                    text: item.text,
                    checked: isCheckAll || visibleColumnObject.hasOwnProperty(item.value)
                };
            }),
            onLeave: () => onDropdownHidden && onDropdownHidden(this.getColumns())
        });
    }

    getColumns() {
        return this.$visibleColumn.fanexMultiSelect('getAllValues');
    }

    getShowColumns() {
        return this.$visibleColumn.fanexMultiSelect('getCheckedValues');
    }
}
