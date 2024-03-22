/// <reference path="../core.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/sort.ts" />

class CommissionByBettype extends BaseReport {
    dateRangePicker: DateRangePickerComponent;
    sorter: SortComponent;
    $isDescendingSort: JQuery;
    $sortPropertyName: JQuery;

    constructor(
        $: JQueryStatic,
        dateRangePicker: DateRangePickerComponent,
        sorter: SortComponent,
        linkBuilderComponent: LinkBuilderComponent
    ) {
        super($, linkBuilderComponent);
        this.dateRangePicker = dateRangePicker;
        this.sorter = sorter;
    }

    init() {
        this.$isDescendingSort = this.$('#IsDescendingSort');
        this.$sortPropertyName = this.$('#SortPropertyName');
        this.$reportForm.submit(this.onSubmitReport);
        super.registerDrilldownableEvents();

        this.dateRangePicker.render();
        this.sorter.render(
            new SortOption(
                this.$tableReport,
                this.$tableReport.data('cansort').toLowerCase() === 'true',
                true,
                undefined,
                { 0: { sorter: 'text' } }
            )
        );
    }

    onSubmitReport = (e: any) => {
        const sortInfo = this.sorter.getSortInfo();
        this.$sortPropertyName.val(sortInfo.sortPropertyName);
        this.$isDescendingSort.val(String(sortInfo.isDescendingSort));
        super.onSubmitReport(e);
    };
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
