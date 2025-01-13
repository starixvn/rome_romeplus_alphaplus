/// <reference path="../core.ts" />
/// <reference path="../components/productmultiselection.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/tooltip.ts" />
/// <reference path="../components/sort.ts" />
/// <reference path="../base-report.ts" />

class ConsolidationDetailView extends BaseReport {
    productComponent: ProductMultiSelectionComponent;
    levelTypeComponent: SingleDropdownComponent;
    dateRangePickerComponent: DateRangePickerComponent;
    reportTooltip: ReportTooltip;
    sortComponent: SortComponent;

    isFirstSwitchMode = true;

    constructor(
        productComponent: ProductMultiSelectionComponent,
        levelTypeComponent: SingleDropdownComponent,
        dateRangePickerComponent: DateRangePickerComponent,
        reportTooltip: ReportTooltip,
        sortComponent: SortComponent
    ) {
        super();

        this.productComponent = productComponent;
        this.levelTypeComponent = levelTypeComponent;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.reportTooltip = reportTooltip;
        this.sortComponent = sortComponent;
    }

    init() {
        $.blockUI();

        this.initComponents();
        this.registerEvents();

        $.unblockUI();
    }

    initComponents() {
        this.productComponent.render();
        this.levelTypeComponent.render({ dropdownId: 'dropdown-level-type', hiddenInputId: 'level-type' });
        this.dateRangePickerComponent.render(
            new DateRangePickerOption(undefined, undefined, undefined, this.resetConsolidationParams)
        );
        this.sortComponent.initSort(this.$tableReport, { 0: { sorter: 'text' } });
        // remove style="visibility:hidden"
        // TableSorter cannot run with style="display:none"
        this.$tableReport.removeAttr('style');
    }

    registerEvents() {
        $('.icon-switchmode').on('click', this.onSwitchMode);
        super.registerDrilldownableEvents();

        const consolidationLink = this.$pageTitle.find('.breadcrumb-link[data-consolidation="true"]');
        consolidationLink.off().click(this.backToConsolidationReport);

        this.$reportForm.find('.btn-submit').click(this.resetConsolidationParams);
    }

    backToConsolidationReport = () => {
        $.blockUI();
        const copyAndRemove = (sourceId: string, destinationId: string) => {
            const $sourceId = $(`#${sourceId}`);
            $(`#${destinationId}`).val($sourceId.val());
            $sourceId.remove();
        };

        $('#CustId').remove();
        $('#CustName').remove();
        $('#CustLevelId').remove();
        $('#BreadcrumbsJson').remove();
        $('#ReportType').remove();
        copyAndRemove('ConsolidationProductIds', 'product-ids');
        copyAndRemove('ConsolidationResourceIds', 'resource-ids');
        copyAndRemove('ConsolidationFromDate', 'from-date');
        copyAndRemove('ConsolidationToDate', 'to-date');
        copyAndRemove('ConsolidationUserSelectedDateRangeOption', 'hidden-range-option');
        this.$reportForm.attr('action', $('#hidden-consolidation-url').attr('href')).submit();
    };

    resetConsolidationParams() {
        const copyValue = (sourceId: string, destinationId: string) =>
            $(`#${destinationId}`).val($(`#${sourceId}`).val());
        copyValue('product-ids', 'ConsolidationProductIds');
        copyValue('resource-ids', 'ConsolidationResourceIds');
        copyValue('from-date', 'ConsolidationFromDate');
        copyValue('to-date', 'ConsolidationToDate');
        copyValue('hidden-range-option', 'ConsolidationUserSelectedDateRangeOption');
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
    new ConsolidationDetailView(
        new ProductMultiSelectionComponent(),
        new SingleDropdownComponent(),
        new DateRangePickerComponent(),
        new ReportTooltip(),
        new SortComponent()
    ).init();
});
