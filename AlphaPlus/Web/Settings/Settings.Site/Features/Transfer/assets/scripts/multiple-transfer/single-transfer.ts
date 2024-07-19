interface Window {
    closePopupAndReloadMainFrameAfterUpdateSuccess: Function;
}

class SingleTransferView {
    private alert: AlertComponent;

    constructor(alert: AlertComponent) {
        this.alert = alert;
    }

    init() {
        $('#Amount').on('input', function () {
            this.value = this.value.replace(/\D/g, '').replace(/^0+(?!$)/, '');
        });

        $('#form-update').submit((event) => {
            event.preventDefault();

            const amount = parseFloat($('#Amount').val().replace(/,/g, ''));
            if (isNaN(amount)) {
                this.alert.showError($('#hidden-InvalidValue').val());
                return;
            }

            this.alert.hideAlert();
            if (!confirm($('#hidden-ConfirmTransfer').val())) {
                return;
            }

            $.blockUI();
            const $formUpdate = $(event.target);
            $.ajax({
                headers: {
                    RequestVerificationToken: $("input[name='__RequestVerificationToken']").val(),
                    'X-Requested-With': 'XMLHttpRequest'
                },
                type: 'POST',
                url: $formUpdate.attr('action'),
                data: { CustId: $('#CustId').val(), Amount: amount },
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
    }
}

$(function () {
    new SingleTransferView(new AlertComponent()).init();
});
