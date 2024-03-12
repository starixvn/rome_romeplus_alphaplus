class BonusDetail extends BaseReport {
    private dateRangePickerComponent: DateRangePickerComponent;
    private pagerComponent: PagerComponent;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        pagerComponent: PagerComponent,
        linkBuilderComponent: LinkBuilderComponent
    ) {
        super($, linkBuilderComponent);
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.pagerComponent = pagerComponent;
    }

    public init() {
        this.$.blockUI();

        this.initComponents();
        this.bindEvents();

        this.$.unblockUI();
    }

    public initComponents() {
        this.dateRangePickerComponent.render(
            new DateRangePickerOption('daterange-picker', 'report-form', null, () => this.$('#PageIndex').val('1'))
        );

        const dropdownOptions = { numberDisplayed: 1 };
        this.$('#dropdown-statusId').multiselect(dropdownOptions);
        this.$('#dropdown-isComplained').multiselect(dropdownOptions);

        this.pagerComponent.pageList = [10, 50, 100, 500];
        this.pagerComponent.render();
    }

    public bindEvents() {
        this.$reportForm.submit(() => this.$.blockUI());
        this.$('#btn-submit').click(() => this.$('#PageIndex').val(''));

        const $popupConfirmPayment = this.$('#popup-confirmPayment');
        if ($popupConfirmPayment.length) {
            new ConfirmPayment(this.$, () => this.$reportForm.submit(), $popupConfirmPayment).init();
        }
    }
}

$(function () {
    new BonusDetail($, new DateRangePickerComponent($), new PagerComponent($), new LinkBuilderComponent($)).init();
});
