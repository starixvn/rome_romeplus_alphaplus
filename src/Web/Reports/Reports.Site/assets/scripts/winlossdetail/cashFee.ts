interface Object {
    entries: any;
}

declare var winlossDetailFromData: any;

class CashFeeView extends BaseReportNew {
    private dateRangeComponent: DateRangePickerJQueryComponent;
    private breadcrumbLevelComponent: BreadcrumbLevelComponent;

    private cashFeeCopyHtml: CashFeeCopyHtml;
    private $tblReportContainer: JQuery;

    constructor(
        dateRangeComponent: DateRangePickerJQueryComponent,
        breadcrumbLevelComponent: BreadcrumbLevelComponent,
        cashFeeCopyHtml: CashFeeCopyHtml
    ) {
        super();
        this.dateRangeComponent = dateRangeComponent;
        this.breadcrumbLevelComponent = breadcrumbLevelComponent;
        this.cashFeeCopyHtml = cashFeeCopyHtml;
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
        this.cashFeeCopyHtml.bindEvent($('#tbl-report'));

        const $iconExcel = $('#icon-excel');
        const excelUrl = $iconExcel.attr('href');
        $iconExcel
            .click((e) => {
                let excelParams = {
                    custId: winlossDetailFromData.custId,
                    custName: winlossDetailFromData.custName
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
            { custId: winlossDetailFromData.custId, custName: winlossDetailFromData.custName },
            this.dateRangeComponent.getDateRangeValues()
        );
        const queryString = $.param(queryStringObject);
        const url = 'GetCashFeeData';
        $.get(`${url}?${queryString}`, (html) => {
            this.$tblReportContainer.html(html);
            this.$tblReportContainer.unblock();
        }).fail((jqXHR) => {
            alphaReports.ajaxFail(jqXHR);
            this.$tblReportContainer.unblock();
        });
    }
}

$(function () {
    new CashFeeView(
        new DateRangePickerJQueryComponent(),
        new BreadcrumbLevelComponent(),
        new CashFeeCopyHtml(Clipboard)
    ).init();
});
