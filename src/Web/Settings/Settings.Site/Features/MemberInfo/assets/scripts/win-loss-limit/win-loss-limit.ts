/// <reference path="../components/single-selection-dropdown.ts" />
/// <reference path="../components/pager.ts" />
'use strict';

interface JQuery {
    stickyTableHeaders(): void;
}

class WinLossLimit {
    $: JQueryStatic;
    productComponent: SingleSelectionDropdownComponent;
    masterListComponent: SingleSelectionDropdownComponent;
    agentListComponent: SingleSelectionDropdownComponent;
    pager: PagerComponent;

    $productList: JQuery;
    $sportType: JQuery;
    $betType: JQuery;
    $productId: JQuery;
    $winLossLimitForm: JQuery;

    $masterList: JQuery;
    $agentList: JQuery;
    $masterId: JQuery;
    $agentId: JQuery;
    $username: JQuery;
    $playerId: JQuery;
    $tableReport: JQuery;
    $pageIndex: JQuery;

    constructor(
        $: JQueryStatic,
        productComponent: SingleSelectionDropdownComponent,
        masterListComponent: SingleSelectionDropdownComponent,
        agentListComponent: SingleSelectionDropdownComponent,
        pager: PagerComponent
    ) {
        this.$ = $;
        this.productComponent = productComponent;
        this.masterListComponent = masterListComponent;
        this.agentListComponent = agentListComponent;
        this.pager = pager;
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

        this.$tableReport.stickyTableHeaders();
        this.pager.render(new PagerOption(this.submitForm, 'report-pager', 'win-loss-limit-form'));
    };
    cacheDOMElements = () => {
        this.$productList = this.$('#dropdown-product');
        this.$sportType = this.$('#SportType');
        this.$betType = this.$('#BetType');
        this.$productId = this.$('#UserSelectedProductId');
        this.$winLossLimitForm = this.$('#win-loss-limit-form');

        this.$masterList = this.$('#dropdown-master');
        this.$agentList = this.$('#dropdown-agent');

        this.$masterId = this.$('#MasterId');
        this.$agentId = this.$('#RecommendId');
        this.$username = this.$('#txt-username');
        this.$playerId = this.$('#txt-player-id');
        this.$tableReport = this.$('#tbl-report');
        this.$pageIndex = this.$('#PageIndex');
    };
    bindEvents = () => {
        this.$username.keyup(this.doneInputTextbox);
        this.$playerId.keyup(this.doneInputTextbox);
    };
    changeProduct = () => {
        var selectedOption = this.$productList.find(':selected');
        var sportType = selectedOption.attr('data-sport-type');
        var betType = selectedOption.attr('data-bet-type');
        var productId = selectedOption.attr('data-product-id');
        this.$pageIndex.val(1);

        this.$sportType.val(sportType);
        this.$betType.val(betType);
        this.$productId.val(productId);
        this.submitForm();
    };
    changeMaster = () => {
        this.$masterId.val(this.$masterList.val());
        this.$agentId.val(0);
        this.$pageIndex.val(1);
        this.submitForm();
    };
    changeAgent = () => {
        this.$agentId.val(this.$agentList.val());
        this.$pageIndex.val(1);
        this.submitForm();
    };
    submitForm = () => {
        this.$.blockUI();
        this.$winLossLimitForm.submit();
    };
    doneInputTextbox = (event: any) => {
        var keyCode = event.keyCode || event.which;

        if (keyCode === 13) {
            this.$pageIndex.val(1);
            this.submitForm();
        }
    };
}

const winLossLimit = new WinLossLimit(
    $,
    new SingleSelectionDropdownComponent($),
    new SingleSelectionDropdownComponent($),
    new SingleSelectionDropdownComponent($),
    new PagerComponent($)
);

$(() => {
    winLossLimit.init();
});
