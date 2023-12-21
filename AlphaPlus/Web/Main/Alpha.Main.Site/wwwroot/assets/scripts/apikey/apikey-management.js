/// <reference path="../core.ts" />
"use strict";
var ApiKeyManagement = /** @class */ (function () {
    function ApiKeyManagement($, topWindow, clipboard) {
        var _this = this;
        this.apiNameClick = false;
        this.init = function () {
            _this.$showURL = _this.$(".dummy-text");
            _this.$urlMask = _this.$(".service-url-mask");
            _this.$apiKeyNames = _this.$(".api-key-name");
            _this.$apiKeyPopups = _this.$(".api-key-popup");
            _this.registerApiKeyEditEvent();
            _this.registerApiKeyRevokeEvent();
        };
        this.resources = {
            editAPIKey: document.getElementById("EditAPIKey").value,
            areYouSureRevokeAPIKey: (document.getElementById("AreYouSureRevokeAPIKey")).value,
            thisActionCannotBeUndone: (document.getElementById("ThisActionCannotBeUndone")).value,
        };
        this.registerApiKeyEditEvent = function () {
            var self = _this;
            self.$(".btn-edit").click(function () {
                var apikey = self.$(this).attr("apikey");
                var url = alpha.urls.mainRootPath + ("ApiKey/EditApiKey?key=" + apikey);
                self.topWindow.popupManager.open(url, self.resources.editAPIKey, 380, 540, null, null, null, null, null, null, { customClass: "api-modal" });
            });
            _this.$showURL.click(_this.showURL);
            _this.$apiKeyNames.click(function () {
                self.showApiKey(self.$(this));
            });
            if (window.document.queryCommandSupported &&
                window.document.queryCommandSupported("copy")) {
                _this.clipboard
                    .on("success", function (e) {
                    self.$(e.trigger).tooltip("show");
                    setTimeout(function () {
                        self.$apiKeyPopups.addClass("hide");
                    }, 3000);
                })
                    .on("error", function (e) {
                    self.$(e.trigger).remove();
                    window.console.error("Action:", e.action);
                    window.console.error("Trigger:", e.trigger);
                });
            }
            _this.$(window).click(function (e) {
                if (self.apiNameClick) {
                    self.apiNameClick = false;
                    return;
                }
                var $elm = self.$(e.target);
                if ($elm.closest(".api-key-popup").length == 0) {
                    self.$apiKeyPopups.addClass("hide");
                }
            });
        };
        this.registerApiKeyRevokeEvent = function () {
            var self = _this;
            self.$(".btn-revoke").click(function () {
                var _this = this;
                var apiName = self.$(this).attr("apiname");
                var title = self.resources.areYouSureRevokeAPIKey + " <b>" + apiName + "</b>?  " + self.resources.thisActionCannotBeUndone;
                self.topWindow.bootboxConfirm(title, undefined, function () {
                    var key = self.$(_this).attr("apikey");
                    self.$.ajax({
                        type: "POST",
                        url: alpha.urls.mainRootPath + "ApiKey/RevokeApiKey",
                        data: { key: key },
                        dataType: "json",
                        success: function (data, textStatus, jqXHR) {
                            window.location.reload();
                        },
                        complete: function (jqXHR, textStatus) { },
                    });
                });
            });
        };
        this.showURL = function () {
            _this.$urlMask.removeClass("close");
            _this.$urlMask.addClass("open");
        };
        this.showApiKey = function ($elm) {
            _this.$apiKeyPopups.addClass("hide");
            var popup = $elm.parent().find(".api-key-popup");
            popup.removeClass("hide");
            popup.css({ left: $elm.position().left });
            _this.apiNameClick = true;
        };
        this.$ = $;
        this.topWindow = topWindow;
        this.clipboard = clipboard;
    }
    return ApiKeyManagement;
}());
$(function () {
    // TODO:
    // const apiKeyManagement = new ApiKeyManagement(
    //   $,
    //   window.top,
    //   new Clipboard(".icon-content-copy")
    // );
    // apiKeyManagement.init();
});
//# sourceMappingURL=apikey-management.js.map