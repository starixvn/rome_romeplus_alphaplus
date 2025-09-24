'use strict';

class ReachedThresholdDataTable {
    private $: JQueryStatic;
    private topWindow: Window;
    private readonly compactFeedback: CompactFeedbackComponent;
    private reachedThresholdData?: ReachedThresholdPagingData;

    private isMobile: boolean;
    private isShowLastLoginColumn: boolean;
    private textLocked: string;
    private textConfirmSuspendMember: string;
    private canUpdateAccount: boolean;

    private $txtUsername: JQuery;
    private $fromDate: JQuery;
    private $toDate: JQuery;
    private $selStatus: JQuery;
    private $selLimitType: JQuery;

    private $table: JQuery;

    constructor($: JQueryStatic, topWindow: Window, compactFeedback: CompactFeedbackComponent, reachedThresholdData?: ReachedThresholdPagingData) {
        this.$ = $;
        this.topWindow = topWindow;
        this.compactFeedback = compactFeedback;
        this.reachedThresholdData = reachedThresholdData;
    }

    public init(baseUrl: string) {
        this.cacheDOMElements();
        this.bindEvents(baseUrl);
    }

    private cacheDOMElements() {
        this.isMobile = this.$('#is-mobile').val() === 'true';
        this.isShowLastLoginColumn = this.$('#is-show-last-login').val() === 'true';
        this.textLocked = this.$('#text-locked').val();
        this.textConfirmSuspendMember = this.$('#text-confirm-suspend-member').val();
        this.canUpdateAccount = this.$('#can-update-account').val() === 'true';

        this.$txtUsername = this.$('#txt-username');
        this.$fromDate = this.$('#from-date');
        this.$toDate = this.$('#to-date');
        this.$selStatus = this.$('#status-dropdown');
        this.$selLimitType = this.$('#limit-type-dropdown');
    }

    private bindEvents(baseUrl: string) {
        const $reportForm = this.$('#report-form');

        const $tableReport = this.$('#tblRpt');
        this.$table = DataTableHelper.initDataTable(
            $tableReport,
            this.buildDataTableOptions($reportForm.attr('action')),
            this.buildDataTablePagingOptions()
        );

        $tableReport.on('click', '.open-button', (event) => this.handleSuspend(event, baseUrl));
        $tableReport.on('click', '.reach-count', (event) => this.handleOpenReachHistory(event, baseUrl));
        $reportForm.submit((event) => {
            event.preventDefault();
            this.$table.ajax.reload();
        });
        this.$('#icon-excel').click(this.handleExportExcel);
    }

    private buildDataTableOptions(reachedThresholdUrl: string) {
        const sortDefinitions = {
            Username: 1,
            LastLogin: 2,
            Status: 3,
            LastReach: 4,
            LastLimitTypeReach: 5,
            WinningLosingAmount: 6,
            LastReachWinningThreshold: 7,
            LastReachLosingThreshold: 8,
            NoOfReach: 9,
            IsPA: 11
        };

        return {
            ajax: (data, callback) => {
                const columnName = data.columns[data.order[0].column].name;
                const sortType = sortDefinitions[columnName];
                const formData = {
                    username: this.$txtUsername.val().trim(),
                    fromDate: this.$fromDate.val(),
                    toDate: this.$toDate.val(),
                    status: this.$selStatus.val(),
                    limitType: this.$selLimitType.val(),
                    sortType,
                    isDesc: data.order[0].dir === 'desc',
                    pageIndex: data.start / data.length + 1,
                    pageSize: data.length
                };

                if (this.reachedThresholdData) {
                    callback({
                        data: this.reachedThresholdData.ReachlistAccounts,
                        recordsTotal: this.reachedThresholdData.TotalItems,
                        recordsFiltered: this.reachedThresholdData.TotalItems
                    });
                    this.compactFeedback.init(formData, this.reachedThresholdData.TotalItems > 0);
                    delete this.reachedThresholdData;
                } else {
                    AJAXHelper.postJSON(reachedThresholdUrl, formData, (jsonData) => {
                        callback({
                            data: jsonData.ReachlistAccounts,
                            recordsTotal: jsonData.TotalItems,
                            recordsFiltered: jsonData.TotalItems
                        });
                        this.compactFeedback.init(formData, jsonData.TotalItems > 0);
                    });
                }
            },
            columns: this.buildDataTableColumns(),
            createdRow: (row, data) => this.$(row).addClass(CustomerStatusHelper.getRowClassName(data)),
            order: [[this.isMobile ? 0 : 1, 'asc']],
            language: {
                emptyTable: this.$('#no-info').val()
            }
        };
    }

    private buildDataTableColumns() {
        let columns = [];

        !this.isMobile && columns.push({ className: 'center', data: 'RowNumber', orderable: false });

        columns.push({
            className: 'left col-username',
            data: null,
            render: (data, type) => this.renderUsernameCell(data, type),
            name: 'Username'
        });

        this.isShowLastLoginColumn &&
            columns.push({ className: 'center', data: 'LastLoggedString', name: 'LastLogin' });

        columns.push(
            {
                className: 'center',
                data: null,
                render: (data, type) => this.renderStatusCell(data, type),
                name: 'Status'
            },
            { className: 'center', data: 'IsPAText', name: 'IsPA' },
            { className: 'center', data: 'LastReachedDateString', name: 'LastReach' },
            { className: 'center', data: 'TranslatedLastLimitTypeReach', name: 'LastLimitTypeReach' },
            { className: 'right', data: 'WinLossAmountString', name: 'WinningLosingAmount' },
            { className: 'right', data: 'WinningThresholdString', name: 'LastReachWinningThreshold' },
            { className: 'right', data: 'LosingThresholdString', name: 'LastReachLosingThreshold' },
            {
                className: 'right',
                data: null,
                render: this.renderReachCountColumn,
                name: 'NoOfReach'
            },
            { data: null }
        );

        return columns;
    }

    private renderUsernameCell(data, type) {
        if (type === 'display') {
            return data.Locked
                ? [
                      '<div class="cell-username"><div class="cell-username-text">',
                      data.Username,
                      '</div><div><span class="icon-locked" title="',
                      this.textLocked,
                      '"></span></div></div>'
                  ].join('')
                : data.Username;
        }

        return data;
    }

    private renderStatusCell(data, type) {
        if (type === 'display') {
            return data.IsOpen && this.canUpdateAccount
                ? `<div class="open-button" data-custid="${data.CustId}" data-username="${data.Username}">${data.TranslatedStatus}</div>`
                : data.TranslatedStatus;
        }

        return data;
    }

    private renderReachCountColumn(data, type) {
        return type === 'display'
            ? `<a class="reach-count" href="javascript:;" data-custid="${data.CustId}" data-username="${data.Username}">${data.ReachedCountString}</a>`
            : data;
    }

    private buildDataTablePagingOptions() {
        return {
            paging: true,
            pageLength: 100,
            pageSizes: [10, 50, 100, this.isMobile ? 200 : 500],
            pageSizeText: this.$('#page-size').val(),
            pageText: this.$('#page').val(),
            ofText: this.$('#of').val()
        };
    }

    private handleSuspend(event, baseUrl: string) {
        const $this = this.$(event.target);
        const SearchAccountId = $this.data('custid');
        const SearchAccountName = $this.data('username');

        this.topWindow.bootboxConfirm(this.textConfirmSuspendMember, this.isMobile ? null : event.target, () => {
            AJAXHelper.postForm(
                `${baseUrl}CustomerList/UpdateCustomerStatus`,
                {
                    SearchAccountId,
                    SearchAccountName,
                    SearchAccountRoleId: 1,
                    StatusName: 'Suspended',
                    StatusValue: true
                },
                (result) => {
                    if (result && result.IsSuccessful) {
                        this.$table.ajax.reload();
                        return;
                    }

                    this.topWindow.bootboxAlert(result.Message);
                }
            );
        });
    }

    private handleOpenReachHistory(event, baseUrl) {
        const custId = this.$(event.currentTarget).data('custid');
        const username = this.$(event.currentTarget).data('username');

        const formData = {
            selectedCustId: custId,
            fromDate: this.$fromDate.val(),
            toDate: this.$toDate.val(),
            limitType: this.$selLimitType.val()
        };

        this.topWindow.popupManager.open(
            `${baseUrl}ReachedThreshold/ReachHistoryIndex?${this.$.param(formData)}`,
            `${username} - ${this.$('#reach-history-popup-title').val()}`,
            900,
            600,
            'modal-mobile-friendly'
        );
    }

    private handleExportExcel = () => {
        this.$('#Username').val(this.$txtUsername.val().trim());
        this.$('#FromDate').val(this.$fromDate.val());
        this.$('#ToDate').val(this.$toDate.val());
        this.$('#Status').val(this.$selStatus.val());
        this.$('#LimitType').val(this.$selLimitType.val());

        this.$('#export-excel-form').submit();
    };
}
