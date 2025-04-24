declare var store: any;
class AlphaCore {
    store: any;
    nonDigitalCharsRegex: RegExp = /\D/g;
    leadingZerosRegex: RegExp = /^0+(?!$)/;
    thousandsSeparatorRegex: RegExp = /\B(?=(\d{3})+(?!\d))/g;

    constructor(store: any) {
        this.store = store;
    }

    screenSmallThan768 = () => {
        var currentWindowWidth = screen.width || $(window).width();

        return currentWindowWidth < 768;
    };

    localStorage = {
        get: (key: string) => {
            return this.store.enabled ? this.store.get(key) : $.cookie(key);
        },

        set: (key: string, value: any) => {
            this.store.enabled
                ? this.store.set(key, value)
                : $.cookie(key, value, this.localStorage.getCookieExpireOptions());
        },

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

    replaceQueryStringValue = (queryString: string, paramName: string, paramValue: any) => {
        if (typeof queryString === 'undefined' || queryString === null) {
            return '';
        }

        var regex = new RegExp('([?&])' + paramName + '=[^&]*', 'i');

        var newQueryString = regex.test(queryString)
            ? queryString.replace(regex, '$1' + paramName + '=' + paramValue)
            : queryString + (queryString.indexOf('?') >= 0 ? '&' : '?') + paramName + '=' + paramValue;

        return newQueryString;
    };

    convertAttributeToBool = (attrValue: string) => !!attrValue && attrValue.toString().toLowerCase() === 'true';

    forceInputDecimal = (value: string) =>
        value
            .replace(this.nonDigitalCharsRegex, '')
            .replace(this.leadingZerosRegex, '')
            .replace(this.thousandsSeparatorRegex, ',');

    updateQueryString(queryString: string) {
        try {
            window.top.indexManager.updateCurrentUrl(queryString);
        } catch {}
    }
}

const alphaBetLists = new AlphaCore(store);

interface JQuery {
    stickyTableHeaders(): void;
}
