class WinLossView extends BaseReport {
    isAgentLevel: boolean;

    isFirstSwitchMode = true;
    levels = ['member', 'agent', 'master', 'super'];

    productComponent: ProductMultiSelectionComponent;
    dateRangePickerComponent: DateRangePickerComponent;
    sortComponent: SortComponent;
    reportTooltip: ReportTooltip;
    monthPickerComponent: MonthPickerComponent;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        productComponent: ProductMultiSelectionComponent,
        sortComponent: SortComponent,
        reportTooltip: ReportTooltip,
        monthPickerComponent: MonthPickerComponent
    ) {
        super($, new LinkBuilderComponent($));

        this.dateRangePickerComponent = dateRangePickerComponent;
        this.productComponent = productComponent;
        this.sortComponent = sortComponent;
        this.reportTooltip = reportTooltip;
        this.monthPickerComponent = monthPickerComponent;
    }

    init() {
        this.$.blockUI();

        this.isAgentLevel = this.$('#IsAgentLevel').val().toLowerCase() === 'true';
        this.initComponents();
        this.registerEvents();

        // remove style="visibility:hidden"
        // TableSorter cannot run with style="display:none"
        this.$tableReport.removeAttr('style');

        this.$.unblockUI();
    }

    initComponents() {
        if (this.$('#IsSetDefaultStackMode').val().toLowerCase() === 'true' && alphaReports.screenSmallThan768()) {
            this.setStackMode();
            this.reportTooltip.show(
                new ReportTooltipOption('icon-normal-column', parseInt(this.$('#CustId').val(), 0))
            );
        }

        this.productComponent.render();

        if (this.$('#IsHistoricReport').val().toLowerCase() === 'true') {
            this.monthPickerComponent.render();
        } else {
            this.dateRangePickerComponent.render();
        }

        this.sortComponent.initSort(this.$tableReport, { 0: { sorter: 'text' } });
    }

    registerEvents() {
        this.$('.icon-switchmode').click(this.onSwitchMode);
        super.registerHistoricReportEvent();
        super.registerDrilldownableEvents();
    }

    onSubmitReport = (event: any) => {
        if (!this.productComponent.validateProductIds()) {
            event.preventDefault();
            return;
        }

        super.onSubmitReport(event, true, this.sortComponent.getSortInfo());
    };

    onSwitchMode = () => {
        const isStackMode = this.isStackMode();
        this.setCssBySwitchMode(isStackMode);
        if (this.isFirstSwitchMode) {
            this.renderAtFirstSwitchMode(isStackMode);
            this.isFirstSwitchMode = false;
        }
        this.sortComponent.bindTableSorter(this.getSortedColumnName());
    };

    renderAtFirstSwitchMode = (isSwitchToNormalMode: boolean) =>
        isSwitchToNormalMode ? this.splitColumns() : this.combineColumns();

    setStackMode() {
        this.setCssBySwitchMode(false);
        this.renderAtFirstSwitchMode(false);
        this.isFirstSwitchMode = false;
    }

    setCssBySwitchMode(isSwitchToNormalMode: boolean) {
        if (isSwitchToNormalMode) {
            this.$pageTitle.removeClass('stack-mode');
            this.$tableReport.removeClass('stack-mode');
        } else {
            this.$pageTitle.addClass('stack-mode');
            this.$tableReport.addClass('stack-mode');
        }
    }

    splitColumns() {
        alphaReports.splitCell('col-turnover');
        this.levels.forEach(function (level: string, index: number) {
            if (level === 'master') {
                alphaReports.splitCell(`col-${level}-winloss`, index % 2 ? '' : ' altercol');
            } else {
                alphaReports.splitCell(`col-${level}-winloss`, index % 2 ? ' altercol' : '');
            }
        });
    }

    combineColumns() {
        this.isAgentLevel ? this.combineToTurnOverColumnAgentLevel() : this.combineToTurnOverColumnSuperMasterLevel();
        this.levels.forEach((element: string, index: number) => this.combineToWinLossColumn(element, index));
    }

    combineToTurnOverColumnAgentLevel() {
        alphaReports.combineConsecutiveCells(this.$('td.col-turnover'), 2, 'text-right col-turnover-combined');
    }

    combineToTurnOverColumnSuperMasterLevel() {
        alphaReports.combineConsecutiveCells(this.$('td.col-turnover'), 3, 'text-right col-turnover-combined');
    }

    combineToWinLossColumn(level: string, index: number) {
        let cellCss = `text-right col-${level}-winloss-combined`;

        if (level === 'master') {
            cellCss = cellCss + (index % 2 ? '' : ' altercol');
        } else {
            cellCss = cellCss + (index % 2 ? ' altercol' : '');
        }

        alphaReports.combineConsecutiveCells(this.$('td.col-' + level + '-winloss'), 3, cellCss);
    }

    getSortedColumnName() {
        const sortedColumn = this.$tableReport.data('sortedcolumn');
        if (!this.isStackMode()) {
            return sortedColumn;
        }

        if (sortedColumn === 'GrossComm') {
            return 'Turnover';
        }

        if (sortedColumn === 'MemberComm' || sortedColumn === 'MembeTotal') {
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
    }
}

$(function () {
    new WinLossView(
        $,
        new DateRangePickerComponent($),
        new ProductMultiSelectionComponent($),
        new SortComponent($, new LinkBuilderComponent($)),
        new ReportTooltip($),
        new MonthPickerComponent($)
    ).init();
});
