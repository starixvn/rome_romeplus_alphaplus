/// <reference path="../core.ts" />

class ReportTooltipOption {
    constructor(public tooltipId: string = 'tooltip', public custId: number = 0) {}
}

class ReportTooltip {
    $: JQueryStatic;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    tooltipCookieId = 'TooltipShowCount';
    timeToShowTooltip = 5000;

    show = (tooltipOption: ReportTooltipOption = new ReportTooltipOption()) => {
        let tooltipCookieId = this.tooltipCookieId + '-' + tooltipOption.tooltipId + '-' + tooltipOption.custId;
        let tooltipShowCount = parseInt(alphaReports.localStorage.get(tooltipCookieId), 0) || 0;
        if (tooltipShowCount >= alphaReports.maxTimesToShowTooltip) {
            return;
        }

        const tooltipElement = this.$('#' + tooltipOption.tooltipId);
        tooltipElement.tooltip({ placement: 'bottom' });
        tooltipElement.tooltip('show');
        alphaReports.localStorage.set(tooltipCookieId, tooltipShowCount + 1);

        const $document = this.$(document);
        setTimeout(() => {
            tooltipElement.tooltip('hide');
            $document.off('click.tooltip');
        }, this.timeToShowTooltip);

        $document.off('click.tooltip').on('click.tooltip', function () {
            tooltipElement.tooltip('hide');
        });
    };
}
