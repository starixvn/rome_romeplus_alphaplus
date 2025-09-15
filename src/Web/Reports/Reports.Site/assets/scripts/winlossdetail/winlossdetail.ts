interface Object {
    entries: any;
}

declare var productList: any;
declare var productDetailList: any;
declare var currentFromData: any;
declare var viewerLevelId: number;

class WinLossDetailView extends BaseReportNew {
    private totalEnteredNonEnteredComponent: TotalEnteredNonEnteredComponent;
    private dateRangeComponent: DateRangePickerJQueryComponent;
    private productComponent: ProductDropdownComponent;
    private levelTypeComponent: SingleDropdownComponent;
    private usernameSuggestionComponent: UsernameSuggestionComponent;
    private weekComponent: WeekComponent;
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
        totalEnteredNonEnteredComponent: TotalEnteredNonEnteredComponent,
        dateRangeComponent: DateRangePickerJQueryComponent,
        productComponent: ProductDropdownComponent,
        levelTypeComponent: SingleDropdownComponent,
        usernameSuggestionComponent: UsernameSuggestionComponent,
        weekComponent: WeekComponent,
        breadcrumbLevelComponent: BreadcrumbLevelComponent,
        winLossDetailColumns: WinLossDetailColumns,
        winLossDetailCopyHtml: WinLossDetailCopyHtml
    ) {
        super();
        this.totalEnteredNonEnteredComponent = totalEnteredNonEnteredComponent;
        this.dateRangeComponent = dateRangeComponent;
        this.productComponent = productComponent;
        this.levelTypeComponent = levelTypeComponent;
        this.usernameSuggestionComponent = usernameSuggestionComponent;
        this.weekComponent = weekComponent;
        this.breadcrumbLevelComponent = breadcrumbLevelComponent;
        this.winLossDetailColumns = winLossDetailColumns;
        this.winLossDetailCopyHtml = winLossDetailCopyHtml;
    }

    init() {
        super.initReportLegend();
        super.initShowHideFilter();
        this.$reportTitle = $('#report-title');
        this.globalFormData = currentFromData;
        this.$tblReportContainer = $('#tbl-report-container');
        this.$tblReport = this.$tblReportContainer.find('#tbl-report');
        this.compactFeeback.init(currentFromData);

        this.dateRangeComponent.render({ maxDays: 92, onClosed: () => this.getWinLossDetail() });
        this.productComponent.render(productList, productDetailList, () => this.getWinLossDetail());
        this.levelTypeComponent.render({
            dropdownId: 'dropdown-level-type',
            hiddenInputId: 'level-type',
            onChange: () => this.getWinLossDetail()
        });
        this.showHideLevelTypeFilter(this.globalFormData.custLevelId);
        this.usernameSuggestionComponent.init({
            onBlur: () => {
                this.resetLevelFilter();
                this.clearCurrentCust();
                this.getWinLossDetail();
            }
        });
        this.weekComponent.init($('#week-container'), (dateRange) => this.getData(dateRange, true));
        this.breadcrumbLevelComponent.init((custInfo) => this.getWinLossDetailUpOrDownline(custInfo));
        this.winLossDetailColumns.init(() => this.updateQueryString());
        this.registerEvents();
        $('.flex').removeAttr('style');

        const breadcrumbsJson = JSON.stringify(this.breadcrumbLevelComponent.getBreadcrumbs());
        $.extend(this.globalFormData, this.productComponent.getCheckedValues(), { breadcrumbsJson });

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
            .find('.downline-link')
            .off()
            .click((e) => {
                const $this = $(e.target);
                const custId = $this.data('custid');
                const custName = $this.data('custname').toString();
                const custLevelId = $this.data('custlevelid');
                const custInfo: any = { custId, custName, custLevelId };
                const breadcrumbs = this.breadcrumbLevelComponent.getBreadcrumbs();

                if (custLevelId === this.MemberLevel) {
                    $.blockUI();
                    breadcrumbs.push(custInfo);
                    const breadcrumbsJson = JSON.stringify(breadcrumbs);
                    const visibleColumns = this.winLossDetailColumns.getShowColumns().join(',');
                    let betListParams = $.extend({}, this.globalFormData, custInfo, {
                        breadcrumbsJson,
                        visibleColumns
                    });
                    betListParams = this.changeObjectKey(betListParams, 'userSelectedProductIds', 'productIds');
                    betListParams = this.changeObjectKey(betListParams, 'userSelectedResourceIds', 'resourceIds');
                    $this.attr('href', `${$('#hidden-betListUrl').val()}?${$.param(betListParams)}`);
                } else {
                    e.preventDefault();
                    custInfo.breadcrumbsJson = JSON.stringify(breadcrumbs);
                    this.getWinLossDetailUpOrDownline(custInfo);
                }
            });
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
                $.blockUI();
                const $this = $(e.target);
                const visibleColumns = this.winLossDetailColumns.getShowColumns().join(',');
                let cashFeeParams = $.extend({}, this.globalFormData, {
                    visibleColumns
                });
                $this.attr('href', `${$('#hidden-cashFeeUrl').val()}?${$.param(cashFeeParams)}`);
            });
    }

    getWinLossDetail() {
        this.clearActiveWeek();
        const dateRange = this.dateRangeComponent.getDateRangeValues();
        const productsAndResources = this.productComponent.getCheckedValues();
        const others: any = {
            username: this.usernameSuggestionComponent.getUsername()
        };
        const levelType = this.levelTypeComponent.getSelectedValue();
        if (levelType) {
            others.levelType = levelType;
        }

        const formData = $.extend({}, dateRange, productsAndResources, others);
        if (
            formData.userSelectedProductIds.length === 0 ||
            formData.custLevelId > viewerLevelId ||
            this.isGettingData
        ) {
            return;
        }

        this.getData(formData, true);
    }

    getWinLossDetailUpOrDownline(custInfo) {
        this.clearLevelType();
        this.resetLevelFilter();
        this.clearUsername();
        this.getData(custInfo, true);
    }

    getData(formData, isGetWeeks: boolean) {
        this.$tblReportContainer.block();
        this.isGettingData = true;
        const currentCustLevelId = this.globalFormData.custLevelId;
        const queryStringObject = $.extend({}, this.globalFormData, formData);
        const queryString = $.param(queryStringObject);
        isGetWeeks && this.getWeeks(queryString);
        const url = $('#hidden-getReportDataUrl').attr('href') || 'GetData';
        $.get(`${url}?${queryString}`, (html) => {
            this.$tblReportContainer.html(html);
            this.$tblReport = this.$tblReportContainer.find('#tbl-report');
            this.compactFeeback.init(queryStringObject);
            this.getDataSuccess();
            this.changeReportTitleAndLevelTypeFilter(currentCustLevelId);
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
        const url = $('#hidden-getReportWeeksUrl').attr('href') || 'GetWeeks';
        $.get(`${url}?${queryString}`, (html) => this.weekComponent.render(html)).fail((jqXHR) =>
            alphaReports.ajaxFail(jqXHR)
        );
    }

    getDataSuccess() {
        this.breadcrumbLevelComponent.bindEvents();
        this.registerEvents();
        this.totalEnteredNonEnteredComponent.render(JSON.parse($('#hidden-totalEnteredResultsJson').val()));
    }

    private changeReportTitleAndLevelTypeFilter(currentCustLevelId: number) {
        const custLevelId = +$('#hidden-custLevelId').val();
        if (currentCustLevelId !== custLevelId) {
            const titleText = $(`#hidden-titleText${custLevelId}`).val();
            this.$reportTitle.text(titleText);
            this.showHideLevelTypeFilter(custLevelId);
        }
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

    private clearLevelType() {
        delete this.globalFormData.levelType;
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
            const cashFeeRow = this.$tblReport.find('.cash-fee-row');
            cashFeeRow.length && this.$tblReport.append(cashFeeRow);
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

    private showHideLevelTypeFilter(custLevelId: number) {
        const levelTypeDropdownContainer = this.levelTypeComponent.$dropdown.closest('.level-type-multiselect');

        if (custLevelId === 2) {
            levelTypeDropdownContainer.hide();
        } else if (custLevelId === 3 || custLevelId === 4) {
            const masterAgentText = $(`#hidden-masterAgentText${custLevelId}`).val();
            // update option text
            this.levelTypeComponent.$dropdown.find('option[value="1"]').text(masterAgentText);

            // update dropdown text
            const label = levelTypeDropdownContainer
                .find('.dropdown-menu')
                .find('input[type="radio"][value="1"]')
                .parent();
            label
                .contents()
                .filter(function () {
                    return this.nodeType === 3; /* Node type 3 is a text node */
                })
                .first()
                .replaceWith(masterAgentText);

            levelTypeDropdownContainer.show();
        }
    }

    private resetLevelFilter() {
        if (this.levelTypeComponent.getSelectedValue() !== '0') {
            // update selected value to All
            this.levelTypeComponent.setSelectedValue('0');
        }
    }
}

$(function () {
    new WinLossDetailView(
        new TotalEnteredNonEnteredComponent(),
        new DateRangePickerJQueryComponent(),
        new ProductDropdownComponent(),
        new SingleDropdownComponent(),
        new UsernameSuggestionComponent(),
        new WeekComponent(),
        new BreadcrumbLevelComponent(),
        new WinLossDetailColumns(new VisibleColumnComponent()),
        new WinLossDetailCopyHtml(Clipboard)
    ).init();
});
