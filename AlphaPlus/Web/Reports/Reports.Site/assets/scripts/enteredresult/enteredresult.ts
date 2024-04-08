class EnterResultView extends BaseReport {
    dateRangePickerComponent: DateRangePickerComponent;
    sortComponent: SortComponent;

    constructor(
        dateRangePickerComponent: DateRangePickerComponent,
        sortComponent: SortComponent,
        linkBuilderComponent: LinkBuilderComponent
    ) {
        super(linkBuilderComponent);
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
    const linkBuilderComponent = new LinkBuilderComponent();
    new EnterResultView(new DateRangePickerComponent(), new SortComponent(), linkBuilderComponent).init();
});
