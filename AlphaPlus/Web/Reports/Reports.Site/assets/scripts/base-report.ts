/// <reference path="components/link-builder.ts" />

interface JQueryStatic {
    blockUI(): void;
    unblockUI(): void;
}

class BaseReport {
    $: JQueryStatic;
    linkBuilderComponent: LinkBuilderComponent;

    $reportForm: JQuery;
    $pageTitle: JQuery;
    $tableReport: JQuery;
    $isFilterVisible: JQuery;

    reportLegendUrl: string;
    $reportLegend: JQuery;
    $iconInformation: JQuery;

    constructor($: JQueryStatic, linkBuilderComponent: LinkBuilderComponent) {
        this.$ = $;
        this.linkBuilderComponent = linkBuilderComponent;
        this.initBaseReport();
    }

    private initBaseReport() {
        // cache elements
        this.$reportForm = this.$('#report-form');
        this.$pageTitle = this.$('.page-title');
        this.$tableReport = this.$('#tbl-report');
        this.$isFilterVisible = this.$('#IsFilterVisible');

        // register event
        this.$('.icon-filter').click(this.onClickIconFilter);
        this.initBaseHelp();

        this.$tableReport.stickyTableHeaders();
    }

    private onClickIconFilter = () => {
        const filter = this.$reportForm;
        const currentHideState = filter.hasClass('hide');

        if (currentHideState) {
            this.$pageTitle.removeClass('hide-filter');
            this.$reportForm.removeClass('hide');
            this.$isFilterVisible.val('true');
        } else {
            this.$pageTitle.addClass('hide-filter');
            this.$reportForm.addClass('hide');
            this.$isFilterVisible.val('false');
        }

        //this.replaceHrefParam('IsFilterVisible', currentHideState);
    };

    // private replaceHrefParam(paramName: string, paramValue: any) {
    //     this.linkBuilderComponent.updateParam(paramName, paramValue);
    // }

    private initBaseHelp() {
        const $loadLegendURL = this.$('#LoadLegendURL');
        if ($loadLegendURL.length) {
            this.reportLegendUrl = $loadLegendURL.attr('href');
            $loadLegendURL.remove();

            this.$iconInformation = this.$('#icon-information');
            this.$iconInformation.click(this.showLegend);
            this.loadLegend();
        }
    }

    private showLegend = () => {
        this.$reportLegend.modal({ show: true, backdrop: false });
        const iconHelpPosition = this.$iconInformation.position();
        this.$reportLegend.css({
            bottom: 'auto',
            top: this.$('.entered-non-entered').length > 0 ? iconHelpPosition.top + 90 : iconHelpPosition.top + 60
        });
    };

    private loadLegend() {
        this.$.ajax({
            method: 'GET',
            url: this.reportLegendUrl,
            cache: false,
            success: this.renderLegend,
            complete: this.$.unblockUI
        });
    }

    private renderLegend = (html: string) => {
        this.$reportLegend && this.$reportLegend.remove();
        this.$('body').append(html);

        if (this.$.trim(html) === '') {
            this.$iconInformation.parent().remove();
        } else {
            this.$iconInformation.parent().removeClass('hide');
        }

        this.$reportLegend = this.$('#report-legend');
    };
}
