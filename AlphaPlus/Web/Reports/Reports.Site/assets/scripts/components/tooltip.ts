/// <reference path="../core.ts" />

class ReportTooltipOption {
    constructor(public tooltipId: string = 'tooltip', public custId: string = '0') {}
}

class ReportTooltip {
    tooltipCookieId = 'TooltipShowCount';
    maxTimesToShowTooltip = 5;
    timeToShowTooltip = 5000;

    show = (tooltipOption: ReportTooltipOption = new ReportTooltipOption()) => {
        let tooltipCookieId = this.tooltipCookieId + '-' + tooltipOption.tooltipId + '-' + tooltipOption.custId;
        let tooltipShowCount = parseInt(sessionStorage.getItem(tooltipCookieId), 0) || 0;
        if (tooltipShowCount >= this.maxTimesToShowTooltip) {
            return;
        }

        const tooltipElement = $('#' + tooltipOption.tooltipId);
        tooltipElement.tooltip({ placement: 'bottom' });
        tooltipElement.tooltip('show');
        sessionStorage.setItem(tooltipCookieId, (tooltipShowCount + 1).toString());

        const $document = $(document);
        setTimeout(() => {
            tooltipElement.tooltip('hide');
            $document.off('click.tooltip');
        }, this.timeToShowTooltip);

        $document.off('click.tooltip').on('click.tooltip', function () {
            tooltipElement.tooltip('hide');
        });
    };
}
