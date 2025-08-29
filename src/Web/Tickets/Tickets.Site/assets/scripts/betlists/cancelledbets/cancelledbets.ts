class CancelledBetsListView {
    $betlistForm: JQuery;
    $pageTitle: JQuery;
    dateRangePicker: DateRangePickerComponent;
    productMultiSelection: ProductMultiSelectionComponent;
    statusMultiSelection: DropdownListComponent;
    pagerComponent: PagerComponent;
    compactFeedback: CompactFeedbackComponent;
    constructor(
        dateRangePicker: DateRangePickerComponent,
        productMultiSelection: ProductMultiSelectionComponent,
        statusMultiSelection: DropdownListComponent,
        pagerComponent: PagerComponent,
        compactFeedback: CompactFeedbackComponent
    ) {
        this.dateRangePicker = dateRangePicker;
        this.productMultiSelection = productMultiSelection;
        this.statusMultiSelection = statusMultiSelection;
        this.pagerComponent = pagerComponent;
        this.compactFeedback = compactFeedback;
    }

    public init() {
        this.cacheElements();
        this.registerEvents();

        this.dateRangePicker.render();
        this.productMultiSelection.render();
        this.statusMultiSelection.render('dropdown-status', {}, () => {
            $('#status-ids').val(this.statusMultiSelection.getSelected().join(','));
        });
        this.pagerComponent.pageList = this.pagerComponent.smallPageList;
        this.pagerComponent.render();
        this.compactFeedback.init(this.$betlistForm);
    }

    private cacheElements() {
        this.$betlistForm = $('#betlist-form');
        this.$pageTitle = $('.bl_title');
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
        $('#IsFilterVisible').val(String(alphaBetLists.screenSmallThan768() ? false : isFilterVisible));
    };
}

$(function () {
    new CancelledBetsListView(
        new DateRangePickerComponent(),
        new ProductMultiSelectionComponent(),
        new DropdownListComponent(),
        new PagerComponent(),
        new CompactFeedbackComponent()
    ).init();
});
