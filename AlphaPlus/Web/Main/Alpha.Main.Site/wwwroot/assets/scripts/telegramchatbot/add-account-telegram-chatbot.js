/// <reference path="../core.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var AddAccountTelegramChatBot = /** @class */ (function (_super) {
    __extends(AddAccountTelegramChatBot, _super);
    function AddAccountTelegramChatBot($, topWindow) {
        var _this = _super.call(this, $, topWindow) || this;
        _this.resources = {
            language: document.getElementById('Language').value
        };
        _this.init = function () {
            _this.cachedDOMElements();
            _this.registerEvents();
        };
        _this.cachedDOMElements = function () {
            _this.$registerTelegramPanel = _this.$("#registerTelegramPanel");
            _this.$addAccountMessage = _this.$("#add-account-message");
            _this.$btnGenQRCode = _this.$("#btnGenQRCode");
            _this.$logoTelegram = _this.$("#logoTelegram");
            _this.$imgQRCode = _this.$("#imgQRCode");
            _this.$linkShowQRCode = _this.$("#showQRCode");
            _this.$linkHideQRCode = _this.$("#hideQRCode");
        };
        _this.registerEvents = function () {
            _this.$btnGenQRCode.click(_this.genQRCodeEvent);
            _this.$linkShowQRCode.click(function () {
                _this.$logoTelegram.addClass("hide");
                _this.$imgQRCode.removeClass("hide");
                _this.$linkShowQRCode.addClass("hide");
                _this.$linkHideQRCode.removeClass("hide");
            });
            _this.$linkHideQRCode.click(function () {
                _this.$imgQRCode.addClass("hide");
                _this.$logoTelegram.removeClass("hide");
                _this.$linkHideQRCode.addClass("hide");
                _this.$linkShowQRCode.removeClass("hide");
            });
        };
        _this.genQRCodeEvent = function (e) {
            e.preventDefault();
            _this.$.blockUI();
            _this.$.ajax({
                type: "POST",
                url: alpha.urls.mainRootPath + "TelegramChatBot/GenerateQRCode",
                dataType: "text",
                success: _this.onGenQRCodeSuccess,
                complete: _this.onComplete
            });
        };
        _this.onGenQRCodeSuccess = function (data) {
            var result = _this.safelyParseJSON(data);
            if (result && result.IsNotAuthorized) {
                _this.topWindow.bootboxAlert(result.Message);
                return;
            }
            _this.$btnGenQRCode.attr("disabled", "disabled");
            _this.$registerTelegramPanel.append(data);
            _this.verifyAccountTelegramChatbot.init();
        };
        _this.verifyAccountTelegramChatbot = new VerifyAccountTelegramChatbot($, topWindow);
        return _this;
    }
    return AddAccountTelegramChatBot;
}(BaseTelegramChatBot));
$(function () {
    var addAccountTelegramChatbot = new AddAccountTelegramChatBot($, window.top);
    addAccountTelegramChatbot.init();
});
//# sourceMappingURL=add-account-telegram-chatbot.js.map