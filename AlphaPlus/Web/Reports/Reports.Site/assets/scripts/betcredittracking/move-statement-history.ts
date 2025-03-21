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

        const $txtMember = $('#Member');
        this.checkAndShowError($txtMember);
        $txtMember.on('input', () => TipsyHelper.clear($txtMember));

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

    private checkAndShowError($txtMember: JQuery) {
        const $errMsg = $('#ErrorMessage');
        $errMsg.length && TipsyHelper.showError($txtMember, $errMsg.val(), 'nw');
    }
}

$(function () {
    new MoveStatementHistory(new MonthPickerComponent(), new PagerComponent()).init();
});
