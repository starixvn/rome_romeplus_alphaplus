/// <reference path="../core.ts" />
/// <reference path="../components/monthpicker.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/productmultiselection.ts" />
/// <reference path="../components/tooltip.ts" />

class ProgressiveContributionView extends BaseReport {
    dateRangePickerComponent: DateRangePickerComponent;
    monthPickerComponent: MonthPickerComponent;
    productMultiSelectionComponent: ProductMultiSelectionComponent;
    reportTooltip: ReportTooltip;

    $historicReport: JQuery;
    $buttonProduct: JQuery;
    $switchModeIcon: JQuery;
    $fromDate: JQuery;
    $toDate: JQuery;

    $isHistoricReport: JQuery;
    $isSetDefaultStackMode: JQuery;
    $isStackMode: JQuery;
    $isAgentLevel: JQuery;

    $custId: JQuery;
    $custLevelId: JQuery;

    isHistoricReport = false;
    isFirstSwitchMode = true;
    levels = ['member', 'agent', 'master', 'super'];

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        monthPickerComponent: MonthPickerComponent,
        productMultiSelectionComponent: ProductMultiSelectionComponent,
        reportTooltip: ReportTooltip,
        linkBuilderComponent: LinkBuilderComponent
    ) {
        super($, linkBuilderComponent);

        this.dateRangePickerComponent = dateRangePickerComponent;
        this.monthPickerComponent = monthPickerComponent;
        this.productMultiSelectionComponent = productMultiSelectionComponent;
        this.reportTooltip = reportTooltip;
    }

    init = () => {
        if (!this.checkReady()) {
            return;
        }

        this.cacheDOMElements();
        this.bindFilter();
        this.$tableReport.stickyTableHeaders();
        this.registerEvents();
    };

    checkReady = () => {
        return this.$('#dropdown-product').length > 0;
    };

    bindFilter = () => {
        this.productMultiSelectionComponent.render(undefined);
        this.$buttonProduct = this.$('#dropdown-product').parent().find('.multiselect.btn');

        if (
            this.$isSetDefaultStackMode.val().toString().toLowerCase() === 'true' &&
            alphaReports.screenSmallThan768()
        ) {
            this.setStackMode();
            this.reportTooltip.show(new ReportTooltipOption('icon-normal-column', parseInt(this.$custId.val(), 0)));
        }

        this.isHistoricReport = this.$isHistoricReport.val().toLocaleLowerCase() === 'true';
        if (this.isHistoricReport) {
            this.$reportForm.removeClass('hide');
            this.monthPickerComponent.render();
            if (this.$isFilterVisible.val().toLowerCase() !== 'true') {
                this.$reportForm.addClass('hide');
            }
        } else {
            this.dateRangePickerComponent.render();
        }

        if (this.$isFilterVisible.val().toString().toLowerCase() === 'false') {
            this.hideFilter();
        }
    };

    hideFilter = () => {
        this.$pageTitle.addClass('hide-filter');
        this.$reportForm.addClass('hide');
    };

    cacheDOMElements = () => {
        this.$isSetDefaultStackMode = this.$('#IsSetDefaultStackMode');
        this.$isStackMode = this.$('#IsStackMode');
        this.$isHistoricReport = this.$('#IsHistoricReport');
        this.$isAgentLevel = this.$('#IsAgentLevel');

        this.$custId = this.$('#CustId');
        this.$custLevelId = this.$('#CustLevelId');
        this.$historicReport = this.$('#historic-report');
        this.$fromDate = this.$('#from-date');
        this.$toDate = this.$('#to-date');

        this.$switchModeIcon = this.$('.icon-switchmode');
    };

    public registerEvents() {
        this.$historicReport.on('click', this.onSubmitHistoricReport);
        this.$switchModeIcon.on('click', this.onSwitchMode);
    }

    onSubmitReport = (e: any) => {
        const selectedProducts = this.productMultiSelectionComponent.getSelectedItems();

        if (selectedProducts.length === 0) {
            this.$buttonProduct.addClass('error');
            e.preventDefault();
            return;
        }

        this.$isHistoricReport.val(String(this.isHistoricReport));
        this.$isStackMode.val(String(this.isStackMode()));
        super.onSubmitReport(e);
    };

    onSubmitHistoricReport = () => {
        this.$fromDate.val('');
        this.$toDate.val('');
        this.isHistoricReport = !this.$historicReport.data('is-historic-report');
        this.$reportForm.trigger('submit');
    };

    onSwitchMode = () => {
        this.$.blockUI();
        const isStackMode = this.isStackMode();
        this.setCssBySwitchMode(isStackMode);

        if (this.isFirstSwitchMode) {
            this.renderAtFirstSwitchMode(isStackMode);
            this.isFirstSwitchMode = false;
        }

        this.$.unblockUI();
    };

    setStackMode = () => {
        let isAgentLevel = this.$isAgentLevel.val().toLocaleLowerCase() === 'true';
        if (isAgentLevel) {
            return;
        }

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

    renderAtFirstSwitchMode = (isSwitchToNormalMode: boolean) => {
        let custLevelId = parseInt(this.$custLevelId.val());
        let level = this.levels[custLevelId - 1];

        if (isSwitchToNormalMode) {
            alphaReports.splitCell('col-' + level + '-contribution-share', '', '');
        } else {
            alphaReports.combineConsecutiveCells(
                this.$('td.col-' + level + '-contribution-share'),
                custLevelId - 1,
                'text-right col-' + level + '-contribution-share-combined'
            );
        }
    };
}

$(() => {
    const progressiveContributionView = new ProgressiveContributionView(
        $,
        new DateRangePickerComponent($),
        new MonthPickerComponent($),
        new ProductMultiSelectionComponent($),
        new ReportTooltip($),
        new LinkBuilderComponent($)
    );

    progressiveContributionView.init();
});
