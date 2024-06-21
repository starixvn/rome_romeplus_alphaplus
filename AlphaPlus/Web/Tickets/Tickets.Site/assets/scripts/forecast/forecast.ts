interface JQuery {
    fanexAutoRefresh(option: any): void;
}

class Forecast {
    dataUrl: string;

    initAutoRefresh($content: JQuery) {
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
            refreshHandler: () => {
                const $ddlLeague = $('#ddlLeague');
                $.get(autoRefreshUrl + ($ddlLeague.length ? $ddlLeague.val() : ''), (html) => {
                    $content.html(html);
                    this.bindLeagueChangeEvent();
                    this.bindViewForecast();
                    this.viewBetList();
                });
            },
            savedSecondKey: $('#hidden-autoRefreshSavedSecondKey').val()
        });
    }

    bindLeagueChangeEvent() {
        $('#ddlLeague')
            .off('change')
            .change((e) => {
                $.get(this.dataUrl + $(e.target).val(), (html) => {
                    $('#tblRpt tbody').html(html);
                    this.bindViewForecast();
                });
            });
    }

    bindViewForecast() {
        $('.tbl-hdpoulive .icon-fc')
            .off('click')
            .click(function (e) {
                e.preventDefault();
                top.popupManager.open($(e.target).attr('href'), '', 600, 290);
            });
    }

    viewBetList() {
        $('.view-betlist')
            .off('click')
            .click(function (e) {
                e.preventDefault();
                top.popupManager.openMultiple($(e.target).attr('href'), '', 1500);
            });
    }

    init() {
        this.dataUrl = $('#hidden-dataUrl').val();
        this.initAutoRefresh($('#content'));
        this.bindLeagueChangeEvent();
        this.bindViewForecast();
        this.viewBetList();
    }
}

$(function () {
    new Forecast().init();
});
