class WinLossView extends BaseReport {
    productComponent: ProductMultiSelectionComponent;
    dateRangePickerComponent: DateRangePickerComponent;
    monthPickerComponent: MonthPickerComponent;
    sortComponent: SortComponent;
    reportTooltip: ReportTooltip;

    isAgentLevel: boolean;
    isFirstSwitchMode = true;

    constructor(
        productComponent: ProductMultiSelectionComponent,
        dateRangePickerComponent: DateRangePickerComponent,
        monthPickerComponent: MonthPickerComponent,
        sortComponent: SortComponent,
        reportTooltip: ReportTooltip
    ) {
        super(new LinkBuilderComponent());

        this.productComponent = productComponent;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.monthPickerComponent = monthPickerComponent;
        this.sortComponent = sortComponent;
        this.reportTooltip = reportTooltip;
    }

    init() {
        $.blockUI();

        this.isAgentLevel = $('#IsAgentLevel').val().toLowerCase() === 'true';
        this.initComponents();
        this.registerEvents();

        // remove style="visibility:hidden"
        // TableSorter cannot run with style="display:none"
        this.$tableReport.removeAttr('style');

        $.unblockUI();
    }

    initComponents() {
        if ($('#IsSetDefaultStackMode').val().toLowerCase() === 'true' && alphaReports.screenSmallThan768()) {
            this.setStackMode();
            this.reportTooltip.show(new ReportTooltipOption('icon-normal-column', 'winloss'));
        }

        this.productComponent.render();

        if ($('#IsHistoricReport').val().toLowerCase() === 'true') {
            this.monthPickerComponent.render();
        } else {
            this.dateRangePickerComponent.render();
        }

        this.sortComponent.initSort(this.$tableReport, { 0: { sorter: 'text' } });
    }

    registerEvents() {
        $('.icon-switchmode').click(this.onSwitchMode);
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
        alphaReports.combineConsecutiveCells($('td.col-turnover'), 2, 'text-right col-turnover-combined');
    }

    combineToTurnOverColumnSuperMasterLevel() {
        alphaReports.combineConsecutiveCells($('td.col-turnover'), 3, 'text-right col-turnover-combined');
    }

    combineToWinLossColumn(level: string, index: number) {
        let cellCss = `text-right col-${level}-winloss-combined`;

        if (level === 'master') {
            cellCss = cellCss + (index % 2 ? '' : ' altercol');
        } else {
            cellCss = cellCss + (index % 2 ? ' altercol' : '');
        }

        alphaReports.combineConsecutiveCells($('td.col-' + level + '-winloss'), 3, cellCss);
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
        new ProductMultiSelectionComponent(),
        new DateRangePickerComponent(),
        new MonthPickerComponent(),
        new SortComponent(new LinkBuilderComponent()),
        new ReportTooltip()
    ).init();
});
