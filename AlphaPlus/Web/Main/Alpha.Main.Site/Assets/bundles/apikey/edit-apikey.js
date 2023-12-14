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
'use strict';
var EditApiKey = /** @class */ (function () {
    function EditApiKey($, topWindow, clipboard) {
        var _this = this;
        this.init = function () {
            _this.cachedDOMElements();
            _this.renderDatetimePicker();
            _this.cacheOldApiKeyValue();
            _this.handleSelectScopes();
            _this.registerEvents();
        };
        this.resources = {
            pleaseInputAPIKeyName: document.getElementById('PleaseInputAPIKeyName').value,
            apiKeyNameCanNotContainSpecialCharacters: document.getElementById('APIKeyNameCanNotContainSpecialCharacters').value,
            noMoreThan50Characters: document.getElementById('NoMoreThan50Characters').value,
            cantSelectTimeFromPast: document.getElementById('CantSelectTimeFromPast').value,
            scopesIsRequired: document.getElementById('ScopesIsRequired').value,
            updateSuccessful: document.getElementById('UpdateSuccessful').value,
            currentDate: document.getElementById('CurrentDate').value,
            oldExpirationDate: document.getElementById('ExpirationDate').value,
        };
        this.cachedDOMElements = function () {
            _this.$datePicker = _this.$("#dpkContainer");
            _this.$datePickerInput = _this.$("#expirationdate");
            _this.$ckbAll = _this.$("#ckbAll");
            _this.$btnCancel = _this.$("#btnCancel");
            _this.$btnSubmit = _this.$("#btnSubmit");
            _this.$errorMessage = _this.$("#error-message");
        };
        this.cacheOldApiKeyValue = function () {
            _this.apiKeyName = _this.$("#apiname").val();
            _this.apiKeyExpirationDate = _this.$("#expirationdate").val();
            _this.apiKeyScopes = _this.getScopeValue();
        };
        this.registerEvents = function () {
            _this.$ckbAll.click(_this.handleSelectAllScopes);
            _this.$("input[name='scope']").click(_this.handleSelectScopes);
            _this.$btnCancel.click(_this.handleCancelForm);
            _this.$btnSubmit.click(_this.handleUpdateApiKey);
            var self = _this;
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
        };
        this.handleCancelForm = function () {
            _this.topWindow.popupManager.close();
        };
        this.handleUpdateApiKey = function (e) {
            e.preventDefault();
            var apiName = _this.$("#apiname").val();
            var apikey = _this.$("#key").val();
            var expirationDate = _this.$("#expirationdate").val();
            var scopes = _this.getScopeValue();
            if (apiName == _this.apiKeyName && expirationDate == _this.apiKeyExpirationDate && scopes == _this.apiKeyScopes) {
                parent.window.location.href = _this.removeBookmarksInUrl(parent.window.location.href);
                return;
            }
            var error = _this.validateFormInput(apiName, expirationDate, scopes);
            if (error) {
                alpha.showMessage(_this.$errorMessage, new Result({ Code: 1, Message: error }));
            }
            else {
                var self_1 = _this;
                _this.$.ajax({
                    type: 'POST',
                    url: alpha.urls.mainRootPath + "ApiKey/UpdateApiKey",
                    data: { apikey: apikey, apiName: apiName, expirationDate: expirationDate, scopes: scopes },
                    dataType: 'json',
                    success: function (data, textStatus, jqXHR) {
                        if (data.IsFailed) {
                            alpha.showMessage(self_1.$errorMessage, new Result(data));
                        }
                        else {
                            alpha.showMessage(self_1.$errorMessage, new Result({ Code: 0, Message: self_1.resources.updateSuccessful }));
                            parent.window.location.href = self_1.removeBookmarksInUrl(parent.window.location.href);
                        }
                    },
                    complete: function (jqXHR, textStatus) {
                    }
                });
            }
        };
        this.validateFormInput = function (apiName, expirationDate, scopes) {
            var regexName = /^[a-zA-Z0-9\s]+$/m;
            if (apiName == '') {
                return _this.resources.pleaseInputAPIKeyName;
            }
            if (!regexName.test(apiName)) {
                return _this.resources.apiKeyNameCanNotContainSpecialCharacters;
            }
            if (apiName.length > 50) {
                return _this.resources.noMoreThan50Characters;
            }
            var isChangeDate = _this.getTimeofDate(new Date(expirationDate.toString())) !== _this.getTimeofDate(new Date(_this.resources.oldExpirationDate));
            var isPastDate = _this.getTimeofDate(new Date(expirationDate.toString())) < _this.getTimeofDate(new Date(_this.resources.currentDate));
            if (isPastDate && isChangeDate) {
                return _this.resources.cantSelectTimeFromPast;
            }
            if (scopes == "") {
                return _this.resources.scopesIsRequired;
            }
            return null;
        };
        this.getTimeofDate = function (date) {
            return new Date(date.getFullYear(), date.getMonth(), date.getDate()).getTime();
        };
        this.getScopeValue = function () {
            var self = _this;
            var scopes = [];
            _this.$.each(_this.$("input[name='scope']:checked"), function (index, checkbox) {
                scopes.push(self.$(checkbox).val());
            });
            return scopes.join(",");
        };
        this.handleSelectAllScopes = function () {
            var self = _this;
            var isChecked = _this.$ckbAll.is(":checked");
            _this.$.each(_this.$("input[name='scope']"), function (index, checkbox) {
                if (isChecked) {
                    self.$(checkbox).prop('checked', true).attr('checked', 'checked');
                }
                else {
                    self.$(checkbox).prop('checked', false).removeAttr('checked');
                }
            });
        };
        this.handleSelectScopes = function () {
            if (_this.isSelectAllScopes()) {
                _this.$ckbAll.prop('checked', true).attr('checked', 'checked');
                return;
            }
            _this.$ckbAll.prop('checked', false).removeAttr('checked');
        };
        this.isSelectAllScopes = function () {
            var self = _this;
            var isCheckAll = true;
            _this.$.each(_this.$("input[name='scope']"), function (index, checkbox) {
                if (!self.$(checkbox).is(":checked")) {
                    isCheckAll = false;
                }
            });
            return isCheckAll;
        };
        this.renderDatetimePicker = function () {
            var options = {
                useCurrent: false,
                allowInputToggle: true,
                format: "MM/DD/YYYY",
                defaultDate: null
            };
            _this.$datePicker.datepicker(options);
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
        this.$ = $;
        this.topWindow = topWindow;
        this.clipboard = clipboard;
    }
    return EditApiKey;
}());
$(function () {
    var editApiKey = new EditApiKey($, window.top, new Clipboard('.icon-content-copy'));
    editApiKey.init();
});
//# sourceMappingURL=edit-apikey.js.map