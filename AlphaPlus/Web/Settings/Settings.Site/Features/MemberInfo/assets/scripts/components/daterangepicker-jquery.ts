"use strict";

class DateRangePickerJQueryComponent {
    $: JQueryStatic;
    private dateFormat = "MM/DD/YYYY";
    private locale = {
        "th-TH": "th",
        "vi-VN": "vi",
        "zh-CN": "cn",
        "zh-TW": "tc"
    };
    private fromDate: string;
    private toDate: string;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    render(onClosed: Function = undefined, options = {}) {
        const dateRangePicker = this.$("#daterange-picker");
        this.fromDate = dateRangePicker.data("fromdate");
        this.toDate = dateRangePicker.data("todate");
        options = this.$.extend({
            format: this.dateFormat,
            separator: " - ",
            language: this.locale[dateRangePicker.data("language")] || "default",
            startDate: dateRangePicker.data("mindate"),
            endDate: dateRangePicker.data("maxdate"),
            duration: 0,
            singleMonth: true,
            monthSelect: true,
            yearSelect: true
        }, options);
        
        dateRangePicker
            .dateRangePicker(options).bind("datepicker-change", (_, obj) => {
                this.fromDate = moment(obj.date1).format(this.dateFormat);
                this.toDate = moment(obj.date2).format(this.dateFormat);
            })
            .bind("datepicker-closed", () => onClosed && onClosed(this.getDateRangeValues()))
            .data("dateRangePicker").setDateRange(this.fromDate, this.toDate, false);
    }

    getDateRangeValues() {
        return { fromDate: this.fromDate, toDate: this.toDate };
    }
}
