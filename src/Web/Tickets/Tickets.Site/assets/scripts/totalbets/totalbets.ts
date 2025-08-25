interface JQuery {
    fanexAutoRefresh(option: any): void;
}

interface Window {
    popupManager: {
        open(url: string, title: string, width: number): void;
        open(url: string, title: string, width: number, height: number): void;
        open(
            url: string,
            title: string,
            width: number,
            height: number,
            windowCss,
            titleCss,
            closeBtnCss,
            left,
            top,
            iframeAttributes,
            options
        ): void;
        openMultiple(url: string, title: string, width: number): void;
        close(): void;
    };
}

class TotalBets {
    private compactFeedback: CompactFeedbackComponent = null;

    initAutoRefresh($tblRpt: JQuery) {
        const autoRefreshSeconds = $('#hidden-autoRefreshSeconds')
            .val()
            .split(',')
            .map(function (second) {
                return +second;
            });
        const autoRefreshDefaultSelected = $('#hidden-autoRefreshDefaultSelected');
        const autoRefreshUrl = $('#hidden-autoRefreshUrl').val();
        $('.auto-refresh').fanexAutoRefresh({
            buttonRefreshTooltip: $('#hidden-autoRefreshRefreshText').val(),
            seconds: autoRefreshSeconds,
            selectedSecond: autoRefreshDefaultSelected.length ? +autoRefreshDefaultSelected.val() : undefined,
            refreshHandler: () => {
                $.get(autoRefreshUrl, (html) => {
                    $tblRpt.html(html);
                    this.bindViewForecast($tblRpt);
                    this.initCompactFeedback($tblRpt);
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
                e.preventDefault();
                top.popupManager.open($(e.target).attr('href'), '', 900);
            });
    }

    initCompactFeedback($tblRpt: JQuery) {
        if (this.compactFeedback === null) {
            this.compactFeedback = new CompactFeedbackComponent();
            this.compactFeedback.init();
        } else {
            const firstCell = $tblRpt.find('tr:first td:first');
            this.compactFeedback.toggleShowHide(firstCell.length > 0 && !firstCell.data('noinfo'));
        }
    }

    init() {
        const $tblRpt = $('#tblRpt').find('tbody');
        this.initAutoRefresh($tblRpt);
        this.bindViewForecast($tblRpt);
        this.initCompactFeedback($tblRpt);
    }
}

$(function () {
    new TotalBets().init();
});
