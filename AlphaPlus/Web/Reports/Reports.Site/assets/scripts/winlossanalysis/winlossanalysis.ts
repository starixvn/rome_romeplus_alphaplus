/// <reference path="../core.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/tooltip.ts" />
/// <reference path="../components/productmultiselection.ts" />
/// <reference path="../components/sort.ts" />

class WinLossAnalysisView extends BaseReport {
    $reportTypeId: JQuery;
    $isSetDefaultStackMode: JQuery;
    isFirstSwitchMode = true;

    productComponent: ProductMultiSelectionComponent;
    dateRangePickerComponent: DateRangePickerComponent;
    sortComponent: SortComponent;
    reportTooltip: ReportTooltip;

    constructor(
        productComponent: ProductMultiSelectionComponent,
        dateRangePickerComponent: DateRangePickerComponent,
        sortComponent: SortComponent,
        reportTooltip: ReportTooltip
    ) {
        super();

        this.dateRangePickerComponent = dateRangePickerComponent;
        this.productComponent = productComponent;
        this.sortComponent = sortComponent;
        this.reportTooltip = reportTooltip;
    }

    init() {
        $.blockUI();

        this.cacheDOMElements();
        this.initComponents();
        this.registerEvents();

        $.unblockUI();
    }

    cacheDOMElements() {
        this.$reportTypeId = $('#ReportTypeId');
        this.$isSetDefaultStackMode = $('#IsSetDefaultStackMode');
    }

    initComponents() {
        this.productComponent.render();
        this.dateRangePickerComponent.render();

        if (
            this.$isSetDefaultStackMode.val().toString().toLowerCase() === 'true' &&
            alphaReports.screenSmallThan768()
        ) {
            this.setStackMode();
            this.reportTooltip.show(new ReportTooltipOption('icon-normal-column', 'winlossanalysis'));
        }

        this.sortComponent.initSort(this.$tableReport, { 0: { sorter: this.getSorterName() } });
        // remove style="visibility:hidden"
        // TableSorter cannot run with style="display:none"
        this.$tableReport.removeAttr('style');
    }

    getSorterName = () => {
        switch (this.$reportTypeId.val()) {
            case '1':
                return 'shortDate';
            case '2':
                return 'shortMonth';
            default:
                return 'text';
        }
    };

    public registerEvents() {
        $('.icon-switchmode').on('click', this.onSwitchMode);
        $(':radio[name=ReportTypeId]').change(() => this.$reportForm.submit());
    }

    onSubmitReport = (event: any) => {
        if (!this.productComponent.validateProductIds()) {
            event.preventDefault();
            return;
        }

        super.onSubmitReport(event);
    };

    renderAtFirstSwitchMode = (isSwitchToNormalMode: boolean) =>
        isSwitchToNormalMode ? this.splitColumns() : this.combineColumns();

    onSwitchMode = () => {
        const isStackMode = this.isStackMode();
        this.setCssBySwitchMode(isStackMode);
        if (this.isFirstSwitchMode) {
            this.renderAtFirstSwitchMode(isStackMode);
            this.isFirstSwitchMode = false;
        }
        this.sortComponent.bindTableSorter(this.getSortedColumnName());
    };

    setStackMode = () => {
        $.blockUI();
        this.setCssBySwitchMode(false);
        this.renderAtFirstSwitchMode(false);
        this.isFirstSwitchMode = false;
        $.unblockUI();
    };

    setCssBySwitchMode = (isSwitchToNormalMode: boolean) => {
        if (isSwitchToNormalMode) {
            this.$pageTitle.removeClass('stack-mode');
            this.$tableReport.removeClass('stack-mode');
        } else {
            this.$pageTitle.addClass('stack-mode');
            this.$tableReport.addClass('stack-mode');
        }
    };

    splitColumns = () => {
        this.levels.forEach(function (level, index) {
            alphaReports.splitCell(`col-${level}-winloss`, index % 2 ? ' altercol' : '');
        });
    };

    combineColumns = () => {
        this.levels.forEach(function (level, index) {
            const cellCss = `col-${level}-winloss-combined${index % 2 ? ' altercol' : ''}`;
            alphaReports.combineConsecutiveCells($(`td.col-${level}-winloss`), 3, cellCss);
        });
    };

    getSortedColumnName = () => {
        const sortedColumn = this.$tableReport.data('sortedcolumn');
        if (!this.isStackMode()) {
            return sortedColumn;
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
    new WinLossAnalysisView(
        new ProductMultiSelectionComponent(),
        new DateRangePickerComponent(),
        new SortComponent(),
        new ReportTooltip()
    ).init();
});
