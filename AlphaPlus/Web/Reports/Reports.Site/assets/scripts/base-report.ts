/// <reference path="components/link-builder.ts" />

interface JQueryStatic {
    blockUI(): void;
    unblockUI(): void;
}

class BaseReport {
    linkBuilderComponent: LinkBuilderComponent;

    $pageTitle: JQuery;
    $reportForm: JQuery;
    $tableReport: JQuery;

    $reportLegend: JQuery;
    $iconInformation: JQuery;

    levels = ['member', 'agent', 'master', 'super'];

    constructor(linkBuilderComponent: LinkBuilderComponent) {
        this.linkBuilderComponent = linkBuilderComponent;
        this.initBaseReport();
    }

    private initBaseReport() {
        // cache elements
        this.$pageTitle = $('.page-title');
        this.$reportForm = $('#report-form');
        this.$tableReport = $('#tbl-report');

        // register event
        this.initBaseReportLegend();
        $('.icon-filter').click(this.onClickIconFilter);
        this.$reportForm.submit((event) => this.onSubmitReport(event));

        this.$tableReport.stickyTableHeaders();
    }

    private initBaseReportLegend() {
        const $loadLegendURL = $('#LoadLegendURL');
        if ($loadLegendURL.length) {
            const reportLegendUrl = $loadLegendURL.attr('href');
            $loadLegendURL.remove();

            this.$iconInformation = $('#icon-information').click(this.showLegend);
            $.ajax({
                method: 'GET',
                url: reportLegendUrl,
                cache: false,
                success: this.renderLegend,
                complete: $.unblockUI
            });
        }
    }

    private showLegend = () => {
        this.$reportLegend.modal({ show: true, backdrop: false });
        const iconHelpPosition = this.$iconInformation.position();
        this.$reportLegend.css({
            bottom: 'auto',
            top: $('.entered-non-entered').length > 0 ? iconHelpPosition.top + 90 : iconHelpPosition.top + 60
        });
    };

    private renderLegend = (html: string) => {
        this.$reportLegend && this.$reportLegend.remove();
        $('body').append(html);

        if ($.trim(html) === '') {
            this.$iconInformation.parent().remove();
        } else {
            this.$iconInformation.parent().removeClass('hide');
        }

        this.$reportLegend = $('#report-legend');
    };

    private onClickIconFilter = () => {
        const filter = this.$reportForm;
        const currentHideState = filter.hasClass('hide');
        currentHideState ? this.showFilter() : this.hideFilter();

        //this.replaceHrefParam('IsFilterVisible', currentHideState);
    };

    private showFilter() {
        this.$pageTitle.removeClass('hide-filter');
        this.$reportForm.removeClass('hide');
    }

    protected hideFilter() {
        this.$pageTitle.addClass('hide-filter');
        this.$reportForm.addClass('hide');
    }

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
        $('#historic-report').click((e) => {
            const isHistoricReport = $(e.target).data('is-historic-report');
            $('#from-date').val('');
            $('#to-date').val('');
            $('#IsHistoricReport').val(isHistoricReport ? 'false' : 'true');
            this.$reportForm.submit();
        });
    }

    protected registerDrilldownableEvents() {
        const handleDrilldownable = ($element) => {
            const $custLevelId = $('#CustLevelId');
            const custLevelIdValue = $element.data('custlevelid') || +$custLevelId.val() - 1;

            $custLevelId.val(custLevelIdValue);
            $('#CustId').val($element.data('custid'));
            $('#CustName').val($element.data('custname'));
            $('#product-ids').val(this.$tableReport.data('productids'));
            $('#resource-ids').val(this.$tableReport.data('resourceids'));
            $('#from-date').val(this.$tableReport.data('fromdate'));
            $('#to-date').val(this.$tableReport.data('todate'));

            this.$reportForm.submit();
        };

        this.$pageTitle.find('.breadcrumb-link').click((e) => handleDrilldownable($(e.target)));

        this.$tableReport.find('.downline-link').click((e) => handleDrilldownable($(e.target)));
    }
}
