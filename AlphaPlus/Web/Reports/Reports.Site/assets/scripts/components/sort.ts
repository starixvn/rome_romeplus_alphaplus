/// <reference path="../core.ts" />

interface JQuery {
    tablesorter(option: any): void;
}

class SortOption {
    constructor(
        public sortContainer: any = null,
        public canSort: boolean = true,
        public hasAllDataOnClient: boolean = false,
        public afterSortFunction: any = null,
        public headers: any = null,
        public reportForm: JQuery = null
    ) {}
}

class SortComponent {
    $: JQueryStatic;
    linkBuilderComponent: LinkBuilderComponent;
    options = new SortOption();

    constructor($: JQueryStatic, linkBuilderComponent: LinkBuilderComponent) {
        this.$ = $;
        this.linkBuilderComponent = linkBuilderComponent;
    }

    render = (options: SortOption = new SortOption()) => {
        this.options = <SortOption>this.$.extend({}, this.options, options);
        const $sortContainer = this.options.sortContainer;
        const sortColumns = $sortContainer.find('[data-sortedname]');

        if (!this.options.canSort) {
            sortColumns.removeAttr('data-sortedname').removeClass('sorting');
            return;
        }

        const self = this;

        if (!this.options.hasAllDataOnClient) {
            $sortContainer
                .find(`[data-sortedname="${$sortContainer.data('sortedcolumn')}"]`)
                .addClass(this.getSortClassWhenSortOnServer());

            sortColumns.off('click.sortcomponent').on('click.sortcomponent', function () {
                const $sortColumn = self.$(this);

                self.onSort($sortColumn.data('sortedname'), self.getSortClassWhenSortOnServer() === 'sorting-asc');
            });
        }

        this.bindTableSorter($sortContainer.data('sortedcolumn'));

        return sortColumns;
    };

    getSortClassWhenSortOnServer = () => {
        const $sortContainer = this.options.sortContainer;
        const descendingSort = $sortContainer.data('descendingsort').toString();

        return descendingSort === '1' || descendingSort.toLowerCase() === 'true' ? 'sorting-desc' : 'sorting-asc';
    };

    onSort = (sortedColumnName: string, isDecendingSort: boolean) => {
        const $sortContainer = this.options.sortContainer;
        $sortContainer.data('descendingsort', isDecendingSort ? '1' : '0').data('sortedcolumn', sortedColumnName);

        if (this.options.afterSortFunction && typeof this.options.afterSortFunction === 'function') {
            this.options.afterSortFunction();
        }
    };

    getSortInfo = () => {
        const $sortContainer = this.options.sortContainer;
        return {
            sortPropertyName: $sortContainer.data('sortedcolumn'),
            isDescendingSort:
                $sortContainer.data('descendingsort').toString() === '1' ||
                $sortContainer.data('descendingsort').toString().toLowerCase() === 'true'
        };
    };

    bindTableSorter = (sortedColumnName: string) => {
        if (!this.options.canSort || !this.options.hasAllDataOnClient) {
            return;
        }

        const self = this;
        const $sortContainer = this.options.sortContainer;
        const disabledSortColumns = $sortContainer.find("th[data-disabledsort='true']");
        disabledSortColumns.each(function () {
            self.options.headers[this.cellIndex] = { sorter: false };
        });

        let sortList = [];
        if (sortedColumnName && sortedColumnName !== '') {
            let sortedColumnHeader = $sortContainer.find(
                "[data-sortedname='" +
                    sortedColumnName +
                    "']" +
                    ($sortContainer.hasClass('stack-mode')
                        ? ":not([class*='hidden-stack-mode'])"
                        : ":not([class*='col-combine'])")
            )[0];
            const descendingSort = $sortContainer.data('descendingsort').toString();
            const isDescendingSort = descendingSort === '1' || descendingSort.toLowerCase() === 'true';

            sortList = [[this.$(sortedColumnHeader).data('colindex'), isDescendingSort ? 1 : 0]];
        }

        try {
            $sortContainer.tablesorter({
                defaultParser: 1,
                headers: this.options.headers,
                sortList: sortList,
                textExtraction: this.textExtraction
            });
        } catch (e) {
            console.log(e);
        }

        $sortContainer.off('sortEnd').on('sortEnd', () => {
            let $sortedcolumn = $sortContainer.find(
                'thead.tableFloatingHeaderOriginal th.sorting.sorting-asc:visible' +
                    ',thead.tableFloatingHeaderOriginal th.sorting.sorting-desc:visible'
            );

            const modeFilterClass = $sortContainer.hasClass('stack-mode')
                ? '.hidden-stack-mode'
                : '.visible-stack-mode';
            $sortedcolumn = $sortedcolumn.not(modeFilterClass);

            if ($sortedcolumn.length === 1) {
                this.onSort($sortedcolumn.attr('data-sortedname'), $sortedcolumn.hasClass('sorting-desc'));
            }
        });
    };

    textExtraction = (node: any) => {
        const $node = this.$(node);

        if ($node.data('islink')) {
            return $node.find('a').html();
        }

        if ($node.data('sortedvalue')) {
            return String($node.data('sortedvalue'));
        }

        const html = $node.hasClass('combinedcell') ? $node.find('ul>li:first-child').html() : $node.html();

        return html;
    };

    afterSort = () => {
        const this$ = this.$;
        if (this.options.hasAllDataOnClient) {
            let sortedColumn = this$('.sorting-asc:visible');
            const direction = !sortedColumn.length;
            if (sortedColumn.length === 0) {
                sortedColumn = this$('.sorting-desc');
            }

            const sortedColumnName = sortedColumn.data('sortedname');

            this.linkBuilderComponent.updateParam(
                'SortPropertyName',
                sortedColumnName || sortedColumn.parent().data('sortedname')
            );
            this.linkBuilderComponent.updateParam('IsDescendingSort', direction.toString());

            alphaReports.correctEvenRowClass(this.options.sortContainer);
        } else {
            this.options.reportForm.trigger('submit');
        }
    };

    mapSortedColumnToStackMode = (sortedColumn: string, stackColumnGroups: string[][]) => {
        for (let i = 0, groupsLength = stackColumnGroups.length; i < groupsLength; i++) {
            const stackColumnGroup = stackColumnGroups[i];
            const firstColum = stackColumnGroup[0];

            for (let j = 0, groupLength = stackColumnGroup.length; j < groupLength; j++) {
                if (sortedColumn.toLowerCase() === stackColumnGroup[j].toLowerCase()) {
                    return firstColum;
                }
            }
        }

        return sortedColumn;
    };
}
