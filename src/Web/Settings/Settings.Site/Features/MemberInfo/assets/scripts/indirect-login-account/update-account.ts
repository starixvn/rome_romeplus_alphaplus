﻿"use strict";

class UpdateAccount extends BaseAccount {
    custId: number;
    subAccId: number;

    constructor($: JQueryStatic,
        topWindow: any,
        encryptor: Encryptor,
        alert: Alert) {
        super($, topWindow, encryptor, alert);
    }

    public cacheDOMElements() {
        super.cacheDOMElements();
        this.custId = parseInt(this.$("#txt-custId").val());
        this.subAccId = parseInt(this.$("#txt-subAccId").val());
        this.$("#txt-browserSize").val(this.getBrowserSize());
    }

    public getFormData() {
        let formData = super.getFormData();

        if (!formData) {
            return;
        }

        formData["custId"] = this.custId;
        formData["subAccId"] = this.subAccId;

        return formData;
    }

    public onSubmitFormSuccess() {
        this.topWindow.closePopupAndReloadMainFrameAfterUpdateSuccess(1);
        document["formRedirect"].submit();
    }
}

$(() => {
    const updateAccount = new UpdateAccount($, window.top, new Encryptor(), new Alert($));
    updateAccount.init();
    updateAccount.txtPassword.focus();
});