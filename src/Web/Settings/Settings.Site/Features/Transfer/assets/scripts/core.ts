interface JQuery {
    stickyTableHeaders(method?: any): void;
}

class AlphaTransfer {
    store: any;

    numberDisplayedSelectedOnFilter = 1;
    pager = {
        pageSize: 500,
        pageIndex: 1
    };

    constructor(store: any) {
        this.store = store;
    }

    localStorage = {
        get: (key: string) => (this.store.enabled ? this.store.get(key) : $.cookie(key)),
        set: (key: string, value: any) =>
            this.store.enabled
                ? this.store.set(key, value)
                : $.cookie(key, value, this.localStorage.getCookieExpireOptions()),
        getCookieExpireOptions: () => {
            var cookieExpire: any = { expires: 365, path: window.location.pathname };

            if (this.isIe()) {
                cookieExpire = { expires: 365 };
            }

            return cookieExpire;
        }
    };

    isIe = () => {
        var ua = window.navigator.userAgent;
        var msie = ua.indexOf('MSIE ');

        return msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./);
    };

    bindForceDecimalInputWithFormat($input: JQuery) {
        $input.on('input', this.forceDecimalInputWithFormat);
    }

    forceDecimalInputWithFormat(e) {
        let value = e.target.value;
        const originalLength = value.length;
        const caretPosition = e.target.selectionStart;

        // Remove all non-numeric characters except for the decimal point and the minus sign
        value = value.replace(/[^0-9.-]/g, '');

        // Ensure only one decimal point is present
        const decimalCount = (value.match(/\./g) || []).length;
        if (decimalCount > 1) {
            value = value.slice(0, value.length - 1);
        }

        // Ensure only one minus sign is present and it's at the beginning
        const minusCount = (value.match(/-/g) || []).length;
        if (minusCount > 1) {
            value = value.replace(/-/g, '');
        }
        if (value.indexOf('-') > 0) {
            value = value.replace(/-/g, '');
            value = '-' + value;
        }

        // Format the value with thousands separators
        const parts = value.split('.');
        parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        value = parts.join('.');

        // Update the input field with the formatted value
        e.target.value = value;

        // Maintain cursor position
        const newLength = value.length;
        e.target.setSelectionRange(
            caretPosition + (newLength - originalLength),
            caretPosition + (newLength - originalLength)
        );
    }

    updateCurrentUrl = (queryString: string) => {
        try {
            window.top.indexManager.updateCurrentUrl(queryString);
        } catch (e) {}
    };

    showFeedback(id: number) {
        setTimeout(function () {
            try {
                window.top.indexManager && window.top.indexManager.initFeedbackFunction(id);
            } catch (e) {}
        }, 1000);
    }
}

const alphaTransfer = new AlphaTransfer(store);
