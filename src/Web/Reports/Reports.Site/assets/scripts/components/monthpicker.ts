/// <reference path="../core.ts" />

interface JQuery {
    monPickr(option: any): void;
    monPickr(methodName: string, format: any): string;
}

class MonthPickerOption {
    selectorId: string;

    constructor(selectorId: string = 'dropdown-month') {
        this.selectorId = selectorId;
    }
}

class MonthPickerComponent {
    $fromDate: JQuery;
    $toDate: JQuery;
    $dropdownMonth: JQuery;

    options = new MonthPickerOption();

    render = (options: MonthPickerOption = new MonthPickerOption()) => {
        var pickerOptions = $.extend({}, this.options, options);
        var dropdownMonth = $('#' + pickerOptions.selectorId);
        var maxDate = dropdownMonth.data('maxdate');
        this.$fromDate = $('#from-date');
        this.$toDate = $('#to-date');

        dropdownMonth.monPickr({
            longMonthsInYear: dropdownMonth.data('longmonths-inyear').split(','),
            maxDate: maxDate === '' ? dropdownMonth.data('currentdate') : maxDate,
            minDate: dropdownMonth.data('mindate'),
            month: new Date(dropdownMonth.data('dateinmonth')),

            fromDay: dropdownMonth.data('fromdate'),
            toDay: dropdownMonth.data('todate'),

            showDateRange: true,
            monthFormat: 'MMMM - yyyy',
            fromDayId: 'ddl-fromday',
            toDayId: 'ddl-today',
            fromDayLabel: '',
            toDayLabel: '-',
            sortDesc: true,
            log: function (msg: string) {
                console.warn(msg);
            },
            afterChangeMonth: this.onChange,
            afterChangeDay: this.onChange
        });

        this.$dropdownMonth = dropdownMonth;

        return dropdownMonth;
    };

    onChange = () => {
        this.$fromDate.val(this.$dropdownMonth.monPickr('getFromDate', 'MM/dd/yyyy'));
        this.$toDate.val(this.$dropdownMonth.monPickr('getToDate', 'MM/dd/yyyy'));
    };
}
