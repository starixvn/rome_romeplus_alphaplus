/// <reference path="../core.ts" />

interface JQuery {
    nexDatePicker(option: any): void;
}

class DateRangePickerOption {
    constructor(
        public selectorId: string = 'daterange-picker',
        public reportFormId: string = 'report-form',
        public onChange: Function = null,
        public onBeforeQuickChooseChange: Function = null
    ) {
        this.selectorId = selectorId;
        this.reportFormId = reportFormId;
        this.onChange = onChange;
        this.onBeforeQuickChooseChange = onBeforeQuickChooseChange;
    }
}

class DateRangePickerComponent {
    options = new DateRangePickerOption();
    quickChooseType = {
        specificDates: 7,
        olderThan3Months: 8
    };
    isCompleteInitDateRangePicker = false;
    $reportForm: JQuery;

    render = (options: DateRangePickerOption = new DateRangePickerOption()) => {
        this.options = $.extend({}, this.options, options);

        const dateRangePicker = $('#' + this.options.selectorId);
        const currentDate = dateRangePicker.data('currentdate');
        const minDate = dateRangePicker.data('mindate');
        const maxDate = dateRangePicker.data('maxdate');
        this.$reportForm = $('#' + this.options.reportFormId);

        dateRangePicker.nexDatePicker({
            datePickerFormat: {
                dateFormat: 'MM/DD/YYYY'
            },
            selectedItem: dateRangePicker.data('selecteddaterange'),
            minDate: new Date(minDate),
            maxDate: maxDate === '' ? null : maxDate,
            currentDate: new Date(currentDate),
            onSelectDatePicker: () => this.options.onChange && this.options.onChange(),
            onQuickChooseChange: (selectValue: number) => {
                this.options.onBeforeQuickChooseChange && this.options.onBeforeQuickChooseChange();

                if (this.isCompleteInitDateRangePicker && selectValue !== this.quickChooseType.specificDates) {
                    if (selectValue === this.quickChooseType.olderThan3Months) {
                        this.$reportForm.find('#historic-report').trigger('click');
                    } else {
                        this.$reportForm.submit();
                    }
                }
            }
        });

        this.isCompleteInitDateRangePicker = true;

        return dateRangePicker;
    };
}
