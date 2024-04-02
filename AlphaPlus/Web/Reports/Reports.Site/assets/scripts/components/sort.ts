/// <reference path="../core.ts" />

interface JQuery {
    tablesorter(option: any): void;
}

interface SortInfo {
    sortPropertyName: string;
    descendingSort: number;
}

class SortComponent {
    linkBuilderComponent: LinkBuilderComponent;
    $tableReport: JQuery;
    headers: any;
    afterSortFunction: Function;
    canSort = false;

    constructor(linkBuilderComponent: LinkBuilderComponent) {
        this.linkBuilderComponent = linkBuilderComponent;
    }

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

        // if (!this.options.hasAllDataOnClient) {
        //     $sortContainer
        //         .find(`[data-sortedname="${$sortContainer.data('sortedcolumn')}"]`)
        //         .addClass(this.getSortClassWhenSortOnServer());

        //     sortColumns.off('click.sortcomponent').on('click.sortcomponent', function () {
        //         const $sortColumn = self.$(this);

        //         self.onSort($sortColumn.data('sortedname'), self.getSortClassWhenSortOnServer() === 'sorting-asc');
        //     });
        // }

        this.bindTableSorter(this.$tableReport.data('sortedcolumn'));
    };

    // getSortClassWhenSortOnServer = () => {
    //     const $sortContainer = this.options.sortContainer;
    //     const descendingSort = $sortContainer.data('descendingsort').toString();

    //     return descendingSort === '1' || descendingSort.toLowerCase() === 'true' ? 'sorting-desc' : 'sorting-asc';
    // };

    bindTableSorter = (sortedColumnName: string) => {
        if (!this.canSort) {
            return;
        }

        let sortList = [];
        if (sortedColumnName) {
            const sortedColumnHeader = this.$tableReport.find(
                "[data-sortedname='" +
                    sortedColumnName +
                    "']" +
                    (this.$tableReport.hasClass('stack-mode')
                        ? ":not([class*='hidden-stack-mode'])"
                        : ":not([class*='col-combine'])")
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

            const dormantAccountsRow = this.$tableReport.find('.dormant-accounts-mark');
            dormantAccountsRow.length && this.$tableReport.append(dormantAccountsRow);
        });
    };

    private textExtraction = (node: any) => {
        const $node = $(node);

        if ($node.data('islink')) {
            return $node.find('a').html();
        }

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

    getSortInfo = (): SortInfo => {
        return {
            sortPropertyName: this.$tableReport.data('sortedcolumn'),
            descendingSort: this.$tableReport.data('descendingsort')
        };
    };

    // afterSort = () => {
    //     const this$ = this.$;
    //     if (this.options.hasAllDataOnClient) {
    //         let sortedColumn = this$('.sorting-asc:visible');
    //         const direction = !sortedColumn.length;
    //         if (sortedColumn.length === 0) {
    //             sortedColumn = this$('.sorting-desc');
    //         }

    //         const sortedColumnName = sortedColumn.data('sortedname');

    //         this.linkBuilderComponent.updateParam(
    //             'SortPropertyName',
    //             sortedColumnName || sortedColumn.parent().data('sortedname')
    //         );
    //         this.linkBuilderComponent.updateParam('IsDescendingSort', direction.toString());

    //         alphaReports.correctEvenRowClass(this.options.sortContainer);
    //     } else {
    //         this.options.reportForm.trigger('submit');
    //     }
    // };

    // mapSortedColumnToStackMode = (sortedColumn: string, stackColumnGroups: string[][]) => {
    //     for (let i = 0, groupsLength = stackColumnGroups.length; i < groupsLength; i++) {
    //         const stackColumnGroup = stackColumnGroups[i];
    //         const firstColum = stackColumnGroup[0];

    //         for (let j = 0, groupLength = stackColumnGroup.length; j < groupLength; j++) {
    //             if (sortedColumn.toLowerCase() === stackColumnGroup[j].toLowerCase()) {
    //                 return firstColum;
    //             }
    //         }
    //     }

    //     return sortedColumn;
    // };
}
