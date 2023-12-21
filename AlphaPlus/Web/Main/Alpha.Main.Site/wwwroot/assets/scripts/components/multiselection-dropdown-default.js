/// <reference path="../core.ts" />
"use strict";
var MultiSelectionDropdownDefaultOption = /** @class */ (function () {
    function MultiSelectionDropdownDefaultOption(dropdownId, inputId) {
        if (dropdownId === void 0) { dropdownId = "multiSelectionDropdownId"; }
        if (inputId === void 0) { inputId = "selectedId"; }
        this.dropdownId = dropdownId;
        this.inputId = inputId;
    }
    return MultiSelectionDropdownDefaultOption;
}());
var MultiSelectionDropdownDefaultComponent = /** @class */ (function () {
    function MultiSelectionDropdownDefaultComponent($) {
        var _this = this;
        this.options = new MultiSelectionDropdownDefaultOption();
        this.render = function (options) {
            if (options === void 0) { options = new MultiSelectionDropdownDefaultOption(); }
            _this.options = _this.$.extend({}, _this.options, options);
            _this.$multiSelectionDropdown = _this.$("#" + _this.options.dropdownId);
            _this.$hiddenInput = _this.$("#", _this.options.inputId);
            var selectAllText = _this.$multiSelectionDropdown.data("select-all-text");
            _this.$multiSelectionDropdown.multiselect({
                maxHeight: 300,
                numberDisplayed: 2,
                selectAllText: selectAllText,
                includeSelectAllOption: true,
            });
            return _this.$multiSelectionDropdown;
        };
        this.getSelectedItems = function () {
            var selected = [];
            var this$ = _this.$;
            _this.$multiSelectionDropdown.find("option:selected").each(function () {
                selected.push(this$(this).val());
            });
            return selected;
        };
        this.$ = $;
    }
    return MultiSelectionDropdownDefaultComponent;
}());
//# sourceMappingURL=multiselection-dropdown-default.js.map