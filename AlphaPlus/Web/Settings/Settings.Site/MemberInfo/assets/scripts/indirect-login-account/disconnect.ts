﻿"use strict";

class Disconnect {
    $: JQueryStatic;
    topWindow: any;
    alert: Alert;

    constructor(
        $: JQueryStatic,
        topWindow: any,
        alert: Alert) {
        this.$ = $;
        this.topWindow = topWindow;
        this.alert = alert;
    }

    public init() {
        const self = this;
        const form = this.$("#form-disconnect");

        form.submit(function (e) {
            e.preventDefault();
            self.onDisconnect(form.attr("action"));
        });
    }

    public onDisconnect(url: string) {
        this.$.blockUI();

        this.$.ajax({
            type: "POST",
            url: url,
            beforeSend: xhr => xhr.setRequestHeader("RequestVerificationToken", this.$('input[name="__RequestVerificationToken"]').val()),
            data: {
                custId: this.$("#txt-custId").val(),
                subAccId: this.$("#txt-subAccId").val(),
                custname: this.$("#txt-custname").val()
            },
            dataType: "json",
            success: data => this.onDisconnectSuccess(data),
            complete: this.$.unblockUI
        });
    }

    public onDisconnectSuccess(data) {
        if (data.ErrCode) {
            this.alert.showError(data.ErrMsg);
        } else {
            this.alert.show(data.Code, data.message);

            if (data.IsOk) {
                this.topWindow.closePopupAndReloadMainFrameAfterUpdateSuccess(3000);
            }
        }
    }
}

$(() => {
    const disconnect = new Disconnect($, window.top, new Alert($));
    disconnect.init();
});