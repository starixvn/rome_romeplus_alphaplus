interface JQuery {
    stickyTableHeaders(): void;
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

    constructor(store: StoreJSStatic) {
        this.store = store;
    }

    format = {
        monthFormat: 'MMMM - yyyy',
        shortDateWithSlash_DatePicker: 'MM/DD/YYYY',
        shortDateWithSlash_MonthPicker: 'MM/dd/yyyy',
        monthDayWithSlash: 'MM/DD',
        dayMonthName: 'DD MMM',
        day: 'DD'
    };

    sort = {
        none: 0,
        ascending: 1,
        descending: -1
    };

    keyCode = {
        enter: 13
    };

    numberDisplayedProductsOnFilter = 1;
    maxTimesToShowTooltip = 5;
    isLocalStorageEnabled = true;

    screenSmallThan768() {
        const currentWindowWidth = screen.width || $(window).width();

        return currentWindowWidth < 768;
    }

    screenLandscapeMode() {
        return screen.width > screen.height || $(window).width() > $(window).height();
    }

    splitCell = (firstCell: string, cellClass: string = '', lastCellStyle: string = 'bold') => {
        const combinedCells = $('.' + firstCell + '-combined');

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

    replaceQueryStringValue(queryString: string, paramName: string, paramValue: any) {
        if (typeof queryString === 'undefined' || queryString === null) {
            return '';
        }

        const regex = new RegExp('([?&])' + paramName + '=[^&]*', 'i');
        const newQueryString = regex.test(queryString)
            ? queryString.replace(regex, '$1' + paramName + '=' + paramValue)
            : queryString + (queryString.indexOf('?') >= 0 ? '&' : '?') + paramName + '=' + paramValue;

        return newQueryString;
    }

    correctEvenRowClass($tableReport: JQuery) {
        let isEvenRow = false;
        $tableReport.find('tbody > tr').each(function () {
            const $row = $(this);
            if ($row.hasClass('even-row') && !isEvenRow) {
                $row.removeClass('even-row');
            } else if (!$row.hasClass('even-row') && isEvenRow) {
                $row.addClass('even-row');
            }

            isEvenRow = !isEvenRow;
        });
    }

    convertAttributeToBool(attrValue: string) {
        return !!attrValue && attrValue.toString().toLowerCase() === 'true';
    }

    formatNumber(value: number) {
        return value.toLocaleString('en-US');
    }

    formatCurrency(value: number) {
        return value.toFixed(2).replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,');
    }

    getFormData($form: JQuery): any {
        const unindexed_array = $form.serializeArray();
        const indexed_array = {};

        $.map(unindexed_array, function (n, i) {
            indexed_array[n['name']] = n['value'];
        });

        return indexed_array;
    }

    updateQueryString(queryString: string) {
        try {
            window.history.replaceState({}, '', `${window.location.href.split('?')[0]}?${queryString}`);
        } catch {}
    }
}

const alphaReports = new AlphaReports(store);
