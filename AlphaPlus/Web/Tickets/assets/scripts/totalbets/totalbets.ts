interface JQuery {
    fanexAutoRefresh(option: any): void;
}

interface Window {
    popupManager: {
        open(url: string, title: string, width: number): void;
    };
}

class TotalBets {
    initAutoRefresh($tblRpt: JQuery) {
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
                $.get(autoRefreshUrl, (html) => {
                    $tblRpt.html(html);
                    this.bindViewForecast($tblRpt);
                });
            },
            savedSecondKey: $('#hidden-autoRefreshSavedSecondKey').val()
        });
    }

    bindViewForecast($tblRpt: JQuery) {
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
