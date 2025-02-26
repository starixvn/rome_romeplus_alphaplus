interface JQuery {
    fanexAutoRefresh(option: any): void;
}

class Forecast {
    private initAutoRefresh() {
        const $ddlLeague = $('#ddlLeague');
        const url = `${$('#hidden-autoRefreshUrl').val()}${$ddlLeague.length ? $ddlLeague.val() : ''}`;

        $('.auto-refresh').fanexAutoRefresh({
            buttonRefreshTooltip: $('#hidden-autoRefreshRefreshText').val(),
            seconds: $('#hidden-autoRefreshSeconds')
                .val()
                .split(',')
                .map((second) => +second),
            refreshHandler: () => this.handleAutoRefresh(url),
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
        this.bindLeagueChangeEvent();
        this.bindViewForecast();
        this.bindViewBetList();
        this.bindViewAH();
    }

    private bindLeagueChangeEvent() {
        $('#ddlLeague')
            .off('change')
            .change((e) => {
                $.get(`${$('#hidden-dataUrl').val()}${$(e.target).val()}`, (html) => {
                    $('#tblRpt tbody').html(html);
                    this.bindViewForecast();
                });
            });
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

    init() {
        this.initAutoRefresh();
        this.bindEvents();
    }
}

$(function () {
    new Forecast().init();
});
