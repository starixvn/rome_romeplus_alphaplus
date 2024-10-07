'use strict';

class CustomerListDataTable {
    private $: JQueryStatic;
    private $table: JQuery;
    private customerListPopup: CustomerListPopup;
    private customerListServices: CustomerListServices;
    private editSingleSetting: EditSingleSetting;
    private editMultipleSetting: EditMultipleSetting;
    private editStatus: EditStatus;
    private unlock: Unlock;
    private viewModel: CustomerListViewModel;
    private topWindow: any;
    private username: string;
    private status: string;
    private doubleComm: string;
    private allowAutoTransfer: string;
    private enableAllDayBalanceTransfer: string;

    resources = {
        ThereIsNoData: (<HTMLInputElement>document.getElementById('thereIsNoData')).value,
        PageSize: (<HTMLInputElement>document.getElementById('pageSize')).value,
        Of: (<HTMLInputElement>document.getElementById('of')).value,
        Page: (<HTMLInputElement>document.getElementById('page')).value
    };

    constructor(
        $: JQueryStatic,
        topWindow: any,
        viewModel: CustomerListViewModel,
        customerListPopup: CustomerListPopup,
        customerListServices: CustomerListServices,
        editSingleSetting: EditSingleSetting,
        editMultipleSetting: EditMultipleSetting,
        editStatus: EditStatus,
        unlock: Unlock
    ) {
        this.$ = $;
        this.topWindow = topWindow;
        this.viewModel = viewModel;
        this.customerListPopup = customerListPopup;
        this.customerListServices = customerListServices;
        this.editSingleSetting = editSingleSetting;
        this.editMultipleSetting = editMultipleSetting;
        this.editStatus = editStatus;
        this.unlock = unlock;

        this.topWindow.customerListNew = true;
    }

    public bindingEvent = () => {
        this.allowAutoTransfer = this.$('#dautoTransferFilter').val();

        this.editMultipleSetting.bindingEvent();
        this.initDataTable();
    };

    private getRowClass = (data) => {
        if (!data.Enabled) {
            return statusClasses.Disabled.Css;
        } else if (data.Closed) {
            const closed = statusClasses.Closed.filter((closedReason) => data.ClosedReason === closedReason.Id);
            return closed && closed.length ? closed[0].Css : '';
        } else if (data.Suspended) {
            return statusClasses.Suspended.Css;
        } else if (data.Locked) {
            const locked = statusClasses.Locked.filter((locked) => data.Locked === locked.Id);
            return locked && locked.length ? locked[0].Css : '';
        }

        return data.Enabled.Css;
    };

    private initDataTable = () => {
        const maxPageSize = this.viewModel.IsMobileView ? 200 : 500;
        const details = {
                type: 'column',
                target: -1,
                minWidth: '100px',
                auto: false,
                renderer: (api, rowIdx) => this.renderDetailColumn(api, rowIdx),
                bindingCallback: this.detailCallBack
            },
            lengthMenu =
                `${this.resources.PageSize} <select>` +
                '<option value="10">10</option>' +
                '<option value="50">50</option>' +
                '<option value="100">100</option>' +
                `<option value="${maxPageSize}">${maxPageSize}</option>` +
                '</select>',
            oPaginate = {
                sFirst: '',
                sPrevious: '',
                sNext: '',
                sLast: '',
                info: `${this.resources.Page} _INPUT_ ${this.resources.Of} _TOTAL_`
            };

        this.$table = this.$('#tbl-customer-list')
            //.on('processing.dt', (e, settings, processing) => processing ? this.$.blockUI() : this.$.unblockUI())
            .DataTable({
                serverSide: true,
                ajax: (data, callback) => {
                    if (this.viewModel.CustomerListData) {
                        callback({
                            data: this.viewModel.CustomerListData.DownlinesList,
                            recordsTotal: this.viewModel.CustomerListData.TotalDownlines,
                            recordsFiltered: this.viewModel.CustomerListData.TotalDownlines
                        });
                        delete this.viewModel.CustomerListData;
                    } else {
                        const params = {
                            CustName: this.viewModel.SearchAccountName,
                            CustId: this.viewModel.SearchAccountId,
                            CustLevelId: this.viewModel.SearchAccountRoleId,
                            PageIndex: data.start / data.length + 1,
                            PageSize: data.length,
                            Username: this.username,
                            Status: this.status,
                            DoubleComm: this.doubleComm,
                            AllowAutoTransfer: this.allowAutoTransfer,
                            EnableAllDayBalanceTransfer: this.enableAllDayBalanceTransfer
                        };

                        this.enableAllDayBalanceTransfer = undefined;
                        this.customerListServices.getCutomerList(params, callback);
                    }
                },
                columns: this.buildDataTableColumns(),
                createdRow: (row, data) => {
                    this.$(row).addClass(this.getRowClass(data));
                },
                responsive: { details },
                columnDefs: [
                    {
                        className: 'control',
                        data: null,
                        orderable: false,
                        defaultContent: '',
                        targets: -1
                    }
                ],
                stripeClasses: [],
                ordering: false,
                paging: true,
                pagingType: 'input',
                pageLength: 100,
                searching: false,
                bInfo: false,
                fixedHeader: true,
                processing: true,
                dom: '<"top"iflp<"clear">>rt<"bottom"ifp<"clear">>',
                language: {
                    processing: '',
                    lengthMenu,
                    oPaginate,
                    emptyTable: this.resources.ThereIsNoData
                },
                drawCallback: this.drawCallback,
                isSubmit: false
            });

        this.$('#tbl-customer-list')
            .find('tbody')
            .on('click', 'td.control', (event) => this.doAdjustHeader(event));

        this.$('thead tr th.control')
            .unbind()
            .bind('click', (event) => this.collapseExpandAll(event));
    };

    private collapseExpandAll = (event) => {
        this.$.blockUI();
        setTimeout(() => {
            const targetParent = this.$(event.currentTarget).parent();

            if (targetParent.hasClass('parent')) {
                targetParent.removeClass('parent');
                this.triggerClickDetailWhenSwitchAll(this.$('tbody tr.parent'));
                this.$(event.currentTarget).attr('title', this.$('#expandAllText').val());
            } else {
                targetParent.addClass('parent');
                this.triggerClickDetailWhenSwitchAll(this.$('tbody tr').not('.parent'));
                this.$(event.currentTarget).attr('title', this.$('#collapseAllText').val());
            }

            this.$.unblockUI();
        }, 100);
    };

    private triggerClickDetailWhenSwitchAll = (controlArray) => {
        controlArray.each((index, detailButton) => this.$(detailButton).find('.control').click());
        this.$table.fixedHeader.adjust();
    };

    private doAdjustHeader = (event) => {
        if (!event.isTrigger) {
            this.$table.fixedHeader.adjust();
        }
    };

    private buildDataTableColumns = () => {
        let columns = [];

        if (!this.viewModel.IsMobileView) {
            columns.push({
                className: 'center',
                data: 'RowNumber',
                width: '50px;'
            });
        }

        if (this.viewModel.ShowEditMultiple) {
            columns.push({
                className: 'center',
                data: null,
                render: (item) =>
                    `<input type="checkbox" class="checkbox" data-custid="${item.CustId}" data-username="${
                        item.Username
                    }" data-autotransfer="${item.AutoTransfer ? 1 : 0}"/>`
            });
        }

        columns.push(
            {
                className: 'center',
                data: null,
                render: (item) => this.renderEditColumn(item)
            },
            {
                className: 'left',
                data: null,
                render: (item) => this.renderUsernameColumn(item)
            },
            {
                className: 'center',
                data: (item) => this.renderStatusColumn(item)
            }
        );

        if (this.viewModel.CanViewReport) {
            columns.push({
                className: 'center',
                data: null,
                render: (item) => this.renderStatementColumn(item)
            });
        }

        columns.push({
            className: 'left',
            data: null,
            render: (item) =>
                `<div class="${this.viewModel.IsMobileView ? '' : 'break-long-word'}">${item.FirstName || ''} ${
                    item.LastName || ''
                }</div>`
        });

        if (this.viewModel.SearchAccountRoleId === CustomerLevel.Agent) {
            columns.push({
                className: 'center',
                data: 'UGroup'
            });
        }

        if (this.viewModel.SearchAccountRoleId === CustomerLevel.Agent) {
            columns.push(
                {
                    className: 'left',
                    data: 'Discount'
                },
                {
                    className: 'left',
                    data: 'Discount1x2'
                },
                {
                    className: 'left',
                    data: 'DiscountCS'
                }
            );

            if (this.viewModel.IsShowMyanmarOdds) {
                columns.push({
                    className: 'center',
                    data: 'AdiscountMyanmarOdds'
                });
            }

            columns.push(
                {
                    className: 'left',
                    data: 'PlayerDiscount'
                },
                {
                    className: 'left',
                    data: 'PlayerDiscount1x2'
                },
                {
                    className: 'left',
                    data: 'PlayerDiscountCS'
                }
            );

            if (this.viewModel.IsShowMyanmarOdds) {
                columns.push({
                    className: 'center',
                    data: 'PlayerDiscountMyanmarOdds'
                });
            }
        } else {
            columns.push({
                className: 'center',
                data: 'GroupA'
            });

            if (!this.viewModel.IsShowMyanmarOdds) {
                columns.push(
                    {
                        className: 'center',
                        data: 'GroupB'
                    },
                    {
                        className: 'center',
                        data: 'GroupC'
                    },
                    {
                        className: 'center',
                        data: 'GroupD'
                    }
                );
            }

            columns.push(
                {
                    className: 'center',
                    data: 'Discount1x2'
                },
                {
                    className: 'center',
                    data: 'DiscountCS'
                }
            );

            if (this.viewModel.IsShowMyanmarOdds) {
                columns.push({
                    className: 'center',
                    data: 'DiscountMyanmarOdds'
                });
            }
        }

        if (!this.viewModel.LimitedDateToShowInfo) {
            columns.push({
                className: 'center',
                data: 'CreateDateFormat'
            });
        }

        columns.push(
            {
                className: 'center',
                data: null,
                render: (item) => this.renderLoginIP(item)
            },
            {
                data: null,
                render: () => ''
            }
        );

        return columns;
    };

    private renderUsernameColumn = (item) => {
        if (item.RoleId === CustomerLevel.Member) {
            return item.Locked
                ? `<div class='col-username'><span>${item.Username}</span><br/><span class='grey-color'>${
                      item.UserName2 === '' || item.UserName2 === null ? '--' : item.UserName2
                  }</span>${this.renderLockedIcon(item)}</div>`
                : `<span>${item.Username}</span><br/><span class='grey-color'>${
                      item.UserName2 === '' || item.UserName2 === null ? '--' : item.UserName2
                  }</span>`;
        }

        const content = `<span>${item.Username}</span><br/><span class='grey-color'>${
            item.UserName2 === '' || item.UserName2 === null ? '--' : item.UserName2
        }</span>`;
        const queryString = {
            CustLevelId: item.RoleId,
            CustId: item.CustId,
            CustName: item.Username,
            Status: this.$('#statusFilter').val(),
            DoubleComm: this.$('#dcommFilter').val(),
            AllowAutoTransfer: this.$('#dautoTransferFilter').val(),
            UnconfirmSetting: this.$('#unconfirmSettingFilter').val()
        };

        const viewDownlineLink = `${this.viewModel.MemberInfoUrl}CustomerList/${
            this.viewModel.DownlineLevelName
        }?${this.$.param(queryString)}&BreadcrumbLevelJson=${this.viewModel.BreadcrumbLevelJson}`;
        return item.Locked
            ? `<div class='col-username' title='${this.$(
                  '#usernameTooltipText'
              ).val()}'><a class="view-downline" data-url="${viewDownlineLink}" >${content}</a>${this.renderLockedIcon(item)}</div>`
            : `<div title='${this.$(
                  '#usernameTooltipText'
              ).val()}'><a class="view-downline" data-url="${viewDownlineLink}" >${content}</a></div>`;
    };

    private renderLockedIcon = (item) => {
        if (!this.viewModel.CanUpdateAccount) {
            return "<span class='icon-locked'></span>";
        }

        let confirmMessage = '';
        if (item.Locked === 1) {
            confirmMessage = this.$('#confirmUnlockInCaseFalseOTPSC').val();
        } else if (item.Locked === 2) {
            confirmMessage = this.$('#confirmUnlockInCaseFalsePassword').val();
        }

        const params = {
            SearchAccountId: item.CustId,
            SearchAccountName: item.Username,
            SearchAccountRoleId: item.RoleId,
            StatusName: 'Unlock'
        };

        return `<span class='icon-locked' title='${this.$(
            '#unlockTooltipText'
        ).val()}' data-message='${confirmMessage}' data-url='UpdateCustomerStatus' data-reloadcustomerlist='true' data-params=${JSON.stringify(
            params
        )}></span>`;
    };

    private renderEditColumn = (item) => {
        const attrs = {
            class: 'edit icon-table-edit',
            id: `setting-${item.CustId}`,
            'data-custid': item.CustId,
            'data-username': item.Username,
            'data-url': this.buildGetAllStatusLink(item),
            title: this.$('#editSettingText').val()
        };

        return `<div ${buildAttributes(attrs)}/>`;
    };

    private renderStatusColumn = (item) => {
        const attrs = {
            class: 'status-popup',
            id: `status-${item.CustId}`,
            'data-custid': item.CustId,
            'data-username': item.Username,
            'data-url': this.buildGetAllStatusLink(item),
            title: this.$('#editStatusText').val()
        };

        return `<a ${buildAttributes(attrs)}><span>${item.Status}</span><span class='icon-menu-down'/></a>`;
    };

    private renderStatementColumn = (item) => {
        const queryString = {
                CustId: item.CustId,
                CustName: item.Username,
                CustLevelId: item.RoleId
            },
            actionName = item.RoleId === CustomerLevel.Member ? 'MemberStatement' : 'SMAStatement';

        const reportDomain =
            this.topWindow.menuBar.domElements.reportDomain ||
            this.viewModel.ReportsUrl.substr(0, this.viewModel.ReportsUrl.length - 1);
        const viewStatementLink = `${reportDomain}${
            this.topWindow.menuBar.domElements.cookieless
        }Statement/${actionName}?${this.$.param(queryString)}`;
        return `<a data-url='${viewStatementLink}' title='${this.$(
            '#statementTooltipText'
        ).val()}' class='icon-statement' />`;
    };

    private renderLoginIP = (item) => {
        if (item.LastLoginIP === '' || item.LastLoginIP === null) {
            return '';
        }

        if (!item.AllowOpenInfoPopup) {
            return item.LastLoginIP;
        }

        const attrs = {
            class: 'login-ip',
            'data-title': `${this.$('#loginIPText').val()} - ${item.Username}`,
            'data-url': `/main/IPInfo`,
            'data-params': JSON.stringify({ ip: item.LastLoginIP }),
            'data-width': '500px',
            'data-height': '400px',
            title: this.$('#viewIPTooltipText').val()
        };

        return `<a ${buildAttributes(attrs)}>${item.LastLoginIP}</a>`;
    };

    private renderDetailColumn = (api, rowIdx) => {
        const self = this;
        let commissionColumns = [];
        let otherColumns = [];
        let ugroupColumnsOnAgentLevel = [];

        const statusCss = self.getRowClass(api.data()[rowIdx]);

        api.cells(rowIdx, ':hidden')
            .eq(0)
            .each(function (cell) {
                const header = self.$(api.column(cell.column).header());
                const idx = api.cell(cell).index();

                if (header.hasClass('control') || header.hasClass('never')) {
                    return '';
                }

                const dtPrivate = api.settings()[0];
                const cellData = dtPrivate.oApi._fnGetCellData(dtPrivate, idx.row, idx.column, 'display');

                if (header.hasClass('ugroup')) {
                    ugroupColumnsOnAgentLevel.push(
                        `<tr class="${statusCss}" data-dtr-index="${idx.column}">` +
                            `<td class="dtr-title">${header.text()}</td> ` +
                            `<td class="dtr-data">${cellData}</td></tr>`
                    );
                } else if (header.hasClass('commission')) {
                    if (header.hasClass('startGroup')) {
                        commissionColumns.push(
                            `<tr class="${statusCss}"><td class='commission-wrapper' colspan='2'>` +
                                `<table class='commission-table'><tr class="${statusCss}">` +
                                `<td class='commission-header' colspan="2">${self.$('#commissionText').val()}</td></tr>`
                        );
                    }

                    commissionColumns.push(
                        `<tr class="${statusCss}" data-dtr-index="${idx.column}">` +
                            `<td class="dtr-title">${header.text()}</td> ` +
                            `<td class="dtr-data">${cellData}</td></tr>`
                    );

                    if (header.hasClass('endGroup')) {
                        commissionColumns.push('</table></td></tr>');
                    }
                } else {
                    otherColumns.push(
                        `<tr class="${statusCss}" data-dtr-index="${idx.column}">` +
                            `<td class="dtr-title">${header.text()}</td> ` +
                            `<td class="dtr-data">${cellData}</td></tr>`
                    );
                }
            });

        const detailsColumn = otherColumns.concat(ugroupColumnsOnAgentLevel).concat(commissionColumns);
        return detailsColumn
            ? self.$(`<table class="detail-table" data-dtr-index="${rowIdx}"/>`).append(detailsColumn.join(''))
            : false;
    };

    public reloadData = () => {
        this.$.unblockUI();
        this.$table.ajax.reload();
    };

    private detailCallBack = () => {
        this.$('.login-ip')
            .unbind()
            .bind('click', (event) => this.customerListPopup.openProductPopup(event.currentTarget));

        this.$('.icon-statement')
            .unbind()
            .bind('click', (event) => this.viewDataURL(event));

            this.$('.view-downline')
            .unbind()
            .bind('click', (event) => this.viewDataURL(event));

        this.editSingleSetting.bindingEvent();

        this.editStatus.bindingEvent();

        this.unlock.bindingEvent();

        this.$('td.control').each((index, element) => {
            this.$(element).attr(
                'title',
                this.$(element).parent().hasClass('parent')
                    ? this.$('#collapseText').val()
                    : this.$('#expandText').val()
            );
        });
    };

    private drawCallback = () => {
        this.detailCallBack();

        this.$('#tbl-customer-list').removeClass('hidden');

        this.$('.btn-submit')
            .unbind()
            .bind('click', () => {
                this.username = this.$('#txt-username').val();
                this.status = this.$('#statusFilter').val();
                this.doubleComm = this.$('#dcommFilter').val();
                this.allowAutoTransfer = this.$('#dautoTransferFilter').val();
                this.enableAllDayBalanceTransfer = this.$('#enableAllDayBalanceTransfer').val();

                this.$('.select-all').prop('checked', false);
                this.$table.ajax.reload();
            });

        this.$('tbody .checkbox')
            .unbind()
            .bind('click', () => {
                this.$('.select-all').prop(
                    'checked',
                    this.$('tbody .checkbox').length === this.$('tbody .checkbox:checked').length
                );
            });

        this.$('td.control').each((index, element) => {
            this.$(element).attr(
                'title',
                this.$(element).parent().hasClass('parent')
                    ? this.$('#collapseText').val()
                    : this.$('#expandText').val()
            );
        });

        this.$('thead tr th.control').parent().removeClass('parent');
    };

    private viewDataURL = (event) => {
        event.preventDefault();

        const mainFrame = this.topWindow.$('#main');
        if (!this.topWindow.alpha.core.isIe()) {
            mainFrame.contents().find('head').append(this.$('#AssetCommonCssLink').val());
            mainFrame.contents().find('body').empty().block();
        }

        mainFrame.attr('src', this.$(event.currentTarget).data('url'));
    };

    private buildGetAllStatusLink = (item) => {
        const queryString = {
            SearchAccountId: item.CustId,
            SearchAccountName: item.Username,
            SearchAccountRoleId: item.RoleId,
            UplineId: this.viewModel.SearchAccountId,
            UplineName: this.viewModel.SearchAccountName,
            UplineRoleId: this.viewModel.SearchAccountRoleId
        };

        return `${this.viewModel.MemberInfoUrl}CustomerList/GetCustomerAllStatus?${this.$.param(queryString)}`;
    };
}
