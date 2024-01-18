"use strict";
class TotalBets {
    initAutoRefresh($tblRpt) {
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
                $.get(autoRefreshUrl, (html) => {
                    $tblRpt.html(html);
                    this.bindViewForecast();
                });
            },
            savedSecondKey: $('#hidden-autoRefreshSavedSecondKey').val()
        });
    }
    bindViewForecast($tblRpt) {
        $tblRpt
            .find('.icon-fc')
            .off('click')
            .click(function (e) {
            //e.preventDefault();
            top.popupManager.open($(e.target).attr('href'), '', 900);
        });
    }
    init() {
        this.initAutoRefresh($('#tblRpt').find('tbody'));
    }
}
$(function () {
    new TotalBets().init();
});
