class WinLossDetailSort {
    private sortColumnName = 'username';
    private sortIndicator = 0; // asc: 0, desc: 1

    init($tblReport: JQuery) {
        const canSort = $tblReport.data('cansort').toLowerCase() === 'true';
        const sortColumns = $tblReport.find('.sorting');

        if (canSort) {
            const headers = {};
            sortColumns.each(function (index) {
                headers[index] = { sorter: index === 0 || index === 1 ? 'text' : 'digit' };
            });

            $tblReport.tablesorter({
                headers: headers,
                sortList: [[this.mapSortColumnNameToIndex($tblReport), this.sortIndicator]],
                textExtraction: (node: any) => {
                    const $node = $(node);

                    return $node.data('islink') ? $node.find('a').html() : $node.html();
                }
            });
            $tblReport.off('sortEnd').on('sortEnd', () => {
                const sortedColumns = $tblReport.find('.sorting-asc, .sorting-desc');
                if (sortedColumns.length) {
                    const firstSortedColumn = sortedColumns.eq(0);
                    this.sortColumnName = firstSortedColumn.data('colname');
                    this.sortIndicator = firstSortedColumn.hasClass('sorting-desc') ? 1 : 0; // asc: 0, desc: 1
                }

                const dormantAccountsRow = $tblReport.find('.dormant-accounts-mark');
                dormantAccountsRow.length && $tblReport.append(dormantAccountsRow);
            });
        } else {
            sortColumns.removeClass('sorting');
        }
    }

    private mapSortColumnNameToIndex($tblReport: JQuery) {
        const tableHeaderCells = $tblReport.find(`thead th[data-colname="${this.sortColumnName}"]`);
        if (tableHeaderCells.length) {
            return tableHeaderCells.eq(0).data('colindex');
        }

        // reset to default
        this.sortColumnName = 'username';
        this.sortIndicator = 0;
        return 0;
    }
}
