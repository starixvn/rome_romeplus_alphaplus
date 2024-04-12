"use strict";
interface JQuery {
    multiselect(option: object): void;
}

class SingleSelectionDropdownOptionTransfer {
    constructor(
        public dropdownId = "dropdown-single",
        public inputId = "UserSelectedId",
        public onChange: Function = null) {
    }
}

class SingleSelectionDropdownComponentTransfer {
    $: JQueryStatic;
    $hiddenInput: JQuery;
    $dropdown: JQuery;
    $dropdownButton: JQuery;

    selectedValue: string;

    options = new SingleSelectionDropdownOptionTransfer();

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    render = (options: SingleSelectionDropdownOptionTransfer = new SingleSelectionDropdownOptionTransfer()) => {
        this.options = this.$.extend({}, this.options, options) as SingleSelectionDropdownOptionTransfer;

        const $dropdown = this.$("#" + this.options.dropdownId);
        this.$dropdown = $dropdown;

        const $hiddenInput = this.$("#" + this.options.inputId);
        this.$hiddenInput = $hiddenInput;

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
        const selected = [];
        const selectedOptions = this.$dropdown.find("option:selected");
        const this$ = this.$;

        selectedOptions.each(function () {
            selected.push([this$(this).val()]);
        });

        return selected.length ? selected[0].toString() : "";
    };
    updateSelectedValue = () => {
        this.$hiddenInput.val(this.selectedValue);
    };

    onchange = () => {
        this.selectedValue = this.getSelectedItem();
        if (this.options.onChange) {
            this.options.onChange();
        }
    };
}
