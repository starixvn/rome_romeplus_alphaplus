/// <reference path="../core.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/sort.ts" />

class CommissionByBettype extends BaseReport {
    dateRangePicker: DateRangePickerComponent;
    sortComponent: SortComponent;

    constructor(dateRangePicker: DateRangePickerComponent, sortComponent: SortComponent) {
        super();
        this.dateRangePicker = dateRangePicker;
        this.sortComponent = sortComponent;
    }

    init() {
        $.blockUI();

        this.dateRangePicker.render();
        this.sortComponent.initSort(this.$tableReport, { 0: { sorter: 'text' } });
        // remove style="visibility:hidden"
        // TableSorter cannot run with style="display:none"
        this.$tableReport.removeAttr('style');

        super.registerDrilldownableEvents();

        $.unblockUI();
    }

    onSubmitReport = (event) => super.onSubmitReport(event, undefined, this.sortComponent.getSortInfo());
}

$(function () {
    new CommissionByBettype(new DateRangePickerComponent(), new SortComponent()).init();
});
