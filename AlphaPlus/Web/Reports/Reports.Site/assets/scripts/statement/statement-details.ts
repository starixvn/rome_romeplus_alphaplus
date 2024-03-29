class StatementDetailView {
    $popup: JQuery;

    constructor($popup: JQuery) {
        this.$popup = $popup;
    }

    init() {
        this.$popup.find('#tbl-report').stickyTableHeaders();
    }
}

$(function () {
    new StatementDetailView($(document)).init();
});
