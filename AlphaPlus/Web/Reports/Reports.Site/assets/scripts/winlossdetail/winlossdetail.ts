interface Object {
    entries: any;
}

declare var viewResources: any;
declare var productList: any;
declare var productDetailList: any;
declare var currentFromData: any;
declare var viewerLevelId: number;

class WinLossDetailView extends BaseReportNew {
    private totalEnteredNonEnteredComponent: TotalEnteredNonEnteredComponent;
    private dateRangeComponent: DateRangePickerJQueryComponent;
    private productComponent: ProductDropdownComponent;
    private usernameSuggestionComponent: UsernameSuggestionComponent;
    private weekComponent: WeekComponent;
    private breadcrumbLevelComponent: BreadcrumbLevelComponent;
    private $reportTitle: JQuery;
    private globalFormData: any;
    private isGettingData = false;

    private MemberLevel = 1;
    private winLossDetailColumns: WinLossDetailColumns;
    private winLossDetailCopyHtml: WinLossDetailCopyHtml;
    private winLossDetailSort: WinLossDetailSort;
    private $tblReportContainer: JQuery;
    private $tblReport: JQuery;

    constructor(
        totalEnteredNonEnteredComponent: TotalEnteredNonEnteredComponent,
        dateRangeComponent: DateRangePickerJQueryComponent,
        productComponent: ProductDropdownComponent,
        usernameSuggestionComponent: UsernameSuggestionComponent,
        weekComponent: WeekComponent,
        breadcrumbLevelComponent: BreadcrumbLevelComponent,
        winLossDetailColumns: WinLossDetailColumns,
        winLossDetailCopyHtml: WinLossDetailCopyHtml,
        winLossDetailSort: WinLossDetailSort
    ) {
        super();
        this.totalEnteredNonEnteredComponent = totalEnteredNonEnteredComponent;
        this.dateRangeComponent = dateRangeComponent;
        this.productComponent = productComponent;
        this.usernameSuggestionComponent = usernameSuggestionComponent;
        this.weekComponent = weekComponent;
        this.breadcrumbLevelComponent = breadcrumbLevelComponent;
        this.winLossDetailColumns = winLossDetailColumns;
        this.winLossDetailCopyHtml = winLossDetailCopyHtml;
        this.winLossDetailSort = winLossDetailSort;
    }

    init() {
        this.$reportTitle = $('#report-title');
        this.globalFormData = currentFromData;
        this.$tblReportContainer = $('#tbl-report-container');
        this.$tblReport = this.$tblReportContainer.find('#tbl-report');

        this.dateRangeComponent.render({ maxDays: 92 }, () => this.getWinLossDetail());
        this.productComponent.render(productList, productDetailList, () => this.getWinLossDetail());
        this.usernameSuggestionComponent.init(
            new UsernameSuggestionOption($('#hidden-usernameSuggestionUrl').attr('href'), () => {
                this.clearCurrentCust();
                this.getWinLossDetail();
            })
        );
        this.weekComponent.init($('#week-container'), (dateRange) => this.getData(dateRange, undefined, false));
        this.breadcrumbLevelComponent.init((custInfo) =>
            this.getWinLossDetailUpOrDownline(custInfo, () =>
                this.$reportTitle.text(viewResources[custInfo.custLevelId])
            )
        );
        this.winLossDetailColumns.init(() => this.updateQueryString());
        this.registerEvents();
        this.$tblReportContainer.removeAttr('style');

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
                if (this.globalFormData.custLevelId > viewerLevelId || this.isGettingData) {
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

        this.winLossDetailSort.init(this.$tblReport);

        this.$tblReport.find('[data-toggle="tooltip"]').tooltip();
        this.$tblReport
            .find('.downline-link')
            .off()
            .click((e) => {
                const $this = $(e.target);
                const custId = $this.data('custid');
                const custName = $this.data('custname');
                const custLevelId = $this.data('custlevelid');
                const custInfo: any = { custId, custName, custLevelId };
                const breadcrumbs = this.breadcrumbLevelComponent.getBreadcrumbs();

                if (custLevelId === this.MemberLevel) {
                    $.blockUI();
                    breadcrumbs.push(custInfo);
                    const breadcrumbsJson = JSON.stringify(breadcrumbs);
                    const visibleColumns = this.winLossDetailColumns.getShowColumns().join(',');
                    const betListParams = $.extend({}, this.globalFormData, custInfo, {
                        breadcrumbsJson,
                        visibleColumns
                    });
                    $this.attr('href', `${$('#hidden-betListUrl').val()}?${$.param(betListParams)}`);
                } else {
                    e.preventDefault();
                    custInfo.breadcrumbsJson = JSON.stringify(breadcrumbs);
                    this.getWinLossDetailUpOrDownline(custInfo, () =>
                        this.$reportTitle.text(viewResources[custInfo.custLevelId])
                    );
                }
            });
        this.$tblReport
            .find('.icon-plus-box')
            .off('click')
            .click((e) => {
                window.top['reloadMyWatchlistDataTable'] = () => this.getData(this.globalFormData, undefined, true);
                const custName = $(e.target).data('custname');
                const url = `${$(
                    '#hidden-settingsUrl'
                ).val()}MyWatchlistAccounts/ManageWatchlistAccounts?usernames=${custName}`;
                const width = 820;
                const height = 550;
                window.top['popupManager'].open(url, undefined, width, height, 'modal-mobile-friendly');
            });
        this.winLossDetailColumns.showHideColumns(this.$tblReport);
    }

    getWinLossDetail(isClearActiveWeek = true) {
        isClearActiveWeek && this.clearActiveWeek();
        const dateRange = this.dateRangeComponent.getDateRangeValues();
        const productsAndResources = this.productComponent.getCheckedValues();
        const username = this.usernameSuggestionComponent.getUsername();

        const formData = $.extend({}, dateRange, productsAndResources, { username });
        if (
            formData.userSelectedProductIds.length === 0 ||
            formData.custLevelId > viewerLevelId ||
            this.isGettingData
        ) {
            return;
        }

        this.getData(formData, undefined, true);
    }

    getWinLossDetailUpOrDownline(custInfo, callback: Function) {
        this.clearUsername();
        this.getData(custInfo, callback, true);
    }

    getData(formData, callback: Function, isGetWeeks: boolean) {
        this.$tblReportContainer.block();
        this.isGettingData = true;
        const queryStringObject = $.extend({}, this.globalFormData, formData);
        const queryString = $.param(queryStringObject);
        isGetWeeks && this.getWeeks(queryString);
        $.get(`GetData?${queryString}`, (html) => {
            this.$tblReportContainer.html(html);
            this.$tblReport = this.$tblReportContainer.find('#tbl-report');
            this.getDataSuccess();
            callback && callback();
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

    getWeeks(queryString: string) {
        $.get(`GetWeeks?${queryString}`, (html) => this.weekComponent.render(html)).fail((jqXHR) =>
            alphaReports.ajaxFail(jqXHR)
        );
    }

    getDataSuccess() {
        this.breadcrumbLevelComponent.bindEvents();
        this.registerEvents();
        this.totalEnteredNonEnteredComponent.render(JSON.parse($('#hidden-totalEnteredResultsJson').val()));
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

    private clearCurrentCust() {
        delete this.globalFormData.custId;
        delete this.globalFormData.custName;
        delete this.globalFormData.custLevelId;
    }

    private clearUsername() {
        this.usernameSuggestionComponent.clearUsername();
        delete this.globalFormData.username;
    }
}

$(function () {
    new WinLossDetailView(
        new TotalEnteredNonEnteredComponent(),
        new DateRangePickerJQueryComponent(),
        new ProductDropdownComponent(),
        new UsernameSuggestionComponent(),
        new WeekComponent(),
        new BreadcrumbLevelComponent(),
        new WinLossDetailColumns(new VisibleColumnComponent()),
        new WinLossDetailCopyHtml(Clipboard),
        new WinLossDetailSort()
    ).init();
});
