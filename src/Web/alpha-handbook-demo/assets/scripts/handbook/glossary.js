var GlossaryPage = /** @class */ (function () {
    function GlossaryPage($, topWindow) {
        var _this = this;
        this.init = function () {
            _this.registerEvents();
        };
        this.registerEvents = function () {
            var self = _this;
            _this.$(".list-glossary > a").click(function () {
                var s = self.$(this).attr('data-val');
                self.viewGlossary(s);
            });
        };
        this.viewGlossary = function (s) {
            var url = _this.$("#RootPath").val() + 'HandbookGlossary/Index?group=' + encodeURIComponent(s);
            if (_this.topWindow.setUrl) {
                _this.topWindow.setUrl(url);
                return;
            }
            location.href = url;
        };
        this.$ = $;
        this.topWindow = topWindow;
    }
    return GlossaryPage;
}());
;
$(function () {
    var page = new GlossaryPage($, window.top);
    page.init();
});
//# sourceMappingURL=glossary.js.map