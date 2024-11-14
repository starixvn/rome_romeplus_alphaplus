class GlossaryPage {
    $: JQueryStatic;
    topWindow: any;

    constructor($: JQueryStatic, topWindow: any) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    init = () => {
        this.registerEvents();
    };
    registerEvents = () => {
        var self = this;
        this.$(".list-glossary > a").click(function () {
            var s = self.$(this).attr('data-val');
            self.viewGlossary(s);
        });
    };

    viewGlossary = (s) => {
        var url = this.$("#RootPath").val() + 'HandbookGlossary/Index?group=' + encodeURIComponent(s);

        if (this.topWindow.setUrl) {
            this.topWindow.setUrl(url);
            return;
        }

        location.href = url;
    };
};

$(function () {
    const page = new GlossaryPage($, window.top);
    page.init();
});
