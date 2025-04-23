interface ListDataItem {
    text: string;
    value: any;
    checked?: boolean;
}

interface MultipleDropdownOption {
    selectorId: string;
    listData: ListDataItem[];
    showFilter?: boolean;
    onLeave?: Function;
    minWidth?: number;
    maxHeight?: number;
}

class MultipleDropdownComponent {
    private $dropdownComponent: JQuery;

    render(options: MultipleDropdownOption) {
        const $dropdown = $('#' + options.selectorId);
        this.$dropdownComponent = $dropdown.fanexMultiSelect({
            selectAllText: $dropdown.data('label-selectall'),
            selectedText: '# ' + $dropdown.data('label-selected'),
            noneSelectedText: $dropdown.data('label-noneselected'),
            customSelectAllText: $dropdown.data('label-all') + ' (#)',
            filterPlaceHolder: $dropdown.data('label-filterplaceholder'),
            noItemFoundText: $dropdown.data('label-noitemfound'),
            noItemText: $dropdown.data('label-nodatatext'),
            showTooltip: false,
            listData: options.listData,
            showFilter: options.showFilter,
            onLeave: options.onLeave,
            minWidth: options.minWidth || 220,
            maxHeight: options.maxHeight || 250
        });
    }

    getCheckedValues(): string {
        if (this.$dropdownComponent && this.$dropdownComponent.length) {
            return this.$dropdownComponent.fanexMultiSelect('getCheckedValues').join(',');
        }
    }
}
