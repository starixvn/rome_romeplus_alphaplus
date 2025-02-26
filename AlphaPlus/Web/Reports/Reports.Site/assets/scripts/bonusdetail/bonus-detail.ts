class BonusDetail extends BaseReport {
    private dateRangePickerComponent: DateRangePickerComponent;
    private pagerComponent: PagerComponent;

    constructor(dateRangePickerComponent: DateRangePickerComponent, pagerComponent: PagerComponent) {
        super();
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.pagerComponent = pagerComponent;
    }

    public init() {
        $.blockUI();

        this.initComponents();
        this.bindEvents();

        $.unblockUI();
    }

    public initComponents() {
        this.dateRangePickerComponent.render(
            new DateRangePickerOption('daterange-picker', 'report-form', null, () => $('#PageIndex').val('1'))
        );

        const dropdownOptions = { numberDisplayed: 1 };
        $('#dropdown-statusId').multiselect(dropdownOptions);
        $('#dropdown-isComplained').multiselect(dropdownOptions);

        this.pagerComponent.pageList = [10, 50, 100, 500];
        this.pagerComponent.render();
    }

    public bindEvents() {
        $('#btn-submit').click(() => $('#PageIndex').val(''));

        const $popupConfirmPayment = $('#popup-confirmPayment');
        if ($popupConfirmPayment.length) {
            new ConfirmPayment(() => this.$reportForm.submit(), $popupConfirmPayment).init();
        }
    }
}

$(function () {
    new BonusDetail(new DateRangePickerComponent(), new PagerComponent()).init();
});
