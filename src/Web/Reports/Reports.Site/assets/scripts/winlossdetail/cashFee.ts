interface Object {
    entries: any;
}

class CashFeeView extends BaseReportNew {
    private dateRangeComponent: DateRangePickerJQueryComponent;
    private breadcrumbLevelComponent: BreadcrumbLevelComponent;
    private $reportTitle: JQuery;
    private globalFormData: any;
    private isGettingData = false;

    private MemberLevel = 1;
    private winLossDetailCopyHtml: WinLossDetailCopyHtml;
    private $tblReportContainer: JQuery;
    private $tblReport: JQuery;

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
        this.$reportTitle = $('#report-title');
        this.$tblReportContainer = $('#tbl-report-container');
        this.$tblReport = this.$tblReportContainer.find('#tbl-report');

        this.dateRangeComponent.render({ maxDays: 92, onClosed: () => this.getCashFee() });
        this.breadcrumbLevelComponent.init(breadcrumbParam => {

            $.blockUI();
            //const breadcrumbLevels = this.breadcrumbLevelComponent.getBreadcrumbLevels(breadcrumbParam.CustLevelId);
            //if (breadcrumbLevels.length) {
            //    reportParams.breadcrumbsJson = JSON.stringify(breadcrumbLevels);
            //}
            //reportParams.visibleColumns = $('#hidden-show-columns').val();
            //$.extend(reportParams, this.globalFormData, breadcrumbParam, this.dateRangeComponent.getDateRangeValues());

            location.href = $('#winloss-detail-report-path').val();
        });
        this.registerEvents();
        $('.flex').removeAttr('style');

        const breadcrumbsJson = JSON.stringify(this.breadcrumbLevelComponent.getBreadcrumbs());

        setTimeout(function () {
            window.top.indexManager && window.top.indexManager.initFeedbackFunction(2);
        }, 1000);
    }

    registerEvents() {
        this.winLossDetailCopyHtml.bindEvent(this.$tblReport);

        const $iconExcel = $('#icon-excel');
        const excelUrl = $iconExcel.attr('href');
        $iconExcel
            .click((e) => {
                if (
                    this.globalFormData.userSelectedProductIds.length === 0 ||
                    this.globalFormData.custLevelId > viewerLevelId ||
                    this.isGettingData
                ) {
                    e.preventDefault();
                    return;
                }

                let excelParams = {
                   
                };
                $.extend(excelParams, this.globalFormData, excelParams);
                $iconExcel.attr('href', `${excelUrl}?${$.param(excelParams)}`);
                setTimeout(function () {
                    $iconExcel.attr('href', 'javascript:;');
                }, 1000);
            })
            .tooltip()
            .attr('href', 'javascript:;');

        this.$tblReport
            .find('.cash-fee-link')
            .off()
            .click((e) => {
                const $this = $(e.target);
                const custId = $this.data('custid');
                const custName = $this.data('custname').toString();
                const custLevelId = $this.data('custlevelid');
                const custInfo: any = { custId, custName };
                const breadcrumbs = this.breadcrumbLevelComponent.getBreadcrumbs();

                if (custLevelId === this.MemberLevel) {
                $.blockUI();
                breadcrumbs.push(custInfo);
                const breadcrumbsJson = JSON.stringify(breadcrumbs);

                let cashFeeParams = $.extend({}, this.globalFormData, custInfo, {
                    breadcrumbsJson
                });
                    $this.attr('href', `${$('#hidden-cashFeeUrl').val()}?${$.param(cashFeeParams)}`);
                } else {
                e.preventDefault();
                custInfo.breadcrumbsJson = JSON.stringify(breadcrumbs);
               }
            });
    }

    getCashFee() {
        const dateRange = this.dateRangeComponent.getDateRangeValues();

        const formData = $.extend({}, dateRange);
        if (
            formData.custLevelId > viewerLevelId ||
            this.isGettingData
        ) {
            return;
        }

        this.getData(formData);
    }

    getData(formData) {
        this.$tblReportContainer.block();
        this.isGettingData = true;
        const queryStringObject = $.extend({}, this.globalFormData, formData);
        const queryString = $.param(queryStringObject);
        const url = $('#hidden-cashFeeUrl').attr('href') || 'GetData';
        $.get(`${url}?${queryString}`, (html) => {
            this.$tblReportContainer.html(html);
            this.$tblReport = this.$tblReportContainer.find('#tbl-report');
            this.getDataSuccess();
            this.globalFormData = queryStringObject;
            this.isGettingData = false;
            this.$tblReportContainer.unblock();
        }).fail((jqXHR) => {
            alphaReports.ajaxFail(jqXHR);
            this.isGettingData = false;
            this.$tblReportContainer.unblock();
        });
    }
   

    getDataSuccess() {
        this.breadcrumbLevelComponent.bindEvents();
        this.registerEvents();
    }
}

$(function () {
    new CashFeeView(
        new DateRangePickerJQueryComponent(),
        new BreadcrumbLevelComponent(),
        new WinLossDetailCopyHtml(Clipboard)
    ).init();
});
