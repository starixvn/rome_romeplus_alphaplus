/// <reference path="../components/single-selection-dropdown.ts" />
/// <reference path="../components/pager.ts" />
'use strict';

interface Window {
    bootboxConfirm(message: string, callback: (result: boolean) => void): void;
}

interface JQuery {
    stickyTableHeaders(): void;
}

class WinLossLimit {
    $: JQueryStatic;
    productComponent: SingleSelectionDropdownComponent;
    masterListComponent: SingleSelectionDropdownComponent;
    agentListComponent: SingleSelectionDropdownComponent;
    accountTypeComponent: SingleSelectionDropdownComponent;
    pager: PagerComponent;
    compactFeedback: CompactFeedbackComponent;

    $productList: JQuery;
    $sportType: JQuery;
    $betType: JQuery;
    $productId: JQuery;
    $winLossLimitForm: JQuery;

    $masterList: JQuery;
    $agentList: JQuery;
    $accountTypeList: JQuery;
    $masterId: JQuery;
    $agentId: JQuery;
    $accountTypeId: JQuery;
    $username: JQuery;
    $playerId: JQuery;
    $tableReport: JQuery;
    $resultMsg: JQuery;
    $notAuthorizedMessage: JQuery;

    constructor(
        $: JQueryStatic,
        productComponent: SingleSelectionDropdownComponent,
        masterListComponent: SingleSelectionDropdownComponent,
        agentListComponent: SingleSelectionDropdownComponent,
        accountTypeComponent: SingleSelectionDropdownComponent,
        pager: PagerComponent,
        compactFeedback: CompactFeedbackComponent
    ) {
        this.$ = $;
        this.productComponent = productComponent;
        this.masterListComponent = masterListComponent;
        this.agentListComponent = agentListComponent;
        this.accountTypeComponent = accountTypeComponent;
        this.pager = pager;
        this.compactFeedback = compactFeedback;
    }

    init = () => {
        this.cacheDOMElements();
        this.bindEvents();

        var self = this;
        this.productComponent.render(
            new SingleSelectionDropdownOption(
                'dropdown-product',
                'UserSelectedProductId',
                this.changeProduct,
                true,
                true
            )
        );

        if (this.$masterList.length) {
            this.masterListComponent.render(
                new SingleSelectionDropdownOption('dropdown-master', 'MasterId', this.changeMaster)
            );
        }

        if (this.$agentList.length) {
            this.agentListComponent.render(
                new SingleSelectionDropdownOption('dropdown-agent', 'AgentId', this.changeAgent)
            );
        }

        if (this.$accountTypeList.length) {
            this.accountTypeComponent.render(
                new SingleSelectionDropdownOption('dropdown-account-type', 'AccountTypeId', this.changeAccountType)
            );
        }

        this.$tableReport.stickyTableHeaders();
        this.pager.render(new PagerOption(this.submitForm, 'report-pager', 'win-loss-limit-form'));
        this.compactFeedback.init(this.$winLossLimitForm);
    };
    cacheDOMElements = () => {
        this.$productList = this.$('#dropdown-product');
        this.$sportType = this.$('#SportType');
        this.$betType = this.$('#BetType');
        this.$productId = this.$('#UserSelectedProductId');
        this.$winLossLimitForm = this.$('#win-loss-limit-form');

        this.$masterList = this.$('#dropdown-master');
        this.$agentList = this.$('#dropdown-agent');
        this.$accountTypeList = this.$('#dropdown-account-type');

        this.$masterId = this.$('#MasterId');
        this.$agentId = this.$('#RecommendId');
        this.$accountTypeId = this.$('#AccountTypeId');
        this.$username = this.$('#txt-username');
        this.$playerId = this.$('#txt-player-id');
        this.$tableReport = this.$('#tbl-report');

        this.$resultMsg = this.$('#result-msg');

        this.$notAuthorizedMessage = this.$('input[name="NotAuthorizedMessage"]');
    };
    bindEvents = () => {
        this.$username.keyup(this.doneInputTextbox);
        this.$playerId.keyup(this.doneInputTextbox);

        // Bind reset win amount icon click event
        this.$tableReport.on('click', '.reset-win-amount', this.resetWinAmount);
    };
    changeProduct = () => {
        var selectedOption = this.$productList.find(':selected');
        var sportType = selectedOption.attr('data-sport-type');
        var betType = selectedOption.attr('data-bet-type');
        var productId = selectedOption.attr('data-product-id');
        this.$('#PageIndex').val(1);

        this.$sportType.val(sportType);
        this.$betType.val(betType);
        this.$productId.val(productId);
        this.submitForm();
    };
    changeMaster = () => {
        this.$masterId.val(this.$masterList.val());
        this.$agentId.val(0);
        this.$('#PageIndex').val(1);
        this.submitForm();
    };
    changeAgent = () => {
        this.$agentId.val(this.$agentList.val());
        this.$('#PageIndex').val(1);
        this.submitForm();
    };
    changeAccountType = () => {
        this.$accountTypeId.val(this.$accountTypeList.val());
        this.$('#PageIndex').val(1);
        this.submitForm();
    };
    submitForm = () => {
        this.$.blockUI();
        this.$winLossLimitForm.submit();
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

            var selectedOption = this.$productList.find(':selected');
            var sportType = selectedOption.attr('data-sport-type');
            var betType = selectedOption.attr('data-bet-type');
            var productId = selectedOption.attr('data-product-id');

            const errorMessage = this.$('#ResetCurrentWinAmountErrorMessage').val();
            this.$.ajax({
                url: '/settings/WinLossLimit/ResetCurrentWinAmount',
                type: 'POST',
                headers: {
                    'X-Requested-With': 'XMLHttpRequest',
                    RequestVerificationToken: this.$('input[name="__RequestVerificationToken"]').val()
                },
                data: {
                    custId: custId,
                    username: username,
                    productId: productId,
                    sportType: sportType,
                    betType: betType
                },
                success: (response: any) => {
                    this.$resultMsg.html(response.Message);
                    this.$resultMsg.removeClass('msg-success msg-error msg-warning');

                    if (response && response.IsSuccessful) {
                        this.$resultMsg.addClass('msg-success');
                        setTimeout(() => {
                            // Refresh the page to show updated data
                            this.$resultMsg.addClass('hide');
                            this.submitForm();
                        }, 3000);
                    } else {
                        this.$resultMsg.addClass('msg-error');
                    }

                    this.$resultMsg.removeClass('hide');
                    window.scrollTo(0, 0);
                },
                error: (xhr: any, status: any, error: any) => {
                    if (JSON.parse(xhr.responseText) && JSON.parse(xhr.responseText).message === 'NoPermission') {
                        error = this.$notAuthorizedMessage.val();
                    }
                    this.$resultMsg.html(error || errorMessage);
                    this.$resultMsg.removeClass('msg-success msg-error msg-warning');
                    this.$resultMsg.addClass('msg-error');
                    this.$resultMsg.removeClass('hide');
                    window.scrollTo(0, 0);
                },
                complete: () => {
                    this.$.unblockUI();
                }
            });
        });
    };
}

const winLossLimit = new WinLossLimit(
    $,
    new SingleSelectionDropdownComponent($),
    new SingleSelectionDropdownComponent($),
    new SingleSelectionDropdownComponent($),
    new SingleSelectionDropdownComponent($),
    new PagerComponent($),
    new CompactFeedbackComponent()
);

$(() => {
    winLossLimit.init();
});
