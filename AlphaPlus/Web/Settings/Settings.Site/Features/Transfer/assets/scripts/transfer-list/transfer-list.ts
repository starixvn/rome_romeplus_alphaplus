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
        const $formTransfer = $('#form-transfer').submit(function () {
            $.blockUI();
        });
        const $tableTransfer = $('#tbl-transfer');

        this.initComponents($formTransfer, $tableTransfer);

        $('#AllYesterdayBalance').change(function () {
            $formTransfer.submit();
        });

        const $btnTransfer = $('#btn-transfer');
        const $btnAutoTransfer = $('#btn-autoTransfer');
        const $checkboxAll = $('#checkbox-all');
        const $checkboxList = $('.checkbox');
        $checkboxAll.change(function () {
            const checked = this.checked;
            $btnTransfer.prop('disabled', !checked);
            $btnAutoTransfer.prop('disabled', !checked);
            $checkboxList.prop('checked', checked);
        });
        $checkboxList.change(function () {
            const checkedCount = $checkboxList.filter(':checked').length;
            if (checkedCount === 0) {
                $checkboxAll.prop({
                    indeterminate: false,
                    checked: false
                });
                $btnTransfer.prop('disabled', true);
                $btnAutoTransfer.prop('disabled', true);
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
                $btnTransfer.prop('disabled', false);
                $btnAutoTransfer.prop('disabled', false);
            }
        });

        $btnAutoTransfer.click(() => this.openPopupMultipleAutoTransfer($tableTransfer));
        $('#menu-transfer').click(() => this.handleMultipleTransferYesterdayBalance($tableTransfer));
        $('#menu-transfer-allpages').click((e) => {
            if ($(e.target).parent().hasClass('disabled')) {
                return;
            }

            const username = $.trim($('#Username').val());
            const status = $('#Status').val();
            const autoTransfer = $('#AutoTransfer').val();
            const allYesterdayBalance = $('#AllYesterdayBalance').is(':checked');
            this.handleMultipleTransferYesterdayBalance({
                username,
                status,
                autoTransfer,
                allYesterdayBalance
            });
        });
        $('#icon-information').click(() => this.openPopup($('#hidden-transferHintUrl').attr('href'), 560, 400));

        $('.switch-auto-transfer, .icon-calendar').click((event) => {
            event.preventDefault();
            this.openPopup($(event.target).attr('href'), 600);
        });

        $('.transfer-link').click((event) => {
            event.preventDefault();
            const $this = $(event.target);
            const width = $this.data('w') || 400;
            const height = $this.data('h') || 360;
            this.openPopup($this.attr('href'), width, height);
        });

        $tableTransfer.removeClass('hide').stickyTableHeaders();
    }

    private initComponents($formTransfer: JQuery, $tableTransfer: JQuery) {
        this.transferListPaging.init(() => $formTransfer.submit());
        this.usernameSuggestionComponent.init(
            new UsernameSuggestionOption($('#Username'), $('#hidden-usernameSuggestionUrl').val(), true)
        );
        this.statusDropdownComponent.render('Status');
        this.autoTransferDropdownComponent.render('AutoTransfer');
        this.visibleColumnContextMenu.init(`transfer_${$('#ViewerId').val()}`, $tableTransfer);
    }

    private openPopupMultipleAutoTransfer($tableTransfer: JQuery) {
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
            this.openPopup(url, 600, 500);
        }
    }
    /*
    private handleMultipleTransferYesterdayBalance($tableTransfer: JQuery) {
        const custIds = [];
        $tableTransfer.find('.checkbox:checked').each(function () {
            const $this = $(this);
            $this.is(':checked') && $this.data('value') !== 0 && custIds.push($this.data('custid'));
        });

        if (!custIds.length) {
            window.top.bootboxAlert($('#hidden-noTransferMade').val());
            return;
        }

        this.handleMultipleTransferYesterdayBalanceAllPages({ custIds });

        if (!confirm($('#hidden-confirmTransfer').val())) {
            return;
        }
    }
*/
    private handleMultipleTransferYesterdayBalance(formData) {
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
            url: $('#hidden-transferYesterdayBalanceMultipleUrl').attr('href'),
            data: formData,
            dataType: 'json',
            success: (result) => {
                if (result.Code === 6001) {
                    this.reloadPage(result.Message);
                } else if (result.Code === 0) {
                    window.top.bootboxAlert(result.Message);
                    window.top.closePopupAndReloadMainFrameAfterUpdateSuccess &&
                        window.top.closePopupAndReloadMainFrameAfterUpdateSuccess(3000);
                } else {
                    this.alert.showError(result.Message);
                }
            },
            complete: $.unblockUI
        });
    }

    private openPopup(url: string, width: number, height = 360) {
        if (window.top.popupManager) {
            window.top.popupManager.open(url, undefined, width, height, 'modal-mobile-friendly');
        } else {
            window.open(url, '_blank').focus();
        }
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
