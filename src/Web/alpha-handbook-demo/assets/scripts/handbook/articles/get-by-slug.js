var ArticleGetBySlug = /** @class */ (function () {
    function ArticleGetBySlug($) {
        var _this = this;
        this.init = function () {
            _this.makeFloatingContentBox();
            _this.registerZoomImageEvents();
            _this.$('.zoomed-img-container').click(function () {
                var container = _this.$('.zoomed-img-container');
                container.addClass('hidden');
                container.removeClass('show');
                container.empty();
            });
            ;
        };
        this.makeFloatingContentBox = function () {
            var headlings = _this.$(".article-content h1, .article-content h2, .article-content h3");
            if (headlings.length === 0) {
                return;
            }
            var html = '<ul class="list-content">';
            var hs = ['h1', 'h2', 'h3'];
            headlings.each(function (i, e) {
                var $e = _this.$(e);
                var t = $e.html();
                var newH = _this.$('<div>' + t + '</div>');
                newH.find('a').remove();

                var newHtml = newH.html(); 
                hs.forEach(function (h) {
                    if ($e.is(h)) {
                        html += '<li><' + h + '><a href="#' + $e.find('a').attr('id') + '" title="' + newHtml + '">' +
                            newHtml +
                            '</a></' + h + '></li>';
                        return;
                    }
                });
            });
            html += '</ul>';
            _this.$('#float-content').append(html);
        };
        this.registerZoomImageEvents = function () {
            var self = _this;
            _this.$('img').click(function (e) {
                var container = self.$('.zoomed-img-container');
                container.empty();
                var $this = self.$(this);
                var c = $this.clone();
                var w = c.width() * 2;
                var h = c.height() * 2;
                var mw = self.$('body').width();
                c.css({
                    width: 'unset',
                    height: 'unset',
                    'max-width': '100%',
                    'max-height': self.$('body').height(),
                    'margin-top': self.$(window).scrollTop()
                });
                container.css({
                    width: mw + 'px',
                    height: (self.$('.container').height() + 30) + 'px',
                });
                container.append(c);
                container.removeClass('hidden');
                setTimeout(function () {
                    container.addClass('show');
                }, 100);
                e.preventDefault();
                return false;
            });
        };
        this.$ = $;
    }
    return ArticleGetBySlug;
}());
;
$(function () {
    var page = new ArticleGetBySlug($);
    page.init();
});
//# sourceMappingURL=get-by-slug.js.map