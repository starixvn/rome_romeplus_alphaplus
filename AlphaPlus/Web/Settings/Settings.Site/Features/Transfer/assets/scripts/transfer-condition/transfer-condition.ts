declare var TipsyHelper;

class TransferConditionView {
    private alert: AlertComponent;

    constructor(alert: AlertComponent) {
        this.alert = alert;
    }

    init() {
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

            const transferOption = this.getTransferOption($checkboxDayOfWeeks, $labelMon);
            if (!transferOption) {
                return;
            }

            $.blockUI();
            $.ajax({
                headers: {
                    RequestVerificationToken: $("input[name='__RequestVerificationToken']").val(),
                    'X-Requested-With': 'XMLHttpRequest'
                },
                type: 'POST',
                url: $formTransfer.attr('action'),
                data: { transferOption, custId: $('#CustId').val() },
                success: (result) => {
                    if (result.IsSuccessful) {
                        this.alert.showSuccess(result.Message);
                        setTimeout(function () {
                            if ($('#hidden-showTitle').val() === '1') {
                                window.location.reload();
                            } else {
                                window.top.popupManager.close();
                            }
                        }, 3000);
                    } else {
                        this.alert.showError(result.Message);
                    }
                },
                complete: $.unblockUI
            });
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
}

$(function () {
    new TransferConditionView(new AlertComponent()).init();
});
