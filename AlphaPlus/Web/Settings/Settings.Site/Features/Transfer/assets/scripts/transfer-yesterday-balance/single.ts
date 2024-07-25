interface Window {
    closePopupAndReloadMainFrameAfterUpdateSuccess: Function;
}

class SingleTransferView {
    private alert: AlertComponent;

    constructor(alert: AlertComponent) {
        this.alert = alert;
    }

    init() {
        const $amount = $('#Amount');
        $amount.on('input', function () {
            this.value = this.value.replace(/\D/g, '').replace(/^0+(?!$)/, '');
        });

        $('#form-update').submit((event) => {
            event.preventDefault();

            const amountValue = parseFloat($amount.val().replace(/,/g, ''));
            if (isNaN(amountValue)) {
                this.alert.showError($('#hidden-InvalidValue').val());
                return;
            }

            this.alert.hideAlert();
            window.top.bootboxConfirm($('#hidden-ConfirmTransfer').val(), null, () => {
                $.blockUI();
                const $formUpdate = $(event.target);
                $.ajax({
                    headers: {
                        RequestVerificationToken: $("input[name='__RequestVerificationToken']").val(),
                        'X-Requested-With': 'XMLHttpRequest'
                    },
                    type: 'POST',
                    url: $formUpdate.attr('action'),
                    data: { CustId: $('#CustId').val(), Amount: amountValue },
                    dataType: 'json',
                    success: (result) => {
                        if (result.Code === 6001) {
                            this.reloadPage(result.Message);
                        } else if (result.Code === 0) {
                            this.alert.showSuccess(result.Message);
                            window.top.closePopupAndReloadMainFrameAfterUpdateSuccess &&
                                window.top.closePopupAndReloadMainFrameAfterUpdateSuccess(3000);
                        } else {
                            this.alert.showError(result.Message);
                        }
                    },
                    complete: $.unblockUI
                });
            });
        });

        $('.btn-cancel').click(function () {
            window.top.popupManager.close();
        });

        $amount.focus();
    }

    private reloadPage(message) {
        window.top.indexManager.addCustomCookies('messageForReloadSite', message);
        window.top.indexManager.updateCurrentUrl('adbt=True');
        window.top.location.reload();
    }
}

$(function () {
    new SingleTransferView(new AlertComponent()).init();
});
