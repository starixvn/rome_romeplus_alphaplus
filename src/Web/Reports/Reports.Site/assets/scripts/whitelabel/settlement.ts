declare var whiteLabels: any;

class SettlementView extends BaseReportNew {
    private monthRangePicker: MonthRangePickerComponent;
    private whiteLabelDropdown: MultipleDropdownComponent;
    private usernameSuggestion: UsernameSuggestionComponent;
    private visibleColumns: SettlementColumns;
    private $tblReportContainer: JQuery;

    constructor(
        monthRangePicker: MonthRangePickerComponent,
        whiteLabelDropdown: MultipleDropdownComponent,
        usernameSuggestion: UsernameSuggestionComponent,
        visibleColumns: SettlementColumns
    ) {
        super();
        this.monthRangePicker = monthRangePicker;
        this.whiteLabelDropdown = whiteLabelDropdown;
        this.usernameSuggestion = usernameSuggestion;
        this.visibleColumns = visibleColumns;
        // default no sort
        this.sortColumnIndex = undefined;
    }

    init() {
        this.$tblReportContainer = $('#tbl-report-container');
        this.initComponents();
        this.bindEventsAfterRenderReportData();
        this.$tblReportContainer.removeAttr('style');
    }

    private initComponents() {
        super.initShowHideFilter((isHideFilter) => $('#settlement-note').toggle(!isHideFilter));
        this.monthRangePicker.render({
            afterChangeMonth: () => this.getReportData()
        });

        if (Array.isArray(whiteLabels)) {
            let maxWhiteLabelLength = 0;
            const listData = whiteLabels.map(function (wl) {
                if (wl.Name.length > maxWhiteLabelLength) {
                    maxWhiteLabelLength = wl.Name.length;
                }

                return {
                    text: wl.Name,
                    value: wl.Id,
                    checked: true
                };
            });
            maxWhiteLabelLength *= 11;
            this.whiteLabelDropdown.render({
                selectorId: 'dropdown-whitelabel',
                listData: listData,
                showFilter: true,
                onLeave: () => this.getReportData(),
                minWidth: maxWhiteLabelLength > 220 ? maxWhiteLabelLength : 220
            });
        }

        this.usernameSuggestion.init({ onBlur: () => this.getReportData() });
        this.visibleColumns.init();

        const $iconExcel = $('#icon-excel').tooltip();
        const excelUrl = $iconExcel.attr('href');
        $iconExcel
            .click(() => {
                $iconExcel.attr('href', `${excelUrl}?${$.param(this.getFormData())}`);
                setTimeout(function () {
                    $iconExcel.attr('href', 'javascript:;');
                }, 1000);
            })
            .attr('href', 'javascript:;');
    }

    private getReportData() {
        $.blockUI();
        const queryString = $.param(this.getFormData());
        $.get(`${$('#hidden-getDataUrl').attr('href')}?${queryString}`, (html) => {
            this.$tblReportContainer.html(html);
            this.bindEventsAfterRenderReportData();
            alphaReports.updateQueryString(queryString);
        })
            .fail(alphaReports.ajaxFail)
            .always($.unblockUI);
    }

    private getFormData() {
        let formData: any = this.monthRangePicker.getDateRangeValues();

        const whiteLabels = this.whiteLabelDropdown.getCheckedValues();
        if (whiteLabels) {
            formData.whiteLabels = whiteLabels;
        }

        const username = this.usernameSuggestion.getUsername();
        if (username) {
            formData.username = username;
        }

        return formData;
    }

    private bindEventsAfterRenderReportData() {
        const $tblReport = $('#tbl-report');
        this.tableSorter($tblReport);
        this.visibleColumns.showHideColumns();
        this.stickyTableHeaders($tblReport);
    }
}

$(function () {
    new SettlementView(
        new MonthRangePickerComponent(),
        new MultipleDropdownComponent(),
        new UsernameSuggestionComponent(),
        new SettlementColumns(new VisibleColumnComponent())
    ).init();
});
