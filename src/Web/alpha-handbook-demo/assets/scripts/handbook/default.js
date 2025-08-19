/// <reference path="auto-suggestion.ts" />
var DefaultPage = /** @class */ (function () {
    function DefaultPage($, topWindow, autoSuggestion, popupManager) {
        var _this = this;
        this.enterKeyCode = 13;
        this.currentUrlCookieName = 'DFFKDHFJKLDHFJDH2';
        this.recheckInIntervalMiliseconds = 30000;
        this.init = function () {
            _this.registerEvents();
            _this.autoSuggestion.init({
                elmSearch: _this.$('#txt-search'),
                boxSuggestion: _this.$('.box-suggestion'),
                suggestUrl: _this.$("#RootPath").val() + "HandbookArticle/Suggest",
                selectedSuggestion: _this.selectedSuggestion
            });
            var setCurrentURL = _this.$("#SetCurrentURL").val() === '1';
            if (setCurrentURL) {
                var currentUrl = _this.getCurrentUrl();
                var mainSrc = currentUrl && currentUrl.indexOf("_Authorization/SignIn.aspx") === -1
                    ? currentUrl : "Home";
                _this.$("#ifr-main").attr("src", mainSrc);
            }
            _this.initFeedbackFunction(undefined, 470);
        };
        this.registerEvents = function () {
            _this.$('.btn-search').click(function () { return _this.searchArticle(); });
            _this.$('#txt-search').keyup(function (ev) {
                var keycode = (ev.keyCode ? ev.keyCode : ev.which);
                if (keycode === _this.enterKeyCode) {
                    _this.searchArticle();
                    return;
                }
                _this.autoSuggestion.suggest();
            });
            _this.$(".language-container").click(_this.showHideLanguage);
            var self = _this;
            _this.$(".dropdown-language > li a").click(function () {
                var s = self.$(this).attr('data-val');
                self.changeLanguage(s);
            });
            _this.$(".item-cat").click(function () {
                var $this = self.$(this);
                if ($this.closest('li').find('>.item-cat').hasClass('open')) {
                    $this.closest('li').find('>.item-cat').removeClass('open');
                    return;
                }
                $this.closest('.list-item-cat').find('>.item-cat').removeClass('open');
                $this.closest('li').find('>.item-cat').addClass('open');
            });
            _this.$(".item-slug").click(function () {
                self.$(".item-cat.active").removeClass('active');
                var $this = self.$(this);
                $this.parents('.item-cat').addClass('active');
            });
            _this.$("#ifr-main").on('load', function () {
                self.cacheCurrentUrl();
            });
            setTimeout(function () {
                _this.reCheckin();
            }, _this.recheckInIntervalMiliseconds);
        };
        this.searchArticle = function () {
            var keyword = _this.$('#txt-search').val().toString();
            if (keyword.length <= 2) {
                return;
            }
            _this.$("#ifr-main").attr("src", _this.$("#RootPath").val() + "HandbookArticle/Search?keyword=" + keyword);
        };
        this.showHideLanguage = function () {
            var ddl = _this.$(".dropdown-language");
            if (ddl.hasClass('open')) {
                ddl.removeClass('open');
            }
            else {
                ddl.addClass('open');
            }
        };
        this.changeLanguage = function (s) {
            _this.$.ajax({
                type: 'GET',
                url: _this.$("#RootPath").val() + "HandbookLanguage/Change?selected=" + s,
                success: function (data, textStatus, jqXHR) {
                    var currentURL = _this.getCurrentUrl();
                    if (currentURL.indexOf('/Article/GetBySlug?') === -1) {
                        _this.topWindow.location.reload(true);
                    }
                    else {
                        _this.topWindow.location.href = currentURL.replace('/HandbookArticle/GetBySlug?', '/HandbookDefault/GetBySlug?');
                    }
                },
                complete: function (jqXHR, textStatus) { }
            });
        };
        this.getCurrentUrl = function () {
            return localStorage.getItem(_this.currentUrlCookieName);
        };
        this.cacheCurrentUrl = function () {
            try {
                var mainFrame = document.getElementById("ifr-main");
                var mainFrameDocument = mainFrame.contentWindow || mainFrame.contentDocument;
                var currentUrl = mainFrameDocument && mainFrameDocument.location.href || mainFrame.src;
                localStorage.setItem(_this.currentUrlCookieName, currentUrl);
            }
            catch (e) { }
        };
        this.reCheckin = function () {
            // Post with return result in JSON format
            _this.$.cookie('ssl', location.href.indexOf('https://') === 0, { expires: 30, path: '/' });
            var self = _this;
            _this.$.ajax({
                type: "POST",
                url: _this.$("#MainRootPath").val() + '/Account/ReCheckIn',
                data: { onid: _this.$("#OnUserId").val() },
                success: function (result) {
                    try {
                        var errCode = result.code;
                        var errMsg = result.message;
                        if (errCode !== 0) {
                            alert(errMsg);
                            setTimeout(function () { self.topWindow.location.reload(true); }, 30000);
                        }
                    }
                    catch (e) {
                    }
                },
                complete: function () {
                    setTimeout(function () { _this.reCheckin(); }, _this.recheckInIntervalMiliseconds);
                }
            });
        };
        this.selectedSuggestion = function (id) {
            var url = _this.$("#RootPath").val() + "HandbookArticle/GetBySlugSuggest";
            _this.$.ajax({
                type: "POST",
                url: url,
                data: { slug: id },
                success: function (result) {
                    var article = result.Article;
                    if (article.TargetFrame == 'main') {
                        var articleURL = _this.$("#RootPath").val() + "HandbookArticle/GetBySlug?slug=" + id;
                        _this.$("#ifr-main").attr("src", articleURL);
                    }
                    else {
                        window.open(article.Url);
                    }
                },
                complete: function () {
                }
            });
        };
        this.getCustomerCookiesName = function (key) {
            var custId = _this.$("#CustId").val();
            var subAccId = _this.$("#SubAccId").val();
            return key + "_" + custId + "_" + subAccId;
        };
        this.initFeedbackFunction = function (width, height) {
            var hideFeedbackFunctionSessionKey = _this.getCustomerCookiesName('HideFeedbackFunction_Handbook');
            if (sessionStorage.getItem(hideFeedbackFunctionSessionKey) === "1") {
                return;
            }
            var functionId = 8;
            var url = _this.$("#MessageRootPath").val() + 'Feedback/CheckExist?id=' + functionId;
            _this.$.get(url, function (result) {
                if (result.IsExist) {
                    return;
                }
                var feedbackFunction = _this.$("#feedback-function");
                var feedbackBox = feedbackFunction.find(".feedback-box");
                feedbackBox.find(".txt-feedback").text(result.FunctionName || "Feedback");
                feedbackBox
                    .off()
                    .click(function () {
                    var url = _this.$("#MessageRootPath").val() + 'Feedback/Function?id=' + functionId;
                    _this.popupManager.open(url, undefined, width || 440, height || 380, null, null, null, null, null, null, { customClass: "feedback-modal" });
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
        this.$ = $;
        this.topWindow = topWindow;
        this.autoSuggestion = autoSuggestion;
        this.popupManager = popupManager;
    }
    return DefaultPage;
}());
;
$(function () {
    var autoSuggestion = new AutoSuggestion($);
    var page = new DefaultPage($, window.top, autoSuggestion, popupManager);
    page.init();
});
//# sourceMappingURL=default.js.map