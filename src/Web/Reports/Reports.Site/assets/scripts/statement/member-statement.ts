/// <reference path="../core.ts" />
/// <reference path="../base-report.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/client-pager.ts" />
/// <reference path="../components/single-dropdown.ts" />
/// <reference path="statement.ts" />

class MemberStatementView extends StatementView {
    reportTypeComponent: SingleDropdownComponent;
    transactionType = {
        WinLoss: 1,
        VoidCancelled: 2,
        Outstanding: 3,
        GrandPrize: 4
    };

    windowLocation: any;

    constructor(
        dateRangePickerComponent: DateRangePickerComponent,
        pagerComponent: ClientPagerComponent,
        topWindow: any,
        reportTypeComponent: SingleDropdownComponent,
        windowLocation: any
    ) {
        super(dateRangePickerComponent, pagerComponent, topWindow);

        this.reportTypeComponent = reportTypeComponent;
        this.windowLocation = windowLocation;
    }

    public init() {
        $.blockUI();

        super.init();
        this.reportTypeComponent.render({
            dropdownId: 'dropdown-report-type',
            onChange: ($option: JQuery) => {
                $.blockUI();
                this.windowLocation.href = $option.data('url');
            }
        });

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

    viewBetList = (element: JQuery, transactionType: number) => {
        const param = {
            CustId: this.custId,
            CustName: this.custName,
            WinLossDate: element.data('transfer-date'),
            TransType: transactionType
        };

        var url = `${$('#BetListLink').val()}?${$.param(param)}`;
        this.topWindow.popupManager.open(url, $('#BetListTitle').val(), 900);
    };
}

$(function () {
    new MemberStatementView(
        new DateRangePickerComponent(),
        new ClientPagerComponent(),
        window.top,
        new SingleDropdownComponent(),
        window.location
    ).init();
});
