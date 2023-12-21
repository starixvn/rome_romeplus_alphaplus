/// <reference path="../core.ts" />
'use strict';
var CreateApiKey = /** @class */ (function () {
    function CreateApiKey($, topWindow) {
        var _this = this;
        this.init = function () {
            _this.cachedDOMElements();
            _this.renderDatetimePicker();
            _this.registerEvents();
        };
        this.resources = {
            apiKey: document.getElementById('APIKey').value,
            pleaseInputAPIKeyName: document.getElementById('PleaseInputAPIKeyName').value,
            apiKeyNameCanNotContainSpecialCharacters: document.getElementById('APIKeyNameCanNotContainSpecialCharacters').value,
            noMoreThan50Characters: document.getElementById('NoMoreThan50Characters').value,
            pleaseInputPassPhrase: document.getElementById('PleaseInputPassPhrase').value,
            APIKey_PassphraseValidate: document.getElementById('APIKey_PassphraseValidate').value,
            apiPassPhraseMustBeBetweenCharacters: document.getElementById('APIPassPhraseMustBeBetweenCharacters').value,
            cantSelectTimeFromPast: document.getElementById('CantSelectTimeFromPast').value,
            scopesIsRequired: document.getElementById('ScopesIsRequired').value,
            currentDate: document.getElementById('CurrentDate').value,
        };
        this.cachedDOMElements = function () {
            _this.$form = _this.$("#apikey-form");
            _this.$datePicker = _this.$("#dpkContainer");
            _this.$ckbAll = _this.$("#ckbAll");
            _this.$showPass = _this.$(".show-pass");
            _this.$errorMessage = _this.$("#error-message");
            _this.$showURL = _this.$(".dummy-text");
            _this.$urlMask = _this.$(".service-url-mask");
        };
        this.registerEvents = function () {
            _this.$ckbAll.click(_this.handleSelectAllScopes);
            _this.$("input[name='scope']").click(_this.handleSelectScopes);
            _this.$showPass.click(_this.showPass);
            _this.$showURL.click(_this.showURL);
            _this.handleSubmitForm();
        };
        this.showPass = function () {
            var inputPass = _this.$("#passphrase");
            _this.changeEyeIcon(_this.$showPass);
            if (inputPass.hasClass("input-pass")) {
                inputPass.removeClass("input-pass");
            }
            else {
                inputPass.addClass("input-pass");
            }
        };
        this.showURL = function () {
            _this.$urlMask.removeClass('close');
            _this.$urlMask.addClass('open');
        };
        this.changeEyeIcon = function (eyeElm) {
            if (eyeElm.hasClass("icon-eye-close")) {
                eyeElm.removeClass("icon-eye-close");
                eyeElm.addClass("icon-eye");
            }
            else {
                eyeElm.removeClass("icon-eye");
                eyeElm.addClass("icon-eye-close");
            }
        };
        this.renderDatetimePicker = function () {
            var minDate = new Date(_this.resources.currentDate);
            var options = {
                minDate: new Date(minDate.getFullYear(), minDate.getMonth(), minDate.getDate()),
                useCurrent: false,
                allowInputToggle: true,
                format: "MM/DD/YYYY",
                defaultDate: null
            };
            _this.$datePicker.datepicker(options);
        };
        this.handleSubmitForm = function () {
            var self = _this;
            _this.$form.submit(function (e) {
                self.handleCreateApiKey();
                e.preventDefault();
            });
        };
        this.handleCreateApiKey = function () {
            var apiName = _this.$("#apiname").val();
            var passPhrase = _this.$("#passphrase").val();
            var expirationDate = _this.$("#expirationdate").val();
            var scopes = _this.getScopeValue();
            var error = _this.validateFormInput(apiName, passPhrase, expirationDate, scopes);
            if (error) {
                alpha.showMessage(_this.$errorMessage, new Result({ Code: 1, Message: error }));
            }
            else {
                var self_1 = _this;
                _this.$.ajax({
                    type: 'POST',
                    url: 'CreateApiKey',
                    data: { apiName: apiName, passPhrase: passPhrase, expirationDate: expirationDate, scopes: scopes },
                    dataType: 'json',
                    success: function (data, textStatus, jqXHR) {
                        if (data.ApiKeyResult) {
                            self_1.resetForm();
                            var url = alpha.urls.mainRootPath + "ApiKey/CreateApiKeyResult?apiname=" + encodeURIComponent(data.ApiKeyResult.ApiName)
                                + "&key=" + encodeURIComponent(data.ApiKeyResult.Key)
                                + "&secret=" + encodeURIComponent(data.ApiKeyResult.ApiSecret);
                            self_1.topWindow.popupManager.open(url, self_1.resources.apiKey, 380, 550, null, null, null, null, null, null, { customClass: 'api-modal' });
                        }
                        else {
                            alpha.showMessage(self_1.$errorMessage, new Result(data.Result));
                        }
                    },
                    complete: function (jqXHR, textStatus) {
                    }
                });
            }
        };
        this.resetForm = function () {
            _this.$("#apiname").val("");
            _this.$("#passphrase").val("");
            _this.$("#expirationdate").val("");
            _this.$("#error-message").empty();
        };
        this.validateFormInput = function (apiName, passPhrase, expirationDate, scopes) {
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
            if (passPhrase == '') {
                return _this.resources.pleaseInputPassPhrase;
            }
            var regexPassPhrase = /^[\w!"#$%&'()*+,-./:;<=>?@[\]^_\`{|}~]+$/m;
            if (!regexPassPhrase.test(passPhrase)) {
                return _this.resources.APIKey_PassphraseValidate;
            }
            if (passPhrase.length < 7 || passPhrase.length > 32) {
                return _this.resources.apiPassPhraseMustBeBetweenCharacters;
            }
            var currentDate = new Date(_this.resources.currentDate);
            var isPastDate = new Date(expirationDate.toString()) < new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate());
            if (isPastDate) {
                return _this.resources.cantSelectTimeFromPast;
            }
            if (scopes == "") {
                return _this.resources.scopesIsRequired;
            }
            return null;
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
        this.$ = $;
        this.topWindow = topWindow;
    }
    return CreateApiKey;
}());
$(function () {
    var createApiKey = new CreateApiKey($, window.top);
    createApiKey.init();
});
//# sourceMappingURL=create-apikey.js.map