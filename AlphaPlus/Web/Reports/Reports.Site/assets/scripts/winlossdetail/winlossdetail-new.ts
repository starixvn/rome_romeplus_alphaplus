interface Window {
    viewResources: any;
    productList: any;
    productDetailList: any;
    viewerLevelId: number;
    breadcrumbLevel: any;
    usernameSuggestionUrl: string;
}

class WinLossDetailViewNew {
    private totalEnteredNonEnteredComponent: TotalEnteredNonEnteredComponent;
    private dateRangeComponent: DateRangePickerJQueryComponent;
    private productComponent: ProductDropdownComponent;
    private usernameSuggestionComponent: UsernameSuggestionComponent;
    private visibleColumnComponent: VisibleColumnComponent;
    private weekComponent: WeekComponent;
    private breadcrumbLevelComponent: BreadcrumbLevelComponent;
    private viewResources: any;
    private globalFormData: any;
    private isGettingData = false;

    private winLossDetailTable: WinLossDetailTable;

    constructor(
        totalEnteredNonEnteredComponent: TotalEnteredNonEnteredComponent,
        dateRangeComponent: DateRangePickerJQueryComponent,
        productComponent: ProductDropdownComponent,
        usernameSuggestionComponent: UsernameSuggestionComponent,
        visibleColumnComponent: VisibleColumnComponent,
        weekComponent: WeekComponent,
        breadcrumbLevelComponent: BreadcrumbLevelComponent,
        winLossDetailTable: WinLossDetailTable
    ) {
        //super();
        this.totalEnteredNonEnteredComponent = totalEnteredNonEnteredComponent;
        this.dateRangeComponent = dateRangeComponent;
        this.productComponent = productComponent;
        this.usernameSuggestionComponent = usernameSuggestionComponent;
        this.visibleColumnComponent = visibleColumnComponent;
        this.weekComponent = weekComponent;
        this.breadcrumbLevelComponent = breadcrumbLevelComponent;
        this.winLossDetailTable = winLossDetailTable;
    }

    init(viewResources) {
        this.viewResources = viewResources;
        this.dateRangeComponent.render({ maxDays: 92 }, () => {
            this.getWinLossDetail({ getWeekData: true });
        });
        this.productComponent.render(window.productList, window.productDetailList, () => {
            this.getWinLossDetail({ getWeekData: true });
        });
        this.usernameSuggestionComponent.init(
            new UsernameSuggestionOption(window.usernameSuggestionUrl, () => {
                this.getWinLossDetail({ getWeekData: true });
            })
        );
        //this.visibleColumnComponent.render();
        this.breadcrumbLevelComponent.init(window.breadcrumbLevel, (custId, custName, custLevelId) => {
            this.usernameSuggestionComponent.resetUsername();
            this.getWinLossDetailDownline(custId, custName, custLevelId);
            this.breadcrumbLevelComponent.removeBreadcrumbLevel(custLevelId);
        });
        this.cacheDOMElements();
        this.globalFormData = {
            custLevelId: window.viewerLevelId
        };
    }

    cacheDOMElements() {}

    getWinLossDetail(formData) {
        this.getData(this.getFormData(formData));
    }

    getFormData(formData) {
        const dateRange = this.dateRangeComponent.getDateRangeValues();
        const productsAndResources = this.productComponent.getCheckedValues();
        formData.username = this.usernameSuggestionComponent.getUsername();
        //this.resetActiveWeek();

        return $.extend({}, dateRange, productsAndResources, formData);
    }

    getData(formData) {
        if (formData.productIds.length === 0 || formData.custLevelId > window.viewerLevelId || this.isGettingData) {
            return;
        }

        this.isGettingData = true;
        $.get(`GetData?${$.param(formData)}`, (json) => {
            this.totalEnteredNonEnteredComponent.render(json.TotalEnteredResults);
            json.Weeks &&
                this.weekComponent.render(json.Weeks, (dateRange) => {
                    console.dir(dateRange);
                });
            json.Breadcrumbs && this.breadcrumbLevelComponent.resetBreadcrumbLevel(json.Breadcrumbs);
            this.winLossDetailTable.renderWinLossDetail(
                json.ReportData,
                json.CustLevelId,
                (custId, custName, custLevelId) => {
                    this.getWinLossDetailDownline(custId, custName, custLevelId);
                    this.breadcrumbLevelComponent.addBreadcrumbLevel({
                        CustId: custId,
                        CustName: custName,
                        CustLevelId: custLevelId
                    });
                }
            );
            this.globalFormData = formData;
        }).always(() => {
            this.isGettingData = false;
        });
    }

    getWinLossDetailDownline(custId: number, custName: string, custLevelId: number) {
        this.getWinLossDetail({ custId, custName, custLevelId, getWeekData: true });
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
        new WinLossDetailTable(window.viewResources)
    ).init(window.viewResources);
});
