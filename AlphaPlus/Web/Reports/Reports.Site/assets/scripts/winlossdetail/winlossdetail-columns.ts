declare var columnList: any;

class WinLossDetailColumns {
    private visibleColumnComponent: VisibleColumnComponent;

    constructor(visibleColumnComponent: VisibleColumnComponent) {
        this.visibleColumnComponent = visibleColumnComponent;
    }

    init(onDropdownHidden: Function) {
        this.visibleColumnComponent.render($('#VisibleColumns').val(), columnList, (columns) => {
            this.showHideColumns($('#tbl-report'), columns);
            onDropdownHidden();
        });
    }

    showHideColumns($tblReport: JQuery, visibleColumn = undefined) {
        visibleColumn = this.visibleColumnComponent.getColumns();
        this.showHideSpecificColumn($tblReport, visibleColumn, 'fullname');
        this.showHideSpecificColumn($tblReport, visibleColumn, 'betcount');
        this.showHideSpecificColumn($tblReport, visibleColumn, 'turnover');
        this.showHideSpecificColumn($tblReport, visibleColumn, 'netturnover');
        this.showHideSpecificColumn($tblReport, visibleColumn, 'grosscomm');
        this.showHideGroupColumn($tblReport, visibleColumn, 'p', 'groupColumnMember');
        this.showHideGroupColumn($tblReport, visibleColumn, 'a', 'groupColumnAgent');
        this.showHideGroupColumn($tblReport, visibleColumn, 'm', 'groupColumnMaster');
        this.showHideGroupColumn($tblReport, visibleColumn, 's', 'groupColumnSuper');
        this.showHideSpecificColumn($tblReport, visibleColumn, 'company');
        this.showHideSpecificColumn($tblReport, visibleColumn, 'reward');
    }

    private showHideGroupColumn($tblReport: JQuery, visibleColumn, prefix: string, groupColumnId: string) {
        const groupColumn = $tblReport.find(`#${groupColumnId}`);
        if (groupColumn.length) {
            let colspan = 0;
            if (this.showHideSpecificColumn($tblReport, visibleColumn, `${prefix}winloss`)) {
                colspan++;
            }
            if (this.showHideSpecificColumn($tblReport, visibleColumn, `${prefix}comm`)) {
                colspan++;
            }
            if (this.showHideSpecificColumn($tblReport, visibleColumn, `${prefix}total`)) {
                colspan++;
            }

            colspan ? groupColumn.attr('colspan', colspan).removeClass('hide') : groupColumn.addClass('hide');
        }
    }

    private showHideSpecificColumn($tblReport: JQuery, visibleColumn, key: string) {
        const matchedColumns = $tblReport.find(`[data-colname=${key}]`);
        const visible = visibleColumn[key];
        visible ? matchedColumns.removeClass('hide') : matchedColumns.addClass('hide');

        return visible;
    }

    isShowFullName() {
        return this.visibleColumnComponent.getShowColumns().includes('fullname');
    }

    getShowColumns() {
        return this.visibleColumnComponent.getShowColumns();
    }
}
