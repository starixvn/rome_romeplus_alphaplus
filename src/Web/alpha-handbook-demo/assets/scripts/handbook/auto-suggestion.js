var AutoSuggestion = /** @class */ (function () {
    function AutoSuggestion($) {
        var _this = this;
        this.minSearchLength = 2;
        this.init = function (options) {
            _this.options = options;
            _this.$(window).click(function () { _this.options.boxSuggestion.addClass('hidden'); });
            var iframe = _this.$('#ifr-main');
            iframe.on('load', function () {
                var f = iframe[0];
                var iframeWindow = f.contentWindow || f.contentDocument;
                _this.$(iframeWindow.document).on('click touchend', function () { _this.options.boxSuggestion.addClass('hidden'); });
            });
        };
        this.suggest = function () {
            var keyword = _this.options.elmSearch.val();
            if (keyword.length <= _this.minSearchLength) {
                _this.options.boxSuggestion.addClass('hidden');
                return;
            }
            _this.search(keyword);
        };
        this.search = function (keyword) {
            var self = _this;
            _this.$.ajax({
                type: 'GET',
                url: _this.options.suggestUrl + "?keyword=" + keyword,
                success: function (data, textStatus, jqXHR) {
                    var ul = _this.options.boxSuggestion.find(">ul");
                    ul.empty();
                    if (data.length) {
                        data.forEach(function (e) {
                            ul.append('<li data-id="' + e.Id + '">' + e.Value + '</li>');
                        });
                        ul.find(">li").click(function () {
                            var $this = self.$(this);
                            self.selectedSuggestion($this);
                        });
                    }
                    else {
                        ul.append('<li>' + _this.$("#NoRecords").val() + '</li>');
                    }
                    _this.options.boxSuggestion.removeClass('hidden');
                },
                error: function () {
                    var ul = _this.options.boxSuggestion.find(">ul");
                    ul.empty();
                    ul.append('<li>' + _this.$("#NoRecords").val() + '</li>');
                    _this.options.boxSuggestion.removeClass('hidden');
                },
                complete: function (jqXHR, textStatus) {
                }
            });
        };
        this.selectedSuggestion = function (elm) {
            var id = elm.attr("data-id");
            _this.options.selectedSuggestion(id);
        };
        this.$ = $;
    }
    return AutoSuggestion;
}());
;
//# sourceMappingURL=auto-suggestion.js.map