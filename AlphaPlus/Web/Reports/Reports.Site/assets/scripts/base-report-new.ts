class BaseReportNew {
    protected sortColumnName: string;
    protected sortColumnIndex = 0;
    protected sortIndicator = 0; // asc: 0, desc: 1

    protected initReportLegend() {
        const $loadLegendURL = $('#LoadLegendURL');
        if ($loadLegendURL.length) {
            const reportLegendUrl = $loadLegendURL.attr('href');
            $loadLegendURL.remove();

            const $iconInformation = $('#icon-information').click(() => this.showLegend($iconInformation));
            $.ajax({
                method: 'GET',
                url: reportLegendUrl,
                cache: false,
                success: (html) => this.renderLegend($iconInformation, html),
                complete: $.unblockUI
            });
        }
    }

    private showLegend($iconInformation: JQuery) {
        const $reportLegend = $('#report-legend').modal({ show: true, backdrop: false });
        const iconHelpPosition = $iconInformation.position();
        $reportLegend.css({
            top: $('.entered-non-entered').length > 0 ? iconHelpPosition.top + 90 : iconHelpPosition.top + 60
        });
    }

    private renderLegend($iconInformation: JQuery, html: string) {
        $('body').append(html);

        if ($.trim(html) === '') {
            $iconInformation.parent().remove();
        } else {
            $iconInformation.parent().removeClass('hide');
        }
    }

    protected initShowHideFilter(showHideFilterCallback?: Function) {
        $('.icon-filter')
            .click(() => {
                const isHideFilter = this.handleShowHideFilter();
                showHideFilterCallback && showHideFilterCallback(isHideFilter);
            })
            .tooltip({ placement: 'left' });
    }

    private handleShowHideFilter() {
        const $pageTitle = $('.page-title');
        const $reportForm = $('#report-form');
        const isHideFilter = $reportForm.hasClass('hide');
        if (isHideFilter) {
            $pageTitle.removeClass('hide-filter');
            $reportForm.removeClass('hide');
        } else {
            $pageTitle.addClass('hide-filter');
            $reportForm.addClass('hide');
        }

        return !isHideFilter;
    }

    protected tableSorter($tblReport: JQuery, canSort: boolean = undefined, sortEndCallback: Function = undefined) {
        canSort = canSort || $tblReport.data('cansort').toLowerCase() === 'true';
        const sortColumns = $tblReport.find('.sorting');

        if (canSort) {
            const headers = {};
            sortColumns.each(function () {
                // sorttype is text or digit
                const $this = $(this);
                headers[$this.data('colindex')] = { sorter: $this.data('sorttype') || 'digit' };
            });
            const sortList =
                typeof this.sortColumnIndex === 'undefined' ? [] : [[this.mapSortColumnIndex(), this.sortIndicator]];

            $tblReport
                .tablesorter({
                    headers: headers,
                    sortList: sortList,
                    textExtraction: this.textExtraction
                })
                .bind('sortEnd', () => {
                    const sortedColumns = $tblReport.find('.sorting-asc, .sorting-desc');
                    if (sortedColumns.length) {
                        const firstSortedColumn = sortedColumns.eq(0);
                        this.sortColumnName = firstSortedColumn.data('colname');
                        this.sortColumnIndex = firstSortedColumn.data('colindex');
                        this.sortIndicator = firstSortedColumn.hasClass('sorting-desc') ? 1 : 0; // asc: 0, desc: 1
                    }

                    sortEndCallback && sortEndCallback();
                });
        } else {
            sortColumns.removeClass('sorting');
        }
    }

    protected mapSortColumnIndex() {
        return this.sortColumnIndex;
    }

    private textExtraction = (node: any) => {
        const $node = $(node);
        const sortedValue = $node.data('sortedvalue');
        if (sortedValue) {
            return String(sortedValue);
        }

        return $node.html();
    };

    protected stickyTableHeaders($tblReport: JQuery) {
        $tblReport.stickyTableHeaders('destroy');
        $tblReport.stickyTableHeaders();
    }
}
