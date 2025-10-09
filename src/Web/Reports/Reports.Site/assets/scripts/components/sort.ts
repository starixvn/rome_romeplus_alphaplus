/// <reference path="../core.ts" />

interface JQuery {
    tablesorter(option: any): any;
}

interface SortInfo {
    sortPropertyName: string;
    descendingSort: number;
}

class SortComponent {
    $tableReport: JQuery;
    headers: any;
    afterSortFunction: Function;
    canSort = false;
    dataSortedName = 'data-sortedname';
    stackModeClass = 'stack-mode';

    initSort = ($tableReport: JQuery, headers: any, afterSortFunction: Function = undefined) => {
        this.$tableReport = $tableReport;
        this.headers = headers;
        this.afterSortFunction = afterSortFunction;
        this.canSort = this.$tableReport.data('cansort').toLowerCase() === 'true';
        const sortColumns = this.$tableReport.find('.sorting');

        if (!this.canSort) {
            sortColumns.removeClass('sorting').removeAttr(this.dataSortedName);
            return;
        }

        this.bindTableSorter(this.$tableReport.data('sortedcolumn'));
    };

    bindTableSorter = (sortedColumnName: string) => {
        if (!this.canSort) {
            return;
        }

        let sortList = [];
        if (sortedColumnName) {
            const isStackMode = this.$tableReport.hasClass(this.stackModeClass);
            const headerScope = 'thead';
            const sortedColumnHeader = this.$tableReport.find(
                `${headerScope} [${this.dataSortedName}="${sortedColumnName}"]${
                    isStackMode ? ':not([class*="hidden-stack-mode"])' : ':not([class*="col-combine"])'
                }:visible`
            )[0];
            const descendingSort = Number(this.$tableReport.data('descendingsort') || 0);
            if (sortedColumnHeader) {
                const colIndexAttr = $(sortedColumnHeader).attr('data-colindex');
                if (colIndexAttr) {
                    sortList = [[ colIndexAttr, descendingSort ]];
                }
            }
        }

        try {
            // Destroy existing tablesorter instance to ensure clean re-initialization
            if (this.$tableReport.hasClass('tablesorter')) {
                // Use the proper destroy method
                this.$tableReport.trigger('destroy.tablesorter');
                // Remove tablesorter classes
                this.$tableReport.removeClass('tablesorter');
            }
            
            this.$tableReport.tablesorter({
                defaultParser: 1,
                headers: this.headers,
                sortList: sortList,
                textExtraction: this.textExtraction
            });

            // Ensure the intended sort is applied immediately after re-init
            if (sortList && sortList.length) {
                this.$tableReport.trigger('sorton', [sortList]);
            }
        } catch {
        }

        this.$tableReport.off('sortEnd').on('sortEnd', () => {
            let $sortedcolumn = this.$tableReport.find(
                'thead.tableFloatingHeaderOriginal th.sorting.sorting-asc:visible' +
                    ',thead.tableFloatingHeaderOriginal th.sorting.sorting-desc:visible'
            );

            const modeFilterClass = this.$tableReport.hasClass(this.stackModeClass)
                ? '.hidden-stack-mode'
                : '.visible-stack-mode';
            $sortedcolumn = $sortedcolumn.not(modeFilterClass);

            if ($sortedcolumn.length !== 1) {
                // Fallback to main thead if floating header not present or not updated yet
                $sortedcolumn = this.$tableReport.find(
                    'thead:not(.tableFloatingHeaderOriginal) th.sorting.sorting-asc:visible' +
                        ',thead:not(.tableFloatingHeaderOriginal) th.sorting.sorting-desc:visible'
                ).not(modeFilterClass);
            }

            if ($sortedcolumn.length === 1) {
                this.onSort($sortedcolumn.attr(this.dataSortedName), $sortedcolumn.hasClass('sorting-desc'));
            }
        });
    };

    private textExtraction = (node: any) => {
        const $node = $(node);

        // if ($node.data('islink')) {
        //     return $node.find('a').html();
        // }
        const sortedValue = $node.data('sortedvalue');
        if (sortedValue) {
            return String(sortedValue);
        }

        const html = $node.hasClass('combinedcell') ? $node.find('ul>li:first-child').html() : $node.html();

        return html;
    };

    private onSort = (sortedColumnName: string, isDecendingSort: boolean) => {
        this.$tableReport.data('descendingsort', isDecendingSort ? 1 : 0).data('sortedcolumn', sortedColumnName);

        this.afterSortFunction && this.afterSortFunction();
    };

    handleDormantAccountsAfterSort = () => {
        const dormantAccountsRow = this.$tableReport.find('.dormant-accounts-mark');
        dormantAccountsRow.length && this.$tableReport.append(dormantAccountsRow);
    };

    handleCashFeeDataAfterSort = () => {
        const cashFeeRow = this.$tableReport.find('.cash-fee-row');
        cashFeeRow.length && this.$tableReport.append(cashFeeRow);
    };

    // getSortInfo = (): SortInfo => {
    //     return {
    //         sortPropertyName: this.$tableReport.data('sortedcolumn'),
    //         descendingSort: this.$tableReport.data('descendingsort')
    //     };
    // };
}
