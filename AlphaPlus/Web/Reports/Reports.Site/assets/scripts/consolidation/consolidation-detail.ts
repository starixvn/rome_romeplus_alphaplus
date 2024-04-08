/// <reference path="../core.ts" />
/// <reference path="../components/productmultiselection.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/tooltip.ts" />
/// <reference path="../components/sort.ts" />
/// <reference path="../base-report.ts" />
/// <reference path="../components/link-builder.ts" />

class ConsolidationDetailView extends BaseReport {
    productComponent: ProductMultiSelectionComponent;
    dateRangePickerComponent: DateRangePickerComponent;
    reportTooltip: ReportTooltip;
    sortComponent: SortComponent;

    isFirstSwitchMode = true;

    constructor(
        productComponent: ProductMultiSelectionComponent,
        dateRangePickerComponent: DateRangePickerComponent,
        reportTooltip: ReportTooltip,
        linkBuilderComponent: LinkBuilderComponent,
        sortComponent: SortComponent
    ) {
        super(linkBuilderComponent);

        this.productComponent = productComponent;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.reportTooltip = reportTooltip;
        this.sortComponent = sortComponent;
    }

    init() {
        $.blockUI();

        this.initComponents();
        this.registerEvents();
        //this.saveConsolidationParam();

        $.unblockUI();
    }

    initComponents() {
        this.productComponent.render();
        this.dateRangePickerComponent.render();
        this.sortComponent.initSort(this.$tableReport, { 0: { sorter: 'text' } });
    }

    registerEvents() {
        $('.icon-switchmode').on('click', this.onSwitchMode);
    }

    onSwitchMode = () => {
        $.blockUI();

        const isStackMode = this.isStackMode();

        if (isStackMode) {
            this.$pageTitle.removeClass('stack-mode');
            this.$tableReport.removeClass('stack-mode');
        } else {
            this.$pageTitle.addClass('stack-mode');
            this.$tableReport.addClass('stack-mode');
        }

        //this.replaceHrefParam('IsStackMode', !isStackMode);
        if (this.isFirstSwitchMode) {
            if (isStackMode) {
                alphaReports.splitCell('col-turnover');

                this.levels.forEach(function (level: string, index: number) {
                    alphaReports.splitCell(`col-${level}-winloss`, index % 2 ? ' altercol' : '');
                });
            } else {
                alphaReports.combineConsecutiveCells($('td.col-turnover'), 3, 'col-turnover-combined');

                this.levels.forEach(function (level: string, index: number) {
                    const cellCss = `col-${level}-winloss-combined${index % 2 ? ' altercol' : ''}`;
                    alphaReports.combineConsecutiveCells($(`td.col-${level}-winloss`), 3, cellCss);
                });
            }

            this.isFirstSwitchMode = false;
        }

        this.sortComponent.bindTableSorter(this.getSortedColumnName());
        $.unblockUI();
    };

    onSubmitReport = (event: any) => {
        if (!this.productComponent.validateProductIds()) {
            event.preventDefault();
            return;
        }

        super.onSubmitReport(event, true, this.sortComponent.getSortInfo());
    };

    // replaceHrefParam = (paramName: string, paramValue: any) => {
    //     this.linkBuilderComponent.updateParam(paramName, paramValue);
    // };

    getSortedColumnName = () => {
        const sortedColumn = this.$tableReport.data('sortedcolumn');
        if (!this.isStackMode()) {
            return sortedColumn;
        }

        if (sortedColumn === 'NetTurnover' || sortedColumn === 'GrossComm') {
            return 'Turnover';
        }

        if (sortedColumn === 'MemberComm' || sortedColumn === 'MemberTotal') {
            return 'MemberWinLoss';
        }

        if (sortedColumn === 'AgComm' || sortedColumn === 'AgTotal') {
            return 'AgWinLoss';
        }

        if (sortedColumn === 'MAgComm' || sortedColumn === 'MAgTotal') {
            return 'MAgWinLoss';
        }

        if (sortedColumn === 'SAgComm' || sortedColumn === 'SAgTotal') {
            return 'SAgWinLoss';
        }

        return sortedColumn;
    };

    /*saveConsolidationParam = () => {
        this.linkBuilderComponent.updateParam('ConsolidationFromDate', this.$consolidationFromDate.val());
        this.linkBuilderComponent.updateParam('ConsolidationToDate', this.$consolidationToDate.val());
        this.linkBuilderComponent.updateParam(
            'ConsolidationUserSelectedDateRangeOption',
            this.$consolidationUserSelectedDateRangeOption.val()
        );
        this.linkBuilderComponent.updateParam('ConsolidationProductIds', this.$consolidationProductIds.val());

        let firstBreadcrumb = this.$('.breadcrumb > li:first-child').find('a');
        let currentUrl = firstBreadcrumb.attr('href');
        currentUrl = alphaReports.replaceQueryStringValue(currentUrl, 'FromDate', this.$consolidationFromDate.val());
        currentUrl = alphaReports.replaceQueryStringValue(currentUrl, 'ToDate', this.$consolidationToDate.val());
        currentUrl = alphaReports.replaceQueryStringValue(
            currentUrl,
            'UserSelectedDateRangeOption',
            this.$consolidationUserSelectedDateRangeOption.val()
        );
        currentUrl = alphaReports.replaceQueryStringValue(
            currentUrl,
            'UserSelectedProductIds',
            this.$consolidationProductIds.val()
        );
        firstBreadcrumb.attr('href', currentUrl);
    };*/
}

$(function () {
    const linkBuilderComponent = new LinkBuilderComponent();
    new ConsolidationDetailView(
        new ProductMultiSelectionComponent(),
        new DateRangePickerComponent(),
        new ReportTooltip(),
        linkBuilderComponent,
        new SortComponent()
    ).init();
});
