/// <reference path="../core.ts" />

interface JQuery {
    multiselect(option: any): void;
}

interface SingleDropdownOption {
    dropdownId: string;
    onChange?($option: JQuery): void;
}

class SingleDropdownComponent {
    render(options: SingleDropdownOption) {
        const $dropdown = $('#' + options.dropdownId);

        $dropdown.multiselect({
            multiple: false,
            numberDisplayed: 1,
            includeSelectAllOption: false,
            maxHeight: 300,
            onChange: function (option) {
                options.onChange && options.onChange($(option));
            }
        });
    }
}
