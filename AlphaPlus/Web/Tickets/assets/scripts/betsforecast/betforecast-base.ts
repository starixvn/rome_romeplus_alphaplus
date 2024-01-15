interface JQuery {
    fanexAutoRefresh(option: any): void;
}

class BetsForecastBase {
    initAutoRefresh($tblRpt: JQuery) {
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

    protected bindViewForecast() {}

    init() {
        this.initAutoRefresh($('#tblRpt'));
    }
}

$(function () {
    new BetsForecastBase().init();
});
