/// <reference path="../components/single-selection-dropdown.ts" />
/// <reference path="../components/pager.ts" />
'use strict';

interface Window {
    bootboxConfirm(message: string, callback: (result: boolean) => void): void;
    bootboxAlert(message: string): void;
}

interface JQuery {
    stickyTableHeaders(): void;
}

class PotentialWinLimit {
    $: JQueryStatic;
    statusComponent: SingleSelectionDropdownComponent;
    problemAccountComponent: SingleSelectionDropdownComponent;
    accountTypeComponent: SingleSelectionDropdownComponent;
    pager: PagerComponent;

    $status: JQuery;
    $problemAccount: JQuery;
    $accountType: JQuery;
    $potentialWinLimitForm: JQuery;

    $username: JQuery;
    $tableReport: JQuery;
    $iconExcel: JQuery;

    constructor(
        $: JQueryStatic,
        statusComponent: SingleSelectionDropdownComponent,
        problemAccountComponent: SingleSelectionDropdownComponent,
        accountTypeComponent: SingleSelectionDropdownComponent,
        pager: PagerComponent
    ) {
        this.$ = $;
        this.statusComponent = statusComponent;
        this.problemAccountComponent = problemAccountComponent;
        this.accountTypeComponent = accountTypeComponent;
        this.pager = pager;
    }

    init = () => {
        this.cacheDOMElements();
        this.bindEvents();

        var self = this;
        this.statusComponent.render(
            new SingleSelectionDropdownOption('dropdown-status', 'Status', this.changeStatus, true, true)
        );

        if (this.$problemAccount.length) {
            this.problemAccountComponent.render(
                new SingleSelectionDropdownOption(
                    'dropdown-problem-account',
                    'ProblemAccount',
                    this.changeProblemAccount
                )
            );
        }

        if (this.$accountType.length) {
            this.accountTypeComponent.render(
                new SingleSelectionDropdownOption(
                    'dropdown-account-type',
                    'AccountType',
                    this.changeAccountType
                )
            );
        }
        this.$tableReport.stickyTableHeaders();
        this.pager.render(new PagerOption(this.submitForm, 'report-pager', 'potential-win-limit-form'));
    };

    cacheDOMElements = () => {
        this.$status = this.$('#dropdown-status');
        this.$problemAccount = this.$('#dropdown-problem-account');
        this.$accountType = this.$('#dropdown-account-type');
        this.$potentialWinLimitForm = this.$('#potential-win-limit-form');

        this.$username = this.$('#txt-username');
        this.$tableReport = this.$('#tbl-report');
        this.$iconExcel = this.$('#icon-excel');
    };

    bindEvents = () => {
        this.$username.keyup(this.doneInputTextbox);
        const excelUrl = this.$iconExcel.attr('href');
        this.$iconExcel
            .click((e) => {
                if (this.$tableReport.find('tbody tr').length === 0) {
                    e.preventDefault();
                    return;
                }

                const formData = {
                    Status: this.$status.val(),
                    ProblemAccount: this.$problemAccount.val(),
                    AccountType: this.$accountType.val(),
                    Username: this.$username.val()
                };
                this.$iconExcel.attr('href', `${excelUrl}?${this.$.param(formData)}`);
                setTimeout(() => {
                    this.$iconExcel.attr('href', 'javascript:;');
                }, 1000);
            })
            .tooltip()
            .attr('href', 'javascript:;');
        
        // Bind reset win amount icon click event
        this.$tableReport.on('click', '.reset-win-amount', this.resetWinAmount);
    };

    changeStatus = () => {
        this.$('#PageIndex').val(1);
        this.$status.val(this.$status.val());
        this.submitForm();
    };

    changeProblemAccount = () => {
        this.$('#PageIndex').val(1);
        this.$problemAccount.val(this.$problemAccount.val());
        this.submitForm();
    };

    changeAccountType = () => {
        this.$('#PageIndex').val(1);
        this.$accountType.val(this.$accountType.val());
        this.submitForm();
    };

    submitForm = () => {
        this.$.blockUI();
        this.$potentialWinLimitForm.submit();
    };

    doneInputTextbox = (event: any) => {
        var keyCode = event.keyCode || event.which;

        if (keyCode === 13) {
            this.$('#PageIndex').val(1);
            this.submitForm();
        }
    };

    resetWinAmount = (event: any) => {
        event.preventDefault();
        
        var $resetIcon = this.$(event.currentTarget);
        var custId = $resetIcon.data('cust-id');
        var username = $resetIcon.data('username');
        
        if (!custId || !username) {
            return;
        }
        
        var confirmMessage = this.$('#ResetCurrentWinAmountConfirmMessage').val();
        confirmMessage = confirmMessage.replace('{0}', username);
        
        window.top.bootboxConfirm(confirmMessage, null, (result: any) => {
            this.$.blockUI();
            
            const errorMessage = this.$('#ResetCurrentWinAmountErrorMessage').val();
            this.$.ajax({
                url: '/settings/WinLossLimit/ResetCurrentSportsbookWinAmount',
                type: 'POST',
                headers: {
                    'X-Requested-With': 'XMLHttpRequest',
                    RequestVerificationToken: this.$('input[name="__RequestVerificationToken"]').val()
                },
                data: {
                    custId: custId,
                    username: username
                },
                success: (response: any) => {
                    window.top.bootboxAlert(response.message || errorMessage);
                    if (response && response.isSuccessful) {
                        setTimeout(() => {
                            // Refresh the page to show updated data
                            this.submitForm();
                        }, 3000);
                    }
                },
                error: (xhr: any, status: any, error: any) => {
                    window.top.bootboxAlert(error || errorMessage);
                },
                complete: () => {
                    this.$.unblockUI();
                }
            });
        });
    };
}

const potentialWinLimit = new PotentialWinLimit(
    $,
    new SingleSelectionDropdownComponent($),
    new SingleSelectionDropdownComponent($),
    new SingleSelectionDropdownComponent($),
    new PagerComponent($)
);

$(() => {
    potentialWinLimit.init();
});
