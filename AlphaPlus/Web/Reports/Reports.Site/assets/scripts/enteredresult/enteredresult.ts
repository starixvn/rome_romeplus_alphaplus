class EnterResultView extends BaseReport {
    dateRangePickerComponent: DateRangePickerComponent;
    sortComponent: SortComponent;

    constructor(dateRangePickerComponent: DateRangePickerComponent, sortComponent: SortComponent) {
        super();
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.sortComponent = sortComponent;
    }

    init() {
        $.blockUI();
        this.dateRangePickerComponent.render();

        super.registerHistoricReportEvent();
        this.sortComponent.initSort(this.$tableReport, { 0: { sorter: 'text' } });

        $.unblockUI();
    }
}

$(function () {
    new EnterResultView(new DateRangePickerComponent(), new SortComponent()).init();
});
