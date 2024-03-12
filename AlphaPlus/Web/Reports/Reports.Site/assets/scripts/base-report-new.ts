/// <reference path="components/link-builder.ts" />

class BaseReportNew {
    $: JQueryStatic;

    $filterIcon: JQuery;
    $iconInformation: JQuery;
    $iconInformationContainer: JQuery;
    $isDescendingSort: JQuery;
    $isFilterVisible: JQuery;
    reportLegendUrl: string;
    $pageTitle: JQuery;
    $reportForm: JQuery;
    $reportLegend: JQuery;
    $sortPropertyName: JQuery;
    $submitButton: JQuery;
    $dataTable: JQuery;
    $tableReport: JQuery;

    linkBuilderComponent: LinkBuilderComponent;
    canSort: boolean;
    currentHideState: boolean;
    isClickedCollapseExpandAll = false;

    constructor($: JQueryStatic, linkBuilderComponent: LinkBuilderComponent) {
        this.$ = $;
        this.linkBuilderComponent = linkBuilderComponent;
    }

    init() {
        this.cacheDOMElements();
    }

    cacheDOMElements() {
        this.$filterIcon = this.$('.icon-filter');
        this.$isDescendingSort = this.$('#IsDescendingSort');
        this.$isFilterVisible = this.$('#IsFilterVisible');
        this.$pageTitle = this.$('.page-title');
        this.$reportForm = this.$('#report-form');
        this.$sortPropertyName = this.$('#SortPropertyName');
        this.$submitButton = this.$('.btn-submit');
        this.$tableReport = this.$('#tbl-report');
        this.canSort = parseInt(this.$tableReport.data('totalrecords'), 0) > 1;
    }

    registerEvents() {
        this.$filterIcon.on('click', () => this.onClickIconFilter());
    }

    initHelp = () => {
        const $loadLegendURL = this.$('#LoadLegendURL');
        if ($loadLegendURL.length) {
            this.reportLegendUrl = $loadLegendURL.attr('href');
            $loadLegendURL.remove();
            this.$iconInformation = this.$('.icon-information');
            this.$iconInformationContainer = this.$('.icon-information-container');
            this.loadLegend();
            this.$iconInformation.on('click', this.showLegend);
        }
    };

    loadLegend = () => {
        this.$.ajax({
            method: 'GET',
            url: this.reportLegendUrl,
            cache: false,
            data: {},
            success: this.renderLegend,
            complete: this.$.unblockUI
        });
    };

    renderLegend = (html: string) => {
        if (this.$reportLegend) {
            this.$reportLegend.remove();
        }

        this.$('body').append(html);

        if (this.$.trim(html) === '') {
            this.$iconInformationContainer.remove();
        } else {
            this.$iconInformationContainer.removeClass('hide');
        }

        this.$reportLegend = this.$('#report-legend');
    };

    showLegend = () => {
        this.$reportLegend.modal({ show: true, backdrop: false });
        const iconHelpPosition = this.$iconInformation.position();
        this.$reportLegend.css({
            bottom: 'auto',
            top: this.$('.entered-non-entered').length > 0 ? iconHelpPosition.top + 90 : iconHelpPosition.top + 60
        });
    };

    isDescendingSort = () => this.$dataTable.order()[0][1] !== 'asc';

    triggerRedrawTable() {
        this.doAdjustHeader();
        this.$('tbody tr.parent').each((index, element) => this.onClickDetailControl(element));
    }

    doAdjustHeader = () => {
        this.$dataTable.fixedHeader.adjust();

        const scrollTo = this.$('.fixedHeader-floating thead').length > 0 ? this.$(document).height() : 0;
        this.$(window).scrollTop(scrollTo);
    };

    onClickFooterRow = (parentRow, childRow) => {
        parentRow.hasClass('parent') ? childRow.addClass('tbl-footer') : childRow.removeClass('tbl-footer');

        if (this.isClickedCollapseExpandAll) {
            this.isClickedCollapseExpandAll = false;
            return;
        }

        setTimeout(() => {
            const scrollTo = this.$(document).height();
            this.$(window).scrollTop(scrollTo + 15);
        }, 100);
    };

    onClickBodyRow = (parentRow, childRow) => {
        if (childRow.hasClass('child')) {
            parentRow.hasClass('even')
                ? childRow.removeClass('odd').addClass('even')
                : childRow.removeClass('even').addClass('odd');
        }
    };

    onClickDetailControl(element) {
        const parentRow = this.$(element).closest('tr');
        const childRow = this.$(parentRow).next();
        parentRow.hasClass('tbl-footer')
            ? this.onClickFooterRow(parentRow, childRow)
            : this.onClickBodyRow(parentRow, childRow);
    }

    onClickIconFilter() {
        this.currentHideState = this.$reportForm.hasClass('hide');
        this.currentHideState ? this.showFilter() : this.hideFilter();
        this.doAdjustHeader();
    }

    showFilter = () => {
        this.$pageTitle.removeClass('hide-filter');
        this.$reportForm.removeClass('hide');
    };

    hideFilter = () => {
        this.$pageTitle.addClass('hide-filter');
        this.$reportForm.addClass('hide');
    };

    replaceHrefParam = (paramName: string, paramValue: any) => {
        this.linkBuilderComponent.updateParam(paramName, paramValue);
    };

    initDataTable(options = {}) {
        const tableOptions = this.getTableOptions();
        this.$.extend(tableOptions, options);

        this.$dataTable = this.$tableReport.DataTable(tableOptions);

        this.$tableReport
            .find('tbody')
            .on('click', 'td.control', (event) => this.onClickDetailControl(event.currentTarget));
        this.$tableReport
            .find('thead')
            .on('click', 'th.control', (event) => this.collapseExpandAll(event.currentTarget));
        this.$dataTable.on('draw.dt', () => this.triggerRedrawTable());
    }

    private collapseExpandAll = (element) => {
        this.$.blockUI();
        this.isClickedCollapseExpandAll = true;
        setTimeout(() => {
            const targetParent = this.$(element).parent();

            if (targetParent.hasClass('parent')) {
                targetParent.removeClass('parent');
                this.triggerClickDetailWhenSwitchAll(this.$('tbody tr.parent'));
            } else {
                targetParent.addClass('parent');
                this.triggerClickDetailWhenSwitchAll(this.$('tbody tr').not('.parent'));
            }

            this.$.unblockUI();
        }, 100);
    };

    private triggerClickDetailWhenSwitchAll = (controlArray) => {
        controlArray.each((index, detailButton) => this.$(detailButton).find('.control').click());
        this.$dataTable.fixedHeader.adjust();
    };

    getTableOptions = () => {
        const self = this;

        return {
            language: {
                processing: '',
                emptyTable: this.$('#NoInformationIsAvailable').val()
            },
            responsive: {
                details: {
                    type: 'column',
                    target: -1,
                    renderer: (api, rowIdx) => {
                        const data = api
                            .cells(rowIdx, ':hidden')
                            .eq(0)
                            .map(function (cell) {
                                const header = self.$(api.column(cell.column).header());
                                const idx = api.cell(cell).index();

                                if (header.hasClass('control') || header.hasClass('never')) {
                                    return '';
                                }

                                const dtPrivate = api.settings()[0];
                                const cellData = dtPrivate.oApi._fnGetCellData(
                                    dtPrivate,
                                    idx.row,
                                    idx.column,
                                    'display'
                                );

                                return self.renderData(idx.column, header.text(), cellData);
                            })
                            .toArray()
                            .join('');

                        return self.renderDetail(rowIdx, data);
                    }
                }
            },
            columnDefs: [
                {
                    className: 'control',
                    orderable: false,
                    targets: -1
                }
            ],
            ordering: this.canSort,
            paging: false,
            searching: false,
            bInfo: false,
            fixedHeader: true,
            processing: true,
            drawCallback: this.drawCallback
        };
    };

    renderData = (idxColumn: any, headerText: string, cellData: any) =>
        `<li data-dtr-index="${idxColumn}">` +
        `<span class="dtr-title">${headerText}</span> ` +
        `<span class="dtr-data">${cellData}</span></li>`;

    renderDetail = (rowIdx: any, data: any) => (data ? this.$(`<ul data-dtr-index="${rowIdx}"/>`).append(data) : false);

    drawCallback = () => {
        this.$('tbody').append(this.$('tr.tbl-footer'));
        this.$tableReport.removeClass('hidden');
    };
}
