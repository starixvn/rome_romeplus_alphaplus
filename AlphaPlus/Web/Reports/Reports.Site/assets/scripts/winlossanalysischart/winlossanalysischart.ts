/// <reference path="../core.ts" />
/// <reference path="../components/month-range-picker.ts" />
/// <reference path="../components/tooltip.ts" />
/// <reference path="../components/productmultiselection.ts" />

class WinLossAnalysisChartView extends BaseReport {
    $multiSelectButton: JQuery;
    $msgError: JQuery;

    monthRangePickerComponent: MonthRangePickerComponent;
    productMultiSelectionComponent: ProductMultiSelectionComponent;
    reportTooltip: ReportTooltip;
    dualAxisChartComponent: DualAxisChartComponent;

    constructor(
        $: JQueryStatic,
        monthRangePickerComponent: MonthRangePickerComponent,
        productMultiSelectionComponent: ProductMultiSelectionComponent,
        reportTooltip: ReportTooltip,
        linkBuilderComponent: LinkBuilderComponent,
        dualAxesChartComponent: DualAxisChartComponent
    ) {
        super($, linkBuilderComponent);
        this.monthRangePickerComponent = monthRangePickerComponent;
        this.productMultiSelectionComponent = productMultiSelectionComponent;
        this.reportTooltip = reportTooltip;
        this.dualAxisChartComponent = dualAxesChartComponent;
    }

    init() {
        this.$msgError = this.$('#msg-error');
        this.initComponents();
    }

    initComponents() {
        this.productMultiSelectionComponent.render();
        this.$multiSelectButton = this.$('#dropdown-product').parent().find('.multiselect.btn');
        this.monthRangePickerComponent.render();
        this.dualAxisChartComponent.render(new DualAxisChartOption(this.$('#IsDarkMode').val() === 'True'));
    }

    onSubmitReport = (e: any) => {
        let isValid = true;
        const params = this.getParams();
        if (!params.IsAnyProductSelected) {
            this.showValidationProductError();
            isValid = false;
        }

        if (!params.IsLeast3MonthToChart) {
            this.showValidationMonthRangeError();
            isValid = false;
        }

        if (!isValid) {
            e.preventDefault();
            return;
        }

        this.hideValidationMonthRangeError();
        super.onSubmitReport(e);
    };

    showValidationProductError = () => {
        this.$multiSelectButton.addClass('error');
        this.productMultiSelectionComponent.addClassToExcludeProductLabels('error');
    };

    showValidationMonthRangeError = () => this.$msgError.removeClass('hide');
    hideValidationMonthRangeError = () => this.$msgError.addClass('hide');

    getParams() {
        const selectedProducts = this.productMultiSelectionComponent.getSelectedItems();

        return {
            IsAnyProductSelected: selectedProducts.length > 0,
            IsLeast3MonthToChart: this.monthRangePickerComponent.isLeast3Month()
        };
    }
}

declare let Chart: any;
$(function () {
    new WinLossAnalysisChartView(
        $,
        new MonthRangePickerComponent($),
        new ProductMultiSelectionComponent($),
        new ReportTooltip($),
        new LinkBuilderComponent($),
        new DualAxisChartComponent($, Chart)
    ).init();
});
