/// <reference path="../../../Features/MemberInfo/assets/scripts/components/single-selection-dropdown.ts" />
'use strict';

interface Window {
    successfulUpdatingCallback: Function;
    reloadPopUpSetting: Function;
    actionAfterUpdatedLiveCasinoSettingMultiple: Function;
    actionAfterUpdatedLiveCasinoSettingSingle: Function;
    successfulUpdatingCallbackAndReloadMainFrame: Function;
    reloadMainFrame: Function;
    customerListPopup: any;
    jquery: JQueryStatic;
    customerListData: any;
}

class CustomerList {
    private $: JQueryStatic;
    private customerListPopup: CustomerListPopup;
    private customerListDataTable: CustomerListDataTable;
    private viewModel: CustomerListViewModel;
    private statusDropdown: SingleSelectionDropdownComponent;
    private doubleCommDropdown: SingleSelectionDropdownComponent;
    private allowAutoTransferDropdown: SingleSelectionDropdownComponent;
    private levelComponent: SingleSelectionDropdownComponent;
    private serviceTypeComponent: SingleSelectionDropdownComponent;

    constructor(
        $: JQueryStatic,
        viewModel: CustomerListViewModel,
        customerListPopup: CustomerListPopup,
        customerListDataTable: CustomerListDataTable,
        statusDropdown: SingleSelectionDropdownComponent,
        doubleCommDropdown: SingleSelectionDropdownComponent,
        allowAutoTransferDropdown: SingleSelectionDropdownComponent,
        levelComponent: SingleSelectionDropdownComponent,
        serviceTypeComponent: SingleSelectionDropdownComponent
    ) {
        this.$ = $;
        this.viewModel = viewModel;
        this.customerListDataTable = customerListDataTable;
        this.customerListPopup = customerListPopup;

        this.statusDropdown = statusDropdown;
        this.doubleCommDropdown = doubleCommDropdown;
        this.allowAutoTransferDropdown = allowAutoTransferDropdown;
        this.levelComponent = levelComponent;
        this.serviceTypeComponent = serviceTypeComponent;
    }

    public init = () => {
        this.statusDropdown.render(new SingleSelectionDropdownOption('statusFilter', 'StatusId', () => {}, true, true));

        this.doubleCommDropdown.render(
            new SingleSelectionDropdownOption('dcommFilter', 'DoubleCommId', () => {}, true, true)
        );

        this.allowAutoTransferDropdown.render(
            new SingleSelectionDropdownOption('dautoTransferFilter', 'AllowAutoTransfer', () => { }, true, true)
        );

        this.levelComponent.render(new SingleSelectionDropdownOption('dropdown-level-type', 'HiddenLevelType'));
        this.serviceTypeComponent.render(new SingleSelectionDropdownOption('serviceType', 'HiddenServiceType', () => {
            const selectedValue = parseInt(this.serviceTypeComponent.getSelectedItem());
            if (selectedValue === ServiceType.All || selectedValue === ServiceType.Cash) {
                this.allowAutoTransferDropdown.disable();
                this.allowAutoTransferDropdown.changeSelection(2);
            } else {
                this.allowAutoTransferDropdown.enable();
            }

            this.serviceTypeComponent.changeSelection(selectedValue);
        }));
        this.serviceTypeComponent.onchange();
        this.bindingTxtUsername();
        this.customerListDataTable.bindingEvent();
        this.customerListPopup.bindingBootstrapModalEvent();
    };

    private bindingTxtUsername = () => {
        const options = {
            url: (phrase) =>
                `${this.viewModel.MemberInfoUrl}UserNameSuggestion?Query=${phrase}&CustId=${this.viewModel.SearchAccountId}&DirectDownlines=true&IsAccInfo=false`,
            getValue: 'UserName',
            requestDelay: 200,
            minCharNumber: 2,
            template: {
                type: 'custom',
                method: (value, item) => {
                    const nickname = item.UserName2 === null || item.UserName2 === '' ? '' : `(${item.UserName2})`;
                    return `<div class='suggestion'>${value} ${nickname}<div>${item.FirstName} ${item.LastName}</div></div>`;
                }
            },
            list: {
                maxNumberOfElements: 10
            }
        };

        const $txtUsername = this.$('#txt-username');
        $txtUsername.easyAutocomplete(options);

        $txtUsername.bind('keypress', (event) => {
            if (event.which == 13) {
                this.$('.btn-submit').click();
            }
        });
    };
}

$(() => {
    $.ajaxSetup({
        beforeSend: function (xhr) {
            var antiXSRFToken = document.getElementsByName('__RequestVerificationToken');
            if (antiXSRFToken.length > 0) {
                xhr.setRequestHeader('RequestVerificationToken', (<HTMLInputElement>antiXSRFToken[0]).value);
            }
        },
        error: function (xhr) {
            if (xhr.status === 401 || xhr.status === 403) {
                top.window.alpha.core.signOut();
            }
        }
    });
});

$(function () {
    const viewModel = new CustomerListViewModel(
        parseInt($('#uplineRoleId').val()),
        parseInt($('#searchAccountId').val()),
        $('#searchAccountName').val(),
        parseInt($('#searchAccountRoleId').val()),
        $('#downlineLevelName').val(),
        JSON.parse($('#showEditMultiple').val()),
        JSON.parse($('#isMobileView').val()),
        $('#breadcrumbLevelJson').val(),
        $('#oldMainUrl').val(),
        $('#reportsUrl').val(),
        $('#memberInfoUrl').val(),
        $('#accountInfoUrl').val(),
        JSON.parse($('#limitedDateToShowInfo').val()),
        JSON.parse($('#canViewReport').val()),
        JSON.parse($('#canUpdateAccount').val()),
        window.customerListData,
        JSON.parse($('#IsDirectMember').val()),
        parseInt($('#HiddenServiceType').val()),
        JSON.parse($('#OfferedCashService').val())
    );

    const customerListPopup = new CustomerListPopup($, window.top, viewModel.IsMobileView),
        customerListServices = new CustomerListServices($, window.top, viewModel),
        editSingleSetting = new EditSingleSetting($, customerListPopup, customerListServices),
        editMultipleSetting = new EditMultipleSetting(
            $,
            viewModel,
            customerListPopup,
            customerListServices,
            editSingleSetting
        ),
        editStatus = new EditStatus($, window.top, viewModel.IsMobileView, customerListServices, customerListPopup),
        unlock = new Unlock($, window.top, viewModel.IsMobileView, customerListServices),
        customerListDataTable = new CustomerListDataTable(
            $,
            window.top,
            viewModel,
            customerListPopup,
            customerListServices,
            editSingleSetting,
            editMultipleSetting,
            editStatus,
            unlock
        ),
        customerList = new CustomerList(
            $,
            viewModel,
            customerListPopup,
            customerListDataTable,
            new SingleSelectionDropdownComponent($),
            new SingleSelectionDropdownComponent($),
            new SingleSelectionDropdownComponent($),
            new SingleSelectionDropdownComponent($),
            new SingleSelectionDropdownComponent($)
        );

    window.top.customerListPopup = customerListPopup;

    window.successfulUpdatingCallback = (isPartialUpdate, isMultipleUpdate) =>
        customerListPopup.successfulUpdatingCallback();

    window.actionAfterUpdatedLiveCasinoSettingMultiple = (isUpdatePartial) =>
        customerListPopup.successfulUpdatingCallback();
    window.actionAfterUpdatedLiveCasinoSettingSingle = (isUpdatePartial) =>
        customerListPopup.successfulUpdatingCallback();

    window.top.successfulUpdatingCallbackAndReloadMainFrame = (reloadAfterThreeSecond = false) => {
        $.blockUI();
        setTimeout(
            () => {
                $.unblockUI();
                customerListDataTable.reloadData();
                customerListPopup.reloadFrame();
            },
            reloadAfterThreeSecond ? 3000 : 1500
        );
    };

    window.top.reloadMainFrame = (reloadAfterThreeSecond = false) => {
        customerListDataTable.reloadData();
    };

    customerList.init();
});
