'use strict';
(function ($) {
    const rootUrl = $('#RootURL').val();
    const $tblRpt = $('#tblRpt tbody');

    function bindViewForecastLink() {
        $('.icon-fc')
            .off('click')
            .click(function () {
                const $this = $(this);
                const url = [
                    rootUrl,
                    'Forecast/FC1x2ByMatchAndBetType?matchId=',
                    $this.data('matchid'),
                    '&betType=',
                    $this.data('bettype')
                ].join('');
                top.popupManager.open(url, '', 900);
            });
    }

    function initAutoRefresh() {
        const autoRefreshSeconds = $('#hidden-autoRefreshSeconds')
            .val()
            .split(',')
            .map(function (second) {
                return +second;
            });
        const autoRefreshUrl = $('#hidden-autoRefreshUrl').val();
        $('.auto-refresh').fanexAutoRefresh({
            buttonRefreshTooltip: $('#hidden-autoRefreshRefreshText').val(),
            seconds: autoRefreshSeconds,
            refreshHandler: function () {
                $.get(autoRefreshUrl, function (html) {
                    $tblRpt.html(html);
                    bindViewForecastLink();
                });
            },
            savedSecondKey: $('#hidden-autoRefreshSavedSecondKey').val()
        });
    }

    $(function () {
        initAutoRefresh();
        bindViewForecastLink();
    });
})(window.jQuery);
