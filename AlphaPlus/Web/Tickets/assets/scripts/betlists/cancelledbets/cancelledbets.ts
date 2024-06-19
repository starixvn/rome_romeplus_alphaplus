class CancelledBetsListView {
    $betlistForm: JQuery;
    $pageTitle: JQuery;
    $isFilterVisible: JQuery;
    dateRangePicker: DateRangePickerComponent;
    productMultiSelection: ProductMultiSelectionComponent;
    statusMultiSelection: DropdownListComponent;
    pagerComponent: PagerComponent;
    constructor(
        dateRangePicker: DateRangePickerComponent,
        productMultiSelection: ProductMultiSelectionComponent,
        statusMultiSelection: DropdownListComponent,
        pagerComponent: PagerComponent
    ) {
        this.dateRangePicker = dateRangePicker;
        this.productMultiSelection = productMultiSelection;
        this.statusMultiSelection = statusMultiSelection;
        this.pagerComponent = pagerComponent;
    }

    public init() {
        this.cacheElements();
        this.registerEvents();

        this.dateRangePicker.render();
        this.productMultiSelection.render();
        this.statusMultiSelection.render('dropdown-status', {});
        this.pagerComponent.pageList = this.pagerComponent.smallPageList;
        this.pagerComponent.render();
    }

    private cacheElements() {
        this.$betlistForm = $('#betlist-form');
        this.$pageTitle = $('.bl_title');
        this.$isFilterVisible = $('#IsFilterVisible');
    }

    public registerEvents() {
        $('#betlist-filter').on('click', this.onClickIconFilter);
        this.$betlistForm.submit(this.onSubmitReport);
    }

    private onClickIconFilter = () => {
        const isHidden = this.$betlistForm.hasClass('hide');

        if (isHidden) {
            this.$pageTitle.removeClass('hide-filter');
            this.$betlistForm.removeClass('hide');
        } else {
            this.$pageTitle.addClass('hide-filter');
            this.$betlistForm.addClass('hide');
        }
    };

    private onSubmitReport = (event: any) => {
        if (!this.productMultiSelection.validateProductIds()) {
            event.preventDefault();
            return;
        }

        if (!this.statusMultiSelection.validateSelectedItems()) {
            event.preventDefault();
            return;
        }

        $.blockUI();
        const isFilterVisible = !this.$betlistForm.is(':hidden');
        this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : isFilterVisible));
    };
}

$(function () {
    new CancelledBetsListView(
        new DateRangePickerComponent(),
        new ProductMultiSelectionComponent(),
        new DropdownListComponent(),
        new PagerComponent()
    ).init();
});
