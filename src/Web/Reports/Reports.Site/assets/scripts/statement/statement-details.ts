class StatementDetailView {
    $popup: JQuery;
    compactFeedback: CompactFeebackComponent;

    constructor($popup: JQuery, compactFeedback: CompactFeebackComponent) {
        this.$popup = $popup;
        this.compactFeedback = compactFeedback;
    }

    init() {
        this.$popup.find('#tbl-report').stickyTableHeaders();
        this.compactFeedback.init(this.getCurrentFormData());
    }

    private getCurrentFormData() {
        const formData = {};
        const params = new URLSearchParams(window.location.search);
        const entries = params.entries();
        for (const [key, value] of entries) {
            formData[key] = value;
        }

        return formData;
    }
}

$(function () {
    new StatementDetailView($(document), new CompactFeebackComponent()).init();
});
