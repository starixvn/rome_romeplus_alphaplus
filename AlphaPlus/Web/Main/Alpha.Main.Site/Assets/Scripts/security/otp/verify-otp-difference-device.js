/// <reference path="../../core.ts" />
/// <reference path="../security-helper.ts" />
var VerifyOtpDefferenceDevice = /** @class */ (function () {
    function VerifyOtpDefferenceDevice($) {
        var _this = this;
        this.$ = $;
        this.lockedAccountCode = 200;
        this.resources = {
            requiredFieldMessage: document.getElementById('requiredFieldMessage').value
        };
        this.init = function () {
            securityHelper.initLanguage();
            _this.cachedElements();
            _this.registerEvents();
            var actorProfilePicture = JSON.parse(localStorage.getItem('actorProfilePicture'));
            _this.$('.profile-avatar').attr("src", actorProfilePicture.Image);
        };
        this.cachedElements = function () {
            _this.$btnBack = _this.$("#btn-back");
            _this.$otpForm = _this.$("#otp-form");
            _this.$txtOtp = _this.$("#txtOTP");
            _this.$btnGenerateOTP = _this.$("#btnGenerateOTP");
            _this.$countDown = _this.$("#countdown");
        };
        this.registerEvents = function () {
            _this.$btnBack.click(function () { return alpha.core.signOut2(); });
            _this.$otpForm.submit(_this.verifyOtpEvent);
            _this.$btnGenerateOTP.click(_this.generateOTP);
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
                _this.$txtOtp.addClass("error").next().text(result.message);
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
        this.verifyOtpEvent = function (e) {
            e.preventDefault();
            var otp = _this.$txtOtp.val();
            if (otp.length === 0) {
                _this.$txtOtp.addClass("error").next().text(_this.resources.requiredFieldMessage);
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
                securityHelper.gotoHomePage();
            }
            else {
                _this.$txtOtp.addClass("error").next().text(result.message);
                if (result.code === _this.lockedAccountCode) {
                    alert(result.message);
                    alpha.core.signOut();
                }
            }
        };
        this.onComplete = function () {
            _this.$.unblockUI();
        };
    }
    return VerifyOtpDefferenceDevice;
}());
var verifyOtpDefferenceDevice = new VerifyOtpDefferenceDevice($);
$(function () {
    verifyOtpDefferenceDevice.init();
});
//# sourceMappingURL=verify-otp-difference-device.js.map