class WinLossDetailTable {
    private viewResources: any;
    private $tableReport: JQuery;
    private SuperLevel = 4;
    private MasterLevel = 3;
    private AgentLevel = 2;
    private MemberLevel = 1;
    private totalColumn = new Map([
        [this.SuperLevel, 19],
        [this.MasterLevel, 16],
        [this.AgentLevel, 13]
    ]);
    private sortColumnName = 'username';
    private sortIndicator = 0; // asc: 0, desc: 1

    constructor(viewResources) {
        this.viewResources = viewResources;
        this.$tableReport = $('#tbl-report');
    }

    renderWinLossDetail(reportData, custLevelId: number, getWinLossDetailHandler: Function) {
        const html = [];
        this.buildHeader(html, custLevelId);

        if (reportData.HasData) {
            html.push('<tbody>');
            reportData.DataSource.forEach((winloss) => this.buildBody(html, custLevelId, winloss, false));
            html.push('</tbody><tfoot>');
            this.buildBody(html, custLevelId, reportData.Total, true);
            html.push('</tfoot>');
        } else {
            this.buildBodyNoData(html, custLevelId);
        }

        this.$tableReport.html(html.join(''));
        this.initTableSorter(reportData.MoreThanOne);
        this.$tableReport
            .find('.downline-link')
            .off()
            .click((e) => {
                e.preventDefault();
                const $this = $(e.target);
                const custId = $this.data('custid');
                const custName = $this.data('custname');
                const custLevelId = $this.data('custlevelid');
                if (custLevelId === this.MasterLevel || custLevelId === this.AgentLevel) {
                    getWinLossDetailHandler(custId, custName, custLevelId);
                } else if (custLevelId === this.MemberLevel) {
                }
            });
    }

    buildHeader(html, custLevelId: number) {
        let index = 0;
        html.push(
            '<thead><tr>',
            `<th rowspan="2" class="sorting" data-colindex="${index++}" data-colname="username">${
                this.viewResources.Username
            }</th>`,
            `<th rowspan="2" class="sorting" data-colindex="${index++}" data-colname="fullname">${
                this.viewResources.FirstLastName
            }</th>`,
            `<th rowspan="2" class="sorting" data-colindex="${index++}" data-colname="betcount">${
                this.viewResources.BetCount
            }</th>`,
            `<th rowspan="2" class="sorting" data-colindex="${index++}" data-colname="turnover">${
                this.viewResources.Turnover
            }</th>`,
            `<th rowspan="2" class="sorting" data-colindex="${index++}" data-colname="netturnover">${
                this.viewResources.NetTurnover
            }</th>`,
            `<th rowspan="2" class="sorting" data-colindex="${index++}" data-colname="grosscomm">${
                this.viewResources.GrossComm
            }</th>`,
            `<th colspan="3" id="groupColumnMember">${this.viewResources.Member}</th>`,
            `<th colspan="3" id="groupColumnAgent">${this.viewResources.Agent}</th>`
        );
        if (custLevelId >= this.MasterLevel) {
            html.push(`<th colspan="3" id="groupColumnMaster">${this.viewResources.Master}</th>`);
        }
        if (custLevelId === this.SuperLevel) {
            html.push(`<th colspan="3" id="groupColumnSuper">${this.viewResources.Super}</th>`);
        }
        html.push(
            `<th rowspan="2" class="sorting" data-colindex="${
                this.totalColumn.get(custLevelId) - 1
            }" data-colname="company">${this.viewResources.Company}</th>`
        );
        html.push('</tr><tr class="thead-row2">');
        html.push(
            `<th class="sorting" data-colindex="${index++}" data-colname="pwinloss">${this.viewResources.WinLoss}</th>`,
            `<th class="sorting" data-colindex="${index++}" data-colname="pcomm">${this.viewResources.Comm}</th>`,
            `<th class="sorting" data-colindex="${index++}" data-colname="ptotal">${this.viewResources.Total}</th>`,
            `<th class="sorting" data-colindex="${index++}" data-colname="awinloss">${this.viewResources.WinLoss}</th>`,
            `<th class="sorting" data-colindex="${index++}" data-colname="acomm">${this.viewResources.Comm}</th>`,
            `<th class="sorting" data-colindex="${index++}" data-colname="atotal">${this.viewResources.Total}</th>`
        );
        if (custLevelId >= this.MasterLevel) {
            html.push(
                `<th class="sorting" data-colindex="${index++}" data-colname="mwinloss">${
                    this.viewResources.WinLoss
                }</th>`,
                `<th class="sorting" data-colindex="${index++}" data-colname="mcomm">${this.viewResources.Comm}</th>`,
                `<th class="sorting" data-colindex="${index++}" data-colname="mtotal">${this.viewResources.Total}</th>`
            );
        }
        if (custLevelId === this.SuperLevel) {
            html.push(
                `<th class="sorting" data-colindex="${index++}" data-colname="swinloss">${
                    this.viewResources.WinLoss
                }</th>`,
                `<th class="sorting" data-colindex="${index++}" data-colname="scomm">${this.viewResources.Comm}</th>`,
                `<th class="sorting" data-colindex="${index++}" data-colname="stotal">${this.viewResources.Total}</th>`
            );
        }
        html.push('</tr></thead>');
    }

    buildBody(html, custLevelId: number, winloss, isTotalRow: boolean) {
        const isDormantAccounts = typeof winloss.CustId === 'undefined';
        html.push(`<tr class="${isTotalRow ? 'tbl-footer' : isDormantAccounts ? 'dormant-accounts-mark' : ''}">`);

        if (isTotalRow) {
            html.push(`<td class="text-left">${this.viewResources.Total}</td>`, '<td data-colname="fullname"></td>');
        } else {
            html.push('<td class="text-left col-username">');
            if (isDormantAccounts) {
                html.push(
                    `<span class="pull-left">${this.viewResources.DormantAccounts}</span>`,
                    `<span class="icon icon-information-outline icon-dormant-accounts" data-toggle="tooltip" data-placement="right" title="${this.viewResources.SummaryOfDormantAccounts}"></span>`
                );
            } else {
                html.push(
                    `<a href="javascript:;" data-islink="true" data-custid="${winloss.CustId}" data-custname="${winloss.CustName}" data-custlevelid="${custLevelId}" class="downline-link">${winloss.CustName}</a>`
                );
            }
            html.push(
                '</td>',
                '<td class="text-left" data-colname="fullname">',
                `<div class="fullname" data-toggle="tooltip" data-placement="bottom" title="${winloss.FullName}">`,
                `${winloss.FullName}</div></td>`
            );
        }

        html.push(
            `<td data-colname="betcount">${alphaReports.formatNumber(winloss.NumberOfTickets)}</td>`,
            `<td data-colname="turnover">${alphaReports.formatDecimal(winloss.Turnover)}</td>`,
            `<td data-colname="netturnover">${alphaReports.formatDecimal(winloss.NetTurnover)}</td>`,
            `<td data-colname="grosscomm">${alphaReports.formatDecimal(winloss.GrossComm)}</td>`,
            `<td data-colname="pwinloss">${alphaReports.formatDecimal(winloss.MemberWinLoss)}</td>`,
            `<td data-colname="pcomm">${alphaReports.formatDecimal(winloss.MemberComm)}</td>`,
            `<td data-colname="ptotal">${alphaReports.formatDecimal(winloss.MemberTotal)}</td>`,
            `<td data-colname="awinloss">${alphaReports.formatDecimal(winloss.AgWinLoss)}</td>`,
            `<td data-colname="acomm">${alphaReports.formatDecimal(winloss.AgComm)}</td>`,
            `<td data-colname="atotal">${alphaReports.formatDecimal(winloss.AgTotal)}</td>`
        );
        if (custLevelId >= this.MasterLevel) {
            html.push(
                `<td data-colname="mwinloss">${alphaReports.formatDecimal(winloss.MAgWinLoss)}</td>`,
                `<td data-colname="mcomm">${alphaReports.formatDecimal(winloss.MAgComm)}</td>`,
                `<td data-colname="mtotal">${alphaReports.formatDecimal(winloss.MAgTotal)}</td>`
            );
        }
        if (custLevelId === this.SuperLevel) {
            html.push(
                `<td data-colname="swinloss">${alphaReports.formatDecimal(winloss.SAgWinLoss)}</td>`,
                `<td data-colname="scomm">${alphaReports.formatDecimal(winloss.SAgComm)}</td>`,
                `<td data-colname="stotal">${alphaReports.formatDecimal(winloss.SAgTotal)}</td>`
            );
        }
        html.push(`<td data-colname="company">${alphaReports.formatDecimal(winloss.Company)}</td>`);
    }

    buildBodyNoData(html, custLevelId: number) {
        html.push(
            `<tbody><tr><td colspan="${this.totalColumn.get(custLevelId)}" align="center">${
                this.viewResources.NoInformationIsAvailable
            }</td></tr></tbody>`
        );
    }

    initTableSorter(canSort) {
        const sortColumns = this.$tableReport.find('.sorting');

        if (canSort) {
            const headers = {};
            sortColumns.each(function (index) {
                headers[index] = { sorter: index === 0 || index === 1 ? 'text' : 'digit' };
            });

            this.$tableReport.tablesorter({
                headers: headers,
                sortList: [[this.mapSortColumnNameToIndex(), this.sortIndicator]],
                textExtraction: (node: any) => {
                    const $node = $(node);

                    return $node.data('islink') ? $node.find('a').html() : $node.html();
                }
            });
            this.$tableReport.off('sortEnd').on('sortEnd', () => {
                const sortedColumns = this.$tableReport.find('.sorting-asc, .sorting-desc');
                if (sortedColumns.length) {
                    const firstSortedColumn = sortedColumns.eq(0);
                    this.sortColumnName = firstSortedColumn.data('colname');
                    this.sortIndicator = firstSortedColumn.hasClass('sorting-desc') ? 1 : 0; // asc: 0, desc: 1
                }

                const dormantAccountsRow = this.$tableReport.find('.dormant-accounts-mark');
                dormantAccountsRow.length && this.$tableReport.append(dormantAccountsRow);
            });
        } else {
            sortColumns.removeClass('sorting');
        }
    }

    mapSortColumnNameToIndex() {
        const tableHeaderCells = this.$tableReport.find(`thead th[data-colname="${this.sortColumnName}"]`);
        if (tableHeaderCells.length) {
            return tableHeaderCells.eq(0).data('colindex');
        }

        // reset to default
        this.sortColumnName = 'username';
        this.sortIndicator = 0;
        return 0;
    }
}
