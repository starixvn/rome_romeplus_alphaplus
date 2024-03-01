"use strict";

class ParlayMaxPayoutService {
    private $: JQueryStatic;
    private alertService: AlertService;

    constructor($: JQueryStatic, alertService: AlertService) {
        this.$ = $;
        this.alertService = alertService;
    }

    public CheckDownline(params: string, onSuccessCallback: Function): void {
        this.callAjax(
            this.$("#tb-checkdownlineurl").val(),
            "POST",
            params,
            data => {
                onSuccessCallback(data, params);
            }
        );
    }

    public UpdateParlayMaxPayout(params: string, onSuccessCallback: Function): void {
        this.callAjax(
            this.$("#tb-updateurl").val(),
            "POST",
            params,
            data => {
                onSuccessCallback(data);
            }
        );
    }

    private callAjax(url: string, type: string, params: string, onSuccessCallback: Function): void {
        this.$.ajax({
            url,
            beforeSend: this.$.blockUI,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            type,
            headers: {
                __RequestVerificationToken: this.$("input[name='__RequestVerificationToken']").val(),
                'X-Requested-With': 'XMLHttpRequest'
            },
            data: params,
            error: jqXHR => {
                if (jqXHR.status === StatusCode.Forbidden) {
                    return this.alertService.showMessage(this.$("#tb-notauthorized-msg").val(), true);
                }

                this.alertService.showMessage("Server error.", true);
            },
            success: data => {
                onSuccessCallback(data);
            },
            complete: this.$.unblockUI
        });
    }
}