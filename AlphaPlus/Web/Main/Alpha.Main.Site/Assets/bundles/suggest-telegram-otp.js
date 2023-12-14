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