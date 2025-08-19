interface Object {
    entries: any;
}

declare var winlossDetailFromData: any;

class CashFeeView extends BaseReportNew {
    private dateRangeComponent: DateRangePickerJQueryComponent;
    private breadcrumbLevelComponent: BreadcrumbLevelComponent;
    private weekComponent: WeekComponent;
    private cashFeeCopyHtml: CashFeeCopyHtml;
    private $tblReportContainer: JQuery;
    private globalFormData: any;

    constructor(
        dateRangeComponent: DateRangePickerJQueryComponent,
        breadcrumbLevelComponent: BreadcrumbLevelComponent,
        cashFeeCopyHtml: CashFeeCopyHtml,
        weekComponent: WeekComponent
    ) {
        super();
        this.dateRangeComponent = dateRangeComponent;
        this.breadcrumbLevelComponent = breadcrumbLevelComponent;
        this.cashFeeCopyHtml = cashFeeCopyHtml;
        this.weekComponent = weekComponent;
    }

    init() {
        super.initShowHideFilter();
        this.$tblReportContainer = $('#tbl-report-container');
        this.globalFormData = winlossDetailFromData;

        this.dateRangeComponent.render({ maxDays: 92, onClosed: () => this.getCashFee(null, true) });
        this.breadcrumbLevelComponent.init((custInfo) => {
            $.blockUI();
            const winlossDetailParams = $.extend(
                {},
                this.globalFormData,
                custInfo,
                this.dateRangeComponent.getDateRangeValues()
            );
            location.href = `${$('#winloss-detail-report-path').val()}?${$.param(winlossDetailParams)}`;
        });
        this.weekComponent.init($('#week-container'), (dateRange) => this.getCashFee(dateRange, true));

        this.registerEvents();
    }

    registerEvents() {
        this.cashFeeCopyHtml.bindEvent($('#tbl-report'));

        const $iconExcel = $('#icon-excel');
        const excelUrl = $iconExcel.attr('href');
        $iconExcel
            .click(() => {
                let excelParams = {
                    custId: winlossDetailFromData.custId,
                    custName: winlossDetailFromData.custName
                };
                $.extend(excelParams, this.globalFormData);
                $iconExcel.attr('href', `${excelUrl}?${$.param(excelParams)}`);
                setTimeout(function () {
                    $iconExcel.attr('href', 'javascript:;');
                }, 1000);
            })
            .tooltip()
            .attr('href', 'javascript:;');
    }

    getCashFee(weekRange, isGetWeeks: boolean) {
        this.clearActiveWeek();
        this.$tblReportContainer.block();
        const dateRange = this.dateRangeComponent.getDateRangeValues();
        const queryStringObject = $.extend({}, this.globalFormData, dateRange, weekRange);
        const queryString = $.param(queryStringObject);
        isGetWeeks && this.getWeeks(queryStringObject);
        this.globalFormData = queryStringObject;
        const url = 'GetCashFeeData';
        $.get(`${url}?${queryString}`, (html) => {
            this.$tblReportContainer.html(html);
            this.cashFeeCopyHtml.bindEvent($('#tbl-report'));
            this.$tblReportContainer.unblock();
        }).fail((jqXHR) => {
            alphaReports.ajaxFail(jqXHR);
            this.$tblReportContainer.unblock();
        });
    }

    getWeeks(queryStringObject: any) {
        const CashBillingServiceProductId = '111';

        const getWeekParams = $.extend({}, queryStringObject);
        getWeekParams.userSelectedProductIds = CashBillingServiceProductId;

        const queryString = $.param(getWeekParams);
        const url = $('#hidden-getReportWeeksUrl').attr('href') || 'GetWeeks';
        $.get(`${url}?${queryString}`, (html) => this.weekComponent.render(html)).fail((jqXHR) =>
            alphaReports.ajaxFail(jqXHR)
        );
    }

    private clearActiveWeek() {
        delete this.globalFormData.weekFromDate;
        delete this.globalFormData.weekToDate;
    }
}

$(function () {
    new CashFeeView(
        new DateRangePickerJQueryComponent(),
        new BreadcrumbLevelComponent(),
        new CashFeeCopyHtml(Clipboard),
        new WeekComponent()
    ).init();
});
