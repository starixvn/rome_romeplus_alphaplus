/// <reference path="../components/single-selection-dropdown.ts" />
/// <reference path="../components/pager.ts" />
'use strict';

interface JQuery {
    stickyTableHeaders(): void;
}

class PotentialWinLimit {
    $: JQueryStatic;
    statusComponent: SingleSelectionDropdownComponent;
    problemAccountComponent: SingleSelectionDropdownComponent;
    pager: PagerComponent;

    $status: JQuery;
    $problemAccount: JQuery;
    $potentialWinLimitForm: JQuery;

    $username: JQuery;
    $tableReport: JQuery;
    $iconExcel: JQuery;
    $pageIndex: JQuery;

    constructor(
        $: JQueryStatic,
        statusComponent: SingleSelectionDropdownComponent,
        problemAccountComponent: SingleSelectionDropdownComponent,
        pager: PagerComponent
    ) {
        this.$ = $;
        this.statusComponent = statusComponent;
        this.problemAccountComponent = problemAccountComponent;
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
        this.$tableReport.stickyTableHeaders();
        this.pager.render(new PagerOption(this.submitForm, 'report-pager', 'win-loss-limit-form'));
    };

    cacheDOMElements = () => {
        this.$status = this.$('#dropdown-status');
        this.$problemAccount = this.$('#dropdown-problem-account');
        this.$potentialWinLimitForm = this.$('#potential-win-limit-form');

        this.$username = this.$('#txt-username');
        this.$tableReport = this.$('#tbl-report');
        this.$iconExcel = this.$('#icon-excel');
        this.$pageIndex = this.$('#PageIndex');
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
                    Username: this.$username.val()
                };
                this.$iconExcel.attr('href', `${excelUrl}?${this.$.param(formData)}`);
                setTimeout(() => {
                    this.$iconExcel.attr('href', 'javascript:;');
                }, 1000);
            })
            .tooltip()
            .attr('href', 'javascript:;');
    };

    changeStatus = () => {
        this.$pageIndex.val(1);
        this.$status.val(this.$status.val());
        this.submitForm();
    };

    changeProblemAccount = () => {
        this.$pageIndex.val(1);
        this.$problemAccount.val(this.$problemAccount.val());
        this.submitForm();
    };

    submitForm = () => {
        this.$.blockUI();
        this.$potentialWinLimitForm.submit();
    };

    doneInputTextbox = (event: any) => {
        var keyCode = event.keyCode || event.which;

        if (keyCode === 13) {
            this.$pageIndex.val(1);
            this.submitForm();
        }
    };
}

const potentialWinLimit = new PotentialWinLimit(
    $,
    new SingleSelectionDropdownComponent($),
    new SingleSelectionDropdownComponent($),
    new PagerComponent($)
);

$(() => {
    potentialWinLimit.init();
});
