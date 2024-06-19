interface JQuery {
    multiselect(option: any): void;
}

class DropdownListComponent {
    private $dropdownList: JQuery;
    private $multiSelectButton: JQuery;
    private $hiddenInput: JQuery;
    isReady = false;

    render(selectorId: string, options = {}, hiddenInputSelectorId: string = undefined) {
        this.$dropdownList = $('#' + selectorId);
        const multiple = this.$dropdownList.attr('multiple');

        let defaults: any = { maxHeight: 300 };
        if (multiple) {
            if (hiddenInputSelectorId) {
                this.$hiddenInput = $('#' + hiddenInputSelectorId);
            }

            defaults = $.extend(defaults, {
                numberDisplayed: 1,
                nonSelectedText: this.$dropdownList.data('label-select'),
                nSelectedText: ` ${this.$dropdownList.data('label-selected')}`,
                allSelectedText: this.$dropdownList.data('label-all'),
                selectAllText: this.$dropdownList.data('label-selectall'),
                includeSelectAllOption: true,
                onChange: () => this.onChange(),
                onSelectAll: () => this.onChange()
            });
        }

        this.$dropdownList.multiselect($.extend(defaults, options));
        this.$multiSelectButton = this.$dropdownList.parent().find('.multiselect.btn');
        this.isReady = true;

        return this.$dropdownList;
    }

    private onChange() {
        if (this.isReady) {
            this.validateSelectedItems();
            this.$hiddenInput && this.$hiddenInput.val(this.getSelected().join(','));
        }
    }

    validateSelectedItems(): boolean {
        const isValid = this.$dropdownList.find('option:selected').length > 0;
        isValid ? this.$multiSelectButton.removeClass('error') : this.$multiSelectButton.addClass('error');

        return isValid;
    }

    getSelected() {
        const selected = [];
        this.$dropdownList.find('option:selected').each(function (_, item: any) {
            selected.push(item.value);
        });

        return selected;
    }

    isSelectAll() {
        return this.$dropdownList.next().find('.multiselect-container .multiselect-all :checkbox').is(':checked');
    }

    enable() {
        this.$dropdownList.multiselect('enable');
    }

    disable() {
        this.$dropdownList.multiselect('disable');
    }
}
