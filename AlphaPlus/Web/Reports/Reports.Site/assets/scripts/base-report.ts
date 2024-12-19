interface JQueryStatic {
    blockUI(): void;
    unblockUI(): void;
}

class BaseReport {
    $pageTitle: JQuery;
    $reportForm: JQuery;
    $tableReport: JQuery;

    $reportLegend: JQuery;
    $iconInformation: JQuery;

    levels = ['member', 'agent', 'master', 'super'];
    isProcessingDrilldownable = false;

    constructor(isSetStickyTableHeaders = true) {
        this.initBaseReport(isSetStickyTableHeaders);
    }

    private initBaseReport(isSetStickyTableHeaders: boolean) {
        // cache elements
        this.$pageTitle = $('.page-title');
        this.$reportForm = $('#report-form');
        this.$tableReport = $('#tbl-report');

        // register event
        this.initBaseReportLegend();
        $('.icon-filter').click(() => this.onClickIconFilter());
        this.$reportForm.submit((event) => this.onSubmitReport(event));

        isSetStickyTableHeaders && this.$tableReport.stickyTableHeaders();
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

    protected onClickIconFilter() {
        const isHideFilter = this.$reportForm.hasClass('hide');
        isHideFilter ? this.showFilter() : this.hideFilter();
    }

    private showFilter() {
        this.$pageTitle.removeClass('hide-filter');
        this.$reportForm.removeClass('hide');
    }

    protected hideFilter() {
        this.$pageTitle.addClass('hide-filter');
        this.$reportForm.addClass('hide');
    }

    protected onSubmitReport(event, sortInfo = undefined) {
        $.blockUI();
        const isFilterVisible = !this.$reportForm.hasClass('hide') && !alphaReports.screenSmallThan768();
        const html = [`<input type="hidden" name="IsFilterVisible" value="${isFilterVisible}"/>`];

        const hasStackIcon = this.$pageTitle.find('#icon-stack-column').length > 0;
        if (hasStackIcon) {
            html.push(`<input type="hidden" name="IsStackMode" value="${this.isStackMode()}"/>`);
        }

        sortInfo = sortInfo || this.getSortInfo();
        if (typeof sortInfo !== 'undefined') {
            html.push(
                `<input type="hidden" name="SortPropertyName" value="${sortInfo.sortPropertyName}"/>`,
                `<input type="hidden" name="DescendingSort" value="${sortInfo.descendingSort}"/>`
            );
        }

        this.$reportForm.append(html.join(''));
    }

    protected isStackMode = () => this.$tableReport.hasClass('stack-mode');

    private getSortInfo(): SortInfo {
        const sortPropertyName = this.$tableReport.data('sortedcolumn');

        if (sortPropertyName) {
            return {
                sortPropertyName,
                descendingSort: this.$tableReport.data('descendingsort')
            };
        }
    }

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
        this.$pageTitle.find('.breadcrumb-link').click((e) => this.handleDrilldownable($(e.target)));
        this.$tableReport.find('.downline-link').click((e) => this.handleDrilldownable($(e.target)));
    }

    private handleDrilldownable($element: JQuery) {
        if (this.isProcessingDrilldownable) {
            return;
        }

        this.isProcessingDrilldownable = true;
        const $custLevelId = $('#CustLevelId');
        const custLevelIdValue = $element.data('custlevelid') || +$custLevelId.val() - 1;

        $custLevelId.val(custLevelIdValue);
        this.addParamDrilldownable('CustId', $element.data('custid'));
        this.addParamDrilldownable('CustName', $element.data('custname'));
        this.addParamDrilldownable('product-ids', this.$tableReport.data('productids'));
        this.addParamDrilldownable('resource-ids', this.$tableReport.data('resourceids'));
        this.addParamDrilldownable('from-date', this.$tableReport.data('fromdate'));
        this.addParamDrilldownable('to-date', this.$tableReport.data('todate'));

        this.$reportForm.submit();
    }

    private addParamDrilldownable(hiddenFieldId: string, value: string) {
        const $hiddenField = this.$reportForm.find(`#${hiddenFieldId}`);
        $hiddenField.length && $hiddenField.val(value);
    }
}
