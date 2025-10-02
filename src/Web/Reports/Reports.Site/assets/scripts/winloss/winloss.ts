class WinLossView extends BaseReport {
    productComponent: ProductMultiSelectionComponent;
    dateRangePickerComponent: DateRangePickerComponent;
    monthPickerComponent: MonthPickerComponent;
    levelTypeComponent: SingleDropdownComponent;
    sortComponent: SortComponent;
    reportTooltip: ReportTooltip;
    winLossColumns: WinLossColumns;

    isAgentLevel: boolean;
    isFirstSwitchMode = true;
    subAccId = $('#SubAccId').val() || 0;
    custId = $('#UserId').val() || 0;

    constructor(
        productComponent: ProductMultiSelectionComponent,
        dateRangePickerComponent: DateRangePickerComponent,
        monthPickerComponent: MonthPickerComponent,
        levelTypeComponent: SingleDropdownComponent,
        sortComponent: SortComponent,
        reportTooltip: ReportTooltip,
        winLossColumns: WinLossColumns
    ) {
        super();

        this.productComponent = productComponent;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.monthPickerComponent = monthPickerComponent;
        this.levelTypeComponent = levelTypeComponent;
        this.sortComponent = sortComponent;
        this.reportTooltip = reportTooltip;
        this.winLossColumns = winLossColumns;
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
        this.levelTypeComponent.render({ dropdownId: 'dropdown-level-type', hiddenInputId: 'level-type' });

        if ($('#IsHistoricReport').val().toLowerCase() === 'true') {
            this.monthPickerComponent.render();
        } else {
            this.dateRangePickerComponent.render();
        }

        this.winLossColumns.init();

        if ($('#IsSetDefaultStackMode').val().toLowerCase() === 'true' && alphaReports.screenSmallThan768()) {
            this.setStackMode();
            this.reportTooltip.show(new ReportTooltipOption('icon-normal-column', 'winloss'));
            super.hideFilter();
            this.winLossColumns.hideInStackMode();
            this.showAllColumnsInStackMode();
        } else {
            // Only apply column visibility if component exists (not in historic mode)
            if ($('#dropdown-visible-column').length > 0) {
                this.winLossColumns.showHideColumns(this.$tableReport);
                this.winLossColumns.showInNormalMode();
            }
        }

        const sortHeaders = this.levelTypeComponent.$dropdown.length
            ? {
                  0: { sorter: 'text' },
                  1: { sorter: 'text' }
              }
            : { 0: { sorter: 'text' } };
        this.sortComponent.initSort(this.$tableReport, sortHeaders, this.afterSortFunction);
        // remove style="visibility:hidden"
        // TableSorter cannot run with style="display:none"
        this.$tableReport.removeAttr('style');
        this.winLossColumns.showHideColumns(this.$tableReport);
    }

    registerEvents() {
        $('.ic-switchmode').click(this.onSwitchMode);
        super.registerHistoricReportEvent();
        super.registerDrilldownableEvents();
    }

    onSubmitReport = (event: any) => {
        if (!this.productComponent.validateProductIds()) {
            event.preventDefault();
            return;
        }

        // Update VisibleColumns hidden input before submit (only if component exists)
        if ($('#dropdown-visible-column').length > 0) {
            const visibleColumns = this.winLossColumns.getShowColumns().join(',');
            const userLevel = this.winLossColumns.getUserLevel();
            localStorage.setItem(userLevel + 'Columns_' + this.custId + '_' + this.subAccId, visibleColumns);
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
        
        // Show/hide visible column component based on mode (only if component exists)
        if ($('#dropdown-visible-column').length > 0) {
            if (isStackMode) {
                // Switching to normal mode - show column selector and apply column visibility
                this.winLossColumns.showInNormalMode();
                this.winLossColumns.showHideColumns(this.$tableReport);
            } else {
                // Switching to stack mode - hide column selector and show all columns
                this.winLossColumns.hideInStackMode();
                this.showAllColumnsInStackMode();
            }
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

    afterSortFunction = () => {
        this.sortComponent.handleDormantAccountsAfterSort();
        this.sortComponent.handleCashFeeDataAfterSort();
    }

    private showAllColumnsInStackMode() {
        // Show all columns when switching to stack mode
        this.$tableReport.find('[data-colname]').removeClass('hide');
        // Hide normal mode group headers (col-level) and show stack mode headers (visible-stack-mode)
        this.$tableReport.find('th.col-level').addClass('hide');
        this.$tableReport.find('th.visible-stack-mode').removeClass('hide');
    }
}

$(function () {
    new WinLossView(
        new ProductMultiSelectionComponent(),
        new DateRangePickerComponent(),
        new MonthPickerComponent(),
        new SingleDropdownComponent(),
        new SortComponent(),
        new ReportTooltip(),
        new WinLossColumns(new VisibleColumnComponent())
    ).init();
});
