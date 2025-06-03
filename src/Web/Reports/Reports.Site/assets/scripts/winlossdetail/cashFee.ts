interface Object {
    entries: any;
}

declare var productList: any;
declare var productDetailList: any;
declare var currentFromData: any;
declare var viewerLevelId: number;

class CashFeeView extends BaseReportNew {
    private dateRangeComponent: DateRangePickerJQueryComponent;
    private breadcrumbLevelComponent: BreadcrumbLevelComponent;
    private $reportTitle: JQuery;
    private globalFormData: any;
    private isGettingData = false;

    private MemberLevel = 1;
    private winLossDetailColumns: WinLossDetailColumns;
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
        super.initReportLegend();
        super.initShowHideFilter();
        this.$reportTitle = $('#report-title');
        this.globalFormData = currentFromData;
        this.$tblReportContainer = $('#tbl-report-container');
        this.$tblReport = this.$tblReportContainer.find('#tbl-report');

        this.dateRangeComponent.render({ maxDays: 92, onClosed: () => this.getWinLossDetail() });
       
        this.winLossDetailColumns.init(() => this.updateQueryString());
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
                    showFullName: this.winLossDetailColumns.isShowFullName()
                };
                $.extend(excelParams, this.globalFormData, excelParams);
                $iconExcel.attr('href', `${excelUrl}?${$.param(excelParams)}`);
                setTimeout(function () {
                    $iconExcel.attr('href', 'javascript:;');
                }, 1000);
            })
            .tooltip()
            .attr('href', 'javascript:;');

        this.initTableSorter();

        this.$tblReport.find('[data-toggle="tooltip"]').tooltip();
        this.$tblReport
            .find('.icon-plus-box')
            .off('click')
            .click((e) => {
                window.top['reloadMyWatchlistDataTable'] = () => this.getData(this.globalFormData, true);
                const custName = $(e.target).data('custname');
                const url = `${$(
                    '#hidden-settingsUrl'
                ).val()}MyWatchlistAccounts/ManageWatchlistAccounts?usernames=${custName}`;
                const width = 820;
                const height = 550;
                window.top['popupManager'].open(url, undefined, width, height, 'modal-mobile-friendly');
            });
        this.winLossDetailColumns.showHideColumns(this.$tblReport);

        this.$tblReport
            .find('.cash-fee-link')
            .off()
            .click((e) => {
                const $this = $(e.target);
                const custId = $this.data('custid');
                const custName = $this.data('custname').toString();
                const custLevelId = $this.data('custlevelid');
                const custInfo: any = { custId, custName };
                //const breadcrumbs = this.breadcrumbLevelComponent.getBreadcrumbs();

                //if (custLevelId === this.MemberLevel) {
                $.blockUI();
                // breadcrumbs.push(custInfo);
                //  const breadcrumbsJson = JSON.stringify(breadcrumbs);
                //const visibleColumns = this.winLossDetailColumns.getShowColumns().join(',');

                // $this.attr('href', `${$('#hidden-getCashFee').attr('href')}`);
                //} else {
                //e.preventDefault();
                // custInfo.breadcrumbsJson = JSON.stringify(breadcrumbs);
                // this.getWinLossDetailUpOrDownline(custInfo);
                //}
            });
    }

    getWinLossDetail() {
        this.clearActiveWeek();
        const dateRange = this.dateRangeComponent.getDateRangeValues();
    }

    getData(formData, isGetWeeks: boolean) {
        this.$tblReportContainer.block();
        this.isGettingData = true;
        const currentCustLevelId = this.globalFormData.custLevelId;
        const queryStringObject = $.extend({}, this.globalFormData, formData);
        const queryString = $.param(queryStringObject);
        const url = $('#hidden-getReportDataUrl').attr('href') || 'GetData';
        $.get(`${url}?${queryString}`, (html) => {
            this.$tblReportContainer.html(html);
            this.$tblReport = this.$tblReportContainer.find('#tbl-report');
            this.getDataSuccess();
            this.globalFormData = queryStringObject;
            this.updateQueryString(queryString);
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
    
    private updateQueryString(queryString: string = undefined) {
        queryString = queryString || $.param(this.globalFormData);
        const visibleColumns = this.winLossDetailColumns.getShowColumns().join(',');
        alphaReports.updateQueryString(`${queryString}&visibleColumns=${visibleColumns}`);
    }

    private clearActiveWeek() {
        delete this.globalFormData.weekFromDate;
        delete this.globalFormData.weekToDate;
    }


    private changeObjectKey(obj, oldKey, newKey) {
        if (oldKey !== newKey && obj.hasOwnProperty(oldKey)) {
            obj[newKey] = obj[oldKey];
            delete obj[oldKey];
        }

        return obj;
    }

    private initTableSorter() {
        this.tableSorter(this.$tblReport, undefined, () => {
            const dormantAccountsRow = this.$tblReport.find('.dormant-accounts-mark');
            dormantAccountsRow.length && this.$tblReport.append(dormantAccountsRow);
        });
    }

    protected mapSortColumnIndex() {
        const tableHeaderCells = this.$tblReport.find(`thead th[data-colname=${this.sortColumnName}]`);
        if (tableHeaderCells.length) {
            this.sortColumnIndex = tableHeaderCells.eq(0).data('colindex');
        } else {
            // reset to default
            this.sortColumnName = 'username';
            this.sortColumnIndex = 0;
            this.sortIndicator = 0;
        }

        return this.sortColumnIndex;
    }
}

$(function () {
    new CashFeeView(
        new DateRangePickerJQueryComponent(),
        new BreadcrumbLevelComponent(),
        new WinLossDetailCopyHtml(Clipboard)
    ).init();
});
