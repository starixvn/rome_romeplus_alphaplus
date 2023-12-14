var ToastMessage = /** @class */ (function () {
    function ToastMessage($, topWindow) {
        this.getMessagesUrl = alpha.urls.messageRootPath + "ToastMessage/Get";
        this.markreadMessagesUrl = alpha.urls.messageRootPath + "ToastMessage/MarkRead";
        this.getMessagesTimeout = 1000 * 60 * 5; // 5 minute
        this.$ = $;
        this.topWindow = topWindow;
    }
    ToastMessage.prototype.init = function () {
        var _this = this;
        this.$minAgo = this.$("#minAgo").val();
        this.$minsAgo = this.$("#minsAgo").val();
        this.$hoursAgo = this.$("#hoursAgo").val();
        this.$daysAgo = this.$("#daysAgo").val();
        this.$here = this.$("#Here").val().toUpperCase();
        this.monitorURL = alpha.urls.transferRootPath + "AutoTransfer/Monitoring?Status=Failed";
        this.showingToasts = {};
        this.toastId = 0;
        this.getMessages();
        setInterval(function () { _this.updateMinsDiff(); }, 10000);
    };
    ToastMessage.prototype.getMessages = function () {
        var self = this;
        this.$.get(this.getMessagesUrl, { LastToastId: this.toastId }, function (data) {
            self.renderMessages(data);
        }).always(function () {
            setTimeout(function () { self.getMessages(); }, self.getMessagesTimeout);
        });
    };
    ToastMessage.prototype.renderMessages = function (data) {
        if (!data || data.length === 0) {
            return;
        }
        var message = data[0];
        this.toastId = message.ToastId;
        this.renderMessage(message);
    };
    ToastMessage.prototype.renderMessage = function (message) {
        var _this = this;
        if (this.showingToasts[message.ToastId]) {
            return;
        }
        Object.keys(this.showingToasts).forEach(function (k) {
            var t = _this.showingToasts[k];
            toastr.clear(t);
        });
        var self = this;
        var optionsOverride = {
            tapToDismiss: false,
            closeOnHover: false,
            timeOut: 0,
            positionClass: 'toast-bottom-right',
            closeButton: true,
            onCloseClick: function () { self.closeMessage(this, message.ToastId); }
        };
        var title = '<span>' + message.Title + '</span> <span class="mins-diff" data-org-time="' + message.CreatedTimeStr + '">' + this.getMinDiffs(message.CreatedTimeStr) + '</span>';
        var content = this.formatContent(message);
        var t1;
        switch (message.TypeId) {
            case 3:
                t1 = toastr.warning(content, title, optionsOverride);
                break;
            case 4:
                t1 = toastr.error(content, title, optionsOverride);
                break;
        }
        var displayInfo = this.getDisplayInfo();
        t1.css(displayInfo);
        this.showingToasts[message.ToastId] = t1;
    };
    ToastMessage.prototype.closeMessage = function (elm, id) {
        this.showingToasts[id] = null;
        var self = this;
        this.$.post(this.markreadMessagesUrl, { id: id }, function (data) {
            if (data.ErrCode) {
                self.topWindow.bootboxAlert(data.ErrMsg);
            }
        }).always(function () {
        });
    };
    ToastMessage.prototype.formatContent = function (message) {
        var type = message.SiteFunctionId;
        var content = message.Body;
        var format = this['format_' + type];
        return format ? format.call(this, content) : content;
    };
    ToastMessage.prototype.format_1 = function (content) {
        return content.replace("{0}", "<a target='main' href='" + this.monitorURL + "'>" + this.$here + "</a>");
    };
    ToastMessage.prototype.getDisplayInfo = function () {
        var feedback = this.$("#feedback-function");
        if (feedback.length && feedback.hasClass('hide')) {
            return {
                bottom: 0,
                width: 350
            };
        }
        return {
            bottom: feedback.height(),
        };
    };
    ToastMessage.prototype.updatePosition = function () {
        var _this = this;
        var displayInfo = this.getDisplayInfo();
        Object.keys(this.showingToasts).forEach(function (k) {
            var t = _this.showingToasts[k];
            t.css(displayInfo);
        });
    };
    ToastMessage.prototype.updateMinsDiff = function () {
        var _this = this;
        var $minsDiff = this.$(".mins-diff");
        $minsDiff.each(function (_, e) {
            var $e = _this.$(e);
            var createdTime = $e.attr("data-org-time");
            var txtDiff = _this.getMinDiffs(createdTime);
            if (txtDiff !== $e.html()) {
                $e.html(txtDiff);
            }
        });
    };
    ToastMessage.prototype.getMinDiffs = function (createdTime) {
        var now = new Date();
        var currentTime = now.getTime();
        var orgTime = new Date(createdTime).getTime();
        var diffs = Math.floor((currentTime - orgTime) / 60000 - 4 * 60 + now.getTimezoneOffset());
        var txtDiff = this.getAgoText(diffs);
        return txtDiff;
    };
    ToastMessage.prototype.getAgoText = function (minDiffs) {
        if (minDiffs <= 1) {
            return this.$minAgo;
        }
        if (minDiffs < 60) {
            return minDiffs + " " + this.$minsAgo;
        }
        if (minDiffs < 24 * 60) {
            return Math.floor(minDiffs / 60) + " " + this.$hoursAgo;
        }
        return Math.floor(minDiffs / 60 / 24) + " " + this.$daysAgo;
    };
    return ToastMessage;
}());
var toastMessage = new ToastMessage($, window.top);
$(function () {
    toastMessage.init();
});
//# sourceMappingURL=toast-message.js.map