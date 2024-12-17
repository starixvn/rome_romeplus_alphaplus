class MoveStatementHistory extends BaseReport {
    monthPickerComponent: MonthPickerComponent;
    pagerComponent: PagerComponent;

    constructor(monthPickerComponent: MonthPickerComponent, pagerComponent: PagerComponent) {
        super();
        this.monthPickerComponent = monthPickerComponent;
        this.pagerComponent = pagerComponent;
    }

    public init() {
        $.blockUI();

        this.initComponents();
        this.bindEvents();

        $.unblockUI();
    }

    public initComponents() {
        this.monthPickerComponent.render();

        this.pagerComponent.pageList = [10, 50, 100, 500];
        this.pagerComponent.render();
    }

    public bindEvents() {
        this.$reportForm.submit(() => $.blockUI());

        $('.btn-submit').click(() => {
            $('#IsMainReport').val('true');
            $('#PageIndex').val('1');
            $('#PageSize').val('');
        });

        $('#back-link').click(() => {
            $('#IsMainReport').val('true');
            $('#PageIndex').val('1');
            this.$reportForm.submit();
        });

        $('#detail-link').click(() => {
            $('#IsMainReport').val('false');
            $('#PageIndex').val('1');
            this.$reportForm.submit();
        });
    }
}

$(function () {
    new MoveStatementHistory(new MonthPickerComponent(), new PagerComponent()).init();
});
