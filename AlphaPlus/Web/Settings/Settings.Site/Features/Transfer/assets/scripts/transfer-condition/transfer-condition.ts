declare var TipsyHelper;

class TransferConditionView {
    private alert: AlertComponent;
    private NotAuthorizedMessage: any;

    constructor(alert: AlertComponent) {
        this.alert = alert;
    }

    init() {
        this.NotAuthorizedMessage = $('input[name="NotAuthorizedMessage"]').val();
        this.showTitle();
        const $checkboxDayOfWeeks = $('input[type=checkbox][name=Weekly]');
        const $labelMon = $('label[for=chk-0]');

        $checkboxDayOfWeeks.change(function () {
            TipsyHelper.clear($labelMon);
            if (this.checked && $checkboxDayOfWeeks.filter(':checked').length === 7) {
                this.checked = !this.checked;
                TipsyHelper.showError($labelMon, $('#hidden-pleaseChooseDailyText').val());
            }
        });

        $('input[type=radio][name=TransferCondition]').change(function () {
            TipsyHelper.clear($labelMon);
            $checkboxDayOfWeeks.prop('disabled', $(this).attr('id') === 'rdo-daily');
        });

        const $formTransfer = $('#form-transfer');
        $formTransfer.submit((event) => {
            event.preventDefault();

            this.alert.hideAlert();
            const transferOption = this.getTransferOption($checkboxDayOfWeeks, $labelMon);
            if (!transferOption) {
                return;
            }

            let confirmMessage = $('#hidden-confirmUpdateTransferConditionText').val();
            if ($('#hidden-isEnableAutoTransfer').val() === '1' && transferOption !== '00000000') {
                confirmMessage += $('#hidden-updateTransferCondition_NoteText').val() + '<br/>';
                confirmMessage +=
                    '<div class="action-auto-transfer"><input id="rad-on-auto-transfer" type="radio" name="rad-change-affect" class="rad-change-affect" value="0" checked="checked" /><label for="rad-on-auto-transfer">' +
                    $('#hidden-updateTransferCondition_OnAutoText').val() +
                    '</label></div>';
                confirmMessage +=
                    '<div class="action-auto-transfer"><input id="rad-off-auto-transfer" type="radio" name="rad-change-affect" class="rad-change-affect" value="1" /><label for="rad-off-auto-transfer">' +
                    $('#hidden-updateTransferCondition_OffAutoText').val() +
                    '</label></div>';
            }
            window.top.bootboxConfirm(
                confirmMessage,
                $('#btn-submit'),
                () => {
                    this.updateTransferOption($formTransfer.attr('action'), transferOption);
                },
                null,
                { type: 'TransferCondition' }
            );
        });
    }

    private getTransferOption($checkboxDayOfWeeks, $labelMon) {
        TipsyHelper.clear($labelMon);
        const $rdoDaily = $('#rdo-daily');
        if ($rdoDaily.is(':checked')) {
            return $rdoDaily.val();
        }

        const checkboxDayOfWeeksCheckedLength = $checkboxDayOfWeeks.filter(':checked').length;
        if (checkboxDayOfWeeksCheckedLength === 0) {
            TipsyHelper.showError($labelMon, $('#hidden-pleaseSelectOneDayText').val());
            return;
        } else if (checkboxDayOfWeeksCheckedLength === 7) {
            TipsyHelper.showError($labelMon, $('#hidden-pleaseChooseDailyText').val());
            return;
        }

        const transferOption = $('#rdo-weekly').val().split('');
        if (transferOption.length === 8) {
            $checkboxDayOfWeeks.each(function () {
                const $checkbox = $(this);
                const index = +$checkbox.val();
                if (0 <= index && index <= 6) {
                    transferOption[index] = $checkbox.is(':checked') ? '1' : '0';
                }
            });
        }

        return transferOption.join('');
    }

    private updateTransferOption(url: string, transferOption: string, autoTransferOption = undefined) {
        if (typeof autoTransferOption === 'undefined') {
            autoTransferOption = 0;
            window.top.$('input[type=radio][name=rad-change-affect]').each(function () {
                const $this = $(this);
                if ($this.is(':checked')) {
                    autoTransferOption = $this.val();
                }
            });
        }

        $.blockUI();
        $.ajax({
            headers: {
                RequestVerificationToken: $("input[name='__RequestVerificationToken']").val(),
                'X-Requested-With': 'XMLHttpRequest'
            },
            type: 'POST',
            url: url,
            data: { transferOption, custId: $('#CustId').val() },
            success: (result) => {
                if (result.code === 999991) {
                    window.top[url] = () => this.updateTransferOption(url, transferOption, autoTransferOption);
                    window.top.ShowSecCodePopup(url);
                    return;
                }

                if (result.IsSuccessful) {
                    this.alert.showSuccess(result.Message);
                    setTimeout(function () {
                        window.location.reload();
                    }, 3000);
                } else {
                    this.alert.showError(result.Message);
                }
            },
            error: (jqXHR) => {
                if (JSON.parse(jqXHR.responseText) && JSON.parse(jqXHR.responseText).message === 'NoPermission') {
                    this.alert.showError(this.NotAuthorizedMessage);
                } else {
                    this.alert.showError('Sorry, there was an unexpected issue on the server. Please seek help from our customer support. [5000]!');
                }
            },
            complete: $.unblockUI
        });
    }

    // Hide Title on top page, when open Popup.
    // Hide Title on top page, when open Popup.
    private showTitle() {
        const popup = parent.document.getElementById('product-popup');
        popup || $('.page-title').removeClass('hide');
    }
}

$(function () {
    new TransferConditionView(new AlertComponent()).init();
});
