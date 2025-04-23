class CommissionByBetType extends BaseReport {
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

        this.levelTypeComponent.render({ dropdownId: 'dropdown-level-type', hiddenInputId: 'level-type' });
        this.dateRangePicker.render();

        const sortHeaders = this.levelTypeComponent.$dropdown.length
            ? {
                  0: { sorter: 'text' },
                  1: { sorter: 'text' }
              }
            : { 0: { sorter: 'text' } };
        this.sortComponent.initSort(this.$tableReport, sortHeaders);
        // remove style="visibility:hidden"
        // TableSorter cannot run with style="display:none"
        this.$tableReport.removeAttr('style');

        super.registerDrilldownableEvents();

        $.unblockUI();
    }
}

$(function () {
    new CommissionByBetType(new SingleDropdownComponent(), new DateRangePickerComponent(), new SortComponent()).init();
});
