/// <reference path="../../core.ts" />
var ChangeOtpTelegramStatus = /** @class */ (function () {
    function ChangeOtpTelegramStatus($, topWindow) {
        var _this = this;
        this.init = function () {
            _this.isShowRedirectToTelegramPage = "IsShowRedirectToTelegramChatPage";
            _this.isEnableTelegramOtp = "IsEnableTelegramOtp";
            _this.cachedDOMElements();
            _this.registerEvents();
        };
        this.resources = {
            unsubscribeWarning: document.getElementById('UnsubscribeWarning').value
        };
        this.cachedDOMElements = function () {
            _this.$btnUnsubscribe = _this.$("#btnUnsubscribe");
            _this.$cbkEnableOtp = _this.$(".cbkEnableOtp");
        };
        this.registerEvents = function () {
            _this.$btnUnsubscribe.click(_this.unsubscribeEvent);
            _this.$cbkEnableOtp.click(_this.updateFunction);
        };
        this.unsubscribeEvent = function (e) {
            e.preventDefault();
            _this.topWindow.bootboxConfirm(_this.resources.unsubscribeWarning, undefined, function () {
                var self = _this;
                _this.$.blockUI();
                _this.$.ajax({
                    type: "POST",
                    url: alpha.urls.mainRootPath + "TelegramChatBot/Unsubscribe",
                    dataType: "text",
                    success: function (data, textStatus, jqXHR) {
                        parent.window.location.href = parent.window.location.href;
                    },
                    complete: function (jqXHR, textStatus) {
                        self.$.unblockUI();
                    }
                });
            });
        };
        this.updateFunction = function (e) {
            var checkbox = _this.$(e.target);
            var enabled = checkbox.is(":checked");
            _this.$.blockUI();
            var self = _this;
            _this.$.ajax({
                type: "POST",
                url: alpha.urls.mainRootPath + "Otp/UpdateOtpFunction",
                data: { enabled: enabled },
                dataType: "json",
                success: function (result) {
                    setTimeout(function () {
                        parent.window.location.href = parent.window.location.href;
                    }, 2000);
                },
                complete: function (jqXHR, textStatus) {
                    self.$.unblockUI();
                }
            });
        };
        this.$ = $;
        this.topWindow = topWindow;
    }
    return ChangeOtpTelegramStatus;
}());
$(function () {
    var changeOtpTelegramStatus = new ChangeOtpTelegramStatus($, window.top);
    changeOtpTelegramStatus.init();
});
//# sourceMappingURL=change-otp-telegram-status.js.map