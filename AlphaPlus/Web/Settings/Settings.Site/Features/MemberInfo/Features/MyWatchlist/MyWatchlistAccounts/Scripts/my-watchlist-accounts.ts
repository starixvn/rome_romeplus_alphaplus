interface Window {
    watchlistAccountData: any;
    reloadMyWatchlistDataTable: Function;
    openAddMultiplePopup: Function;
}

class MyWatchlistAccounts extends MyWatchlistBase {
    private topWindow: any;
    private dataTable: MyWatchlistAccountsDataTable;


    constructor(
        $: JQueryStatic,
        topWindow: any,
        dataTable: MyWatchlistAccountsDataTable) {
        super($);
        this.topWindow = topWindow;
        this.dataTable = dataTable;
    }

    public init() {
        this.initInfoLegend();
        const baseUrl = this.$('#member-info-url').val();

        this.initUsernameAutoSuggestion(baseUrl, this.$('#search-account-id').val());
        this.$("#btn-global-threshold").click(_ =>
            this.topWindow.popupManager.open(
                `${baseUrl}MyWatchlistAccounts/ManageGlobalThreshold`,
                undefined,
                600,
                310,
                "modal-mobile-friendly"));
        this.$("#btn-manage-preset").click(_ =>
            this.topWindow.popupManager.open(
                `${baseUrl}MyWatchlistAccounts/ManagePersonalThresholdPreset`,
                undefined,
                795,
                520,
                "modal-mobile-friendly"));
        this.$("#btn-add-members").click(_ =>
            this.topWindow.popupManager.open(
                `${baseUrl}MyWatchlistAccounts/ManageWatchlistAccounts`,
                undefined,
                820,
                550,
                "modal-mobile-friendly"));

        this.dataTable.init(baseUrl);
    }

    private initUsernameAutoSuggestion(baseUrl: string, searchAccountId: string) {
        this.$('#txt-username').easyAutocomplete({
            url: phrase => `${baseUrl}UserNameSuggestion?Query=${phrase}&CustId=${searchAccountId}&DirectDownlines=true&IsAccInfo=false`,
            getValue: 'UserName',
            requestDelay: 200,
            minCharNumber: 2,
            template: {
                type: "custom",
                method: (value, item) => {
                    const nickname = item.UserName2 === null || item.UserName2 === '' ? '' : `(${item.UserName2})`;
                    return `<div class='suggestion'>${value} ${nickname}<div>${item.FirstName} ${item.LastName}</div></div>`;
                }
            },
            list: {
                maxNumberOfElements: 10
            }
        });
    }
}

$(function () {
    const editSettingPopup = new EditSettingPopup($, window.top);
    const dataTable = new MyWatchlistAccountsDataTable($, editSettingPopup, window.watchlistAccountData, window.top);
    window.top.reloadMyWatchlistDataTable = () => dataTable.reloadData();

    new MyWatchlistAccounts($, window.top, dataTable).init();
});