/// <reference path="../core.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/sort.ts" />

class CommissionByBettype extends BaseReport {
    dateRangePicker: DateRangePickerComponent;
    sortComponent: SortComponent;

    constructor(
        $: JQueryStatic,
        dateRangePicker: DateRangePickerComponent,
        sortComponent: SortComponent,
        linkBuilderComponent: LinkBuilderComponent
    ) {
        super($, linkBuilderComponent);
        this.dateRangePicker = dateRangePicker;
        this.sortComponent = sortComponent;
    }

    init() {
        super.registerDrilldownableEvents();

        this.dateRangePicker.render();
        this.sortComponent.initSort(this.$tableReport, { 0: { sorter: 'text' } });
    }

    onSubmitReport(event) {
        super.onSubmitReport(event, undefined, this.sortComponent.getSortInfo());
    }
}

$(function () {
    const linkBuilderComponent = new LinkBuilderComponent($);
    new CommissionByBettype(
        $,
        new DateRangePickerComponent($),
        new SortComponent($, linkBuilderComponent),
        linkBuilderComponent
    ).init();
});
