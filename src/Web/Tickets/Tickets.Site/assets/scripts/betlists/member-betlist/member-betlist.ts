class MemberBetListView {
    $betlistForm: JQuery;
    $pageTitle: JQuery;
    $getallCheckbox: JQuery;

    betlistTypeComponent: DropdownListComponent;
    productMultiSelectionComponent: ProductMultiSelectionComponent;
    dateRangePickerComponent: DateRangePickerComponent;
    compactFeedback: CompactFeedbackComponent;

    constructor(
        betlistTypeComponent: DropdownListComponent,
        productMultiSelectionComponent: ProductMultiSelectionComponent,
        dateRangePickerComponent: DateRangePickerComponent,
        compactFeedback: CompactFeedbackComponent
    ) {
        this.betlistTypeComponent = betlistTypeComponent;
        this.productMultiSelectionComponent = productMultiSelectionComponent;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.compactFeedback = compactFeedback;
    }

    init() {
        this.cacheElements();
        this.bindFilter();
        this.registerEvents();
        this.compactFeedback.init(this.$betlistForm);
    }

    protected cacheElements() {
        this.$betlistForm = $('#betlist-form');
        this.$pageTitle = $('.bl_title');
        this.$getallCheckbox = $('#GetAll');
    }

    protected bindFilter() {
        this.betlistTypeComponent.render(
            'dropdown-betlist-type',
            {},
            (option) => (window.location.href = option.data('url'))
        );
        this.productMultiSelectionComponent.render();
        this.dateRangePickerComponent.render();
        this.$getallCheckbox.length &&
            this.$getallCheckbox
                .change(function () {
                    const checked = this.checked;
                    $('#from-date').prop('disabled', checked);
                    $('#to-date').prop('disabled', checked);
                    $('#dropdown-daterange').parent().find('.btn-submit').prop('disabled', checked);
                })
                .trigger('change');
    }

    protected registerEvents() {
        $('.icon-filter').on('click', () => this.onClickIconFilter());
        this.$betlistForm.submit((event) => this.onSubmitEvent(event));
    }

    private onClickIconFilter() {
        const currentHideState = this.$betlistForm.hasClass('hide');

        if (currentHideState) {
            this.$pageTitle.removeClass('hide-filter');
            this.$betlistForm.removeClass('hide');
        } else {
            this.$pageTitle.addClass('hide-filter');
            this.$betlistForm.addClass('hide');
        }
    }

    private onSubmitEvent(event: any) {
        if (!this.productMultiSelectionComponent.validateProductIds()) {
            event.preventDefault();
            return;
        }

        $.blockUI();
        const isFilterVisible = !this.$betlistForm.is(':hidden');
        $('#IsFilterVisible').val(String(alphaBetLists.screenSmallThan768() ? false : isFilterVisible));
    }
}

$(function () {
    new MemberBetListView(
        new DropdownListComponent(),
        new ProductMultiSelectionComponent(),
        new DateRangePickerComponent(),
        new CompactFeedbackComponent()
    ).init();
});
