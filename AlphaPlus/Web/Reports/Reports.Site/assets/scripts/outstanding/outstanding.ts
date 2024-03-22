/// <reference path="../core.ts" />
/// <reference path="../base-report-new.ts" />

interface Window {
    $: JQueryStatic;
}

class OutstandingViewNew extends BaseReportNew {
    productMultiSelectionComponent: ProductMultiSelectionComponent;
    linkBuilderComponent: LinkBuilderComponent;
    $buttonProduct: JQuery;
    displayOutstandingLevels: any;

    constructor(
        $: JQueryStatic,
        productMultiSelectionComponent: ProductMultiSelectionComponent,
        linkBuilderComponent: LinkBuilderComponent
    ) {
        super($, linkBuilderComponent);
        this.productMultiSelectionComponent = productMultiSelectionComponent;
    }

    init = () => {
        super.init();

        this.displayOutstandingLevels = JSON.parse(this.$('#DisplayOutstandingLevelsJson').val());
        this.productMultiSelectionComponent.render();
        this.$buttonProduct = this.$('#dropdown-product').parent().find('.multiselect.btn');

        this.linkBuilderComponent.render();
        this.registerEvents();
        this.initHelp();
        this.initDataTable();
        this.registerDrilldownableEvents();
    };

    registerEvents = () => {
        super.registerEvents();
        this.$reportForm.submit(this.onSubmitReport);
    };

    initDataTable = () => {
        let colunmSortIndex = 0;
        this.$.each(this.displayOutstandingLevels, (key, value) => {
            if (
                this.$(this.$sortPropertyName).val().toLowerCase() === `${value.DisplayName}Outstanding`.toLowerCase()
            ) {
                colunmSortIndex = key + 1;
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
        this.$.blockUI();

        const params = this.getParams();
        if (!params.IsAnyProductSelected) {
            this.showValidateProductError();
            this.$.unblockUI();
            e.preventDefault();

            return;
        }

        const columnSortName = this.getColumnSortName();
        const isDescendingSort = this.isDescendingSort();

        this.replaceHrefParam('SortPropertyName', columnSortName);
        this.replaceHrefParam('IsDescendingSort', isDescendingSort);

        this.$sortPropertyName.val(columnSortName);
        this.$isDescendingSort.val(isDescendingSort ? 'True' : 'False');

        this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : params.IsFilterVisible));
    };

    getParams = () => {
        const selectedProducts = this.productMultiSelectionComponent.getSelectedItems();

        return {
            IsAnyProductSelected: selectedProducts.length > 0,
            IsFilterVisible: !this.$reportForm.is(':hidden')
        };
    };

    showValidateProductError = () => this.$buttonProduct.addClass('error');
}

$(function () {
    new OutstandingViewNew($, new ProductMultiSelectionComponent($), new LinkBuilderComponent($)).init();
});
