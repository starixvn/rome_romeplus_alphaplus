class DateRangePickerJQueryOption {
    constructor(public onClosed = Function(), public language = 'default') {
        this.onClosed = onClosed;
        this.language = language;
    }
}

class DateRangePickerJQueryComponent {
    private fromDate: string;
    private toDate: string;

    render(options: DateRangePickerJQueryOption) {
        const $dateRangePicker = $('#daterange-picker');
        this.fromDate = $dateRangePicker.data('fromdate');
        this.toDate = $dateRangePicker.data('todate');
        const customShortcuts = this.buildCustomShortcuts();

        $dateRangePicker
            .dateRangePicker({
                format: 'MM/DD/YYYY',
                separator: ' - ',
                language: options.language,
                startDate: $dateRangePicker.data('mindate'),
                endDate: $dateRangePicker.data('maxdate'),
                showShortcuts: typeof customShortcuts !== 'undefined',
                shortcuts: null,
                customShortcuts: customShortcuts,
                duration: 0,
                singleMonth: true,
                monthSelect: true,
                yearSelect: true,
                maxDays: 92
            })
            .bind('datepicker-change', (_, obj) => {
                this.fromDate = moment(obj.date1).format('MM/DD/YYYY');
                this.toDate = moment(obj.date2).format('MM/DD/YYYY');
            })
            .bind('datepicker-closed', () => options.onClosed(this.getDateRangeValues()))
            .data('dateRangePicker')
            .setDateRange(this.fromDate, this.toDate, false);
        $dateRangePicker.closest('.daterangepicker-container').removeClass('hide');
    }

    private buildCustomShortcuts() {
        const $customShortcuts = $('#daterange-customshortcuts');
        if ($customShortcuts.length > 0) {
            const customShortcutsValue = JSON.parse($customShortcuts.val());
            $customShortcuts.remove();

            return customShortcutsValue.map(function (element) {
                return {
                    name: element.Name,
                    dates: function () {
                        return element.Dates;
                    }
                };
            });
        }
    }

    getDateRangeValues() {
        return { fromDate: this.fromDate, toDate: this.toDate };
    }
}
