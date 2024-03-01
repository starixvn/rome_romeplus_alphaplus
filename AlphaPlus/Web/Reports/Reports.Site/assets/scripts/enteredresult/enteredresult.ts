class EnterResultView {
    $: JQueryStatic;
    $pageTitle: JQuery;
    $tableReport: JQuery;
    $reportForm: JQuery;
    $isFilterVisible: JQuery;
    $userSelectedDateRangeOption: JQuery;
    $historicReport: JQuery;
    $isHistoricReport: JQuery;
    $filterIcon: JQuery;
    $submitButton: JQuery;
    $fromDate: JQuery;
    $toDate: JQuery;

    dateRangePickerComponent: DateRangePickerComponent;
    sortComponent: SortComponent;

    constructor($: JQueryStatic, dateRangePickerComponent: DateRangePickerComponent, sortComponent: SortComponent) {
        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.sortComponent = sortComponent;
    }

    init = () => {
        this.$.blockUI();
        this.dateRangePickerComponent.render();
        this.cacheDOMElements();

        const canSort = parseInt(this.$tableReport.data('totalrecords'), 0) > 1;
        this.sortComponent.render(
            new SortOption(this.$tableReport, canSort, true, function () {}, { 0: { sorter: 'text' } })
        );

        this.$tableReport.stickyTableHeaders();

        this.registerEvents();

        this.$.unblockUI();
    };

    cacheDOMElements = () => {
        this.$pageTitle = this.$('.page-title ');
        this.$tableReport = this.$('#tbl-enteredresult-report');
        this.$reportForm = this.$('#report-form');
        this.$isFilterVisible = this.$('#IsFilterVisible');
        this.$historicReport = this.$('#historic-report');
        this.$isHistoricReport = this.$('#IsHistoricReport');
        this.$userSelectedDateRangeOption = this.$('#UserSelectedDateRangeOption');
        this.$filterIcon = this.$('.icon-filter');
        this.$submitButton = this.$('.btn-submit');
        this.$fromDate = this.$('#from-date');
        this.$toDate = this.$('#to-date');
    };

    registerEvents = () => {
        this.$historicReport.on('click', this.onSubmitHistoricReportEvent);
        this.$filterIcon.on('click', this.onFilterEvent);
        this.$reportForm.submit(this.onSubmitNormalReportEvent);
    };

    onFilterEvent = () => {
        const currentHideState = this.$reportForm.hasClass('hide');
        currentHideState ? this.showFilter() : this.hideFilter();
    };

    showFilter = () => {
        this.$pageTitle.removeClass('hide-filter');
        this.$reportForm.removeClass('hide');
    };

    hideFilter = () => {
        this.$pageTitle.addClass('hide-filter');
        this.$reportForm.addClass('hide');
    };

    onSubmitNormalReportEvent = () => {
        this.setHiddenValue();
    };

    onSubmitHistoricReportEvent = () => {
        this.$fromDate.val('');
        this.$toDate.val('');
        //this.$isHistoricReport.val('true');
        this.setHiddenValue();
        this.$reportForm.trigger('submit');
    };

    setHiddenValue = () => {
        this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : true));
        this.$.blockUI();
    };
}

$(function () {
    new EnterResultView($, new DateRangePickerComponent($), new SortComponent($, new LinkBuilderComponent($))).init();
});
