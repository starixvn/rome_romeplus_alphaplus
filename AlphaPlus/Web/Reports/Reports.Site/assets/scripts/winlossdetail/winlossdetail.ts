/// <reference path="../../../assets/scripts/core.ts" />
/// <reference path="../../../assets/scripts/base-report.ts" />
/// <reference path="../../../assets/scripts/components/daterangepicker.ts" />
/// <reference path="../../../assets/scripts/components/monthpicker.ts" />
/// <reference path="../../../assets/scripts/components/pager.ts" />
/// <reference path="../../../assets/scripts/components/productmultiselection.ts" />
/// <reference path="../../../assets/scripts/components/sort.ts" />
/// <reference path="../../../assets/scripts/components/tooltip.ts" />

class WinLossDetailView extends BaseReport {
    productComponent: ProductMultiSelectionComponent;
    dateRangePickerComponent: DateRangePickerComponent;
    reportTooltip: ReportTooltip;
    sortComponent: SortComponent;
    monthPickerComponent: MonthPickerComponent;

    isFirstSwitchMode = true;

    constructor(
        productComponent: ProductMultiSelectionComponent,
        dateRangePickerComponent: DateRangePickerComponent,
        reportTooltip: ReportTooltip,
        sortComponent: SortComponent,
        monthPickerComponent: MonthPickerComponent
    ) {
        super();

        this.productComponent = productComponent;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.reportTooltip = reportTooltip;
        this.sortComponent = sortComponent;
        this.monthPickerComponent = monthPickerComponent;
    }

    init() {
        $.blockUI();

        this.initComponents();
        this.registerEvents();

        $.unblockUI();
    }

    initComponents() {
        this.productComponent.render();

        if ($('#IsHistoricReport').val().toLowerCase() === 'true') {
            this.monthPickerComponent.render();
        } else {
            this.dateRangePickerComponent.render();
        }

        if (
            $('#IsSetDefaultStackMode').val().toString().toLowerCase() === 'true' &&
            alphaReports.screenSmallThan768()
        ) {
            this.setStackMode();
            this.reportTooltip.show(new ReportTooltipOption('icon-normal-column', 'winlossdetail'));
            super.hideFilter();
        }

        this.sortComponent.initSort(
            this.$tableReport,
            { 0: { sorter: 'text' } },
            this.sortComponent.handleDormantAccountsAfterSort
        );
        // remove style="visibility:hidden"
        // TableSorter cannot run with style="display:none"
        this.$tableReport.removeAttr('style');
    }

    registerEvents() {
        $('.icon-switchmode').click(this.onSwitchMode);
        super.registerHistoricReportEvent();
        super.registerDrilldownableEvents();
    }

    setStackMode() {
        $.blockUI();

        this.$pageTitle.addClass('stack-mode');
        this.$tableReport.addClass('stack-mode');

        alphaReports.combineConsecutiveCells($('td.col-turnover'), 3, 'col-turnover-combined');

        this.levels.forEach(function (level: string, index: number) {
            const cellCss = `col-${level}-winloss-combined${index % 2 ? ' altercol' : ''}`;
            alphaReports.combineConsecutiveCells($(`td.col-${level}-winloss`), 3, cellCss);
        });

        this.isFirstSwitchMode = false;
        this.sortComponent.bindTableSorter(this.getSortedColumnName());

        $.unblockUI();
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

        super.onSubmitReport(event);
    };

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
}

$(function () {
    new WinLossDetailView(
        new ProductMultiSelectionComponent(),
        new DateRangePickerComponent(),
        new ReportTooltip(),
        new SortComponent(),
        new MonthPickerComponent()
    ).init();
});
