/// <reference path="../core.ts" />

interface JQuery {
    tablesorter(option: any): void;
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

    initSort = ($tableReport: JQuery, headers: any, afterSortFunction: Function = undefined) => {
        this.$tableReport = $tableReport;
        this.headers = headers;
        this.afterSortFunction = afterSortFunction;
        this.canSort = this.$tableReport.data('cansort').toLowerCase() === 'true';
        const sortColumns = this.$tableReport.find('.sorting');

        if (!this.canSort) {
            sortColumns.removeClass('sorting').removeAttr('data-sortedname');
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
            const isStackMode = this.$tableReport.hasClass('stack-mode');
            const sortedColumnHeader = this.$tableReport.find(
                `[data-sortedname="${sortedColumnName}"]${
                    isStackMode ? ':not([class*="hidden-stack-mode"])' : ':not([class*="col-combine"])'
                }`
            )[0];
            const descendingSort = this.$tableReport.data('descendingsort');
            sortList = [[$(sortedColumnHeader).data('colindex'), descendingSort]];
        }

        try {
            this.$tableReport.tablesorter({
                defaultParser: 1,
                headers: this.headers,
                sortList: sortList,
                textExtraction: this.textExtraction
            });
        } catch (e) {
            console.log(e);
        }

        this.$tableReport.off('sortEnd').on('sortEnd', () => {
            let $sortedcolumn = this.$tableReport.find(
                'thead.tableFloatingHeaderOriginal th.sorting.sorting-asc:visible' +
                    ',thead.tableFloatingHeaderOriginal th.sorting.sorting-desc:visible'
            );

            const modeFilterClass = this.$tableReport.hasClass('stack-mode')
                ? '.hidden-stack-mode'
                : '.visible-stack-mode';
            $sortedcolumn = $sortedcolumn.not(modeFilterClass);

            if ($sortedcolumn.length === 1) {
                this.onSort($sortedcolumn.attr('data-sortedname'), $sortedcolumn.hasClass('sorting-desc'));
            }
        });
    };

    private textExtraction = (node: any) => {
        const $node = $(node);

        // if ($node.data('islink')) {
        //     return $node.find('a').html();
        // }

        if ($node.data('sortedvalue')) {
            return String($node.data('sortedvalue'));
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

    getSortInfo = (): SortInfo => {
        return {
            sortPropertyName: this.$tableReport.data('sortedcolumn'),
            descendingSort: this.$tableReport.data('descendingsort')
        };
    };
}
