/// <reference path="../core.ts" />
/// <reference path="../base-report.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/client-pager.ts" />
/// <reference path="../components/link-builder.ts" />

class StatementView extends BaseReport {
    dateRangePickerComponent: DateRangePickerComponent;
    pagerComponent: ClientPagerComponent;
    topWindow: any;

    custId: string;
    custName: string;
    statementDetailsURL: string;

    popupStatementDetails: JQuery;
    popupBodyStatementDetails: JQuery;
    statementDetailView: StatementDetailView;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        pagerComponent: ClientPagerComponent,
        topWindow: any,
        linkBuilderComponent: LinkBuilderComponent
    ) {
        super($, linkBuilderComponent);

        this.dateRangePickerComponent = dateRangePickerComponent;
        this.pagerComponent = pagerComponent;
        this.topWindow = topWindow;
    }

    init() {
        this.cacheElements();
        this.dateRangePickerComponent.render();

        this.pagerComponent.pageList = this.pagerComponent.smallPageList;
        this.pagerComponent.render();

        this.registerEvents();
    }

    cacheElements() {
        this.custId = this.$('#CustId').val();
        this.custName = this.$('#CustName').val();
        const $tmpStatementDetailsURL = this.$('#StatementDetailsURL');
        this.statementDetailsURL = $tmpStatementDetailsURL.attr('href');
        $tmpStatementDetailsURL.remove();

        this.popupStatementDetails = this.$('#popup-statement-details');
        this.popupBodyStatementDetails = this.popupStatementDetails.find('.modal-body');
    }

    registerEvents() {
        this.$reportForm.submit(this.onSubmitReport);

        this.statementDetailView = new StatementDetailView(this.$, this.popupStatementDetails);
        this.$('.column-transfer-amount').on('click', (e) =>
            this.loadStatementDetail(this.$(e.target).data('transfer-date'))
        );
        this.popupStatementDetails.draggable({ handle: '.modal-header' });
    }

    onSubmitReport = () => {
        this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : !this.$reportForm.is(':hidden')));
        this.$.blockUI();
    };

    loadStatementDetail = (transferDate: string) => {
        const param = {
            CustId: this.custId,
            CustName: this.custName,
            TransferDate: transferDate
        };
        const url = `${this.statementDetailsURL}?${this.$.param(param)}`;
        const title = `Transfer Details (${this.custName})`;
        this.topWindow.popupManager.open(url, title, 900);
    };
}
