var PopupAdvertisement = /** @class */ (function () {
    function PopupAdvertisement($) {
        this.$ = $;
    }
    PopupAdvertisement.prototype.getMyWatchlist = function () {
        var _this = this;
        this.$.get(alpha.urls.mainRootPath + "Advertisement/MyWatchlist", function (data) {
            if (data.length) {
                var popup_1 = _this.$("#advertisement-modal");
                popup_1.find("#advertisement-modal-body").html(data);
                popup_1.on("shown.bs.modal", function () { return _this.bindMyWatchlistEvents(popup_1); });
                popup_1.modal({ show: true, backdrop: false })
                    .tinyDraggable({ exclude: "h3, ol, li, p, img, label, input[type='checkbox'], a, button" });
            }
        });
    };
    PopupAdvertisement.prototype.bindMyWatchlistEvents = function (popup) {
        var _this = this;
        var chkDontShowAgain = popup.find("#chb-dontShow");
        chkDontShowAgain.change(function () { return _this.setDontShowAgainLocalStorage(chkDontShowAgain.is(":checked")); });
        popup.find("#btn-setup").click(function () {
            popup.modal("hide");
            _this.$("#userLevel").val() === "agent"
                ? _this.activateSidebarMenu(_this.$("#sub-menu-MYWATCHLIST"))
                : _this.activateSidebarMenu(_this.$("#sub-menu-MYTHRESHOLDS"));
        });
    };
    PopupAdvertisement.prototype.setDontShowAgainLocalStorage = function (value) {
        alpha.localStorage.set(this.dontShowAgainKey, value ? "1" : "0");
    };
    PopupAdvertisement.prototype.getDontShowAgainLocalStorage = function () {
        return alpha.localStorage.get(this.dontShowAgainKey) === "1";
    };
    PopupAdvertisement.prototype.activateSidebarMenu = function (menu) {
        menu.trigger("click");
        this.$(".sub-menu-item").removeClass("active");
        var mainIframe = this.$("#main");
        try {
            if (!alpha.core.isIe()) {
                var mainIframeContent = mainIframe.contents();
                mainIframeContent.find("head").append(this.$("#AssetCommonCssLink").val());
                mainIframeContent.find("body").empty().block();
            }
        }
        catch (e) { }
        mainIframe.attr("src", menu.attr("href")).attr("target", "main");
    };
    PopupAdvertisement.prototype.init = function () {
        this.dontShowAgainKey = [
            "AdvertisementDontShowAgainKey",
            "179067",
            this.$("#userId").val(),
            this.$("#subUserId").val()
        ].join("_");
        this.$("#subPageAccess").val().includes("[H]")
            && !this.getDontShowAgainLocalStorage()
            && this.getMyWatchlist();
    };
    return PopupAdvertisement;
}());
//$(function () {
//    new PopupAdvertisement($).init();
//});
//# sourceMappingURL=popup-advertisement.js.map