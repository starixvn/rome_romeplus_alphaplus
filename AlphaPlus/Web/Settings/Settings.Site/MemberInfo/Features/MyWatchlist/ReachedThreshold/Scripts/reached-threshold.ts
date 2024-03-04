"use strict";

interface Window {
    reachlistAccountData: any;
}

class ReachedThreshold extends MyWatchlistBase {
    private dateRangePickerComponent: DateRangePickerComponent;
    private dataTable: ReachedThresholdDataTable;
    private statusDropdown: SingleSelectionDropdownComponent;
    private limitTypeDropdown: SingleSelectionDropdownComponent;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        dataTable: ReachedThresholdDataTable) {
        super($);
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.dataTable = dataTable;
        this.statusDropdown = new SingleSelectionDropdownComponent($);
        this.limitTypeDropdown = new SingleSelectionDropdownComponent($);
    }

    public init() {
        this.initInfoLegend();

        const baseUrl = this.$('#member-info-url').val();
        this.initUsernameAutoSuggestion(baseUrl, this.$('#search-account-id').val());

        this.dateRangePickerComponent.render(
            new DateRangePickerOption());

        this.statusDropdown.render(
            new SingleSelectionDropdownOption("status-dropdown", "", () => { }, true, true));

        this.limitTypeDropdown.render(
            new SingleSelectionDropdownOption("limit-type-dropdown", "", () => { }, true, true));

        this.dataTable.init(baseUrl);
    }

    private initUsernameAutoSuggestion(baseUrl: string, searchAccountId: string) {
        this.$('#txt-username').easyAutocomplete({
            url: phrase => `${baseUrl}UserNameSuggestion/GetUsernamesSuggestion?SuggestionQuery=${phrase}&CustId=${searchAccountId}&DirectDownlines=true&IsAccInfo=false`,
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
    const dateRangePickerComponent = new DateRangePickerComponent($);
    const dataTable = new ReachedThresholdDataTable($, window.top, window.reachlistAccountData);

    new ReachedThreshold($, dateRangePickerComponent, dataTable).init();
})