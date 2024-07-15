class DropdownTransferComponent {
    $dropdownList: JQuery;
    private $multiSelectButton: JQuery;
    private onChangeCallback: Function;
    private isReady = false;

    render(selectorId: string, options = {}, onChangeCallback: Function = undefined) {
        this.$dropdownList = $('#' + selectorId);
        this.onChangeCallback = onChangeCallback;
        let defaults: any = {
            maxHeight: 300,
            onChange: (option) => this.onChangeCallback && this.onChangeCallback(option)
        };
        if (this.$dropdownList.attr('multiple')) {
            defaults = $.extend(defaults, {
                numberDisplayed: 1,
                nonSelectedText: this.$dropdownList.data('label-select'),
                nSelectedText: ` ${this.$dropdownList.data('label-selected')}`,
                allSelectedText: this.$dropdownList.data('label-all'),
                selectAllText: this.$dropdownList.data('label-selectall'),
                includeSelectAllOption: true,
                onChange: (option, checked) => this.onChangeMultiple(option, checked),
                onSelectAll: (option, checked) => this.onChangeMultiple(option, checked)
            });
        }

        this.$dropdownList.multiselect($.extend(defaults, options));
        this.$multiSelectButton = this.$dropdownList.parent().find('.multiselect.btn');
        this.isReady = true;

        return this.$dropdownList;
    }

    private onChangeMultiple(option: any, checked: boolean) {
        if (this.isReady) {
            this.validateSelectedItems();
            this.onChangeCallback && this.onChangeCallback(option, checked);
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
