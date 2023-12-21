var BaseTelegramChatBot = /** @class */ (function () {
    function BaseTelegramChatBot($, topWindow) {
        var _this = this;
        this.onComplete = function () {
            _this.$.unblockUI();
        };
        this.safelyParseJSON = function (json) {
            var parsed;
            try {
                parsed = JSON.parse(json);
            }
            catch (e) { }
            return parsed;
        };
        this.convertAttributeToBool = function (attrValue) {
            return !!attrValue && attrValue.toString().toLowerCase() === "true";
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
        this.reloadPage = function () {
            parent.window.location.href = _this.removeBookmarksInUrl(parent.window.location.href);
        };
        this.$ = $;
        this.topWindow = topWindow;
    }
    return BaseTelegramChatBot;
}());
//# sourceMappingURL=base-telegram-chatbot.js.map