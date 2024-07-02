class WinLossDetailTable {
    $tableReport: JQuery;
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

    constructor() {
        this.$tableReport = $('#tbl-report');
    }

    renderWinLossDetail(reportData, custLevelId: number, columns) {
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

        this.$tableReport.html(html.join('')).removeClass('hide');
        this.$tableReport.prev('.breadcrumb-table').removeClass('hide');
        this.showHideColumns(columns);
        this.initTableSorter(reportData.MoreThanOne);
        this.stickyTableHeaders();
    }

    registerTableEvents(getWinLossDetailHandler: Function, drilldownBetListHandler: Function) {
        this.$tableReport.find('[data-toggle="tooltip"]').tooltip();
        this.$tableReport
            .find('.downline-link')
            .off()
            .click((e) => {
                const $this = $(e.target);
                const custId = $this.data('custid');
                const custName = $this.data('custname');
                const custLevelId = $this.data('custlevelid');
                if (custLevelId === this.MasterLevel || custLevelId === this.AgentLevel) {
                    e.preventDefault();
                    getWinLossDetailHandler({ custId, custName, custLevelId });
                } else if (custLevelId === this.MemberLevel) {
                    drilldownBetListHandler($this, { custId, custName, custLevelId });
                }
            });
    }

    private buildHeader(html, custLevelId: number) {
        let index = 0;
        html.push(
            '<thead><tr>',
            `<th rowspan="2" class="sorting" data-colindex="${index++}" data-colname="username">${
                viewResources.Username
            }</th>`,
            `<th rowspan="2" class="sorting" data-colindex="${index++}" data-colname="fullname">${
                viewResources.FirstLastName
            }</th>`,
            `<th rowspan="2" class="sorting" data-colindex="${index++}" data-colname="betcount">${
                viewResources.BetCount
            }</th>`,
            `<th rowspan="2" class="sorting" data-colindex="${index++}" data-colname="turnover">${
                viewResources.Turnover
            }</th>`,
            `<th rowspan="2" class="sorting" data-colindex="${index++}" data-colname="netturnover">${
                viewResources.NetTurnover
            }</th>`,
            `<th rowspan="2" class="sorting" data-colindex="${index++}" data-colname="grosscomm">${
                viewResources.GrossComm
            }</th>`,
            `<th colspan="3" id="groupColumnMember">${viewResources.Member}</th>`,
            `<th colspan="3" id="groupColumnAgent">${viewResources.Agent}</th>`
        );
        if (custLevelId >= this.MasterLevel) {
            html.push(`<th colspan="3" id="groupColumnMaster">${viewResources.Master}</th>`);
        }
        if (custLevelId === this.SuperLevel) {
            html.push(`<th colspan="3" id="groupColumnSuper">${viewResources.Super}</th>`);
        }
        html.push(
            `<th rowspan="2" class="sorting" data-colindex="${
                this.totalColumn.get(custLevelId) - 1
            }" data-colname="company">${viewResources.Company}</th>`
        );
        html.push('</tr><tr class="thead-row2">');
        html.push(
            `<th class="sorting" data-colindex="${index++}" data-colname="pwinloss">${viewResources.WinLoss}</th>`,
            `<th class="sorting" data-colindex="${index++}" data-colname="pcomm">${viewResources.Comm}</th>`,
            `<th class="sorting" data-colindex="${index++}" data-colname="ptotal">${viewResources.Total}</th>`,
            `<th class="sorting" data-colindex="${index++}" data-colname="awinloss">${viewResources.WinLoss}</th>`,
            `<th class="sorting" data-colindex="${index++}" data-colname="acomm">${viewResources.Comm}</th>`,
            `<th class="sorting" data-colindex="${index++}" data-colname="atotal">${viewResources.Total}</th>`
        );
        if (custLevelId >= this.MasterLevel) {
            html.push(
                `<th class="sorting" data-colindex="${index++}" data-colname="mwinloss">${viewResources.WinLoss}</th>`,
                `<th class="sorting" data-colindex="${index++}" data-colname="mcomm">${viewResources.Comm}</th>`,
                `<th class="sorting" data-colindex="${index++}" data-colname="mtotal">${viewResources.Total}</th>`
            );
        }
        if (custLevelId === this.SuperLevel) {
            html.push(
                `<th class="sorting" data-colindex="${index++}" data-colname="swinloss">${viewResources.WinLoss}</th>`,
                `<th class="sorting" data-colindex="${index++}" data-colname="scomm">${viewResources.Comm}</th>`,
                `<th class="sorting" data-colindex="${index++}" data-colname="stotal">${viewResources.Total}</th>`
            );
        }
        html.push('</tr></thead>');
    }

    private buildBody(html, custLevelId: number, winloss, isTotalRow: boolean) {
        const isDormantAccounts = typeof winloss.CustId === 'undefined';
        html.push(`<tr class="${isTotalRow ? 'tbl-footer' : isDormantAccounts ? 'dormant-accounts-mark' : ''}">`);

        if (isTotalRow) {
            html.push(`<td class="text-left">${viewResources.Total}</td>`, '<td data-colname="fullname"></td>');
        } else {
            html.push('<td class="text-left col-username">');
            if (isDormantAccounts) {
                html.push(
                    `<span class="pull-left">${viewResources.DormantAccounts}</span>`,
                    `<span class="icon icon-information-outline icon-dormant-accounts" data-toggle="tooltip" data-placement="right" title="${viewResources.SummaryOfDormantAccounts}"></span>`
                );
            } else {
                html.push(
                    `<a href="javascript:;" data-islink="true" data-custid="${winloss.CustId}" data-custname="${
                        winloss.CustName
                    }" data-custlevelid="${custLevelId - 1}" class="downline-link">${winloss.CustName}</a>`
                );
            }

            const fullName = winloss.FullName || '';
            html.push(
                '</td>',
                '<td class="text-left" data-colname="fullname">',
                `<div class="fullname" data-toggle="tooltip" data-placement="bottom" title="${fullName}">`,
                `${fullName}</div></td>`
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

    private buildBodyNoData(html, custLevelId: number) {
        html.push(
            `<tbody><tr><td colspan="${this.totalColumn.get(custLevelId)}" align="center">${
                viewResources.NoInformationIsAvailable
            }</td></tr></tbody>`
        );
    }

    private stickyTableHeaders() {
        this.$tableReport.stickyTableHeaders('destroy');
        this.$tableReport.stickyTableHeaders();
    }

    private initTableSorter(canSort) {
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

    private mapSortColumnNameToIndex() {
        const tableHeaderCells = this.$tableReport.find(`thead th[data-colname="${this.sortColumnName}"]`);
        if (tableHeaderCells.length) {
            return tableHeaderCells.eq(0).data('colindex');
        }

        // reset to default
        this.sortColumnName = 'username';
        this.sortIndicator = 0;
        return 0;
    }

    copyHtmlTable() {
        const html = ['<table>'];
        this.buildHtmlHeader(this.$tableReport, html);
        this.buildHtmlBody(this.$tableReport, html);
        this.buildHtmlFooter(this.$tableReport, html);
        html.push('</table>');

        return html.join('');
    }

    private buildHtmlHeader($tableReport: JQuery, html: string[]) {
        html.push('<thead><tr>');
        $tableReport.find('thead tr:first th:not(.hide)').each(function () {
            const $this = $(this);
            html.push('<th');

            const rowspan = $this.attr('rowspan');
            if (rowspan) {
                html.push(` rowspan="${rowspan}"`);
            }

            const colspan = $this.attr('colspan');
            if (colspan) {
                html.push(` colspan="${colspan}"`);
            }

            html.push('>', this.innerText, '</th>');
        });

        html.push('</tr><tr>');
        $tableReport.find('thead tr:last th:not(.hide)').each(function () {
            html.push('<th>', this.innerText, '</th>');
        });

        html.push('</tr></thead>');
    }

    private buildHtmlBody($tableReport: JQuery, html: string[]) {
        const getColumnText = function (td) {
            let title;
            const $td = $(td);
            if ($td.data('colname') === 'fullname') {
                title = $td.find('div:first').data('original-title');
            }

            return title || td.innerText;
        };
        html.push('<tbody>');

        $tableReport.find('tbody tr').each(function () {
            html.push('<tr>');
            $(this)
                .find('td:not(.hide)')
                .each(function () {
                    html.push('<td>', getColumnText(this), '</td>');
                });
            html.push('</tr>');
        });

        html.push('</tbody>');
    }

    private buildHtmlFooter($tableReport: JQuery, html: string[]) {
        const footerRows = $tableReport.find('tfoot tr td:not(.hide)');

        if (footerRows.length) {
            html.push('<tfoot><tr>');
            footerRows.each(function () {
                html.push('<td>', this.innerText, '</td>');
            });

            html.push('</tr></tfoot>');
        }
    }

    showHideColumns(visibleColumn) {
        this.showHideSpecificColumn(visibleColumn, 'fullname');
        this.showHideSpecificColumn(visibleColumn, 'betcount');
        this.showHideSpecificColumn(visibleColumn, 'turnover');
        this.showHideSpecificColumn(visibleColumn, 'netturnover');
        this.showHideSpecificColumn(visibleColumn, 'grosscomm');
        this.showHideGroupColumn(visibleColumn, 'p', 'groupColumnMember');
        this.showHideGroupColumn(visibleColumn, 'a', 'groupColumnAgent');
        this.showHideGroupColumn(visibleColumn, 'm', 'groupColumnMaster');
        this.showHideGroupColumn(visibleColumn, 's', 'groupColumnSuper');
        this.showHideSpecificColumn(visibleColumn, 'company');
    }

    private showHideGroupColumn(visibleColumn, prefix: string, groupColumnId: string) {
        const groupColumn = this.$tableReport.find(`#${groupColumnId}`);
        if (groupColumn.length) {
            let colspan = 0;
            if (this.showHideSpecificColumn(visibleColumn, `${prefix}winloss`)) {
                colspan++;
            }
            if (this.showHideSpecificColumn(visibleColumn, `${prefix}comm`)) {
                colspan++;
            }
            if (this.showHideSpecificColumn(visibleColumn, `${prefix}total`)) {
                colspan++;
            }

            colspan ? groupColumn.attr('colspan', colspan).removeClass('hide') : groupColumn.addClass('hide');
        }
    }

    private showHideSpecificColumn(visibleColumn, key: string) {
        const matchedColumns = this.$tableReport.find(`[data-colname=${key}]`);
        const visible = visibleColumn[key];
        visible ? matchedColumns.removeClass('hide') : matchedColumns.addClass('hide');

        return visible;
    }
}
