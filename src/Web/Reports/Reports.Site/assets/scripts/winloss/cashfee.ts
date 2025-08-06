class CashFeeReport {
    init() {
        new CompactFeebackView().initCompactFeedback(true, false, this.getCurrentFormData());
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
    new CashFeeReport().init();
});
