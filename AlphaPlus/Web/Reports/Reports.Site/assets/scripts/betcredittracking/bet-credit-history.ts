class BetCreditHistory extends BaseReport {
    dateRangePickerComponent: DateRangePickerComponent;
    pagerComponent: PagerComponent;
    $txtMember: JQuery;
    $txtRefNo: JQuery;
    resources: any;

    MAX_LENGTH_REFNO = 19;

    constructor(dateRangePickerComponent: DateRangePickerComponent, pagerComponent: PagerComponent) {
        super();
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.pagerComponent = pagerComponent;
    }

    init() {
        $.blockUI();

        this.cacheElements();
        this.initComponents();
        this.bindEvents();

        $.unblockUI();
    }

    cacheElements() {
        this.$txtMember = $('#Member');
        this.checkAndShowError();

        this.$txtRefNo = $('#RefNo');
        this.resources = {
            usernameIsRequired: $('#UsernameIsRequired').val(),
            invalidRefNo: $('#InvalidRefNo').val()
        };
    }

    initComponents() {
        this.dateRangePickerComponent.render(
            new DateRangePickerOption('daterange-picker', 'report-form', null, this.resetPaging)
        );

        this.pagerComponent.pageList = [10, 50, 100, 500];
        this.pagerComponent.render();
    }

    bindEvents() {
        this.$txtMember.on('input', () => TipsyHelper.clear(this.$txtMember));
        this.$txtRefNo.on('input', () => TipsyHelper.clear(this.$txtRefNo));
        $('.btn-submit').click(this.resetPaging);
        $('#icon-excel').click((event) => this.onExportExcel(event));
    }

    resetPaging = () => {
        $('#PageIndex').val('');
        $('#PageSize').val('');
    };

    onExportExcel = (event) => {
        $.blockUI();
        if (!this.validateUsername()) {
            event.preventDefault();
        }

        $.unblockUI();
    };

    onSubmitReport = (event: any) => {
        if (!this.validateUsername() || !this.validateRefNo()) {
            event.preventDefault();
            return;
        }

        super.onSubmitReport(event);
    };

    private validateUsername() {
        if ($.trim(this.$txtMember.val()).length) {
            TipsyHelper.clear(this.$txtMember);
            return true;
        }

        TipsyHelper.showError(this.$txtMember, this.resources.usernameIsRequired, 'nw');
        return false;
    }

    private validateRefNo() {
        const refno = $.trim(this.$txtRefNo.val());
        const isValid = refno.length <= this.MAX_LENGTH_REFNO && refno.match(/^[0-9]*$/);
        isValid
            ? TipsyHelper.clear(this.$txtRefNo)
            : TipsyHelper.showError(this.$txtRefNo, this.resources.invalidRefNo, 'nw');

        return isValid;
    }

    private checkAndShowError() {
        const $errMsg = $('#ErrorMessage');
        $errMsg.length && TipsyHelper.showError(this.$txtMember, $errMsg.val(), 'nw');
    }
}

$(function () {
    new BetCreditHistory(new DateRangePickerComponent(), new PagerComponent()).init();
});
