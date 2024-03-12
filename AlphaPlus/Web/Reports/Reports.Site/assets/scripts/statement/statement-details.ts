class StatementDetailView {
    $: JQueryStatic;
    $popup: JQuery;

    constructor($: JQueryStatic, $popup: JQuery) {
        this.$ = $;
        this.$popup = $popup;
    }

    init() {
        this.$popup.find('#tbl-report').stickyTableHeaders();
    }
}

$(function () {
    new StatementDetailView($, $(document)).init();
});
