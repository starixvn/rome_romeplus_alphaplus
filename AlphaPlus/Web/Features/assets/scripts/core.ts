"use strict";

class AlphaMessages {
    $: JQueryStatic;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    format = {
        monthFormat: "MMMM - yyyy",
        shortDateWithSlash_DatePicker: "MM/DD/YYYY",
        shortDateWithSlash_MonthPicker: "MM/dd/yyyy"
    };

    sort = {
        none: 0,
        ascending: 1,
        descending: -1
    };
    
}

const alphaMessages = new AlphaMessages($);

interface JQuery {
    stickyTableHeaders(): void;
}