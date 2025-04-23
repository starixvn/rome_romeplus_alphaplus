"use strict";

abstract class MyWatchlistBase {
    protected $: JQueryStatic;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    protected initInfoLegend() {
        const iconInformation = this.$("#icon-information");
        const reportLegend = this.$("#report-legend");
        iconInformation.click(_ => this.showLegend(iconInformation, reportLegend));
    }

    private showLegend(iconInformation, reportLegend) {
        reportLegend.modal({ show: true, backdrop: false });

        const iconHelpPosition = iconInformation.position();
        reportLegend.css({
            bottom: "auto",
            top: iconHelpPosition.top + 40,
            left: 0
        });
    }
}
