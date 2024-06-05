"use strict";
interface Window {
    clearPopupSubtitle: Function,
    popupManager: {
        open: Function,
        openWithHanlder: Function,
        close: Function,
        closeAllPopup: Function
    }
}

class SubAccount {
    topWindow: Window;
    $: JQueryStatic;

    constructor($: JQueryStatic, topWindow: Window) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    Init = () => {
        this.$("#btn-add").click(() => { this.Add(); });
        this.$(".info").click((item) => {
            var subAccId = this.$(item.currentTarget).data("subaccid");
            this.Edit(subAccId);
        });
        this.$(".pwd").click((item) => {
            var subAccId = this.$(item.currentTarget).data("subaccid");
            var username = this.$(item.currentTarget).data("username");
            this.ResetPassword(subAccId, username);
        });
        this.$(".sc").click((item) => {
            var subAccId = this.$(item.currentTarget).data("subaccid");
            var username = this.$(item.currentTarget).data("username");
            this.ResetSecurityCode(subAccId, username);
        });
        this.$(".otp").click((item) => {
            var subAccId = this.$(item.currentTarget).data("subaccid");
            var username = this.$(item.currentTarget).data("username");
            this.RemoveOtp(subAccId, username);
        });
    };

    Add = () => {
        this.topWindow.popupManager.open("/settings/SubAccount/Add", "Add Sub Account", 734, 536);
    };

    Edit = (subAccId: number) => {
        this.topWindow.popupManager.open("/settings/SubAccount/Get?subAccId=" + subAccId, "Edit Sub Account", 734, 536);
    };

    ResetPassword = (subAccId: number, username: string) => {
        this.topWindow.popupManager.open("/main/Password/ResetSubAccountPassword?subAccountId=" + subAccId + "&Username=" + username, "", 734, 536);
    };

    ResetSecurityCode = (subAccId: number, username: string) => {
        this.topWindow.popupManager.open("/main/SecurityCode/ResetSecurityCode?subAccId=" + subAccId + "&Username=" + username, "", 734, 536);
        this.topWindow.clearPopupSubtitle(true);
    };

    RemoveOtp = (subAccId: number, username: string) => {
        this.topWindow.popupManager.open("/main/Otp/ResetOtp?subAccountId=" + subAccId + "&Username=" + username, "", 734, 536);
        this.topWindow.clearPopupSubtitle(true);
    };
}

const subAcc = new SubAccount($, top);

$(() => {
    subAcc.Init();
});