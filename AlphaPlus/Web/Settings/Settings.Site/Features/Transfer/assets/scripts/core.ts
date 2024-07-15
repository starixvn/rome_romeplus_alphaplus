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
}

const alphaTransfer = new AlphaTransfer(store);
