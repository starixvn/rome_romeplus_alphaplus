/// <reference path="auto-suggestion.ts" />

class HomePage {
    $: JQueryStatic;
    autoSuggestion: AutoSuggestion;

    enterKeyCode = 13;

    constructor($: JQueryStatic, autoSuggestion: AutoSuggestion) {
        this.$ = $;
        this.autoSuggestion = autoSuggestion;
    }

    init = () => {
        this.registerEvents();

        this.autoSuggestion.init({
            elmSearch: this.$('#txt-search'),
            boxSuggestion: this.$('.box-suggestion'),
            suggestUrl: this.$("#RootPath").val() + "HandbookArticle/Suggest",
            selectedSuggestion: this.selectedSuggestion
        });
    };

    registerEvents = () => {
        this.$('.btn-search').click(() => this.searchArticle());
        this.$('#txt-search').keyup((ev) => {
            var keycode = (ev.keyCode ? ev.keyCode : ev.which);
            if (keycode === this.enterKeyCode) {
                this.searchArticle();
                return;
            }

            this.autoSuggestion.suggest();
        });
    };

    searchArticle = () => {
        var keyword = this.$('#txt-search').val().toString();
        if (keyword.length <= 2) {
            return;
        }

        location.href = this.$("#RootPath").val() + "HandbookArticle/Search?keyword=" + keyword;
    };

    selectedSuggestion = (id) => {
        var url = this.$("#RootPath").val() + "HandbookArticle/GetBySlugSuggest";
        this.$.ajax({
            type: "POST",
            url: url,
            data: { slug: id },
            success: (result) => {
                var article = result.Article;
                if (article.TargetFrame == 'main') {
                    location.href = this.$("#RootPath").val() + "HandbookArticle/GetBySlug?slug=" + id;
                } else {
                    window.open(article.Url);
                }
            },
            complete: () => {
            }
        });
    };
};

$(function () {
    const autoSuggestion = new AutoSuggestion($);

    const page = new HomePage($, autoSuggestion);
    page.init();
});
