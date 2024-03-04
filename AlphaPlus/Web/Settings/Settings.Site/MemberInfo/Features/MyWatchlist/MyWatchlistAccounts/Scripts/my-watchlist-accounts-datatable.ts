"use strict";

class MyWatchlistAccountsDataTable {
    private $: JQueryStatic;
    private editSettingPopup: EditSettingPopup;
    private topWindow: any;

    private isMobile: boolean;
    private isShowLastLoginColumn: boolean;
    private canUpdateAccount: boolean;
    private $username: JQuery;

    private $tableReport: JQuery;
    private $dataTable: JQuery;
    private myWatchlistAccountData: any;

    constructor($: JQueryStatic, editSettingPopup: EditSettingPopup, myWatchlistAccountData: any, topWindow: any) {
        this.$ = $;
        this.editSettingPopup = editSettingPopup;
        this.myWatchlistAccountData = myWatchlistAccountData;
        this.topWindow = topWindow;
    }

    public reloadData() {
        this.$dataTable.ajax.reload();
    }

    public init(baseUrl: string) {
        this.cacheDOMElements();
        const $reportForm = this.$("#report-form");

        this.editSettingPopup.init(this.$tableReport, baseUrl);
        this.$dataTable = DataTableHelper.initDataTable(
            this.$tableReport,
            this.buildDataTableOptions($reportForm.attr("action")),
            this.buildDataTablePagingOptions());
        $reportForm.submit(event => {
            event.preventDefault();
            this.$dataTable.ajax.reload();
        });
    }

    private cacheDOMElements() {
        this.isMobile = this.$("#is-mobile").val() === "true";
        this.isShowLastLoginColumn = this.$("#is-show-last-login").val() === "true";
        this.canUpdateAccount = this.$("#can-update-account").val() === "true";
        this.$username = this.$("#txt-username");
        this.$tableReport = this.$('#tblRpt');
    }

    private buildDataTableOptions(myWatchlistUrl: string) {
        const sortDefinitions = {
            Username: 1,
            LastLogin: 2,
            Status: 3,
            WinningThreshold: 4,
            LosingThreshold: 5,
            IsPA: 7
        };

        return {
            ajax: (data, callback) => {
                if (this.myWatchlistAccountData) {
                    callback({
                        data: this.myWatchlistAccountData.WatchlistAccounts,
                        recordsTotal: this.myWatchlistAccountData.TotalItems,
                        recordsFiltered: this.myWatchlistAccountData.TotalItems
                    });
                    delete this.myWatchlistAccountData;
                } else {
                    const columnName = data.columns[data.order[0].column].name;
                    const sortType = sortDefinitions[columnName];
                    const formData = {
                        sortType,
                        isDesc: data.order[0].dir === 'desc',
                        pageIndex: (data.start / data.length) + 1,
                        pageSize: data.length,
                        username: this.$username.val().trim()
                    };
                    AJAXHelper.postJSON(
                        myWatchlistUrl,
                        formData,
                        jsonData => callback({
                            data: jsonData.WatchlistAccounts,
                            recordsTotal: jsonData.TotalItems,
                            recordsFiltered: jsonData.TotalItems
                        }));
                }
            },
            columns: this.buildDataTableColumns(),
            createdRow: (row, data) => this.$(row).addClass(CustomerStatusHelper.getRowClassName(data)),
            order: [[this.getDefaultOrderColumn(), 'asc']],
            language: {
                emptyTable: this.$("#no-info").val()
            },
            drawCallback: () => this.drawCallback()
        };
    }

    private getDefaultOrderColumn() {
        if (this.isMobile) {
            if (!this.canUpdateAccount) {
                return 0;
            }

            return 2;
        } else {
            if (!this.canUpdateAccount) {
                return 1;
            }

            return 3;
        }
    }

    private buildDataTableColumns() {
        let columns = [];

        !this.isMobile &&
            columns.push({ className: 'center', data: "RowNumber", orderable: false });

        if (this.canUpdateAccount) {
            columns.push(
                { className: 'center', data: null, render: item => this.renderCheckboxColumn(item), orderable: false },
                { className: 'center', data: null, render: item => this.renderEditColumn(item), orderable: false }
            );
        }

        columns.push({ className: 'left', data: "Username", name: "Username" });

        this.isShowLastLoginColumn &&
            columns.push({ className: 'center', data: "LastLoggedString", name: "LastLogin" });

        columns.push(
            { className: 'center', data: "TranslatedStatus", name: "Status" },
            { className: 'center', data: "IsPAText", name: "IsPA" },
            { className: 'right', data: "WinningThresholdString", name: "WinningThreshold" },
            { className: 'right', data: "LosingThresholdString", name: "LosingThreshold" },
            { data: null }
        );

        return columns;
    }

    private renderCheckboxColumn(item) {
        const attrs = {
            'class': 'checkbox',
            'data-custid': item.CustId,
            'data-username': item.Username,
            'data-winningthreshold': item.WinningThreshold,
            'data-losingthreshold': item.LosingThreshold
        };

        return `<input type="checkbox" ${this.buildAttributes(attrs)} />`;
    }

    private renderEditColumn(item) {
        const attrs = {
            'class': 'edit icon-table-edit',
            'id': `setting-${item.CustId}`,
            'data-custid': item.CustId,
            'data-username': item.Username,
            'data-winningthreshold': item.WinningThreshold,
            'data-losingthreshold': item.LosingThreshold
        };

        return `<div ${this.buildAttributes(attrs)}/>`;
    }

    private buildAttributes = attrs => this.$.map(attrs, (value, key) => `${key}='${value}'`).join(' ');

    private buildDataTablePagingOptions() {
        return {
            paging: true,
            pageLength: 100,
            pageSizes: [10, 50, 100, this.isMobile ? 200 : 500],
            pageSizeText: this.$("#page-size").val(),
            pageText: this.$("#page").val(),
            ofText: this.$("#of").val()
        };
    }

    private drawCallback() {
        this.$tableReport.removeClass("hidden");
        this.editSettingPopup.bindEvents(this.$tableReport);

        const isOpenFromProblemAccount = this.$('#isOpenFromProblemAccount').val();
        isOpenFromProblemAccount && JSON.parse(isOpenFromProblemAccount) && setTimeout(() => {
            this.openAddMultiplePopup();
            this.topWindow.menuBar.activeMyWatchlistSubMenuItem();
        }, 1500);
    }

    private openAddMultiplePopup() {
        const custIds = this.$('#custIds').val();
        this.$('#custIds').val("");
        this.$('#isOpenFromProblemAccount').val("");

        const baseUrl = this.$('#member-info-url').val();

        this.topWindow.popupManager.open(
            `${baseUrl}MyWatchlistAccounts/ManageWatchlistAccounts?Usernames=${custIds}&isEdite=true`,
            undefined,
            820,
            550,
            "modal-mobile-friendly");
    }
}