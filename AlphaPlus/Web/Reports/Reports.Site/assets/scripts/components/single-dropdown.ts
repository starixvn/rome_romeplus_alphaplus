/// <reference path="../core.ts" />

interface SingleDropdownOption {
    dropdownId: string;
    hiddenInputId?: string;
    onChange?($option: JQuery): void;
}

class SingleDropdownComponent {
    $dropdown: JQuery;
    private selectedValue: string;

    render(options: SingleDropdownOption) {
        this.$dropdown = $('#' + options.dropdownId);

        if (this.$dropdown.length) {
            this.selectedValue = this.$dropdown.find('option:selected').val();
            const $hiddenInput = options.hiddenInputId ? $('#' + options.hiddenInputId) : undefined;

            this.$dropdown.multiselect({
                multiple: false,
                numberDisplayed: 1,
                includeSelectAllOption: false,
                maxHeight: 300,
                onChange: (option) => {
                    const $option = $(option);
                    this.selectedValue = $option.val();
                    $hiddenInput && $hiddenInput.length && $hiddenInput.val(this.selectedValue);
                    options.onChange && options.onChange($option);
                }
            });
        }
    }

    getSelectedValue() {
        return this.selectedValue;
    }

    setSelectedValue(value, triggerOnChange = false) {
        this.$dropdown.multiselect('select', value, triggerOnChange);
        this.$dropdown.multiselect('refresh');
        if (!triggerOnChange) {
            this.selectedValue = value;
        }
    }
}
