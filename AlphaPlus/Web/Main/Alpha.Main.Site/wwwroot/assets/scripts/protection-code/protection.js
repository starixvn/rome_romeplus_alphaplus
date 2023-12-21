"use strict";
var ProtectionCode = /** @class */ (function () {
    function ProtectionCode($) {
        this.$ = $;
    }
    ProtectionCode.prototype.init = function () {
        this.cacheElements();
        this.bindEvents();
        this.txtNewProtectionCode.focus();
    };
    ProtectionCode.prototype.cacheElements = function () {
        this.msg = $("#msg");
        this.txtNewProtectionCode = $("#txt-newProtectionCode");
        this.txtConfirmProtectionCode = $("#txt-confirmProtectionCode");
        this.resources = {
            requiredNewProtectionCode: this.$("#requiredNewProtectionCode").val(),
            minLengthNewProtectionCodeError: this.$("#minLengthNewProtectionCodeError").val(),
            requiredConfirmProtectionCode: this.$("#requiredConfirmProtectionCode").val(),
            notMatchedConfirmProtectionCode: this.$("#notMatchedConfirmProtectionCode").val(),
            containsSpaceOrUnicodeCharacterError: this.$("#containsSpaceOrUnicodeCharacterError").val()
        };
    };
    ProtectionCode.prototype.bindEvents = function () {
        var _this = this;
        this.txtNewProtectionCode.keyup(function (_) { return _this.validateProtectionCode(_this.txtNewProtectionCode); });
        this.txtConfirmProtectionCode.keyup(function (_) { return _this.validateProtectionCode(_this.txtConfirmProtectionCode); });
        var chkShowProtectionCode = $("#chk-showProtectionCode");
        chkShowProtectionCode.change(function (_) { return _this.showHideProtectionCode(chkShowProtectionCode.is(":checked")); });
        var frmProtectionCode = $("#frm-protectionCode");
        frmProtectionCode.submit(function (e) {
            e.preventDefault();
            _this.updateProtectionCode(frmProtectionCode.attr("action"));
        });
        this.$("#btn-reset").click(function (_) {
            _this.showHideProtectionCode(false);
            securityHelper.removeValidationIcon(_this.txtNewProtectionCode);
            securityHelper.removeValidationIcon(_this.txtConfirmProtectionCode);
        });
    };
    ProtectionCode.prototype.validateProtectionCode = function ($textbox) {
        this.checkValidProtectionCode($textbox.val()).isValid
            ? securityHelper.showSuccessfullyIcon($textbox)
            : securityHelper.showFailIcon($textbox);
    };
    ProtectionCode.prototype.checkValidProtectionCode = function (protectionCode) {
        if (protectionCode.length < 6) {
            return {
                isValid: false,
                error: this.resources.minLengthNewProtectionCodeError
            };
        }
        var Space = 32;
        var MaxAnsiCode = 127;
        var charCode;
        for (var i = 0, max = protectionCode.length; i < max; i++) {
            charCode = protectionCode.charCodeAt(i);
            if (charCode === Space || charCode > 127) {
                return {
                    isValid: false,
                    error: this.resources.containsSpaceOrUnicodeCharacterError
                };
            }
        }
        return {
            isValid: true,
            error: undefined
        };
    };
    ProtectionCode.prototype.showHideProtectionCode = function (isShow) {
        if (isShow) {
            this.txtNewProtectionCode.addClass("show-hint");
            this.txtConfirmProtectionCode.addClass("show-hint");
        }
        else {
            this.txtNewProtectionCode.removeClass("show-hint");
            this.txtConfirmProtectionCode.removeClass("show-hint");
        }
    };
    ProtectionCode.prototype.updateProtectionCode = function (url) {
        var _this = this;
        var formData = this.validateFormData();
        if (formData) {
            this.$.blockUI();
            formData.protectionCode = formData.protectionCode.encrypt();
            this.$.post(url, formData, function (result) {
                if (result.Code) {
                    securityHelper.showFailMessage(_this.msg, result.Message);
                    _this.txtNewProtectionCode.focus();
                }
                else {
                    securityHelper.showSuccessfullyMessage(_this.msg, result.Message);
                    _this.msg.parent().removeClass("hidden");
                    setTimeout(function () {
                        window.location.href = window.location.href;
                    }, 5000);
                }
            }).always(this.$.unblockUI);
        }
    };
    ProtectionCode.prototype.validateFormData = function () {
        var newProtectionCode = this.txtNewProtectionCode.val();
        if (!newProtectionCode.length) {
            this.txtNewProtectionCode.focus();
            securityHelper.showFailMessage(this.msg, this.resources.requiredNewProtectionCode);
            return;
        }
        var validationResult = this.checkValidProtectionCode(newProtectionCode);
        if (!validationResult.isValid) {
            this.txtNewProtectionCode.focus();
            securityHelper.showFailMessage(this.msg, validationResult.error);
            return;
        }
        var confirmProtectionCode = this.txtConfirmProtectionCode.val();
        if (!confirmProtectionCode.length) {
            this.txtConfirmProtectionCode.focus();
            securityHelper.showFailMessage(this.msg, this.resources.requiredConfirmProtectionCode);
            return;
        }
        if (confirmProtectionCode !== newProtectionCode) {
            this.txtConfirmProtectionCode.focus();
            securityHelper.showFailMessage(this.msg, this.resources.notMatchedConfirmProtectionCode);
            return;
        }
        return {
            protectionCode: newProtectionCode
        };
    };
    return ProtectionCode;
}());
$(function () {
    var protectionCode = new ProtectionCode($);
    protectionCode.init();
});
//# sourceMappingURL=protection.js.map