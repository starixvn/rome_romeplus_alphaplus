/// <reference path="../core.ts" />
/// <reference path="../base-report.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/client-pager.ts" />
/// <reference path="../components/single-selection-dropdown.ts" />
/// <reference path="statement.ts" />

class MemberStatementView extends StatementView {
    reportTypeComponent: SingleSelectionDropdownComponent;
    transactionType = {
        WinLoss: 1,
        VoidCancelled: 2,
        Outstanding: 3,
        GrandPrize: 4
    };

    betListUrl: string;
    betListTitle: string;
    windowLocation: any;

    constructor(
        dateRangePickerComponent: DateRangePickerComponent,
        pagerComponent: ClientPagerComponent,
        topWindow: any,
        reportTypeComponent: SingleSelectionDropdownComponent,
        windowLocation: any
    ) {
        super(dateRangePickerComponent, pagerComponent, topWindow);

        this.reportTypeComponent = reportTypeComponent;
        this.windowLocation = windowLocation;
    }

    public init() {
        $.blockUI();

        super.init();

        const reportTypeComponentOption = new SingleSelectionDropdownOption(
            'dropdown-report-type',
            'ReportType',
            this.changeReportType
        );
        this.reportTypeComponent.render(reportTypeComponentOption);

        this.betListUrl = $('#BetListUrl').val();
        this.betListTitle = $('#BetListTitle').val();

        $.unblockUI();
    }

    public registerEvents() {
        super.registerEvents();

        const self = this;

        this.$tableReport.find('.col-void-amount').click(function () {
            self.viewBetList($(this), self.transactionType.VoidCancelled);
        });
        this.$tableReport.find('.col-outstanding').click(function () {
            self.viewBetList($(this), self.transactionType.Outstanding);
        });
        this.$tableReport.find('.col-win-loss').click(function () {
            self.viewBetList($(this), self.transactionType.WinLoss);
        });
        this.$tableReport.find('.col-grand-prize').click(function () {
            self.viewBetList($(this), self.transactionType.GrandPrize);
        });
    }

    changeReportType = () => {
        const url = this.reportTypeComponent.$dropdown.find('option:selected').data('url');
        this.windowLocation.href = url;
    };

    viewBetList = (element: JQuery, transactionType: number) => {
        const param = {
            CustId: this.custId,
            CustName: this.custName,
            WinLossDate: element.data('transfer-date'),
            TransType: transactionType
        };

        var url = `${this.betListUrl}?${$.param(param)}`;
        this.topWindow.popupManager.open(url, this.betListTitle, 900);
    };
}

$(function () {
    new MemberStatementView(
        new DateRangePickerComponent(),
        new ClientPagerComponent(),
        window.top,
        new SingleSelectionDropdownComponent(),
        window.location
    ).init();
});
