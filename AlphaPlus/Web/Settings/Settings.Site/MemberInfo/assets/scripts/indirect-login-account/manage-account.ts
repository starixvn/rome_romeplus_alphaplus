"use strict";

class ManageAccount {
    $: JQueryStatic;
    topWindow: any;
    rootURL: string;

    constructor(
        $: JQueryStatic,
        topWindow: any) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    public init() {
        this.cacheDOMElements();
        this.registerEvents();
    }

    public cacheDOMElements() {
        this.rootURL = this.$("#txt-rootURL").val();
    }

    public registerEvents() {
        const self = this;

        this.$("#btn-connectToNewAccount").click(function () {
            self.topWindow.alpha.detectResponsiveInformation()
            const url = self.rootURL + "NewAccount";
            self.topWindow.popupManager.open(url, undefined, 630, self.topWindow.alpha.isMobile ? 430 : 350);
        });

        this.$(".icon-indirect-login").not(".disabled").click(function (e) {
            self.$.blockUI();
            let isRedirect = false;
            const $this = self.$(this);
            const redirectURL = $this.attr("href");
            const parent = $this.closest("tr");
            const queryString = self.$.param({
                custId: parent.data("custid"),
                subAccId: parent.data("subaccid"),
                custname: parent.data("custname"),
            });
            self.$.ajax({
                type: "GET",
                url: self.rootURL + "ValidateAccount?" + queryString,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data.IsOk) {
                        if (redirectURL.indexOf("browserSize=") === -1) {
                            $this.attr("href", redirectURL + "&browserSize=" + self.getBrowserSize());
                        } 

                        isRedirect = true;
                    } else if (data.Code === 5001) {
                        self.topWindow.popupManager.open(self.rootURL + "UpdatePassword?" + queryString, undefined, 530, 280);
                    } else {
                        self.topWindow.bootboxAlert(data.Message);
                    }
                },
                complete: self.$.unblockUI
            });

            return isRedirect;
        });

        this.$(".icon-disconnect").click(function () {
            const parent = self.$(this).closest("tr");
            const url = [
                self.rootURL,
                "DisconnectPopup?custid=", parent.data("custid"),
                "&subaccid=", parent.data("subaccid"),
                "&custname=", parent.data("custname")
            ].join("");
            self.topWindow.popupManager.open(url, undefined, 630, 330);
        });
    }

    private getBrowserSize(): string {
        var w = this.topWindow.window,
            d = this.topWindow.document,
            e = d.documentElement,
            g = d.getElementsByTagName("body")[0],
            x = w.innerWidth || e.clientWidth || g.clientWidth,
            y = w.innerHeight || e.clientHeight || g.clientHeight;

        return x + "x" + y;
    }
}

$(() => {
    const manageAccount = new ManageAccount($, window.top);
    manageAccount.init();
});