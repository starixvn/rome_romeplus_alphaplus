/// <reference path="../core.ts" />

interface JQuery {
    multiselect(option: any): void;
}

class SingleSelectionDropdownOption {
    constructor(
        public dropdownId: string = 'dropdown-single',
        public inputId: string = 'UserSelectedId',
        public onChange: Function = null
    ) {}
}

class SingleSelectionDropdownComponent {
    options = new SingleSelectionDropdownOption();
    $dropdown: JQuery;
    $hiddenInput: JQuery;
    selectedValue: string;

    render = (options: SingleSelectionDropdownOption = new SingleSelectionDropdownOption()) => {
        this.options = <SingleSelectionDropdownOption>$.extend({}, this.options, options);

        this.$dropdown = $('#' + this.options.dropdownId);
        this.$hiddenInput = $('#' + this.options.inputId);
        this.selectedValue = this.getSelectedItem();

        this.$dropdown.multiselect({
            multiple: false,
            maxHeight: 300,
            numberDisplayed: 1,
            includeSelectAllOption: false,
            onChange: this.onchange
        });
    };

    getSelectedItem = () => {
        const selectedOption = this.$dropdown.find('option:selected');
        return selectedOption.length ? selectedOption.val() : '';
    };

    updateSelectedValue = () => {
        this.$hiddenInput.val(this.selectedValue);
    };

    onchange = () => {
        this.selectedValue = this.getSelectedItem();
        this.options.onChange && this.options.onChange();
    };
}
