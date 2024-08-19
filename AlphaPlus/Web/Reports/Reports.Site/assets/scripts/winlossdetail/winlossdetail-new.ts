interface Object {
    entries: any;
}

declare var viewResources: any;
declare var reportTitle: any;
declare var productList: any;
declare var productDetailList: any;
declare var columnList: any;
declare var breadcrumbLevel: any;
declare var currentFromData: any;
declare var visibleColumns: string;
declare var betListUrl: string;
declare var viewerLevelId: number;

class WinLossDetailViewNew extends BaseReportNew {
    private totalEnteredNonEnteredComponent: TotalEnteredNonEnteredComponent;
    private dateRangeComponent: DateRangePickerJQueryComponent;
    private productComponent: ProductDropdownComponent;
    private usernameSuggestionComponent: UsernameSuggestionComponent;
    private visibleColumnComponent: VisibleColumnComponent;
    private weekComponent: WeekComponent;
    private breadcrumbLevelComponent: BreadcrumbLevelComponent;
    private $reportTitle: JQuery;
    private globalFormData: any;
    private isGettingData = false;

    private winLossDetailTable: WinLossDetailTable;
    private clipboard: any;

    constructor(
        totalEnteredNonEnteredComponent: TotalEnteredNonEnteredComponent,
        dateRangeComponent: DateRangePickerJQueryComponent,
        productComponent: ProductDropdownComponent,
        usernameSuggestionComponent: UsernameSuggestionComponent,
        visibleColumnComponent: VisibleColumnComponent,
        weekComponent: WeekComponent,
        breadcrumbLevelComponent: BreadcrumbLevelComponent,
        winLossDetailTable: WinLossDetailTable,
        clipboard: any
    ) {
        super();
        this.totalEnteredNonEnteredComponent = totalEnteredNonEnteredComponent;
        this.dateRangeComponent = dateRangeComponent;
        this.productComponent = productComponent;
        this.usernameSuggestionComponent = usernameSuggestionComponent;
        this.visibleColumnComponent = visibleColumnComponent;
        this.weekComponent = weekComponent;
        this.breadcrumbLevelComponent = breadcrumbLevelComponent;
        this.winLossDetailTable = winLossDetailTable;
        this.clipboard = clipboard;
    }

    init() {
        this.dateRangeComponent.render({ maxDays: 92 }, () => {
            this.getWinLossDetail();
        });
        this.productComponent.render(productList, productDetailList, () => {
            this.getWinLossDetail();
        });
        this.usernameSuggestionComponent.init(
            new UsernameSuggestionOption($('#hidden-usernameSuggestionUrl').attr('href'), () => {
                this.clearCurrentCust();
                this.getWinLossDetail();
            })
        );
        this.visibleColumnComponent.render(visibleColumns, columnList, (columns) => {
            this.winLossDetailTable.showHideColumns(columns);
            this.updateQueryString();
        });
        this.breadcrumbLevelComponent.init(breadcrumbLevel, (custInfo) => {
            this.getWinLossDetailUpOrDownline(custInfo, () => {
                this.$reportTitle.text(reportTitle[custInfo.custLevelId]);
                this.breadcrumbLevelComponent.removeBreadcrumb(custInfo.custLevelId);
            });
        });
        this.cacheDOMElements();
        this.registerEvents();
        this.globalFormData = currentFromData;
        this.getWinLossDetail(false);

        setTimeout(function () {
            window.top['indexManager'].initFeedbackFunction(2);
        }, 1000);
    }

    cacheDOMElements() {
        this.$reportTitle = $('#report-title');
    }

    registerEvents() {
        $('#icon-copy').tooltip();
        const clipboard = new this.clipboard('#icon-copy', {
            text: () => this.winLossDetailTable.copyHtmlTable()
        });
        if (window.document.queryCommandSupported && window.document.queryCommandSupported('copy')) {
            clipboard
                .on('success', (e) => {
                    const $this = $(e.trigger);
                    $this.attr('data-original-title', viewResources.Copied);
                    $this.tooltip('show');
                    setTimeout(() => {
                        $this.tooltip('hide');
                        $this.attr('data-original-title', viewResources.Copy);
                    }, 3000);
                })
                .on('error', function (e) {
                    $(e.trigger).remove();
                    console.error('Action:', e.action);
                    console.error('Trigger:', e.trigger);
                });
        }

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

                let excelParams: any = {};
                if (this.visibleColumnComponent.getShowColumns().find((name) => name === 'fullname')) {
                    excelParams.showFullName = true;
                }
                $.extend(excelParams, this.globalFormData, excelParams);
                $iconExcel.attr('href', `${excelUrl}?${$.param(excelParams)}`);
                setTimeout(function () {
                    $iconExcel.attr('href', 'javascript:;');
                }, 1000);
            })
            .tooltip()
            .attr('href', 'javascript:;');
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
        this.winLossDetailTable.$tableReport.block();
        this.isGettingData = true;
        const queryStringObject = $.extend({}, this.globalFormData, formData);
        const queryString = $.param(queryStringObject);
        isGetWeeks && this.getWeeks(queryString);
        $.get(`GetData?${queryString}`, (json) => {
            this.getDataSuccess(json);
            callback && callback();
            this.globalFormData = queryStringObject;
            this.updateQueryString(queryString);
            this.isGettingData = false;
            this.winLossDetailTable.$tableReport.unblock();
        }).fail((jqXHR) => {
            alphaReports.ajaxFail(jqXHR);
            this.isGettingData = false;
            this.winLossDetailTable.$tableReport.unblock();
        });
    }

    getWeeks(queryString: string) {
        $.get(`GetWeeks?${queryString}`, (json) =>
            this.weekComponent.render(json, (dateRange) => this.getData(dateRange, undefined, false))
        ).fail((jqXHR) => alphaReports.ajaxFail(jqXHR));
    }

    getDataSuccess(json) {
        this.totalEnteredNonEnteredComponent.render(json.TotalEnteredResults);
        if (json.Breadcrumbs) {
            this.$reportTitle.text(reportTitle[json.CustLevelId]);
            this.breadcrumbLevelComponent.resetBreadcrumb(json.Breadcrumbs);
        }
        this.winLossDetailTable.renderWinLossDetail(
            json.ReportData,
            json.CustLevelId,
            this.visibleColumnComponent.getColumns()
        );
        json.ReportData.HasData &&
            this.winLossDetailTable.registerTableEvents(
                (custInfo) => {
                    this.getWinLossDetailUpOrDownline(custInfo, () => {
                        this.$reportTitle.text(reportTitle[custInfo.custLevelId]);
                        this.breadcrumbLevelComponent.addBreadcrumb({
                            CustId: custInfo.custId,
                            CustName: custInfo.custName,
                            CustLevelId: custInfo.custLevelId
                        });
                    });
                },
                ($element: JQuery, custInfo) => {
                    $.blockUI();
                    this.breadcrumbLevelComponent.addBreadcrumb(
                        {
                            CustId: custInfo.custId,
                            CustName: custInfo.custName,
                            CustLevelId: custInfo.custLevelId
                        },
                        false
                    );
                    const breadcrumbsJson = this.breadcrumbLevelComponent.toStringBreadcrumb();
                    const visibleColumns = this.visibleColumnComponent.getShowColumns().join(',');
                    const betListParams = $.extend({}, this.globalFormData, custInfo, {
                        breadcrumbsJson,
                        visibleColumns
                    });
                    $element.attr('href', `${betListUrl}?${$.param(betListParams)}`);
                }
            );
    }

    private updateQueryString(queryString: string = undefined) {
        queryString = queryString || $.param(this.globalFormData);
        const breadcrumbsJson = this.breadcrumbLevelComponent.toStringBreadcrumb();
        const visibleColumns = this.visibleColumnComponent.getShowColumns().join(',');
        alphaReports.updateQueryString(
            `${queryString}&breadcrumbsJson=${encodeURI(breadcrumbsJson)}&visibleColumns=${visibleColumns}`
        );
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
    new WinLossDetailViewNew(
        new TotalEnteredNonEnteredComponent(),
        new DateRangePickerJQueryComponent(),
        new ProductDropdownComponent(),
        new UsernameSuggestionComponent(),
        new VisibleColumnComponent(),
        new WeekComponent(),
        new BreadcrumbLevelComponent(),
        new WinLossDetailTable(),
        Clipboard
    ).init();
});
