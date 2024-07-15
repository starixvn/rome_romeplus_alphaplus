class TransferListView {
    usernameSuggestionComponent: UsernameSuggestionComponent;
    statusDropdownComponent: DropdownTransferComponent;
    autoTransferDropdownComponent: DropdownTransferComponent;
    visibleColumnContextMenu: VisibleColumnContextMenuComponent;
    transferListPaging: TransferListPaging;

    constructor(
        usernameSuggestionComponent: UsernameSuggestionComponent,
        statusDropdownComponent: DropdownTransferComponent,
        autoTransferDropdownComponent: DropdownTransferComponent,
        visibleColumnContextMenu: VisibleColumnContextMenuComponent,
        transferListPaging: TransferListPaging
    ) {
        this.usernameSuggestionComponent = usernameSuggestionComponent;
        this.statusDropdownComponent = statusDropdownComponent;
        this.autoTransferDropdownComponent = autoTransferDropdownComponent;
        this.visibleColumnContextMenu = visibleColumnContextMenu;
        this.transferListPaging = transferListPaging;
    }

    init() {
        const $tableTransfer = $('#tbl-transfer');

        $('#AllYesterdayBalance').change(function () {
            $('#form-transfer').submit();
        });

        $('#icon-information').click(function () {
            window.top.popupManager.open($('#hidden-transferHintUrl').attr('href'), undefined, 560, 400);
        });

        this.usernameSuggestionComponent.init(
            new UsernameSuggestionOption($('#Username'), $('#hidden-usernameSuggestionUrl').val(), true)
        );
        this.statusDropdownComponent.render('Status');
        this.autoTransferDropdownComponent.render('AutoTransfer');
        this.visibleColumnContextMenu.init(`transfer_${$('#ViewerId').val()}`, $tableTransfer);
        this.transferListPaging.init(() => $('#form-transfer').submit());

        const $checkboxAll = $('#checkbox-all').change(function () {
            const checked = this.checked;
            $checkboxList.prop('checked', checked);
            $('#btn-transfer').prop('disabled', !checked);
            $('#btn-autoTransfer').prop('disabled', !checked);
        });
        const $checkboxList = $('.checkbox').change(function () {
            const checkedCount = $checkboxList.filter(':checked').length;
            if (checkedCount === 0) {
                $checkboxAll.prop({
                    indeterminate: false,
                    checked: false
                });
                $('#btn-transfer').prop('disabled', true);
                $('#btn-autoTransfer').prop('disabled', true);
            } else {
                if (checkedCount === $checkboxList.length) {
                    $checkboxAll.prop({
                        indeterminate: false,
                        checked: true
                    });
                } else {
                    $checkboxAll.prop({
                        indeterminate: true,
                        checked: false
                    });
                }
                $('#btn-transfer').prop('disabled', false);
                $('#btn-autoTransfer').prop('disabled', false);
            }
        });

        $tableTransfer.removeClass('hide').stickyTableHeaders();
    }
}

$(function () {
    new TransferListView(
        new UsernameSuggestionComponent(),
        new DropdownTransferComponent(),
        new DropdownTransferComponent(),
        new VisibleColumnContextMenuComponent(),
        new TransferListPaging()
    ).init();
});
