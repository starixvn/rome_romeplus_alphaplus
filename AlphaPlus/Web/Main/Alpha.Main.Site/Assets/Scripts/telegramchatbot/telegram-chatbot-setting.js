/// <reference path="../core.ts" />
"use strict";
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
var TelegramChatBotSetting = /** @class */ (function (_super) {
    __extends(TelegramChatBotSetting, _super);
    function TelegramChatBotSetting($, topWindow, clipboard) {
        var _this = _super.call(this, $, topWindow) || this;
        _this.HAS_UNLINKED_ERR_CODE = 102;
        _this.TELEGRAM_OTP_FUNCTION_ID = 15;
        _this.isFirstLoad = true;
        _this.resources = {
            unsubscribeWarning: document.getElementById('UnsubscribeWarning').value,
            updateLanguageFailed: document.getElementById('UpdateLanguageFailed').value,
            hasUnlinkedMessage: document.getElementById('HasUnlinkedMessage').value
        };
        _this.init = function () {
            alpha.detectResponsiveInformation();
            _this.cachedDOMElements();
            _this.initLanguage();
            _this.registerEvents();
        };
        _this.cachedDOMElements = function () {
            _this.$language = _this.$("#language");
            _this.$accountMessage = _this.$("#account-message");
            _this.$languageSelectorContainer = _this.$(".language-selector-container");
            _this.$changeLanguageLink = _this.$(".changelanguage-link");
            _this.$btnUnsubscribe = _this.$("#btnUnsubscribe");
            _this.$copyIcon = _this.$("#copy-icon");
            _this.$userId = _this.$("#UserId");
            _this.$overlay = _this.$("#overlay");
            _this.$container = _this.$("#container");
        };
        _this.showHighlight = function () {
            _this.$container.addClass("chatbot-linked-telegram");
            _this.$(window).scrollTop(900);
            _this.setLocalStorageValue("IsShowedTryItNow", true);
        };
        _this.registerEvents = function () {
            _this.$(".function").click(_this.updateFunction);
            _this.$btnUnsubscribe.click(_this.unsubscribeEvent);
            var self = _this;
            _this.$changeLanguageLink.on('click touchend', function (e) {
                self.changeLanguage(self.$(e.target).data("value"));
            });
            if (window.document.queryCommandSupported
                && window.document.queryCommandSupported("copy")) {
                _this.clipboard
                    .on("success", function (e) {
                    self.$(e.trigger).tooltip("show");
                })
                    .on("error", function (e) {
                    self.$(e.trigger).remove();
                    window.console.error("Action:", e.action);
                    window.console.error("Trigger:", e.trigger);
                });
            }
            else {
                _this.$copyIcon.remove();
            }
            _this.isFirstLoad = !_this.getLocalStorageValue("IsShowedTryItNow");
            var isShowingSuggestOtpTelegram = _this.getLocalStorageValue("IsShowingSuggestEnableOtpPopup");
            if (_this.isFirstLoad && !isShowingSuggestOtpTelegram) {
                _this.showHighlight();
                _this.$overlay.click(function () {
                    _this.$container.removeClass("chatbot-linked-telegram");
                });
            }
        };
        _this.initLanguage = function () {
            var currentLanguage = _this.$language.data("currentlanguage");
            var language = _this.$(".list-language ." + currentLanguage + " > a").html();
            _this.$languageSelectorContainer.prepend(language);
        };
        _this.updateFunction = function (e) {
            var checkbox = _this.$(e.target);
            var functionId = checkbox.attr("data-functionId");
            var functionName = checkbox.attr("data-functionName");
            var type = checkbox.attr("data-type");
            var enabled = checkbox.is(":checked");
            var self = _this;
            _this.$.blockUI();
            _this.$.ajax({
                type: "POST",
                url: alpha.urls.mainRootPath + "TelegramChatBot/UpdateFunction",
                data: { functionId: functionId, functionName: functionName, type: type, enabled: enabled },
                dataType: "json",
                success: function (result) {
                    if (result && result.IsNotOk) {
                        if (result.IsNotAuthorized) {
                            self.topWindow.bootboxAlert(result.Message);
                            return;
                        }
                        if (result.Code == _this.HAS_UNLINKED_ERR_CODE) {
                            _this.showHasUnlinkedMessage();
                            return;
                        }
                        checkbox.prop("checked", !enabled);
                    }
                    if (functionId == _this.TELEGRAM_OTP_FUNCTION_ID.toString()) {
                        setTimeout(function () {
                            parent.window.location.href = parent.window.location.href;
                        }, 2000);
                    }
                },
                complete: _this.onComplete,
            });
        };
        _this.unsubscribeEvent = function (e) {
            e.preventDefault();
            var self = _this;
            _this.topWindow.bootboxConfirm(_this.resources.unsubscribeWarning, undefined, function () {
                _this.$.blockUI();
                _this.$.ajax({
                    type: "POST",
                    url: alpha.urls.mainRootPath + "TelegramChatBot/Unsubscribe",
                    dataType: "json",
                    success: self.onUnSubscribeSuccess,
                    complete: self.onComplete
                });
            });
        };
        _this.onUnSubscribeSuccess = function (result) {
            if (result && result.IsNotOk) {
                if (result.IsNotAuthorized) {
                    _this.topWindow.bootboxAlert(result.Message);
                    return;
                }
                if (result.Code == _this.HAS_UNLINKED_ERR_CODE) {
                    _this.showHasUnlinkedMessage();
                    return;
                }
                alpha.showMessage(_this.$accountMessage, new Result(result));
                return;
            }
            _this.reloadPage();
        };
        _this.changeLanguage = function (lang) {
            _this.$.blockUI();
            var currentLanguage = _this.$language.data("currentlanguage");
            if (lang == currentLanguage) {
                _this.onComplete();
                return;
            }
            _this.$.ajax({
                type: "POST",
                url: alpha.urls.mainRootPath + "TelegramChatBot/UpdateLanguage",
                data: { language: lang },
                success: _this.onChangeLanguageSuccess,
                complete: _this.onComplete
            });
        };
        _this.onChangeLanguageSuccess = function (result) {
            if (result) {
                if (result.IsOk) {
                    _this.reloadPage();
                }
                else {
                    if (result.IsNotAuthorized) {
                        _this.topWindow.bootboxAlert(result.Message);
                        return;
                    }
                    if (result.Code == _this.HAS_UNLINKED_ERR_CODE) {
                        _this.showHasUnlinkedMessage();
                        return;
                    }
                    alpha.showMessage(_this.$accountMessage, new Result({ Code: 1, Message: _this.resources.updateLanguageFailed }));
                }
            }
        };
        _this.showHasUnlinkedMessage = function () {
            _this.topWindow.bootboxAlert(_this.resources.hasUnlinkedMessage, undefined, function () { return _this.reloadPage(); });
        };
        _this.clipboard = clipboard;
        return _this;
    }
    TelegramChatBotSetting.prototype.setLocalStorageValue = function (key, value) {
        alpha.localStorage.set(key, value ? "1" : "0");
    };
    TelegramChatBotSetting.prototype.getLocalStorageValue = function (key) {
        return alpha.localStorage.get(key) === "1";
    };
    return TelegramChatBotSetting;
}(BaseTelegramChatBot));
$(function () {
    var telegramChatBotSetting = new TelegramChatBotSetting($, window.top, new Clipboard('.icon-content-copy'));
    telegramChatBotSetting.init();
});
//# sourceMappingURL=telegram-chatbot-setting.js.map