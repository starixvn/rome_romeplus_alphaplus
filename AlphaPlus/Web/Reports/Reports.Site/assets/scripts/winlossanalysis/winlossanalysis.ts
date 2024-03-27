/// <reference path="../core.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/tooltip.ts" />
/// <reference path="../components/productmultiselection.ts" />
/// <reference path="../components/sort.ts" />

class WinLossAnalysisView extends BaseReport {
    $reportTypeId: JQuery;
    $switchModeIcon: JQuery;
    $isSetDefaultStackMode: JQuery;

    isFirstSwitchMode = true;
    levels = ['member', 'agent', 'master', 'super'];

    dateRangePickerComponent: DateRangePickerComponent;
    productComponent: ProductMultiSelectionComponent;
    sortComponent: SortComponent;
    reportTooltip: ReportTooltip;
    linkBuilderComponent: LinkBuilderComponent;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        productComponent: ProductMultiSelectionComponent,
        sortComponent: SortComponent,
        reportTooltip: ReportTooltip,
        linkBuilderComponent: LinkBuilderComponent
    ) {
        super($, linkBuilderComponent);
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.productComponent = productComponent;
        this.sortComponent = sortComponent;
        this.reportTooltip = reportTooltip;
    }

    init = () => {
        if (!this.checkReady()) {
            return;
        }

        this.$.blockUI();

        this.cacheDOMElements();
        this.bindFilter();
        this.sortComponent.initSort(this.$tableReport, { 0: { sorter: this.getSorterName() } });

        this.registerEvents();

        this.$.unblockUI();
    };

    checkReady = () => this.$('#IsSetDefaultStackMode').length > 0;

    cacheDOMElements() {
        this.$switchModeIcon = this.$('.icon-switchmode');
        this.$reportTypeId = this.$('#ReportTypeId');
        this.$isSetDefaultStackMode = this.$('#IsSetDefaultStackMode');
    }

    bindFilter() {
        this.productComponent.render();

        if (
            this.$isSetDefaultStackMode.val().toString().toLowerCase() === 'true' &&
            alphaReports.screenSmallThan768()
        ) {
            this.setStackMode();
            this.reportTooltip.show(
                new ReportTooltipOption('icon-normal-column', parseInt(this.$('#CustId').val(), 0))
            );
        }

        this.dateRangePickerComponent.render();
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
        this.$reportForm.submit(this.onSubmitReport);
        this.$switchModeIcon.on('click', this.onSwitchModeEvent);

        const self = this;
        this.$(':radio[name=ReportTypeId]').change(function () {
            self.onSubmitReport(this.form);
            self.$reportForm.submit();
        });
    }

    onSubmitReport = (event: any) => {
        if (!this.productComponent.validateProductIds()) {
            event.preventDefault();
            return;
        }

        super.onSubmitReport(event, undefined, this.sortComponent.getSortInfo());
    };

    renderAtFirstSwitchMode = (isSwitchToNormalMode: boolean) =>
        isSwitchToNormalMode ? this.splitColumns() : this.combineColumns();

    onSwitchModeEvent = () => {
        const isStackMode = this.isStackMode();
        this.setCssBySwitchMode(isStackMode);
        if (this.isFirstSwitchMode) {
            this.renderAtFirstSwitchMode(isStackMode);
            this.isFirstSwitchMode = false;
        }
        this.sortComponent.bindTableSorter(this.getSortedColumnName());
    };

    setStackMode = () => {
        this.$.blockUI();
        this.setCssBySwitchMode(false);
        this.renderAtFirstSwitchMode(false);
        this.isFirstSwitchMode = false;
        this.$.unblockUI();
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
        const this$ = this.$;
        this.levels.forEach(function (level, index) {
            const cellCss = `col-${level}-winloss-combined${index % 2 ? ' altercol' : ''}`;
            alphaReports.combineConsecutiveCells(this$(`td.col-${level}-winloss`), 3, cellCss);
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
    const linkBuilderComponent = new LinkBuilderComponent($);
    new WinLossAnalysisView(
        $,
        new DateRangePickerComponent($),
        new ProductMultiSelectionComponent($),
        new SortComponent($, linkBuilderComponent),
        new ReportTooltip($),
        linkBuilderComponent
    ).init();
});
