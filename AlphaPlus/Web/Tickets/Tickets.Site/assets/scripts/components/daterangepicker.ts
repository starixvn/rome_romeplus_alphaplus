interface JQuery {
    nexDatePicker(option: any): void;
}

class DateRangePickerOption {
    constructor(
        public selectorId: string = 'daterange-picker',
        public betlistFormId: string = 'betlist-form',
        public onChange: Function = null
    ) {
        this.selectorId = selectorId;
        this.betlistFormId = betlistFormId;
        this.onChange = onChange;
    }
}

class DateRangePickerComponent {
    options = new DateRangePickerOption();
    quickChooseType = {
        specificDates: 7
    };
    isCompleteInitDateRangePicker = false;
    $betlistForm: JQuery;

    render = (options: DateRangePickerOption = new DateRangePickerOption()) => {
        this.options = $.extend({}, this.options, options);

        const dateRangePicker = $('#' + this.options.selectorId);
        const currentDate = dateRangePicker.data('currentdate');
        const minDate = dateRangePicker.data('mindate');
        const maxDate = dateRangePicker.data('maxdate');
        this.$betlistForm = $('#' + this.options.betlistFormId);

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
                if (this.isCompleteInitDateRangePicker && selectValue !== this.quickChooseType.specificDates) {
                    $('#PageIndex').val(1);
                    this.$betlistForm.submit();
                }
            }
        });

        this.isCompleteInitDateRangePicker = true;

        return dateRangePicker;
    };
}
