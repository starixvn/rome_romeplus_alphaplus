/// <reference path="../core.ts" />
"use strict";
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
            pleaseInputAPIKeyName: (document.getElementById("PleaseInputAPIKeyName")).value,
            apiKeyNameCanNotContainSpecialCharacters: (document.getElementById("APIKeyNameCanNotContainSpecialCharacters")).value,
            noMoreThan50Characters: (document.getElementById("NoMoreThan50Characters")).value,
            cantSelectTimeFromPast: (document.getElementById("CantSelectTimeFromPast")).value,
            scopesIsRequired: (document.getElementById("ScopesIsRequired")).value,
            updateSuccessful: (document.getElementById("UpdateSuccessful")).value,
            currentDate: document.getElementById("CurrentDate")
                .value,
            oldExpirationDate: (document.getElementById("ExpirationDate")).value,
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
            if (window.document.queryCommandSupported &&
                window.document.queryCommandSupported("copy")) {
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
            if (apiName == _this.apiKeyName &&
                expirationDate == _this.apiKeyExpirationDate &&
                scopes == _this.apiKeyScopes) {
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
                    type: "POST",
                    url: alpha.urls.mainRootPath + "ApiKey/UpdateApiKey",
                    data: { apikey: apikey, apiName: apiName, expirationDate: expirationDate, scopes: scopes },
                    dataType: "json",
                    success: function (data, textStatus, jqXHR) {
                        if (data.IsFailed) {
                            alpha.showMessage(self_1.$errorMessage, new Result(data));
                        }
                        else {
                            alpha.showMessage(self_1.$errorMessage, new Result({ Code: 0, Message: self_1.resources.updateSuccessful }));
                            parent.window.location.href = self_1.removeBookmarksInUrl(parent.window.location.href);
                        }
                    },
                    complete: function (jqXHR, textStatus) { },
                });
            }
        };
        this.validateFormInput = function (apiName, expirationDate, scopes) {
            var regexName = /^[a-zA-Z0-9\s]+$/m;
            if (apiName == "") {
                return _this.resources.pleaseInputAPIKeyName;
            }
            if (!regexName.test(apiName)) {
                return _this.resources.apiKeyNameCanNotContainSpecialCharacters;
            }
            if (apiName.length > 50) {
                return _this.resources.noMoreThan50Characters;
            }
            var isChangeDate = _this.getTimeofDate(new Date(expirationDate.toString())) !==
                _this.getTimeofDate(new Date(_this.resources.oldExpirationDate));
            var isPastDate = _this.getTimeofDate(new Date(expirationDate.toString())) <
                _this.getTimeofDate(new Date(_this.resources.currentDate));
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
                    self.$(checkbox).prop("checked", true).attr("checked", "checked");
                }
                else {
                    self.$(checkbox).prop("checked", false).removeAttr("checked");
                }
            });
        };
        this.handleSelectScopes = function () {
            if (_this.isSelectAllScopes()) {
                _this.$ckbAll.prop("checked", true).attr("checked", "checked");
                return;
            }
            _this.$ckbAll.prop("checked", false).removeAttr("checked");
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
                defaultDate: null,
            };
            _this.$datePicker.datepicker(options);
        };
        this.removeBookmarksInUrl = function (url) {
            if (typeof url !== "string") {
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
    // TODO:
    // const editApiKey = new EditApiKey($, window.top, new Clipboard('.icon-content-copy'));
    // editApiKey.init();
});
//# sourceMappingURL=edit-apikey.js.map