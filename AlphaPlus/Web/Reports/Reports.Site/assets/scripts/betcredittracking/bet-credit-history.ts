class BetCreditHistory extends BaseReport {
    dateRangePickerComponent: DateRangePickerComponent;
    pagerComponent: PagerComponent;
    $txtMember: JQuery;
    $txtRefNo: JQuery;
    $validationMsgError: JQuery;
    resources: any;

    MAX_LENGTH_REFNO = 19;

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

        this.cacheElements();
        this.initComponents();
        this.bindEvents();

        this.$.unblockUI();
    }

    public cacheElements() {
        this.$txtMember = this.$('#Member');
        this.$txtRefNo = this.$('#RefNo');
        this.$validationMsgError = this.$('#validation-msg-error');

        this.resources = {
            usernameIsRequired: this.$('#UsernameIsRequired').val(),
            invalidRefNo: this.$('#InvalidRefNo').val()
        };
    }

    public initComponents() {
        this.dateRangePickerComponent.render(new DateRangePickerOption('daterange-picker', 'report-form', null));

        this.pagerComponent.pageList = [10, 50, 100, 500];
        this.pagerComponent.render();
    }

    public bindEvents() {
        this.$reportForm.submit(this.onSubmitReport);
        $('.btn-submit').click(this.resetPaging());
        $('.range-option').click(this.resetPaging());

        const $iconExcel = this.$('#icon-excel');
        $iconExcel.click((event) => this.onExportExcel(event, $iconExcel));
    }

    resetPaging = () => {
        this.$('#PageIndex').val('');
        this.$('#PageSize').val('');
    };

    onExportExcel = (event, $iconExcel: JQuery) => {
        this.$.blockUI();
        if (this.validateUsername()) {
            this.hideValidationMsgError();
            $iconExcel.trigger('click');
        } else {
            event.preventDefault();
            this.showValidationMsgError(this.resources.usernameIsRequired);
        }

        this.$.unblockUI();
    };

    onSubmitReport = (event: any) => {
        this.$.blockUI();

        if (!this.validateUsername()) {
            this.showValidationMsgError(this.resources.usernameIsRequired);
            this.$.unblockUI();
            event.preventDefault();

            return;
        }

        if (!this.validateRefNo()) {
            this.showValidationMsgError(this.resources.invalidRefNo);
            this.$.unblockUI();
            event.preventDefault();

            return;
        }

        this.hideValidationMsgError();
        return true;
    };

    validateUsername = () => this.$.trim(this.$txtMember.val()).length !== 0;

    validateRefNo = () => {
        const refno = this.$.trim(this.$txtRefNo.val());

        return refno.length <= this.MAX_LENGTH_REFNO && refno.match(/^[0-9]*$/);
    };

    showValidationMsgError = (msg) => {
        this.$validationMsgError.text(msg).removeClass('hide');
        this.$tableReport.remove();
        this.$('#report-pager').remove();
    };

    hideValidationMsgError = () => this.$validationMsgError.addClass('hide').text('');
}

$(function () {
    new BetCreditHistory($, new DateRangePickerComponent($), new PagerComponent($), new LinkBuilderComponent($)).init();
});
