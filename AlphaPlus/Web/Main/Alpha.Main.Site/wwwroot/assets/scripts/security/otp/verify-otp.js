/// <reference path="../../core.ts" />
/// <reference path="../security-helper.ts" />
var VerifyOtp = /** @class */ (function () {
    function VerifyOtp($, topWindow) {
        var _this = this;
        this.$ = $;
        this.lockedAccountCode = 200;
        this.resources = {
            requiredFieldMessage: document.getElementById('requiredFieldMessage').value
        };
        this.init = function () {
            _this.cachedElements();
            _this.registerEvents();
            var actorProfilePicture = JSON.parse(localStorage.getItem('actorProfilePicture'));
            _this.$('.profile-avatar').attr("src", actorProfilePicture.Image);
        };
        this.cachedElements = function () {
            _this.$otpForm = _this.$("#otp-form");
            _this.$txtOtp = _this.$("#txtOTP").val("");
            _this.$btnSubmit = _this.$("#btnSubmit");
            _this.$otpMessage = _this.$("#otp-message");
            _this.$btnGenerateOTP = _this.$("#btnGenerateOTP");
            _this.$countDown = _this.$("#countdown");
        };
        this.registerEvents = function () {
            _this.$otpForm.submit(_this.verifyOtpEvent);
            if (top.hasPopupOpen()) {
                top.setSecurityCodePopupTitle(_this.$("#title_header").text());
                _this.$otpForm.addClass("has-popup");
                _this.$("#title_header").remove();
            }
            _this.$btnGenerateOTP.click(_this.generateOTP);
        };
        this.verifyOtpEvent = function (e) {
            e.preventDefault();
            var otp = _this.$txtOtp.val();
            if (otp.length === 0) {
                _this.$(_this.$otpMessage).removeClass('hidden');
                alpha.showMessage(_this.$otpMessage, new Result({ Code: 1, Message: _this.resources.requiredFieldMessage }));
                _this.$txtOtp.focus();
                return;
            }
            _this.$.blockUI();
            _this.$.ajax({
                type: "POST",
                url: alpha.urls.mainRootPath + "Otp/VerifyOtp",
                data: { otp: otp },
                dataType: "json",
                success: _this.onVerifiedSuccess,
                complete: _this.onComplete
            });
        };
        this.onVerifiedSuccess = function (data) {
            var result = new Result(data);
            if (result.isSuccessful) {
                setTimeout(function () {
                    securityHelper.afterVerifed();
                    _this.$txtOtp.attr("disabled", "disabled");
                    _this.$btnGenerateOTP.attr("disabled", "disabled").remove();
                    _this.$btnSubmit.attr("disabled", "disabled").remove();
                }, 3000);
            }
            else {
                _this.$(_this.$otpMessage).removeClass('hidden');
                alpha.showMessage(_this.$otpMessage, result);
                _this.$txtOtp.focus();
                if (result.code === _this.lockedAccountCode) {
                    alert(result.message);
                    alpha.core.signOut();
                }
            }
        };
        this.onComplete = function () {
            _this.$.unblockUI();
        };
        this.generateOTP = function (e) {
            e.preventDefault();
            _this.$.blockUI();
            _this.$.ajax({
                type: "POST",
                url: alpha.urls.mainRootPath + "Otp/GenerateOTP",
                success: _this.onGenerateOTPSuccess,
                complete: _this.onComplete
            });
        };
        this.onGenerateOTPSuccess = function (data) {
            var result = new Result(data);
            if (result.isSuccessful) {
                _this.$txtOtp.focus();
                var countDownTime = new Date().getTime() + 180000;
                _this.countDown(countDownTime);
            }
            else {
                _this.$(_this.$otpMessage).removeClass('hidden');
                alpha.showMessage(_this.$otpMessage, result);
                if (result.code === _this.lockedAccountCode) {
                    alert(result.message);
                    alpha.core.signOut();
                }
            }
        };
        this.countDown = function (countDownTime) {
            _this.$btnGenerateOTP.attr("disabled", "disabled");
            var x = setInterval(function () {
                var now = new Date().getTime();
                var distance = countDownTime - now;
                if (distance < 0) {
                    clearInterval(x);
                    _this.$countDown.text("");
                    _this.$btnGenerateOTP.removeAttr("disabled", "disabled");
                }
                else {
                    var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
                    var seconds = Math.floor((distance % (1000 * 60)) / 1000);
                    _this.$countDown.text("(" + minutes + ":" + seconds + ")");
                }
            }, 1000);
        };
        this.topWindow = topWindow;
    }
    return VerifyOtp;
}());
var verifyOtp = new VerifyOtp($, window.top);
$(function () {
    verifyOtp.init();
});
//# sourceMappingURL=verify-otp.js.map