"use strict";
class BetsForecastBase {
    initAutoRefresh($tblRpt) {
        const autoRefreshSeconds = $('#hidden-autoRefreshSeconds')
            .val()
            .split(',')
            .map((second) => {
            return +second;
        });
        const autoRefreshUrl = $('#hidden-autoRefreshUrl').val();
        $('.auto-refresh').fanexAutoRefresh({
            buttonRefreshTooltip: $('#hidden-autoRefreshRefreshText').val(),
            seconds: autoRefreshSeconds,
            refreshHandler: () => {
                $.get(autoRefreshUrl, (html) => {
                    $tblRpt.html(html);
                    this.bindViewForecast();
                });
            },
            savedSecondKey: $('#hidden-autoRefreshSavedSecondKey').val()
        });
    }
    bindViewForecast() { }
    init() {
        this.initAutoRefresh($('#tblRpt'));
    }
}
$(function () {
    new BetsForecastBase().init();
});
