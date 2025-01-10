class WinLossView extends BaseReport {
    productComponent: ProductMultiSelectionComponent;
    dateRangePickerComponent: DateRangePickerComponent;
    monthPickerComponent: MonthPickerComponent;
    levelTypeComponent: SingleDropdownComponent;
    sortComponent: SortComponent;
    reportTooltip: ReportTooltip;

    isAgentLevel: boolean;
    isFirstSwitchMode = true;

    constructor(
        productComponent: ProductMultiSelectionComponent,
        dateRangePickerComponent: DateRangePickerComponent,
        monthPickerComponent: MonthPickerComponent,
        levelTypeComponent: SingleDropdownComponent,
        sortComponent: SortComponent,
        reportTooltip: ReportTooltip
    ) {
        super();

        this.productComponent = productComponent;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.monthPickerComponent = monthPickerComponent;
        this.levelTypeComponent = levelTypeComponent;
        this.sortComponent = sortComponent;
        this.reportTooltip = reportTooltip;
    }

    init() {
        $.blockUI();

        this.isAgentLevel = $('#IsAgentLevel').val().toLowerCase() === 'true';
        this.initComponents();
        this.registerEvents();

        $.unblockUI();
    }

    initComponents() {
        this.productComponent.render();
        this.levelTypeComponent.render({ dropdownId: 'dropdown-level-type' });

        if ($('#IsHistoricReport').val().toLowerCase() === 'true') {
            this.monthPickerComponent.render();
        } else {
            this.dateRangePickerComponent.render();
        }

        if ($('#IsSetDefaultStackMode').val().toLowerCase() === 'true' && alphaReports.screenSmallThan768()) {
            this.setStackMode();
            this.reportTooltip.show(new ReportTooltipOption('icon-normal-column', 'winloss'));
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

    onSubmitReport = (event: any) => {
        if (!this.productComponent.validateProductIds()) {
            event.preventDefault();
            return;
        }

        super.onSubmitReport(event);
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
            cellCss += index % 2 ? '' : ' altercol';
        } else {
            cellCss += index % 2 ? ' altercol' : '';
        }

        alphaReports.combineConsecutiveCells($(`td.col-${level}-winloss`), 3, cellCss);
    }

    getSortedColumnName() {
        const sortedColumn = this.$tableReport.data('sortedcolumn');
        if (!this.isStackMode()) {
            return sortedColumn;
        }

        if (sortedColumn === 'GrossComm') {
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
    }
}

$(function () {
    new WinLossView(
        new ProductMultiSelectionComponent(),
        new DateRangePickerComponent(),
        new MonthPickerComponent(),
        new SingleDropdownComponent(),
        new SortComponent(),
        new ReportTooltip()
    ).init();
});
