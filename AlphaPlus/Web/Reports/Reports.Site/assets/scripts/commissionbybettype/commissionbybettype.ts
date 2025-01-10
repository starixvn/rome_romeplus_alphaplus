class CommissionByBettype extends BaseReport {
    levelTypeComponent: SingleDropdownComponent;
    dateRangePicker: DateRangePickerComponent;
    sortComponent: SortComponent;

    constructor(
        levelTypeComponent: SingleDropdownComponent,
        dateRangePicker: DateRangePickerComponent,
        sortComponent: SortComponent
    ) {
        super();
        this.levelTypeComponent = levelTypeComponent;
        this.dateRangePicker = dateRangePicker;
        this.sortComponent = sortComponent;
    }

    init() {
        $.blockUI();

        this.levelTypeComponent.render({ dropdownId: 'dropdown-level-type' });
        this.dateRangePicker.render();
        this.sortComponent.initSort(this.$tableReport, { 0: { sorter: 'text' } });
        // remove style="visibility:hidden"
        // TableSorter cannot run with style="display:none"
        this.$tableReport.removeAttr('style');

        super.registerDrilldownableEvents();

        $.unblockUI();
    }
}

$(function () {
    new CommissionByBettype(new SingleDropdownComponent(), new DateRangePickerComponent(), new SortComponent()).init();
});
