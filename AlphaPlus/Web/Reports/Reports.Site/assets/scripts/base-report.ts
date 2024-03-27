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

        // register event
        this.initBaseReportLegend();
        this.$('.icon-filter').click(this.onClickIconFilter);
        this.$reportForm.submit((event) => this.onSubmitReport(event));

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

    protected onSubmitReport(event, hasStackIcon: boolean = undefined, sortInfo: SortInfo = undefined) {
        const isFilterVisibleValue = !this.$reportForm.hasClass('hide') && !alphaReports.screenSmallThan768();
        let html = `<input type="hidden" name="IsFilterVisible" value="${isFilterVisibleValue}" />`;

        if (typeof hasStackIcon !== 'undefined') {
            html += `<input type="hidden" name="IsStackMode" value="${this.isStackMode()}" />`;
        }

        if (typeof sortInfo !== 'undefined') {
            html +=
                `<input type="hidden" name="SortPropertyName" value="${sortInfo.sortPropertyName}" />` +
                `<input type="hidden" name="DescendingSort" value="${sortInfo.descendingSort}" />`;
        }

        this.$reportForm.append(html);
    }

    protected isStackMode = () => this.$tableReport.hasClass('stack-mode');

    protected registerHistoricReportEvent() {
        this.$('#historic-report').click((e) => {
            const isHistoricReport = this.$(e.target).data('is-historic-report');
            this.$('#from-date').val('');
            this.$('#to-date').val('');
            this.$('#IsHistoricReport').val(isHistoricReport ? 'false' : 'true');
            this.$reportForm.submit();
        });
    }

    protected registerDrilldownableEvents() {
        const handleDrilldownable = ($element) => {
            const $custLevelId = this.$('#CustLevelId');
            let custLevelIdValue = $element.data('custlevelid') || +$custLevelId.val() - 1;

            $custLevelId.val(custLevelIdValue);
            this.$('#CustId').val($element.data('custid'));
            this.$('#CustName').val($element.data('custname'));

            this.$reportForm.submit();
        };

        this.$pageTitle.find('.breadcrumb-link').click((e) => handleDrilldownable(this.$(e.target)));

        this.$tableReport.find('.downline-link').click((e) => handleDrilldownable(this.$(e.target)));
    }
}
