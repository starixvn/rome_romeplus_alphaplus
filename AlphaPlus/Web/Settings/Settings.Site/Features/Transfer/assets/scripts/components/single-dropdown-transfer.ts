interface SingleDropdownTransferOption {
    dropdownId: string;
    enableFiltering?: boolean;
    onChange?($option: JQuery): void;
}

class SingleDropdownTransferComponent {
    $dropdown: JQuery;
    private selectedValue: string;

    render(options: SingleDropdownTransferOption) {
        this.$dropdown = $('#' + options.dropdownId);
        const defaults: any = {
            multiple: false,
            numberDisplayed: 1,
            includeSelectAllOption: false,
            maxHeight: 300,
            onChange: (option) => {
                const $option = $(option);
                this.selectedValue = $option.val();
                options.onChange && options.onChange($option);
            }
        };
        if (options.enableFiltering) {
            defaults.enableFiltering = true;
            defaults.enableCaseInsensitiveFiltering = true;
            defaults.filterPlaceholder = this.$dropdown.data('label-filterplaceholder');
        }

        this.$dropdown.multiselect(defaults);
    }

    getSelected(): string {
        return this.selectedValue;
    }
}
