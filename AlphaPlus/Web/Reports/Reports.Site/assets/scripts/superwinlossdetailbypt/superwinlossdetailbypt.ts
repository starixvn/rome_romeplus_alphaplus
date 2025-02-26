class SuperWinLossDetailByPTView extends BaseReport {
    $viewProductPTList: JQuery;
    $viewProductPTListPopup: JQuery;

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
        this.$viewProductPTList = $('#view-product-pt-list');
        this.$viewProductPTListPopup = $('#product-pt-list-popup');
    }

    initComponents() {
        if (
            $('#IsSetDefaultStackMode').val().toString().toLowerCase() === 'true' &&
            alphaReports.screenSmallThan768()
        ) {
            this.setCssBySwitchMode(false);
            this.combineColumns();
            this.isFirstSwitchMode = false;
            this.reportTooltip.show(new ReportTooltipOption('icon-normal-column', 'superwinlossdetailbypt'));
        }

        this.productComponent.render();
        this.dateRangePickerComponent.render();
        this.sortComponent.initSort(this.$tableReport, { 0: { sorter: 'text' } }, this.afterSort);

        // IsMobileView: make sure PT rows is collapsed
        $('#IsMobileView').val() === '1' &&
            this.$tableReport.find('tbody > tr.pt-row').each((_, element) => this.onClickPT($(element)));

        // remove style="visibility:hidden"
        // TableSorter cannot run with style="display:none"
        this.$tableReport.removeAttr('style');
    }

    afterSort = () => {
        this.$tableReport.find('tbody > tr.pt-row').each((_, element) => {
            const parent = $(element);
            const isHide = parent.data('is-hide');
            const ptValue = parent.data('pt-value');
            const children = this.$tableReport.find(`tbody > tr.pt-product-row[data-pt-value="${ptValue}"]`);

            children.insertAfter(parent);
            isHide && children.addClass('hide');
        });
    };

    public registerEvents() {
        $('.icon-switchmode').on('click', this.onSwitchMode);
        this.$viewProductPTList.on('click', this.onClickViewProductPTList);

        const self = this;
        $('.pt-row').click(function () {
            self.onClickPT($(this));
        });
    }

    onSwitchMode = () => {
        const isStackMode = this.isStackMode();
        this.setCssBySwitchMode(isStackMode);

        if (this.isFirstSwitchMode) {
            this.renderAtFirstSwitchMode(isStackMode);
            this.isFirstSwitchMode = false;
        }

        this.sortComponent.bindTableSorter(this.getSortedColumnName());
    };

    onClickViewProductPTList = () => {
        this.$viewProductPTListPopup.modal({ show: true, backdrop: false });

        var position = this.$viewProductPTList.position();
        this.$viewProductPTListPopup.css({
            bottom: 'auto',
            top: position.top + 60
        });
    };

    onClickPT = (ptRow: JQuery) => {
        const ptValue = ptRow.data('pt-value');
        const isHide = ptRow.data('is-hide');
        const columnUsername = ptRow.find('.col-username');
        const ptProductRows = $(`.pt-product-row[data-pt-value="${ptValue}"]`);

        if (isHide) {
            columnUsername.addClass('active');
            columnUsername.find('.icon-arrow-right').removeClass('icon-arrow-right').addClass('icon-arrow-down');
            ptProductRows.removeClass('hide');
        } else {
            columnUsername.removeClass('active');
            columnUsername.find('.icon-arrow-down').removeClass('icon-arrow-down').addClass('icon-arrow-right');
            ptProductRows.addClass('hide');
        }

        ptRow.data('is-hide', !isHide);
    };

    renderAtFirstSwitchMode = (isSwitchToNormalMode: boolean) =>
        isSwitchToNormalMode ? this.splitColumns() : this.combineColumns();

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
        alphaReports.splitCell('col-turnover');
        this.levels.forEach(function (level: string) {
            alphaReports.splitCell(`col-${level}-winloss`);
        });
    };

    combineColumns = () => {
        alphaReports.combineConsecutiveCells($('td.col-turnover'), 3, 'text-right col-turnover-combined');
        this.levels.forEach((level: string) => this.combineToWinLossColumn(level));
    };

    combineToWinLossColumn = (level: string) =>
        alphaReports.combineConsecutiveCells(
            $(`td.col-${level}-winloss`),
            3,
            `text-right col-${level}-winloss-combined`
        );

    onSubmitReport = (event: any) => {
        if (!this.productComponent.validateProductIds()) {
            event.preventDefault();
            return;
        }

        super.onSubmitReport(event);
    };

    getSortedColumnName = () => {
        const sortedColumn = this.$tableReport.data('sortedcolumn');
        if (!this.$tableReport.hasClass('stack-mode')) {
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
    new SuperWinLossDetailByPTView(
        new ProductMultiSelectionComponent(),
        new DateRangePickerComponent(),
        new SortComponent(),
        new ReportTooltip()
    ).init();
});
