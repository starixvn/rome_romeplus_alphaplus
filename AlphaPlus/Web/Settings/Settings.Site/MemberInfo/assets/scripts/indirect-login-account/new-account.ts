"use strict";

class NewAccount extends BaseAccount {
    constructor($: JQueryStatic,
        topWindow: any,
        encryptor: Encryptor,
        alert: Alert) {
        super($, topWindow, encryptor, alert);
    }

    public onSubmitFormSuccess() {
        this.topWindow.closePopupAndReloadMainFrameAfterUpdateSuccess(3000);
    }
}

$(() => {
    const newAccount = new NewAccount($, window.top, new Encryptor(), new Alert($));
    newAccount.init();
    newAccount.txtCustName.focus();
});