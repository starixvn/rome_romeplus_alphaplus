/// <reference path="../core.ts" />
/// <reference path="../components/month-range-picker.ts" />
/// <reference path="../components/tooltip.ts" />
/// <reference path="../components/productmultiselection.ts" />

class WinLossAnalysisChartView extends BaseReport {
    $msgError: JQuery;

    monthRangePickerComponent: MonthRangePickerComponent;
    productComponent: ProductMultiSelectionComponent;
    reportTooltip: ReportTooltip;
    dualAxisChartComponent: DualAxisChartComponent;

    constructor(
        $: JQueryStatic,
        monthRangePickerComponent: MonthRangePickerComponent,
        productComponent: ProductMultiSelectionComponent,
        reportTooltip: ReportTooltip,
        linkBuilderComponent: LinkBuilderComponent,
        dualAxesChartComponent: DualAxisChartComponent
    ) {
        super($, linkBuilderComponent);
        this.monthRangePickerComponent = monthRangePickerComponent;
        this.productComponent = productComponent;
        this.reportTooltip = reportTooltip;
        this.dualAxisChartComponent = dualAxesChartComponent;
    }

    init() {
        this.$msgError = this.$('#msg-error');
        this.initComponents();
    }

    initComponents() {
        this.productComponent.render();
        this.monthRangePickerComponent.render();
        this.dualAxisChartComponent.render(new DualAxisChartOption(this.$('#IsDarkMode').val() === 'True'));
    }

    onSubmitReport = (event: any) => {
        let isValid = true;
        if (!this.productComponent.validateProductIds()) {
            isValid = false;
        }

        if (!this.monthRangePickerComponent.isLeast3Month()) {
            this.showValidationMonthRangeError();
            isValid = false;
        }

        if (!isValid) {
            event.preventDefault();
            return;
        }

        this.hideValidationMonthRangeError();
        super.onSubmitReport(event);
    };

    showValidationMonthRangeError = () => this.$msgError.removeClass('hide');

    hideValidationMonthRangeError = () => this.$msgError.addClass('hide');
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
