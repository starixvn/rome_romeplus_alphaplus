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