"use strict";
interface Window {
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
        this.$("#info").click((item) => {
            var subAccId = this.$(item.currentTarget).data("subaccid");
            this.Edit(subAccId);
        });
    };

    Add = () => {
        this.topWindow.popupManager.open("/settings/SubAccount/Add", "Add Sub Account", 600, 450);
    };

    Edit = (subAccId: number) => {
        this.topWindow.popupManager.open("/settings/SubAccount/Get?subAccId=" + subAccId, "Edit Sub Account", 600, 450);
    };
}

const subAcc = new SubAccount($, top);

$(() => {
    subAcc.Init();
});