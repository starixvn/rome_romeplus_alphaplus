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
    tblReport = '#dropdown-visible-column';

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

        // Set sort component before initializing columns
        this.winLossColumns.setSortComponent(this.sortComponent, this.getSortedColumnName.bind(this));
        this.winLossColumns.init();

        const isServerStackMode = this.isStackMode();
        const isDefaultStackOnMobile = $('#IsSetDefaultStackMode').val().toLowerCase() === 'true' && alphaReports.screenSmallThan768();

        if (isServerStackMode || isDefaultStackOnMobile) {
            this.setStackMode();
            if (isDefaultStackOnMobile) {
                this.reportTooltip.show(new ReportTooltipOption('icon-normal-column', 'winloss'));
                super.hideFilter();
            }
            this.applyStackMode();
        } else {
            if ($(this.tblReport).length > 0) {
                this.applyNormalMode();
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
        if ($(this.tblReport).length > 0) {
            const visibleColumns = this.winLossColumns.getShowColumns().join(',');
            const userLevel = this.winLossColumns.getUserLevel();
            localStorage.setItem(userLevel + 'Columns_' + this.custId + '_' + this.subAccId, visibleColumns);
        }

        // Get sort info with correct column name mapping
        const sortInfo = this.getWinLossSortInfo();
        super.onSubmitReport(event, sortInfo);
    };

    onSwitchMode = () => {
        const isStackMode = this.isStackMode();
        this.setCssBySwitchMode(isStackMode);
        
        // Show/hide visible column component based on mode (only if component exists)
        if ($(this.tblReport).length > 0) {
            if (isStackMode) {
                // Now in normal mode
                this.applyNormalMode();
            } else {
                // Now in stack mode
                this.applyStackMode();
            }
        }
        
        this.sortComponent.bindTableSorter(this.getSortedColumnName());
    };

    // Combine/split handled in CSHTML; no-op
    renderAtFirstSwitchMode = (_: boolean) => {};

    setStackMode() {
        this.setCssBySwitchMode(false);
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

    // Removed split/combine helpers

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

    getWinLossSortInfo(): SortInfo {
        const currentSortedColumn = this.$tableReport.data('sortedcolumn');
        
        if (!currentSortedColumn) {
            return undefined;
        }

        // Map the sorted column name properly (especially important for stack mode)
        const mappedColumnName = this.getSortedColumnName();
        
        return {
            sortPropertyName: mappedColumnName,
            descendingSort: this.$tableReport.data('descendingsort')
        };
    }

    private applyStackMode() {
        // Hide normal body cells and show stack-mode cells
        this.$tableReport.find('td[data-colname], td.hidden-stack-mode').addClass('hide');
        this.$tableReport.find('td.visible-stack-mode').removeClass('hide');
        // Toggle headers: hide normal headers, show stack headers
        this.$tableReport.find('th.col-level').addClass('hide');
        this.$tableReport.find('th.visible-stack-mode').removeClass('hide');
        // Hide visible column selector UI
        this.winLossColumns.hideInStackMode();
    }

    private applyNormalMode() {
        // Hide stack-mode cells and show normal body cells
        this.$tableReport.find('td.visible-stack-mode').addClass('hide');
        this.$tableReport.find('td.hidden-stack-mode').removeClass('hide');
        // Toggle headers: show normal headers, hide stack headers
        this.$tableReport.find('th.col-level').removeClass('hide');
        this.$tableReport.find('th.visible-stack-mode').addClass('hide');
        // Show visible column selector and apply user visibility
        this.winLossColumns.showInNormalMode();
        this.winLossColumns.showHideColumns(this.$tableReport);
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
