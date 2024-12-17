/// <reference path="../core.ts" />
/// <reference path="../base-report.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/client-pager.ts" />

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
        dateRangePickerComponent: DateRangePickerComponent,
        pagerComponent: ClientPagerComponent,
        topWindow: any
    ) {
        super();

        this.dateRangePickerComponent = dateRangePickerComponent;
        this.pagerComponent = pagerComponent;
        this.topWindow = topWindow;
    }

    init() {
        this.cacheElements();
        this.dateRangePickerComponent.render(
            new DateRangePickerOption(undefined, undefined, undefined, () => $('#PageIndex').val('1'))
        );

        this.pagerComponent.pageList = this.pagerComponent.smallPageList;
        this.pagerComponent.render();

        this.registerEvents();
    }

    cacheElements() {
        this.custId = $('#CustId').val();
        this.custName = $('#CustName').val();
        const $tmpStatementDetailsURL = $('#StatementDetailsURL');
        this.statementDetailsURL = $tmpStatementDetailsURL.attr('href');
        $tmpStatementDetailsURL.remove();

        this.popupStatementDetails = $('#popup-statement-details');
        this.popupBodyStatementDetails = this.popupStatementDetails.find('.modal-body');
    }

    registerEvents() {
        this.statementDetailView = new StatementDetailView(this.popupStatementDetails);
        $('.column-transfer-amount').on('click', (e) => this.loadStatementDetail($(e.target).data('transfer-date')));
        this.popupStatementDetails.draggable({ handle: '.modal-header' });
        $('#btn-submit').click(() => $('#PageIndex').val(1));
    }

    loadStatementDetail = (transferDate: string) => {
        const param = {
            CustId: this.custId,
            CustName: this.custName,
            TransferDate: transferDate
        };
        const url = `${this.statementDetailsURL}?${$.param(param)}`;
        const title = `${$('#TransferDetails').val()} (${this.custName})`;
        this.topWindow.popupManager.open(url, title, 900);
    };
}
