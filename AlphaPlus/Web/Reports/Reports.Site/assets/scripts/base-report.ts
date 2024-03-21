/// <reference path="components/link-builder.ts" />

interface JQueryStatic {
    blockUI(): void;
    unblockUI(): void;
}

class BaseReport {
    $: JQueryStatic;
    linkBuilderComponent: LinkBuilderComponent;

    $pageTitle: JQuery;
    $reportForm: JQuery;
    $tableReport: JQuery;
    $isFilterVisible: JQuery;
    $isStackMode: JQuery;

    $reportLegend: JQuery;
    $iconInformation: JQuery;

    constructor($: JQueryStatic, linkBuilderComponent: LinkBuilderComponent) {
        this.$ = $;
        this.linkBuilderComponent = linkBuilderComponent;
        this.initBaseReport();
    }

    private initBaseReport() {
        // cache elements
        this.$pageTitle = this.$('.page-title');
        this.$reportForm = this.$('#report-form');
        this.$tableReport = this.$('#tbl-report');
        this.$isFilterVisible = this.$('#IsFilterVisible');
        this.$isStackMode = this.$('#IsStackMode');

        // register event
        this.initBaseReportLegend();
        this.$('.icon-filter').click(this.onClickIconFilter);
        this.$reportForm.submit((e) => this.onSubmitReport(e));

        this.$tableReport.stickyTableHeaders();
    }

    private initBaseReportLegend() {
        const $loadLegendURL = this.$('#LoadLegendURL');
        if ($loadLegendURL.length) {
            const reportLegendUrl = $loadLegendURL.attr('href');
            $loadLegendURL.remove();

            this.$iconInformation = this.$('#icon-information').click(this.showLegend);
            this.$.ajax({
                method: 'GET',
                url: reportLegendUrl,
                cache: false,
                success: this.renderLegend,
                complete: this.$.unblockUI
            });
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

    private onClickIconFilter = () => {
        const filter = this.$reportForm;
        const currentHideState = filter.hasClass('hide');

        if (currentHideState) {
            this.$pageTitle.removeClass('hide-filter');
            this.$reportForm.removeClass('hide');
        } else {
            this.$pageTitle.addClass('hide-filter');
            this.$reportForm.addClass('hide');
        }

        //this.replaceHrefParam('IsFilterVisible', currentHideState);
    };

    // private replaceHrefParam(paramName: string, paramValue: any) {
    //     this.linkBuilderComponent.updateParam(paramName, paramValue);
    // }

    protected onSubmitReport(e) {
        this.$.blockUI();
        const isFilterVisibleValue = !this.$reportForm.hasClass('hide') && !alphaReports.screenSmallThan768();
        this.$isFilterVisible.val(isFilterVisibleValue.toString());
        this.$isStackMode.length && this.$isStackMode.val(this.isStackMode().toString());
    }

    protected isStackMode = () => this.$tableReport.hasClass('stack-mode');
}
