/// <reference path="../core.ts" />
/// <reference path="../base-report-new.ts" />

interface Window {
    $: JQueryStatic;
}

class OutstandingViewNew extends BaseReportNew {
    productComponent: ProductMultiSelectionComponent;
    linkBuilderComponent: LinkBuilderComponent;
    displayOutstandingLevels: any;

    constructor(productComponent: ProductMultiSelectionComponent, linkBuilderComponent: LinkBuilderComponent) {
        super(linkBuilderComponent);
        this.productComponent = productComponent;
    }

    init() {
        super.init();

        this.displayOutstandingLevels = JSON.parse($('#DisplayOutstandingLevelsJson').val());
        this.productComponent.render();

        this.linkBuilderComponent.render();
        this.registerEvents();
        this.initHelp();
        this.initDataTable();
        this.registerDrilldownableEvents();
    }

    registerEvents = () => {
        super.registerEvents();
        this.$reportForm.submit(this.onSubmitReport);
    };

    initDataTable = () => {
        let colunmSortIndex = 0;
        this.displayOutstandingLevels.forEach((element, index) => {
            if ($(this.$sortPropertyName).val().toLowerCase() === `${element.DisplayName}Outstanding`.toLowerCase()) {
                colunmSortIndex = index + 1;
                return;
            }
        });

        super.initDataTable({
            order: [[colunmSortIndex, JSON.parse(this.$isDescendingSort.val().toLowerCase()) ? 'desc' : 'asc']]
        });
    };

    triggerRedrawTable = () => {
        super.triggerRedrawTable();

        this.replaceHrefParam('SortPropertyName', this.getColumnSortName());
        this.replaceHrefParam('IsDescendingSort', this.isDescendingSort());
    };

    getColumnSortName = () => {
        const order = this.$dataTable.order();
        const length = this.displayOutstandingLevels.length;

        for (let index = 0; index < length; index++) {
            if (order[0][0] - 1 === index) {
                return `${this.displayOutstandingLevels[index].DisplayName}Outstanding`;
            }
        }

        return 'CustName';
    };

    onSubmitReport = (e: any) => {
        $.blockUI();

        if (!this.productComponent.validateProductIds()) {
            $.unblockUI();
            e.preventDefault();

            return;
        }

        const columnSortName = this.getColumnSortName();
        const isDescendingSort = this.isDescendingSort();

        this.replaceHrefParam('SortPropertyName', columnSortName);
        this.replaceHrefParam('IsDescendingSort', isDescendingSort);

        this.$sortPropertyName.val(columnSortName);
        this.$isDescendingSort.val(isDescendingSort ? 'True' : 'False');

        this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : !this.$reportForm.is(':hidden')));
    };
}

$(function () {
    new OutstandingViewNew(new ProductMultiSelectionComponent(), new LinkBuilderComponent()).init();
});
