interface SingleDropdownTransferOption {
    dropdownId: string;
    enableFiltering?: boolean;
    buttonWidth?: number;
    onChange?($option: JQuery): void;
}

class SingleDropdownTransferComponent {
    $dropdown: JQuery;
    private selectedValue: string;

    render(options: SingleDropdownTransferOption) {
        this.$dropdown = $('#' + options.dropdownId);

        if (this.$dropdown.length) {
            this.selectedValue = this.$dropdown.find('option:selected').val();
            const defaults: any = {
                multiple: false,
                numberDisplayed: 1,
                includeSelectAllOption: false,
                maxHeight: 300,
                buttonWidth: options.buttonWidth,
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
    }

    getSelected(): string {
        return this.selectedValue;
    }
}
