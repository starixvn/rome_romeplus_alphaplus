/// <reference path="../core.ts" />
/// <reference path="../components/monthpicker.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/productmultiselection.ts" />
/// <reference path="../components/tooltip.ts" />

class ProgressiveContributionView extends BaseReport {
    dateRangePickerComponent: DateRangePickerComponent;
    monthPickerComponent: MonthPickerComponent;
    productComponent: ProductMultiSelectionComponent;
    reportTooltip: ReportTooltip;

    isAgentLevel: boolean;
    custLevelId: number;
    isFirstSwitchMode = true;

    constructor(
        productComponent: ProductMultiSelectionComponent,
        dateRangePickerComponent: DateRangePickerComponent,
        monthPickerComponent: MonthPickerComponent,
        reportTooltip: ReportTooltip
    ) {
        super();

        this.productComponent = productComponent;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.monthPickerComponent = monthPickerComponent;
        this.reportTooltip = reportTooltip;
    }

    init() {
        this.cacheDOMElements();
        this.initComponents();
        this.registerEvents();
    }

    cacheDOMElements() {
        this.isAgentLevel = $('#IsAgentLevel').val().toLowerCase() === 'true';
        this.custLevelId = +$('#CustLevelId').val();
    }

    initComponents() {
        if (
            $('#IsSetDefaultStackMode').val().toString().toLowerCase() === 'true' &&
            alphaReports.screenSmallThan768()
        ) {
            this.setStackMode();
            this.reportTooltip.show(new ReportTooltipOption('icon-normal-column', 'progressivegamescontribution'));
        }

        this.productComponent.render();
        const isHistoricReport = $('#IsHistoricReport').val().toLocaleLowerCase() === 'true';
        if (isHistoricReport) {
            this.monthPickerComponent.render();
        } else {
            this.dateRangePickerComponent.render();
        }
    }

    public registerEvents() {
        $('.ic-switchmode').on('click', this.onSwitchMode);
        super.registerHistoricReportEvent();
    }

    onSwitchMode = () => {
        $.blockUI();
        const isStackMode = this.isStackMode();
        this.setCssBySwitchMode(isStackMode);

        if (this.isFirstSwitchMode) {
            this.renderAtFirstSwitchMode(isStackMode);
            this.isFirstSwitchMode = false;
        }

        $.unblockUI();
    };

    setStackMode = () => {
        if (this.isAgentLevel) {
            return;
        }

        $.blockUI();

        this.setCssBySwitchMode(false);
        this.renderAtFirstSwitchMode(false);

        this.isFirstSwitchMode = false;
        $.unblockUI();
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

    renderAtFirstSwitchMode = (isSwitchToNormalMode: boolean) => {
        const level = this.levels[this.custLevelId - 1];

        if (isSwitchToNormalMode) {
            alphaReports.splitCell(`col-${level}-contribution-share`, '', '');
        } else {
            alphaReports.combineConsecutiveCells(
                $(`td.col-${level}-contribution-share`),
                this.custLevelId - 1,
                `text-right col-${level}-contribution-share-combined`
            );
        }
    };

    onSubmitReport = (event: any) => {
        if (!this.productComponent.validateProductIds()) {
            event.preventDefault();
            return;
        }

        super.onSubmitReport(event);
    };
}

$(function () {
    new ProgressiveContributionView(
        new ProductMultiSelectionComponent(),
        new DateRangePickerComponent(),
        new MonthPickerComponent(),
        new ReportTooltip()
    ).init();
});
