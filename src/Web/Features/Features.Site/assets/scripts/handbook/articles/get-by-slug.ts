
class ArticleGetBySlug {
    $: JQueryStatic;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init = () => {
        this.makeFloatingContentBox();
        this.registerZoomImageEvents();
        this.$('.zoomed-img-container').click(() => {
            var container = this.$('.zoomed-img-container');
            container.addClass('hidden');
            container.removeClass('show');
            container.empty();
        });;
    };

    makeFloatingContentBox = () => {
        var headlings = this.$(".article-content h1, .article-content h2, .article-content h3");
        if (headlings.length === 0) {
            return;
        }

        var html = '<ul class="list-content">';
        var hs = ['h1', 'h2', 'h3'];

        headlings.each((i, e) => {
            var $e = this.$(e);
            var t = $e.html();
            var newH = this.$('<div>' + t + '</div>');
            newH.find('a').remove();
            var newHtml = newH.html(); 

            hs.forEach(h => {
                if ($e.is(h)) {
                    html += '<li><' + h + '><a href="#' + $e.find('a').attr('id') + '" title="' + newHtml + '">' +
                        newHtml +
                        '</a></' + h + '></li>';
                    return;
                }
            })
        });

        html += '</ul>';

        this.$('#float-content').append(html);
    };

    registerZoomImageEvents = () => {
        var self = this;
        this.$('img').click(function (e) {
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
};

$(function () {
    const page = new ArticleGetBySlug($);
    page.init();
});
