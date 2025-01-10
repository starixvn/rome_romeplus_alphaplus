class OutstandingView extends BaseReport {
    productComponent: ProductMultiSelectionComponent;
    levelTypeComponent: SingleDropdownComponent;
    displayOutstandingLevels: any;
    $dataTable: JQuery;
    isClickedCollapseExpandAll = false;

    constructor(productComponent: ProductMultiSelectionComponent, levelTypeComponent: SingleDropdownComponent) {
        super(false);
        this.productComponent = productComponent;
        this.levelTypeComponent = levelTypeComponent;
    }

    init() {
        this.productComponent.render();
        this.levelTypeComponent.render({ dropdownId: 'dropdown-level-type' });

        this.displayOutstandingLevels = JSON.parse($('#DisplayOutstandingLevelsJson').val());
        this.initDataTable();

        super.registerDrilldownableEvents();
    }

    initDataTable() {
        const canSort = this.$tableReport.data('totalrecords') > 1;
        const sortedcolumn = this.$tableReport.data('sortedcolumn').toLowerCase();
        const isDescendingSort = this.$tableReport.data('descendingsort') === 1;

        let colunmSortIndex = 0;
        this.displayOutstandingLevels.forEach((element, index) => {
            if (sortedcolumn === `${element.DisplayName}Outstanding`.toLowerCase()) {
                colunmSortIndex = index + 1;
                return;
            }
        });

        const tableOptions = this.buildTableOptions();
        $.extend(tableOptions, {
            ordering: canSort,
            order: [[colunmSortIndex, isDescendingSort ? 'desc' : 'asc']]
        });

        this.$dataTable = this.$tableReport.DataTable(tableOptions);

        this.$tableReport
            .find('tbody')
            .on('click', 'td.control', (event) => this.onClickDetailControl(event.currentTarget));
        this.$tableReport
            .find('thead')
            .on('click', 'th.control', (event) => this.collapseExpandAll(event.currentTarget));
        this.$dataTable.on('draw.dt', () => this.triggerRedrawTable());
    }

    buildTableOptions() {
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
            paging: false,
            searching: false,
            bInfo: false,
            fixedHeader: true,
            processing: true,
            drawCallback: this.drawCallback
        };
    }

    renderData = (idxColumn: any, headerText: string, cellData: any) =>
        `<li data-dtr-index="${idxColumn}"><span class="dtr-title">${headerText}</span><span class="dtr-data">${cellData}</span></li>`;

    renderDetail = (rowIdx: any, data: any) => (data ? $(`<ul data-dtr-index="${rowIdx}"/>`).append(data) : false);

    drawCallback = () => {
        $('tbody').append($('tr.tbl-footer'));
        this.$tableReport.removeClass('hide');
    };

    onClickDetailControl(element) {
        const parentRow = $(element).closest('tr');
        const childRow = $(parentRow).next();
        parentRow.hasClass('tbl-footer')
            ? this.onClickFooterRow(parentRow, childRow)
            : this.onClickBodyRow(parentRow, childRow);
    }

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

    triggerRedrawTable = () => {
        this.doAdjustHeader();
        $('tbody tr.parent').each((_, element) => this.onClickDetailControl(element));
    };

    doAdjustHeader = () => {
        this.$dataTable.fixedHeader.adjust();

        const scrollTo = $('.fixedHeader-floating thead').length > 0 ? $(document).height() : 0;
        $(window).scrollTop(scrollTo);
    };

    getColumnSortName = () => {
        const order = this.$dataTable.order();
        const length = this.displayOutstandingLevels.length;

        for (let index = 0; index < length; index++) {
            if (order[0][0] - 1 === index) {
                return `${this.displayOutstandingLevels[index].DisplayName}Outstanding`;
            }
        }

        return 'CustName';
    };

    isDescendingSort = () => this.$dataTable.order()[0][1] !== 'asc';

    onSubmitReport(event: any) {
        if (!this.productComponent.validateProductIds()) {
            event.preventDefault();
            return;
        }

        super.onSubmitReport(event, {
            sortPropertyName: this.getColumnSortName(),
            descendingSort: this.isDescendingSort() ? 1 : 0
        });
    }
}

$(function () {
    new OutstandingView(new ProductMultiSelectionComponent(), new SingleDropdownComponent()).init();
});
