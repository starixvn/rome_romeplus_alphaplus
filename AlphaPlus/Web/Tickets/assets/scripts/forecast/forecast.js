"use strict";
class Forecast {
    initAutoRefresh($content) {
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
                    this.bindLeagueChangeEvent($ddlLeague);
                });
            },
            savedSecondKey: $('#hidden-autoRefreshSavedSecondKey').val()
        });
    }
    bindLeagueChangeEvent($ddlLeague) {
        $ddlLeague.off('change').change(() => {
            $.get(this.dataUrl + $ddlLeague.val(), function (html) {
                $('#tblRpt tbody').html(html);
            });
        });
    }
    init() {
        this.dataUrl = $('#hidden-dataUrl').val();
        this.initAutoRefresh($('#content'));
        this.bindLeagueChangeEvent($('#ddlLeague'));
    }
}
$(function () {
    new Forecast().init();
});
