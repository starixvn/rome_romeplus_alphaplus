interface Object {
    entries: any;
}

declare var winlossDetailFromData: any;

class CashFeeView extends BaseReportNew {
    private dateRangeComponent: DateRangePickerJQueryComponent;
    private breadcrumbLevelComponent: BreadcrumbLevelComponent;

    private winLossDetailCopyHtml: WinLossDetailCopyHtml;
    private $tblReportContainer: JQuery;

    constructor(
        dateRangeComponent: DateRangePickerJQueryComponent,
        breadcrumbLevelComponent: BreadcrumbLevelComponent,
        winLossDetailCopyHtml: WinLossDetailCopyHtml
    ) {
        super();
        this.dateRangeComponent = dateRangeComponent;
        this.breadcrumbLevelComponent = breadcrumbLevelComponent;
        this.winLossDetailCopyHtml = winLossDetailCopyHtml;
    }

    init() {
        super.initShowHideFilter();
        this.$tblReportContainer = $('#tbl-report-container');

        this.dateRangeComponent.render({ maxDays: 92, onClosed: () => this.getCashFee() });
        this.breadcrumbLevelComponent.init((custInfo) => {
            $.blockUI();
            const winlossDetailParams = $.extend(
                {},
                winlossDetailFromData,
                custInfo,
                this.dateRangeComponent.getDateRangeValues()
            );
            location.href = `${$('#winloss-detail-report-path').val()}?${$.param(winlossDetailParams)}`;
        });
        this.registerEvents();

        // setTimeout(function () {
        //     window.top.indexManager && window.top.indexManager.initFeedbackFunction(2);
        // }, 1000);
    }

    registerEvents() {
        this.winLossDetailCopyHtml.bindEvent($('#tbl-report'));

        const $iconExcel = $('#icon-excel');
        const excelUrl = $iconExcel.attr('href');
        $iconExcel
            .click((e) => {
                let excelParams = {
                    custId: winlossDetailFromData.CustId,
                    custName: winlossDetailFromData.CustName
                };
                $.extend(excelParams, this.dateRangeComponent.getDateRangeValues());
                $iconExcel.attr('href', `${excelUrl}?${$.param(excelParams)}`);
                setTimeout(function () {
                    $iconExcel.attr('href', 'javascript:;');
                }, 1000);
            })
            .tooltip()
            .attr('href', 'javascript:;');
    }

    getCashFee() {
        this.$tblReportContainer.block();
        const queryStringObject = $.extend(
            { custId: winlossDetailFromData.CustId, custName: winlossDetailFromData.CustName },
            this.dateRangeComponent.getDateRangeValues()
        );
        const queryString = $.param(queryStringObject);
        const url = $('#hidden-cashFeeUrl').attr('href') || 'GetData';
        $.get(`${url}?${queryString}`, (html) => {
            this.$tblReportContainer.html(html);
            //this.getDataSuccess();
            this.$tblReportContainer.unblock();
        }).fail((jqXHR) => {
            alphaReports.ajaxFail(jqXHR);
            this.$tblReportContainer.unblock();
        });
    }

    // getDataSuccess() {
    //     this.breadcrumbLevelComponent.bindEvents();
    //     this.registerEvents();
    // }
}

$(function () {
    new CashFeeView(
        new DateRangePickerJQueryComponent(),
        new BreadcrumbLevelComponent(),
        new WinLossDetailCopyHtml(Clipboard)
    ).init();
});
