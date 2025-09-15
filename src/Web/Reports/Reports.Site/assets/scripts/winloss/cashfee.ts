class CashFeeReport {
    init() {
        new CompactFeebackComponent().init(this.getCurrentFormData());
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
