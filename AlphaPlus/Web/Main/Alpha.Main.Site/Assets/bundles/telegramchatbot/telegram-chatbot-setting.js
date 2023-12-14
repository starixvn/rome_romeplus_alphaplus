var alpha = {
    urls: {
        oldMainRootPath: top.document.getElementById('oldMainRootPath') != null
            ? top.document.getElementById('oldMainRootPath').value
            : "/ex-main/",
        mainRootPath: top.document.getElementById('mainRootPath') != null
            ? top.document.getElementById('mainRootPath').value
            : "not found",
        accountInfoRootPath: top.document.getElementById('accountInfoRootPath') != null
            ? top.document.getElementById('accountInfoRootPath').value
            : "not found",
        memberInfoRootPath: top.document.getElementById('memberInfoRootPath') != null
            ? top.document.getElementById('memberInfoRootPath').value
            : "not found",
        howToUseRootPath: top.document.getElementById('howToUseRootPath') != null
            ? top.document.getElementById('howToUseRootPath').value
            : "not found",
        exTotalBetsForecastRootPath: top.document.getElementById('exTotalBetsForecastRootPath') != null
            ? top.document.getElementById('exTotalBetsForecastRootPath').value
            : "not found",
        messageRootPath: top.document.getElementById('messageRootPath') != null
            ? top.document.getElementById('messageRootPath').value
            : "not found",
        resultRootPath: top.document.getElementById('resultRootPath') != null
            ? top.document.getElementById('resultRootPath').value
            : "/site-results/",
        transferRootPath: top.document.getElementById('transferRootPath') != null
            ? top.document.getElementById('transferRootPath').value
            : "/site-transfer/"
    },
    user: {
        onUserId: document.getElementById('onUserId') != null
            ? document.getElementById('onUserId').value
            : "not found",
        passwordHash: document.getElementById('passwordHash') != null
            ? document.getElementById('passwordHash').value
            : "not found",
        subPageAccess: document.getElementById('subPageAccess') != null
            ? document.getElementById('subPageAccess').value
            : "not found",
        level: document.getElementById('userLevel') != null
            ? document.getElementById('userLevel').value
            : "not found",
        isSubAccount: document.getElementById('isSubAccount') != null
            ? (document.getElementById('isSubAccount').value.toLowerCase() === "true")
            : "not found",
        isShowIntroduction: document.getElementById('isShowIntroduction') != null
            ? (document.getElementById('isShowIntroduction').value.toLowerCase() === "true")
            : "not found",
        isEnableOtp: document.getElementById('isEnabledOtp') != null
            ? (document.getElementById('isEnabledOtp').value.toLowerCase() === "true")
            : false
    },
    core: {
        convertReponseToJson: function (reponse) {
            return eval("(" + reponse + ")");
        },
        signOut: function (errMsg) {
            if (errMsg === void 0) { errMsg = null; }
            errMsg = errMsg || $('#SessionExpired').val();
            var index = window;
            while (index != index.parent) {
                index = index.parent;
            }
            var isInternal = index.$("#isInternal").val();
            isInternal = isInternal || "0";
            var signOutUrl = alpha.urls.oldMainRootPath + '_Authorization/Handlers/SignOut.ashx?r=' + errMsg;
            if (isInternal && isInternal.toString() !== "0") {
                $.ajax(signOutUrl, {
                    success: function () {
                        index.close();
                    }
                });
            }
            else {
                try {
                    localStorage.removeItem(indexManager.currentUrlCookieName);
                }
                finally {
                    index.location = signOutUrl;
                }
            }
            sessionStorage.clear();
        },
        signOut2: function () {
            var index = window;
            while (index != index.parent) {
                index = index.parent;
            }
            var isInternal = index.$("#isInternal").val();
            isInternal = isInternal || "0";
            var signOutUrl = alpha.urls.oldMainRootPath + '_Authorization/Handlers/SignOut2.ashx';
            if (isInternal && isInternal.toString() !== "0") {
                $.ajax(signOutUrl, {
                    success: function () {
                        index.close();
                    }
                });
            }
            else {
                try {
                    indexManager.removeCookies();
                }
                finally {
                    index.location = signOutUrl;
                }
            }
            sessionStorage.clear();
        },
        isIe: function () {
            return /(MSIE|Trident\/|Edge\/)/i.test(navigator.userAgent);
        },
        IsIe8OrLower: function () {
            var userAgent = window.navigator.userAgent.toLowerCase();
            return (userAgent.indexOf('msie') != -1)
                ? (parseInt(userAgent.split('msie')[1]) < 9)
                : false;
        },
        IsAndroid: function () {
            return /Android/i.test(navigator.userAgent);
        }
    },
    isScreenGreater1024: false,
    isScreenLower768: false,
    isScreenGreater768AndLower1024: false,
    isScreenLower1024: false,
    isMobile: false,
    detectResponsiveInformation: function () {
        if (alpha.core.isIe()) {
            //// Work arround because in IE, bootstrap modal dont delegate focusin event to others.
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
        }
        var currentWindowWidth = window.screen.width;
        if (currentWindowWidth < 768) {
            alpha.isScreenLower768 = true;
        }
        else if (currentWindowWidth <= 1024) {
            alpha.isScreenGreater768AndLower1024 = true;
        }
        else {
            alpha.isScreenGreater1024 = true;
        }
        alpha.isMobile = isMobile.any;
    },
    localStorage: {
        get: function (key) {
            return store.enabled ? store.get(key) : $.cookie(key);
        },
        set: function (key, value) {
            store.enabled ? store.set(key, value) : $.cookie(key, value, alpha.localStorage.getCookieExpireOptions());
        },
        check: function (key) {
            return typeof store.get(key) !== 'undefined' || typeof $.cookie(key) !== 'undefined';
        },
        getCookieExpireOptions: function () {
            var cookieExpire = { expires: 365, path: window.location.pathname };
            if (alpha.core.isIe()) {
                cookieExpire = { expires: 365 };
            }
            return cookieExpire;
        },
    },
    showMessage: function ($target, result) {
        $target.empty();
        if (result.isSuccessful) {
            $target.html("<div class='msg msg-success'>" + result.message + "</div>");
            return;
        }
        if (result.isWarning) {
            $target.html("<div class='msg msg-warning'>" + result.message + "</div>");
            return;
        }
        if (result.isFailed) {
            $target.html("<div class='msg msg-error'>" + result.message + "</div>");
            return;
        }
    },
    key: {
        instruction: 'Instruction',
        mobileInstruction: 'MobileInstruction'
    },
    messageType: {
        Normal: 0,
        Personal: 1,
        Special: 2,
        System: 3
    }
};
$(function () {
    $.ajaxSetup({
        beforeSend: function (xhr) {
            var antiXSRFToken = document.getElementsByName("__RequestVerificationToken");
            if (antiXSRFToken.length > 0) {
                xhr.setRequestHeader("__RequestVerificationToken", antiXSRFToken[0].value);
            }
        },
        error: function (xhr) {
            if (xhr.status === 401
                || xhr.status === 403) {
                alpha.core.signOut();
            }
        }
    });
});
var Result = /** @class */ (function () {
    function Result(data) {
        this.code = data.Code;
        this.message = data.Message || "";
        this.isFailed = this.code !== 0;
        this.isSuccessful = this.code === 0;
        this.isWarning = data.Code === Result.warningCode;
    }
    Result.warningCode = -1000;
    return Result;
}());
//# sourceMappingURL=core.js.map
var BaseTelegramChatBot = /** @class */ (function () {
    function BaseTelegramChatBot($, topWindow) {
        var _this = this;
        this.onComplete = function () {
            _this.$.unblockUI();
        };
        this.safelyParseJSON = function (json) {
            var parsed;
            try {
                parsed = JSON.parse(json);
            }
            catch (e) { }
            return parsed;
        };
        this.convertAttributeToBool = function (attrValue) {
            return !!attrValue && attrValue.toString().toLowerCase() === "true";
        };
        this.removeBookmarksInUrl = function (url) {
            if (typeof url !== 'string') {
                return null;
            }
            var i = url.indexOf("#");
            if (i === -1) {
                return url;
            }
            return url.substr(0, i);
        };
        this.reloadPage = function () {
            parent.window.location.href = _this.removeBookmarksInUrl(parent.window.location.href);
        };
        this.$ = $;
        this.topWindow = topWindow;
    }
    return BaseTelegramChatBot;
}());
//# sourceMappingURL=base-telegram-chatbot.js.map
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