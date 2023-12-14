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
var VerifyAccountTelegramChatbot = /** @class */ (function (_super) {
    __extends(VerifyAccountTelegramChatbot, _super);
    function VerifyAccountTelegramChatbot() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.NOT_START_ERROR_CODE = 102;
        _this.resources = {
            invalidOTP: document.getElementById('InvalidOTP').value,
            otpIsRequired: document.getElementById('OTPIsRequired').value,
            captchaIsRequired: document.getElementById('CaptchaIsRequired').value,
            resendOTPSuccessfully: document.getElementById('ResendOTPSuccessfully').value,
            startBeforeInputOTP: document.getElementById("StartBeforeInputOTP").value
        };
        _this.init = function () {
            _this.isShowCaptcha = _this.convertAttributeToBool(_this.$("#IsShowCaptcha").val());
            _this.cachedDOMElements();
            _this.registerEvents();
            if (_this.isShowCaptcha) {
                _this.cachedCaptchaDOMElements();
                _this.registerCaptchaEvents();
                _this.$captcha.show();
            }
        };
        _this.cachedDOMElements = function () {
            _this.$verificationSection = _this.$("#verification-section");
            _this.$verificationForm = _this.$("#verification-form");
            _this.$txtOTP = _this.$("#txtOTP");
            _this.$verificationMessage = _this.$("#verification-code-message");
            _this.$captchaContainer = _this.$("#captcha-container");
            _this.$btnVerify = _this.$("#btnVerify");
            _this.$btnResend = _this.$("#btnResend");
            _this.$msgExpired = _this.$("#msgExpired");
        };
        _this.registerEvents = function () {
            _this.$btnVerify.click(_this.verifyOTPEvent);
            _this.$btnResend.click(_this.resendEvent);
            _this.$txtOTP.keypress(function (event) {
                if (event.keyCode === 13 && !_this.$btnVerify.is(":disabled")) {
                    _this.$btnVerify.click();
                }
            });
        };
        _this.initCaptcha = function () {
            _this.cachedCaptchaDOMElements();
            _this.registerCaptchaEvents();
            _this.$captcha.show();
        };
        _this.cachedCaptchaDOMElements = function () {
            var captchaId = _this.$("#CaptchaId").val();
            _this.$captcha = _this.$("#" + captchaId);
            _this.$captchaRefreshIcon = _this.$("#captcha-refresh-icon");
            _this.$txtCaptcha = _this.$("#txtCaptcha");
            _this.imageElementId = _this.$("#ModelImageElementId").val();
            _this.$modelImageElement = _this.$("#" + _this.imageElementId);
            _this.$modelTokenParameterName = _this.$("#ModelTokenParameterName");
            _this.tokenElementId = _this.$("#ModelTokenElementId").val();
            _this.$modelTokenElement = _this.$("#" + _this.tokenElementId);
            _this.$modelRefreshUrl = _this.$("#ModelRefreshUrl");
        };
        _this.registerCaptchaEvents = function () {
            _this.$captchaRefreshIcon.click(_this.refreshCaptchaEvent);
            _this.$modelImageElement.click(_this.refreshCaptchaEvent);
            _this.$txtCaptcha.keypress(function (event) {
                if (event.keyCode === 13 && !_this.$btnVerify.is(":disabled")) {
                    _this.$btnVerify.click();
                }
            });
        };
        _this.verifyOTPEvent = function (e) {
            e.preventDefault();
            var OTP = _this.$txtOTP.val();
            if (OTP.length === 0) {
                _this.$txtOTP.focus();
                alpha.showMessage(_this.$verificationMessage, new Result({ Code: 1, Message: _this.resources.otpIsRequired }));
                return;
            }
            else {
                _this.$verificationMessage.html("");
            }
            if (_this.isShowCaptcha) {
                if (_this.$txtCaptcha.val() === "") {
                    _this.$txtCaptcha.focus();
                    alpha.showMessage(_this.$verificationMessage, new Result({ Code: 1, Message: _this.resources.captchaIsRequired }));
                    return;
                }
                else {
                    _this.$verificationMessage.html("");
                }
            }
            _this.$.blockUI();
            _this.$.ajax({
                type: "POST",
                url: alpha.urls.mainRootPath + "TelegramChatBot/VerifyOTP",
                data: _this.$verificationForm.serialize(),
                dataType: "text",
                success: _this.onVerifySuccess,
                complete: _this.onComplete,
            });
        };
        _this.onVerifySuccess = function (data) {
            var result = _this.safelyParseJSON(data);
            if (result) {
                if (result.IsOk) {
                    _this.reloadPage();
                }
                else {
                    if (result.IsNotAuthorized) {
                        _this.topWindow.bootboxAlert(result.Message);
                        return;
                    }
                    _this.isShowCaptcha = _this.convertAttributeToBool(_this.$("#IsShowCaptcha").val());
                    if (_this.isShowCaptcha) {
                        _this.reloadCaptcha();
                    }
                    if (result.Code == -1) {
                        _this.$msgExpired.removeClass("hide");
                    }
                    else if (result.Code == -2 || result.Code == -3) {
                        alpha.showMessage(_this.$verificationMessage, new Result({ Code: 1, Message: _this.resources.invalidOTP }));
                    }
                    else if (result.Code == _this.NOT_START_ERROR_CODE) {
                        alpha.showMessage(_this.$verificationMessage, new Result({ Code: 1, Message: _this.resources.startBeforeInputOTP }));
                    }
                    else {
                        alpha.showMessage(_this.$verificationMessage, new Result({ Code: result.Code, Message: result.Message }));
                    }
                }
                return;
            }
            _this.$captchaContainer.html(data);
            _this.initCaptcha();
            alpha.showMessage(_this.$verificationMessage, new Result({ Code: 1, Message: _this.resources.invalidOTP }));
        };
        _this.resendEvent = function (e) {
            e.preventDefault();
            _this.$.blockUI();
            _this.$.ajax({
                type: "POST",
                url: alpha.urls.mainRootPath + "TelegramChatBot/ResendOTP",
                dataType: "json",
                success: _this.onResendSuccess,
                complete: _this.onComplete,
            });
        };
        _this.onResendSuccess = function (data) {
            if (data) {
                if (data.IsNotAuthorized) {
                    _this.topWindow.bootboxAlert(data.Message);
                    return;
                }
                if (data.Code == _this.NOT_START_ERROR_CODE) {
                    alpha.showMessage(_this.$verificationMessage, new Result({ Code: 1, Message: _this.resources.startBeforeInputOTP }));
                    _this.$msgExpired.addClass("hide");
                    return;
                }
                if (data.IsOk) {
                    alpha.showMessage(_this.$verificationMessage, new Result({ Code: 0, Message: _this.resources.resendOTPSuccessfully }));
                    _this.$msgExpired.addClass("hide");
                    _this.$txtOTP.val("");
                    _this.$txtOTP.focus();
                    setTimeout(function () {
                        _this.$verificationMessage.html("");
                    }, 3000);
                }
            }
        };
        _this.refreshCaptchaEvent = function (e) {
            var self = _this;
            var tokenParams = {};
            tokenParams[_this.$modelTokenParameterName.val()] = _this.$modelTokenElement.val();
            var url = _this.$modelRefreshUrl.val();
            _this.$.post(url, tokenParams)
                .always(function (result) {
                eval(result.responseText);
                self.$captcha.show();
            });
            return false;
        };
        _this.reloadCaptcha = function () {
            _this.$.blockUI();
            _this.$.ajax({
                type: "GET",
                url: alpha.urls.mainRootPath + "TelegramChatBot/Captcha",
                dataType: "text",
                success: function (result) {
                    _this.$captchaContainer.html(result);
                    _this.initCaptcha();
                },
                complete: _this.onComplete,
            });
        };
        return _this;
    }
    return VerifyAccountTelegramChatbot;
}(BaseTelegramChatBot));
//# sourceMappingURL=verify-account-telegram-chatbot.js.map
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