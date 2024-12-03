class BetCreditHistory extends BaseReport {
    dateRangePickerComponent: DateRangePickerComponent;
    pagerComponent: PagerComponent;
    $txtMember: JQuery;
    $txtRefNo: JQuery;
    $validationMsgError: JQuery;
    resources: any;

    MAX_LENGTH_REFNO = 19;

    constructor(dateRangePickerComponent: DateRangePickerComponent, pagerComponent: PagerComponent) {
        super();
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.pagerComponent = pagerComponent;
    }

    public init() {
        $.blockUI();

        this.cacheElements();
        this.initComponents();
        this.bindEvents();

        $.unblockUI();
    }

    public cacheElements() {
        this.$txtMember = $('#Member');
        this.$txtRefNo = $('#RefNo');
        this.$validationMsgError = $('#validation-msg-error');

        this.resources = {
            usernameIsRequired: $('#UsernameIsRequired').val(),
            invalidRefNo: $('#InvalidRefNo').val()
        };
    }

    public initComponents() {
        this.dateRangePickerComponent.render(new DateRangePickerOption('daterange-picker', 'report-form', null));

        this.pagerComponent.pageList = [10, 50, 100, 500];
        this.pagerComponent.render();
    }

    public bindEvents() {
        $('.btn-submit').click(this.resetPaging());
        $('.range-option').click(this.resetPaging());

        $('#icon-excel').click((event) => this.onExportExcel(event));
    }

    resetPaging = () => {
        $('#PageIndex').val('');
        $('#PageSize').val('');
    };

    onExportExcel = (event) => {
        $.blockUI();
        if (this.validateUsername()) {
            this.hideValidationMsgError();
        } else {
            event.preventDefault();
            this.showValidationMsgError(this.resources.usernameIsRequired);
        }

        $.unblockUI();
    };

    onSubmitReport = (event: any) => {
        if (!this.validateUsername()) {
            this.showValidationMsgError(this.resources.usernameIsRequired);
            event.preventDefault();
            return;
        }

        if (!this.validateRefNo()) {
            this.showValidationMsgError(this.resources.invalidRefNo);
            event.preventDefault();
            return;
        }

        this.hideValidationMsgError();
        super.onSubmitReport(event);
    };

    validateUsername = () => $.trim(this.$txtMember.val()).length !== 0;

    validateRefNo = () => {
        const refno = $.trim(this.$txtRefNo.val());

        return refno.length <= this.MAX_LENGTH_REFNO && refno.match(/^[0-9]*$/);
    };

    showValidationMsgError = (msg) => {
        this.$validationMsgError.text(msg).removeClass('hide');
        this.$tableReport.remove();
        $('#report-pager').remove();
    };

    hideValidationMsgError = () => this.$validationMsgError.addClass('hide').text('');
}

$(function () {
    new BetCreditHistory(new DateRangePickerComponent(), new PagerComponent()).init();
});
