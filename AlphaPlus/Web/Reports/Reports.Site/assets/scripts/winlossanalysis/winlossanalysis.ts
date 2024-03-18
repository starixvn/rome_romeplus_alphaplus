/// <reference path="../core.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/tooltip.ts" />
/// <reference path="../components/productmultiselection.ts" />
/// <reference path="../components/sort.ts" />

class WinLossAnalysisView extends BaseReport {
    $multiSelectButton: JQuery;
    $isStackMode: JQuery;
    $reportTypeId: JQuery;
    $switchModeIcon: JQuery;
    $isDescendingSort: JQuery;
    $sortPropertyName: JQuery;
    $isSetDefaultStackMode: JQuery;

    isFirstSwitchMode = true;
    levels = ['member', 'agent', 'master', 'super'];

    dateRangePickerComponent: DateRangePickerComponent;
    productMultiSelectionComponent: ProductMultiSelectionComponent;
    sortComponent: SortComponent;
    reportTooltip: ReportTooltip;
    linkBuilderComponent: LinkBuilderComponent;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        productMultiSelectionComponent: ProductMultiSelectionComponent,
        sortComponent: SortComponent,
        reportTooltip: ReportTooltip,
        linkBuilderComponent: LinkBuilderComponent
    ) {
        super($, linkBuilderComponent);
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.productMultiSelectionComponent = productMultiSelectionComponent;
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
        this.bindSorting();
        this.registerEvents();

        this.$.unblockUI();
    };

    checkReady = () => this.$('#IsSetDefaultStackMode').length > 0;

    cacheDOMElements() {
        this.$isStackMode = this.$('#IsStackMode');
        this.$switchModeIcon = this.$('.icon-switchmode');
        this.$reportTypeId = this.$('#ReportTypeId');
        this.$isDescendingSort = this.$('#IsDescendingSort');
        this.$sortPropertyName = this.$('#SortPropertyName');
        this.$isSetDefaultStackMode = this.$('#IsSetDefaultStackMode');
    }

    bindFilter() {
        this.productMultiSelectionComponent.render();
        this.$multiSelectButton = this.$('#dropdown-product').parent().find('.multiselect.btn');

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

    bindSorting() {
        const canSort = this.$tableReport.data('cansort');
        this.sortComponent.render(
            new SortOption(this.$tableReport, canSort, true, undefined, { 0: { sorter: this.getSorterName() } })
        );
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
        this.$(':radio[name=reportType]').change(function () {
            self.onSubmitReport(this.form);
            self.$reportForm.submit();
        });
    }

    onSubmitReport = (e: any) => {
        var params = this.getParams();
        if (!params.IsAnyProductSelected) {
            this.showValidateProductError();
            e.preventDefault();
            return;
        }

        this.$sortPropertyName.val(this.getSortPropertyName(this.$sortPropertyName.val()));
        this.$isDescendingSort.val(String(params.IsDescendingSort));
        super.onSubmitReport(e);
    };

    getParams() {
        const selectedProducts = this.productMultiSelectionComponent.getSelectedItems();
        const sortInfo = this.sortComponent.getSortInfo();

        return {
            IsAnyProductSelected: selectedProducts.length > 0,
            SortPropertyName: sortInfo.sortPropertyName,
            IsDescendingSort: sortInfo.isDescendingSort
        };
    }

    showValidateProductError() {
        this.$multiSelectButton.addClass('error');
        this.productMultiSelectionComponent.addClassToExcludeProductLabels('error');
    }

    getSortPropertyName = (sortPropertyName: string) =>
        this.$reportTypeId.val() === 3 && sortPropertyName === 'ReportTypeName' ? '' : sortPropertyName;

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
        this.$isStackMode.val(String(isStackMode));
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
