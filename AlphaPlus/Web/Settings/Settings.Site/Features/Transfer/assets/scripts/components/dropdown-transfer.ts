class DropdownTransferComponent {
    $dropdownList: JQuery;
    private onChangeCallback: Function;

    render(selectorId: string, options = {}, onChangeCallback: Function = undefined) {
        this.$dropdownList = $('#' + selectorId);
        this.onChangeCallback = onChangeCallback;
        let defaults: any = {
            maxHeight: 300,
            filterPlaceholder: this.$dropdownList.data('label-filterplaceholder'),
            onChange: (option) => this.onChangeCallback && this.onChangeCallback(option)
        };

        this.$dropdownList.multiselect($.extend(defaults, options));

        return this.$dropdownList;
    }

    getSelected(): string {
        const selected = [];
        this.$dropdownList.find('option:selected').each(function (_, item: any) {
            selected.push(item.value);
        });

        return selected.length ? selected[0] : undefined;
    }
}
