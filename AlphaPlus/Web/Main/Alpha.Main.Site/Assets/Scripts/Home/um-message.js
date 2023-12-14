var UMMessage = /** @class */ (function () {
    function UMMessage($) {
        this.umMessageData = [];
        this.getUMMessagesTimeout = 1000 * 60 * 60; // 1 hour
        this.getUMMessagesUrl = alpha.urls.mainRootPath + "UMMessage/Get";
        this.doNotShowAgainKey = "UMMessageDoNotShowAgain_";
        this.userId = "";
        this.$ = $;
    }
    UMMessage.prototype.init = function () {
        this.cacheDOMElements();
        this.registerEvents();
        this.getUMMessages();
    };
    UMMessage.prototype.cacheDOMElements = function () {
        this.$umMessageBox = this.$("#um-message-box");
        this.$umMessagePopup = this.$("#um-message-popup");
        this.$umMessagePopupBody = this.$umMessagePopup.find("#um-message-popup-body");
        var subUserId = this.$("#subUserId").val();
        this.userId = subUserId === "0" ? this.$("#userId").val() : subUserId;
        var $umClientInterval = this.$("#UMAnnouncementPopupClientIntervalInMinutes");
        if ($umClientInterval.length) {
            this.getUMMessagesTimeout = 1000 * 60 * parseInt($umClientInterval.val());
        }
    };
    UMMessage.prototype.registerEvents = function () {
        var _this = this;
        this.$umMessageBox.on("click", function (_) { return _this.$umMessagePopup.modal("show"); });
        var chkDontShowAgain = this.$umMessagePopup.find("#chb-um");
        chkDontShowAgain.change(function () {
            _this.setDontShowAgainLocalStorage(chkDontShowAgain.is(":checked"));
        });
        this.$umMessagePopup.on("show.bs.modal", function () {
            chkDontShowAgain.prop("checked", _this.getDontShowAgainLocalStorage());
        });
    };
    UMMessage.prototype.getUMMessages = function () {
        var self = this;
        this.$.get(this.getUMMessagesUrl, function (data) {
            self.getUMMessagesSuccessfully(data);
        }).always(function () {
            setTimeout(function () { self.getUMMessages(); }, self.getUMMessagesTimeout);
        });
    };
    UMMessage.prototype.getUMMessagesSuccessfully = function (data) {
        if (data.length) {
            var umMessage = this.extractUMMessage(data);
            this.$umMessagePopupBody.html(umMessage.message);
            if (umMessage.hasNewMessage) {
                this.getDontShowAgainLocalStorage() || this.$umMessagePopup.modal("show");
                this.$umMessageBox.removeClass("hide");
            }
        }
        else {
            this.umMessageData.length = 0;
            this.$umMessagePopupBody.html("");
            this.$umMessagePopup.modal("hide");
            this.$umMessageBox.addClass("hide");
        }
    };
    UMMessage.prototype.extractUMMessage = function (data) {
        var idArray = [];
        var messageArray = [];
        var hasNewMessage = false;
        for (var i = 0, max = data.length; i < max; i++) {
            var datum = data[i];
            idArray.push(datum.Id);
            messageArray.push('<li class="um-message-item">- ');
            messageArray.push(datum.Message);
            messageArray.push("</li>");
            if (this.umMessageData.indexOf(datum.Id) === -1) {
                hasNewMessage = true;
            }
        }
        this.umMessageData = idArray;
        return {
            hasNewMessage: hasNewMessage,
            message: messageArray.join("")
        };
    };
    UMMessage.prototype.setDontShowAgainLocalStorage = function (value) {
        alpha.localStorage.set(this.doNotShowAgainKey + this.userId, value ? "1" : "0");
    };
    UMMessage.prototype.getDontShowAgainLocalStorage = function () {
        return alpha.localStorage.get(this.doNotShowAgainKey + this.userId) === "1";
    };
    return UMMessage;
}());
$(function () {
    new UMMessage($).init();
});
//# sourceMappingURL=um-message.js.map