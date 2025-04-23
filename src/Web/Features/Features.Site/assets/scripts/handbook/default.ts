/// <reference path="auto-suggestion.ts" />

declare var popupManager: any;

class DefaultPage {
    $: JQueryStatic;
    topWindow: any;
    autoSuggestion: AutoSuggestion;
    popupManager: any;

    enterKeyCode = 13;
    currentUrlCookieName = 'DFFKDHFJKLDHFJDH2';
    recheckInIntervalMiliseconds = 30000;

    constructor(
        $: JQueryStatic,
        topWindow: any,
        autoSuggestion: AutoSuggestion,
        popupManager: any) {
        this.$ = $;
        this.topWindow = topWindow;
        this.autoSuggestion = autoSuggestion;
        this.popupManager = popupManager;
    }

    init = () => {
        this.registerEvents();

        this.autoSuggestion.init({
            elmSearch: this.$('#txt-search'),
            boxSuggestion: this.$('.box-suggestion'),
            suggestUrl: this.$("#RootPath").val() + "HandbookArticle/Suggest",
            selectedSuggestion: this.selectedSuggestion
        });

        var setCurrentURL = this.$("#SetCurrentURL").val() === '1';
        if (setCurrentURL) {
            var currentUrl = this.getCurrentUrl();
            var mainSrc = currentUrl && currentUrl.indexOf("_Authorization/SignIn.aspx") === -1
                ? currentUrl : "Home";
            this.$("#ifr-main").attr("src", mainSrc);
        }

        this.initFeedbackFunction(undefined, 470);
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

        this.$(".language-container").click(this.showHideLanguage);

        var self = this;
        this.$(".dropdown-language > li a").click(function () {
            var s = self.$(this).attr('data-val');
            self.changeLanguage(s);
        });

        this.$(".item-cat").click(function () {
            var $this = self.$(this);
            if ($this.closest('li').find('>.item-cat').hasClass('open')) {
                $this.closest('li').find('>.item-cat').removeClass('open');
                return;
            }

            $this.closest('.list-item-cat').find('>.item-cat').removeClass('open');
            $this.closest('li').find('>.item-cat').addClass('open');
        });

        this.$(".item-slug").click(function () {
            self.$(".item-cat.active").removeClass('active');
            var $this = self.$(this);
            $this.parents('.item-cat').addClass('active');
        });

        this.$("#ifr-main").on('load', () => {
            self.cacheCurrentUrl();
        });

        setTimeout(() => {
            this.reCheckin();
        }, this.recheckInIntervalMiliseconds);
    };

    searchArticle = () => {
        var keyword = this.$('#txt-search').val();
        if (keyword.length <= 2) {
            return;
        }

        this.$("#ifr-main").attr("src", this.$("#RootPath").val() + "HandbookArticle/Search?keyword=" + keyword);
    };

    showHideLanguage = () => {
        var ddl = this.$(".dropdown-language");

        if (ddl.hasClass('open')) {
            ddl.removeClass('open');
        } else {
            ddl.addClass('open');
        }
    };

    changeLanguage = (s) => {
        this.$.ajax({
            type: 'GET',
            url: this.$("#RootPath").val() + "HandbookLanguage/Change?selected=" + s,
            success: (data, textStatus, jqXHR) => {
                var currentURL = this.getCurrentUrl();

                if (currentURL.indexOf('/Article/GetBySlug?') === -1) {
                    this.topWindow.location.reload(true);
                } else {
                    this.topWindow.location.href = currentURL.replace('/HandbookArticle/GetBySlug?', '/HandbookDefault/GetBySlug?');
                }
            },
            complete: function (jqXHR, textStatus) { }
        });
    };

    getCurrentUrl = () => {
        return localStorage.getItem(this.currentUrlCookieName);
    };
    cacheCurrentUrl = () => {
        try {
            var mainFrame = <HTMLIFrameElement>document.getElementById("ifr-main");
            var mainFrameDocument = mainFrame.contentWindow || mainFrame.contentDocument;
            var currentUrl = mainFrameDocument && mainFrameDocument.location.href || mainFrame.src;
            localStorage.setItem(this.currentUrlCookieName, currentUrl);
        } catch (e) { }
    };
    reCheckin = () => {
        // Post with return result in JSON format
        this.$.cookie('ssl', location.href.indexOf('https://') === 0, { expires: 30, path: '/' });
        var self = this;

        this.$.ajax({
            type: "POST",
            url: this.$("#MainRootPath").val() + 'Account/ReCheckIn',
            data: { onid: this.$("#OnUserId").val() },
            success: (result) => {
                try { 
                    var errCode = result.code;
                    var errMsg = result.message;

                    if (errCode !== 0) {
                        alert(errMsg);
                        setTimeout(() => {self.topWindow.location.reload(true); }, 30000);
                    }
                }
                catch (e) {
                }
            },
            complete: () => {
                setTimeout(() => { this.reCheckin(); }, this.recheckInIntervalMiliseconds);
            }
        });
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
                    var articleURL = this.$("#RootPath").val() + "HandbookArticle/GetBySlug?slug=" + id;
                    this.$("#ifr-main").attr("src", articleURL);
                } else {
                    window.open(article.Url);
                }
            },
            complete: () => {
            }
        });
    };

    getCustomerCookiesName = (key) => {
        var custId = this.$("#CustId").val();
        var subAccId = this.$("#SubAccId").val();
        return `${key}_${custId}_${subAccId}`;
    };

    initFeedbackFunction = (width, height) => {
        const hideFeedbackFunctionSessionKey = this.getCustomerCookiesName('HideFeedbackFunction_Handbook');
        if (sessionStorage.getItem(hideFeedbackFunctionSessionKey) === "1") {
            return;
        }

        var functionId = 8;
        var url = this.$("#MessageRootPath").val() + 'Feedback/CheckExist?id=' + functionId;
        this.$.get(url, (result) => {
            if (result.IsExist) {
                return;
            }

            var feedbackFunction = this.$("#feedback-function");
            const feedbackBox = feedbackFunction.find(".feedback-box");
            feedbackBox.find(".txt-feedback").text(result.FunctionName || "Feedback");
            feedbackBox
                .off()
                .click(() => {
                    var url = this.$("#MessageRootPath").val() + 'Feedback/Function?id=' + functionId;
                    this.popupManager.open(url, undefined, width || 440, height || 380, null, null, null, null, null, null, { customClass: "feedback-modal" });
                });

            feedbackFunction.find(".icon-close-feedback")
                .off()
                .click(function () {
                    sessionStorage.setItem(hideFeedbackFunctionSessionKey, "1");
                    feedbackFunction.addClass("hidden");
                });

            feedbackFunction.removeClass("hidden");
        });
    };
};

$(function () {
    const autoSuggestion = new AutoSuggestion($);
    const page = new DefaultPage($, window.top, autoSuggestion, popupManager);
    page.init();
});
