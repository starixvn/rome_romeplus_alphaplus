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

    $historicReport: JQuery;
    $switchModeIcon: JQuery;

    $isHistoricReport: JQuery;
    $isSetDefaultStackMode: JQuery;
    $isAgentLevel: JQuery;

    $custId: JQuery;
    $custLevelId: JQuery;

    isHistoricReport = false;
    isFirstSwitchMode = true;

    constructor(
        productComponent: ProductMultiSelectionComponent,
        dateRangePickerComponent: DateRangePickerComponent,
        monthPickerComponent: MonthPickerComponent,
        reportTooltip: ReportTooltip,
        linkBuilderComponent: LinkBuilderComponent
    ) {
        super(linkBuilderComponent);

        this.productComponent = productComponent;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.monthPickerComponent = monthPickerComponent;
        this.reportTooltip = reportTooltip;
    }

    init() {
        this.cacheDOMElements();
        this.bindFilter();
        this.registerEvents();
    }

    cacheDOMElements = () => {
        this.$isSetDefaultStackMode = $('#IsSetDefaultStackMode');
        this.$isHistoricReport = $('#IsHistoricReport');
        this.$isAgentLevel = $('#IsAgentLevel');

        this.$custId = $('#CustId');
        this.$custLevelId = $('#CustLevelId');
        this.$historicReport = $('#historic-report');

        this.$switchModeIcon = $('.icon-switchmode');
    };

    bindFilter = () => {
        this.productComponent.render();

        if (
            this.$isSetDefaultStackMode.val().toString().toLowerCase() === 'true' &&
            alphaReports.screenSmallThan768()
        ) {
            this.setStackMode();
            this.reportTooltip.show(new ReportTooltipOption('icon-normal-column', 'progressivegamescontribution'));
        }

        this.isHistoricReport = this.$isHistoricReport.val().toLocaleLowerCase() === 'true';
        if (this.isHistoricReport) {
            this.monthPickerComponent.render();
        } else {
            this.dateRangePickerComponent.render();
        }
    };

    public registerEvents() {
        super.registerHistoricReportEvent();
        this.$switchModeIcon.on('click', this.onSwitchMode);
    }

    onSubmitReport = (event: any) => {
        if (!this.productComponent.validateProductIds()) {
            event.preventDefault();
            return;
        }

        super.onSubmitReport(event, true);
    };

    onSubmitHistoricReport = () => {
        this.isHistoricReport = !this.$historicReport.data('is-historic-report');
    };

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
        let isAgentLevel = this.$isAgentLevel.val().toLocaleLowerCase() === 'true';
        if (isAgentLevel) {
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
        let custLevelId = parseInt(this.$custLevelId.val());
        let level = this.levels[custLevelId - 1];

        if (isSwitchToNormalMode) {
            alphaReports.splitCell('col-' + level + '-contribution-share', '', '');
        } else {
            alphaReports.combineConsecutiveCells(
                $('td.col-' + level + '-contribution-share'),
                custLevelId - 1,
                'text-right col-' + level + '-contribution-share-combined'
            );
        }
    };
}

$(function () {
    new ProgressiveContributionView(
        new ProductMultiSelectionComponent(),
        new DateRangePickerComponent(),
        new MonthPickerComponent(),
        new ReportTooltip(),
        new LinkBuilderComponent()
    ).init();
});
