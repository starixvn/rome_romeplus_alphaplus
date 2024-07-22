interface JQuery {
    stickyTableHeaders(): void;
    stickyTableHeaders(method: string): void;
}

interface JQueryStatic {
    cookie(key: string): string;
    cookie(key: string, value: any, expires: any): void;
}

interface StoreJSStatic {
    set(key: string, value: any): void;
    get(key: string): string;
    enabled: boolean;
}

declare var store: StoreJSStatic;

class AlphaReports {
    store: StoreJSStatic;
    private defaultLocale = 'en-US';
    private localeStringOptions = { minimumFractionDigits: 2, maximumFractionDigits: 2 };

    constructor(store: StoreJSStatic) {
        this.store = store;
    }

    screenSmallThan768() {
        const currentWindowWidth = screen.width || $(window).width();

        return currentWindowWidth < 768;
    }

    // screenLandscapeMode() {
    //     return screen.width > screen.height || $(window).width() > $(window).height();
    // }

    splitCell = (firstCell: string, cellClass: string = '', lastCellStyle: string = 'bold') => {
        const combinedCells = $(`.${firstCell}-combined`);

        combinedCells.each(function (index: number) {
            const oldValues = $(this).find('li');
            const cellsLength = oldValues.length;
            let oldCells = '';

            for (let i = 0, lastCellIndex = cellsLength - 1; i < cellsLength; i++) {
                oldCells +=
                    '<td class="hidden-stack-mode ' +
                    cellClass +
                    (i === lastCellIndex ? ' ' + lastCellStyle : '') +
                    '">' +
                    $(oldValues[i]).html() +
                    '</td>';
            }

            $(combinedCells[index]).before(oldCells);
        });
    };

    combineConsecutiveCells(
        firstCells: any,
        numberOfCombinedCells: number,
        combinedCellClass: string,
        combinedCellColSpan: number = 1
    ) {
        if (numberOfCombinedCells <= 1) {
            return;
        }

        firstCells.each(function (index: number) {
            let newCell =
                '<td class="visible-stack-mode combinedcell ' +
                combinedCellClass +
                '" colspan="' +
                combinedCellColSpan +
                '"><ul>';
            let currentCell = $(firstCells[index]);
            let isLastCell = false;
            for (let i = 0; i < numberOfCombinedCells; i++) {
                newCell += '<li>' + currentCell.html() + '</li>';

                if (currentCell.next().length) {
                    currentCell = currentCell.next();
                } else {
                    isLastCell = true;
                }
            }

            newCell += '</ul></td>';

            if (isLastCell) {
                currentCell.after(newCell);
            } else {
                currentCell.prev().after(newCell);
            }
        });
    }

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
            let cookieExpire: any = { expires: 365, path: window.location.pathname };

            if (this.isIe()) {
                cookieExpire = { expires: 365 };
            }

            return cookieExpire;
        }
    };

    isIe() {
        const ua = window.navigator.userAgent;
        const msie = ua.indexOf('MSIE ');

        return msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./);
    }

    formatNumber = (value: number) => (value || 0).toLocaleString(this.defaultLocale);

    formatDecimal = (value: number) => (value || 0).toLocaleString(this.defaultLocale, this.localeStringOptions);

    formatCurrency(value: number) {
        return value.toFixed(2).replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,');
    }

    updateQueryString(queryString: string) {
        try {
            window.top['indexManager'].updateCurrentUrl(queryString);
        } catch {}
    }

    ajaxFail(jqXHR) {
        if (jqXHR.status === 429) {
            window.location.href = `${$('#main-url').val()}error/blocked`;
        } else if (jqXHR.status === 500 && jqXHR.responseText) {
            const responseObject = JSON.parse(jqXHR.responseText);
            if (responseObject.UMFrom && responseObject.UMTo) {
                window.location.href = `${$('#main-url').val()}partialum/index?${$.param(responseObject)}`;
            }
        }
    }
}

const alphaReports = new AlphaReports(store);
