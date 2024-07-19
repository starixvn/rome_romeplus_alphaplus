class TransferListView {
    alert: AlertComponent;
    usernameSuggestionComponent: UsernameSuggestionComponent;
    statusDropdownComponent: DropdownTransferComponent;
    autoTransferDropdownComponent: DropdownTransferComponent;
    visibleColumnContextMenu: VisibleColumnContextMenuComponent;
    transferListPaging: TransferListPaging;

    constructor(
        alert: AlertComponent,
        usernameSuggestionComponent: UsernameSuggestionComponent,
        statusDropdownComponent: DropdownTransferComponent,
        autoTransferDropdownComponent: DropdownTransferComponent,
        visibleColumnContextMenu: VisibleColumnContextMenuComponent,
        transferListPaging: TransferListPaging
    ) {
        this.alert = alert;
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

        this.bindEvents($tableTransfer);

        $tableTransfer.removeClass('hide').stickyTableHeaders();
    }

    private bindEvents($tableTransfer: JQuery) {
        const openPopup = function (url: string, width: number, height = 350) {
            if (window.top.popupManager) {
                window.top.popupManager.open(url, undefined, width, height, 'modal-mobile-friendly');
            } else {
                window.open(url, '_blank').focus();
            }
        };

        $('#icon-information').click(function () {
            openPopup($('#hidden-transferHintUrl').attr('href'), 560, 400);
        });

        $('#multiple-transfer').click(() => {
            const custIds = [];
            $tableTransfer.find('.checkbox:checked').each(function () {
                const $this = $(this);
                $this.is(':checked') && $this.data('value') !== 0 && custIds.push($this.data('custid'));
            });

            if (!custIds.length) {
                window.top.bootboxAlert($('#hidden-noTransferMade').val());
                return;
            }

            if (!confirm($('#hidden-confirmTransfer').val())) {
                return;
            }

            $.blockUI();
            $.ajax({
                headers: {
                    RequestVerificationToken: $("input[name='__RequestVerificationToken']").val(),
                    'X-Requested-With': 'XMLHttpRequest'
                },
                type: 'POST',
                url: 'Multiple',
                data: { custIds },
                dataType: 'json',
                success: (result) => {
                    if (result.Code === 6001) {
                        this.reloadPage(result.Message);
                    } else if (result.Code === 0) {
                        window.top.bootboxAlert(result.Message);
                        setTimeout(function () {
                            window.location.reload();
                        }, 3000);
                    } else {
                        this.alert.showError(result.Message);
                    }
                },
                complete: $.unblockUI
            });
        });

        $('#multiple-transfer-allpages:not(.disabled)').click(function () {
            if (!confirm($('#hidden-confirmTransfer').val())) {
                return;
            }

            $.blockUI();
            $.ajax({
                headers: {
                    RequestVerificationToken: $("input[name='__RequestVerificationToken']").val(),
                    'X-Requested-With': 'XMLHttpRequest'
                },
                type: 'POST',
                url: 'Multiple',
                data: { CustIds: '0' },
                dataType: 'json',
                success: (result) => {
                    this.alert.show(result.Code, result.Message);
                    result.IsSuccessful &&
                        window.top.closePopupAndReloadMainFrameAfterUpdateSuccess &&
                        window.top.closePopupAndReloadMainFrameAfterUpdateSuccess(3000);
                },
                complete: $.unblockUI
            });
        });

        $('#btn-autoTransfer').click(function () {
            const onIds = [];
            const onNames = [];
            const offIds = [];
            const offNames = [];
            $tableTransfer.find('.checkbox:checked').each(function () {
                const $this = $(this);
                if ($this.is(':checked')) {
                    const custId = $this.data('custid');
                    const custName = $this.data('custname');
                    if ($this.data('autotransfer') === 'on') {
                        onIds.push(custId);
                        onNames.push(custName);
                    } else {
                        offIds.push(custId);
                        offNames.push(custName);
                    }
                }
            });

            if (onIds.length || offIds.length) {
                const url = [
                    $('#hidden-updateMultipleAutoTransferUrl').val(),
                    '&onIds=',
                    onIds.join(','),
                    '&onNames=',
                    onNames.join(','),
                    '&offIds=',
                    offIds.join(','),
                    '&offNames=',
                    offNames.join(',')
                ].join('');
                openPopup(url, 600, 500);
            }
        });

        $('.switch-auto-transfer, .icon-calendar').click(function (event) {
            event.preventDefault();
            openPopup($(this).attr('href'), 600);
        });

        $('.transfer-link').click(function (event) {
            event.preventDefault();
            openPopup($(this).attr('href'), 400);
        });
    }

    private reloadPage(message) {
        const currentADBT = $('#hidden-isAllDayBalanceTransfer').val().toLowerCase() === 'true';
        window.top.indexManager.addCustomCookies('messageForReloadSite', message);
        window.top.indexManager.updateCurrentUrl('adbt=' + (currentADBT ? 'False' : 'True'));
        window.top.location.reload();
    }
}

$(function () {
    new TransferListView(
        new AlertComponent(),
        new UsernameSuggestionComponent(),
        new DropdownTransferComponent(),
        new DropdownTransferComponent(),
        new VisibleColumnContextMenuComponent(),
        new TransferListPaging()
    ).init();
});
