declare var viewResources: any;
declare var reportTitle: any;
declare var productList: any;
declare var productDetailList: any;
declare var columnList: any;
declare var breadcrumbLevel: any;
declare var activeWeek: any;
declare var usernameSuggestionUrl: string;
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
    private $tableReport: JQuery;
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
            new UsernameSuggestionOption(usernameSuggestionUrl, () => {
                this.clearCurrentCust();
                this.getWinLossDetail();
            })
        );
        this.visibleColumnComponent.render('', columnList, (columns) => {
            this.showHideColumns(columns);
            // const selectedColumnsValue = [];
            // Object.entries(columns).forEach(([key, value]) => value && selectedColumnsValue.push(key));
            // onDropdownHidden(selectedColumnsValue.join());
        });
        this.breadcrumbLevelComponent.init(breadcrumbLevel, (custInfo) => {
            this.getWinLossDetailUpOrDownline(custInfo, () => {
                this.$reportTitle.text(reportTitle[custInfo.custLevelId]);
                this.breadcrumbLevelComponent.removeBreadcrumb(custInfo.custLevelId);
            });
        });
        this.cacheDOMElements();
        this.registerEvents();
        this.globalFormData = activeWeek;
        this.getWinLossDetail(false);
    }

    cacheDOMElements() {
        this.$reportTitle = $('#report-title');
        this.$tableReport = $('#tbl-report');
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

        const formData = $.extend({}, dateRange, productsAndResources, { username, getWeekData: true });
        if (
            formData.userSelectedProductIds.length === 0 ||
            formData.custLevelId > viewerLevelId ||
            this.isGettingData
        ) {
            return;
        }

        this.getData(formData);
    }

    getWinLossDetailUpOrDownline(custInfo, callback) {
        this.clearUsername();
        custInfo.getWeekData = true;
        this.getData(custInfo, callback);
    }

    getData(formData, callback: Function = undefined) {
        this.isGettingData = true;
        const queryStringObject = $.extend({}, this.globalFormData, formData);
        const queryString = $.param(queryStringObject);
        $.get(`GetData?${queryString}`, (json) => {
            this.getDataSuccess(json);
            callback && callback();
            this.globalFormData = queryStringObject;
            console.dir(this.globalFormData);
            this.updateQueryString(queryString);
            this.isGettingData = false;
        }).fail((jqXHR) => {
            alphaReports.ajaxFail(jqXHR);
            this.isGettingData = false;
        });
    }

    getDataSuccess(json) {
        this.totalEnteredNonEnteredComponent.render(json.TotalEnteredResults);
        json.Weeks &&
            this.weekComponent.render(json.Weeks, (dateRange) => {
                dateRange.getWeekData = false;
                this.getData(dateRange);
            });
        if (json.Breadcrumbs) {
            this.$reportTitle.text(reportTitle[json.CustLevelId]);
            this.breadcrumbLevelComponent.resetBreadcrumb(json.Breadcrumbs);
        }
        this.winLossDetailTable.renderWinLossDetail(json.ReportData, json.CustLevelId);
        this.showHideColumns();
        this.$tableReport.stickyTableHeaders('destroy');
        this.$tableReport.stickyTableHeaders();
        json.ReportData.HasData &&
            this.winLossDetailTable.registerTableEvents((custInfo) => {
                this.getWinLossDetailUpOrDownline(custInfo, () => {
                    this.$reportTitle.text(reportTitle[custInfo.custLevelId]);
                    this.breadcrumbLevelComponent.addBreadcrumb({
                        CustId: custInfo.custId,
                        CustName: custInfo.custName,
                        CustLevelId: custInfo.custLevelId
                    });
                });
            });
    }

    private updateQueryString(queryString: string) {
        alphaReports.updateQueryString(
            `${queryString}&breadcrumbsJson=${this.breadcrumbLevelComponent.toStringBreadcrumb()}`
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

    private showHideColumns(visibleColumn = undefined) {
        visibleColumn = visibleColumn || this.visibleColumnComponent.getColumns();
        this.showHideSpecificColumn(visibleColumn, 'fullname');
        this.showHideSpecificColumn(visibleColumn, 'betcount');
        this.showHideSpecificColumn(visibleColumn, 'turnover');
        this.showHideSpecificColumn(visibleColumn, 'netturnover');
        this.showHideSpecificColumn(visibleColumn, 'grosscomm');
        this.showHideGroupColumn(visibleColumn, 'p', 'groupColumnMember');
        this.showHideGroupColumn(visibleColumn, 'a', 'groupColumnAgent');
        this.showHideGroupColumn(visibleColumn, 'm', 'groupColumnMaster');
        this.showHideGroupColumn(visibleColumn, 's', 'groupColumnSuper');
        this.showHideSpecificColumn(visibleColumn, 'company');
    }

    private showHideGroupColumn(visibleColumn, prefix: string, groupColumnId: string) {
        const groupColumn = this.$tableReport.find(`#${groupColumnId}`);
        if (groupColumn.length) {
            let colspan = 0;
            if (this.showHideSpecificColumn(visibleColumn, `${prefix}winloss`)) {
                colspan++;
            }
            if (this.showHideSpecificColumn(visibleColumn, `${prefix}comm`)) {
                colspan++;
            }
            if (this.showHideSpecificColumn(visibleColumn, `${prefix}total`)) {
                colspan++;
            }

            colspan ? groupColumn.attr('colspan', colspan).removeClass('hide') : groupColumn.addClass('hide');
        }
    }

    private showHideSpecificColumn(visibleColumn, key: string) {
        const matchedColumns = this.$tableReport.find(`[data-colname=${key}]`);
        const visible = visibleColumn[key];
        visible ? matchedColumns.removeClass('hide') : matchedColumns.addClass('hide');

        return visible;
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
