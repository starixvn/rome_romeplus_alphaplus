interface JQuery {
    fanexAutoRefresh(option: any): void;
}

class Forecast {
    private readonly dropdownLeagueId = '#ddlLeague';
    private readonly compactFeedback = new CompactFeedbackComponent();

    private initAutoRefresh() {
        $('.auto-refresh').fanexAutoRefresh({
            buttonRefreshTooltip: $('#hidden-autoRefreshRefreshText').val(),
            seconds: $('#hidden-autoRefreshSeconds')
                .val()
                .split(',')
                .map((second) => +second),
            refreshHandler: () => {
                const $ddlLeague = $(this.dropdownLeagueId);
                const url = `${$('#hidden-autoRefreshUrl').val()}${$ddlLeague.length ? $ddlLeague.val() : ''}`;
                this.handleAutoRefresh(url);
            },
            savedSecondKey: $('#hidden-autoRefreshSavedSecondKey').val()
        });
    }

    private handleAutoRefresh(url, succcessCallback: Function = undefined) {
        $.get(url, (html) => {
            $('#content').html(html);
            this.bindEvents();
            succcessCallback && succcessCallback();
        });
    }

    private bindEvents() {
        const $ddlLeague = this.bindLeagueChangeEvent();
        this.bindViewForecast();
        this.bindViewBetList();
        this.bindViewAH();
        this.initCompactFeedback($ddlLeague);
    }

    private bindLeagueChangeEvent(): JQuery {
        const $ddlLeague = $(this.dropdownLeagueId);
        $ddlLeague.off('change').change(() => {
            const leagueId = $ddlLeague.val();
            $.get(`${$('#hidden-dataUrl').val()}${leagueId}`, (html) => {
                $('#tblRpt tbody').html(html);
                this.bindViewForecast();
                this.initCompactFeedback($ddlLeague);
            });
        });

        return $ddlLeague;
    }

    private bindViewForecast() {
        $('.tbl-hdpoulive .icon-fc')
            .off('click')
            .click(function (e) {
                e.preventDefault();
                top.popupManager.open($(e.target).attr('href'), '', 600, 290);
            });
    }

    private bindViewBetList() {
        $('.view-betlist')
            .off('click')
            .click(function (e) {
                e.preventDefault();
                top.popupManager.openMultiple($(e.target).attr('href'), '', 1500);
            });
    }

    private bindViewAH() {
        $('.view-ah')
            .off('click')
            .click((e) => {
                e.preventDefault();
                const url = $(e.target).attr('href');
                this.handleAutoRefresh(url, function () {
                    $('#hidden-autoRefreshUrl').val(url);
                });
            });
    }

    private initCompactFeedback($ddlLeague: JQuery) {
        const firstCell = $('#tblRpt tbody tr:first td:first');
        const hasData = firstCell.length > 0 && !firstCell.data('noinfo');
        this.compactFeedback.init($ddlLeague.length > 0 ? { leagueId: $ddlLeague.val() } : undefined, hasData);
    }

    init() {
        this.initAutoRefresh();
        this.bindEvents();
    }
}

$(function () {
    new Forecast().init();
});
