$(function () {
    const getCurrentFormData = () => {
        const formData = {};
        const params = new URLSearchParams(window.location.search);
        const entries = params.entries();
        for (const [key, value] of entries) {
            formData[key] = value;
        }

        return formData;
    };

    new CompactFeebackComponent().init(getCurrentFormData());
});
