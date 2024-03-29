/// <reference path="components/link-builder.ts" />

class BaseReportNew {
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

    constructor(linkBuilderComponent: LinkBuilderComponent) {
        this.linkBuilderComponent = linkBuilderComponent;
    }

    init() {
        this.cacheDOMElements();
    }

    cacheDOMElements() {
        this.$filterIcon = $('.icon-filter');
        this.$isDescendingSort = $('#IsDescendingSort');
        this.$isFilterVisible = $('#IsFilterVisible');
        this.$pageTitle = $('.page-title');
        this.$reportForm = $('#report-form');
        this.$sortPropertyName = $('#SortPropertyName');
        this.$submitButton = $('.btn-submit');
        this.$tableReport = $('#tbl-report');
        this.canSort = parseInt(this.$tableReport.data('totalrecords'), 0) > 1;
    }

    registerEvents() {
        this.$filterIcon.on('click', () => this.onClickIconFilter());
    }

    initHelp = () => {
        const $loadLegendURL = $('#LoadLegendURL');
        if ($loadLegendURL.length) {
            this.reportLegendUrl = $loadLegendURL.attr('href');
            $loadLegendURL.remove();
            this.$iconInformation = $('.icon-information');
            this.$iconInformationContainer = $('.icon-information-container');
            this.loadLegend();
            this.$iconInformation.on('click', this.showLegend);
        }
    };

    loadLegend = () => {
        $.ajax({
            method: 'GET',
            url: this.reportLegendUrl,
            cache: false,
            data: {},
            success: this.renderLegend,
            complete: $.unblockUI
        });
    };

    renderLegend = (html: string) => {
        if (this.$reportLegend) {
            this.$reportLegend.remove();
        }

        $('body').append(html);

        if ($.trim(html) === '') {
            this.$iconInformationContainer.parent().remove();
        } else {
            this.$iconInformationContainer.parent().removeClass('hide');
        }

        this.$reportLegend = $('#report-legend');
    };

    showLegend = () => {
        this.$reportLegend.modal({ show: true, backdrop: false });
        const iconHelpPosition = this.$iconInformation.position();
        this.$reportLegend.css({
            bottom: 'auto',
            top: $('.entered-non-entered').length > 0 ? iconHelpPosition.top + 90 : iconHelpPosition.top + 60
        });
    };

    isDescendingSort = () => this.$dataTable.order()[0][1] !== 'asc';

    triggerRedrawTable() {
        this.doAdjustHeader();
        $('tbody tr.parent').each((_, element) => this.onClickDetailControl(element));
    }

    doAdjustHeader = () => {
        this.$dataTable.fixedHeader.adjust();

        const scrollTo = $('.fixedHeader-floating thead').length > 0 ? $(document).height() : 0;
        $(window).scrollTop(scrollTo);
    };

    onClickFooterRow = (parentRow, childRow) => {
        parentRow.hasClass('parent') ? childRow.addClass('tbl-footer') : childRow.removeClass('tbl-footer');

        if (this.isClickedCollapseExpandAll) {
            this.isClickedCollapseExpandAll = false;
            return;
        }

        setTimeout(() => {
            const scrollTo = $(document).height();
            $(window).scrollTop(scrollTo + 15);
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
        const parentRow = $(element).closest('tr');
        const childRow = $(parentRow).next();
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

    replaceHrefParam = (paramName: string, paramValue: any) =>
        this.linkBuilderComponent.updateParam(paramName, paramValue);

    initDataTable(options = {}) {
        const tableOptions = this.getTableOptions();
        $.extend(tableOptions, options);

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
        $.blockUI();
        this.isClickedCollapseExpandAll = true;
        setTimeout(() => {
            const targetParent = $(element).parent();

            if (targetParent.hasClass('parent')) {
                targetParent.removeClass('parent');
                this.triggerClickDetailWhenSwitchAll($('tbody tr.parent'));
            } else {
                targetParent.addClass('parent');
                this.triggerClickDetailWhenSwitchAll($('tbody tr').not('.parent'));
            }

            $.unblockUI();
        }, 100);
    };

    private triggerClickDetailWhenSwitchAll = (controlArray) => {
        controlArray.each((_, detailButton) => $(detailButton).find('.control').click());
        this.$dataTable.fixedHeader.adjust();
    };

    getTableOptions = () => {
        const self = this;

        return {
            language: {
                processing: '',
                emptyTable: $('#NoInformationIsAvailable').val()
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
                                const header = $(api.column(cell.column).header());
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

    renderDetail = (rowIdx: any, data: any) => (data ? $(`<ul data-dtr-index="${rowIdx}"/>`).append(data) : false);

    drawCallback = () => {
        $('tbody').append($('tr.tbl-footer'));
        this.$tableReport.removeClass('hidden');
    };

    registerDrilldownableEvents() {
        const handleDrilldownable = ($element) => {
            const $custLevelId = $('#CustLevelId');
            let custLevelIdValue = $element.data('custlevelid') || +$custLevelId.val() - 1;

            $custLevelId.val(custLevelIdValue);
            $('#CustId').val($element.data('custid'));
            $('#CustName').val($element.data('custname'));

            this.$reportForm.submit();
        };

        this.$pageTitle.find('.breadcrumb-link').click((e) => handleDrilldownable($(e.target)));

        this.$tableReport.find('.downline-link').click((e) => handleDrilldownable($(e.target)));
    }
}
