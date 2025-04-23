/// <reference path="../core.ts" />
interface Jquery {
    monPickr(option: any): void;
    monPickr(methodName: string, format: any): string;
}

interface MonthRangePickerOption {
    fromSelectorId?: string;
    toSelectorId?: string;
    afterChangeMonth?: Function;
}

class MonthRangePickerComponent {
    $fromDate: JQuery;
    $toDate: JQuery;
    $dropdownFromMonth: JQuery;
    $dropdownToMonth: JQuery;
    options: MonthRangePickerOption;

    render = (options?: MonthRangePickerOption) => {
        this.options = options || {};
        this.$fromDate = $('#from-date');
        this.$toDate = $('#to-date');

        this.$dropdownFromMonth = this.renderDropdown(this.options.fromSelectorId || 'from-monthpicker');
        this.$dropdownToMonth = this.renderDropdown(this.options.fromSelectorId || 'to-monthpicker');
    };

    renderDropdown = (dropdownId: string) => {
        const dropdownMonth = $('#' + dropdownId);
        const maxDate = dropdownMonth.data('maxdate');

        dropdownMonth.monPickr({
            longMonthsInYear: dropdownMonth.data('longmonths-inyear').split(','),
            maxDate: maxDate,
            minDate: dropdownMonth.data('mindate'),
            month: new Date(dropdownMonth.data('dateinmonth')),
            today: new Date(dropdownMonth.data('currentdate')),

            showDateRange: false,
            monthFormat: 'MMMM - yyyy',
            fromDayId: 'ddl-fromday',
            toDayId: 'ddl-today',
            fromDayLabel: '',
            toDayLabel: '-',
            sortDesc: true,
            log: this.logMessage,
            afterChangeMonth: this.onChange
        });

        return dropdownMonth;
    };

    logMessage = (msg: string) => console.warn(msg);

    onChange = () => {
        this.$fromDate.val(this.$dropdownFromMonth.monPickr('getFromDate', 'MM/dd/yyyy'));
        this.$toDate.val(this.$dropdownToMonth.monPickr('getToDate', 'MM/dd/yyyy'));
        this.options.afterChangeMonth && this.options.afterChangeMonth();
    };

    isLeast3Month = () => {
        const fromDate = new Date(this.$fromDate.val());
        const toDate = new Date(this.$toDate.val());

        return this.getMonthDiff(fromDate, toDate) >= 2;
    };

    getMonthDiff = (fromDate: Date, toDate: Date) => {
        return toDate.getMonth() - fromDate.getMonth() + 12 * (toDate.getFullYear() - fromDate.getFullYear());
    };

    getDateRangeValues = () => {
        return { fromDate: this.$fromDate.val(), toDate: this.$toDate.val() };
    };
}
