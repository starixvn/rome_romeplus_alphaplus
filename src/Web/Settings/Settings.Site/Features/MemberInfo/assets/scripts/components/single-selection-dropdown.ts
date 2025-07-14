"use strict";
interface JQuery {
    multiselect(option: any): void;
    multiselect(func: string, options: any): JQuery;
}

class SingleSelectionDropdownOption {
    constructor(
        public dropdownId: string = "dropdown-single",
        public inputId: string = "UserSelectedId",
        public onChange: Function = null,
        public enableClickableOptGroups: boolean = false,
        public enableCollapsibleOptGroups: boolean = false) {
    }
}

class SingleSelectionDropdownComponent {
    $: JQueryStatic;
    $hiddenInput: JQuery;
    $dropdown: JQuery;
    $dropdownButton: JQuery;

    selectedValue: string;

    options = new SingleSelectionDropdownOption();

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    render = (options: SingleSelectionDropdownOption = new SingleSelectionDropdownOption()) => {
        this.options = <SingleSelectionDropdownOption>this.$.extend({}, this.options, options);

        let $dropdown = this.$("#" + this.options.dropdownId);
        this.$dropdown = $dropdown;

        let $hiddenInput = this.$("#" + this.options.inputId);
        this.$hiddenInput = $hiddenInput;

        let self = this;
        this.selectedValue = self.getSelectedItem();

        this.$dropdown.multiselect({
            multiple: false,
            maxHeight: 300,
            numberDisplayed: 1,
            includeSelectAllOption: false,
            onChange: this.onchange,
            enableClickableOptGroups: this.options.enableClickableOptGroups,
            enableCollapsibleOptGroups: this.options.enableCollapsibleOptGroups,
            optionClass: this.optionClass
        });

        if (this.options.enableClickableOptGroups && this.options.enableCollapsibleOptGroups) {
            this.$dropdown.next().find(".multiselect-container li").removeClass("hidden");
        }
    };
    getSelectedItem = () => {
        let selected = [];
        let selectedOptions = this.$dropdown.find("option:selected");
        let this$ = this.$;

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

    optionClass = (element: HTMLElement) => {
        if (this.$(element).data("isnested") === 1) {
            return "multiselect-nested-item";
        }
    };

    changeSelection = (selection: any) => {
        this.$dropdown.val(selection);
        this.$hiddenInput.val(selection);
        this.$dropdown.multiselect("select", [selection]);
        this.$dropdown.multiselect("refresh");
    };

    disable = () => {
        if (this.$dropdown) {
            this.$dropdown.multiselect("disable");
        }
        if (this.$hiddenInput) {
            this.$hiddenInput.prop("disabled", true);
        }
    };

    enable = () => {
        if (this.$dropdown) {
            this.$dropdown.multiselect("enable");
        }
        if (this.$hiddenInput) {
            this.$hiddenInput.prop("disabled", false);
        }
    };
}















