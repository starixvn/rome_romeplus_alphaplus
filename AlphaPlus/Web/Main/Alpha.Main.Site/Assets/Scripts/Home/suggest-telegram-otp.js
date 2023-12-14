/// <reference path="../core.ts" />
"use strict";
var SuggestTelegramOtp = /** @class */ (function () {
    function SuggestTelegramOtp($) {
        var _this = this;
        this.init = function () {
            _this.isShowedSuggestEnableOtpPopup = "IsShowedSuggestEnableOtpPopup";
            _this.isShowingSuggestEnableOtpPopup = "IsShowingSuggestEnableOtpPopup";
            _this.cachedDOMElements();
            if (!_this.getLocalStorageValue(_this.isShowedSuggestEnableOtpPopup)) {
                _this.showSuggestEnableTelegramOtp();
                _this.setLocalStorageValue(_this.isShowedSuggestEnableOtpPopup, true);
            }
            _this.registerEvents();
        };
        this.cachedDOMElements = function () {
            _this.$suggestOtpMessage = _this.$("#suggest-otp-message");
            _this.enableOTPSuccessfullMessage = _this.$("#EnabledOTPSuccessMessage").val();
        };
        this.registerEvents = function () {
            _this.$("#enableTelegramOtp").click(_this.enableTelegramOtp);
            _this.$("#closeTelegramOtpPopup").click(_this.closeTelegramOtpPopup);
            _this.$("#telegramotp-modal").find(".icon-close").click(_this.handleCloseSuggestPopup);
        };
        this.enableTelegramOtp = function () {
            var self = _this;
            _this.$.blockUI();
            _this.$.ajax({
                type: "POST",
                url: alpha.urls.mainRootPath + "Otp/UpdateOtpFunction",
                data: { enabled: true },
                dataType: "json",
                success: function (result) {
                    alpha.showMessage(_this.$suggestOtpMessage, new Result({ Code: 0, Message: _this.enableOTPSuccessfullMessage }));
                    setTimeout(function () {
                        parent.window.location.href = parent.window.location.href;
                        _this.setLocalStorageValue(_this.isShowingSuggestEnableOtpPopup, false);
                    }, 2000);
                },
                complete: function (jqXHR, textStatus) {
                    self.$.unblockUI();
                }
            });
        };
        this.handleCloseSuggestPopup = function () {
            _this.setLocalStorageValue(_this.isShowingSuggestEnableOtpPopup, false);
        };
        this.closeTelegramOtpPopup = function () {
            var popup = _this.$("#telegramotp-modal");
            popup.modal("hide");
            _this.handleCloseSuggestPopup();
        };
        this.$ = $;
    }
    SuggestTelegramOtp.prototype.showSuggestEnableTelegramOtp = function () {
        var popup = this.$("#telegramotp-modal");
        popup.modal({ show: true, backdrop: false })
            .tinyDraggable({ exclude: "h3, ol, li, p, img, label, input[type='checkbox'], a, button" });
        this.setLocalStorageValue(this.isShowingSuggestEnableOtpPopup, true);
    };
    SuggestTelegramOtp.prototype.setLocalStorageValue = function (key, value) {
        alpha.localStorage.set(key, value ? "1" : "0");
    };
    SuggestTelegramOtp.prototype.getLocalStorageValue = function (key) {
        return alpha.localStorage.get(key) === "1";
    };
    return SuggestTelegramOtp;
}());
$(function () {
    new SuggestTelegramOtp($).init();
});
//# sourceMappingURL=suggest-telegram-otp.js.map