/// <reference path="../core.ts" />
'use strict';
var CreateApiKeyResult = /** @class */ (function () {
    function CreateApiKeyResult($, topWindow, clipboardKey, clipboardSecret) {
        var _this = this;
        this.init = function () {
            _this.$showURL = _this.$(".dummy-text");
            _this.$urlMask = _this.$(".service-url-mask");
            _this.registerEvents();
        };
        this.registerEvents = function () {
            var self = _this;
            _this.$showURL.click(_this.showURL);
            if (window.document.queryCommandSupported
                && window.document.queryCommandSupported("copy")) {
                _this.clipboardKey
                    .on("success", function (e) {
                    self.$(e.trigger).tooltip("show");
                })
                    .on("error", function (e) {
                    self.$(e.trigger).remove();
                    window.console.error("Action:", e.action);
                    window.console.error("Trigger:", e.trigger);
                });
                _this.clipboardSecret
                    .on("success", function (e) {
                    self.$(e.trigger).tooltip("show");
                })
                    .on("error", function (e) {
                    self.$(e.trigger).remove();
                    window.console.error("Action:", e.action);
                    window.console.error("Trigger:", e.trigger);
                });
            }
            _this.$("#btnOk").click(_this.handleBtnOk);
        };
        this.handleBtnOk = function () {
            _this.topWindow.popupManager.close();
        };
        this.showURL = function () {
            _this.$urlMask.removeClass('close');
            _this.$urlMask.addClass('open');
        };
        this.$ = $;
        this.topWindow = topWindow;
        this.clipboardKey = clipboardKey;
        this.clipboardSecret = clipboardSecret;
    }
    return CreateApiKeyResult;
}());
$(function () {
    var createApiKeyResult = new CreateApiKeyResult($, window.top, new Clipboard('.icon-content-copy'), new Clipboard('.icon-copy-secret'));
    createApiKeyResult.init();
});
//# sourceMappingURL=create-apikey-result.js.map