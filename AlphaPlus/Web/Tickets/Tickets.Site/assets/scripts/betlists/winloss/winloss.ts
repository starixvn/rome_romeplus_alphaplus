interface Window {
    productList: any;
    productDetailList: any;
}

class WinLossBetListView {
    private dateRangeComponent: DateRangePickerJQueryComponent;
    private productComponent: ProductDropdownComponent;
    private weekComponent: WeekComponent;
    private breadcrumbLevelComponent: BreadcrumbLevelComponent;
    private productList: any;
    private productDetailList: any;
    private globalFormData: any;

    private betListWeekSelection: JQuery;
    private tableBetList: JQuery;

    constructor(
        dateRangeComponent: DateRangePickerJQueryComponent,
        productComponent: ProductDropdownComponent,
        weekComponent: WeekComponent,
        breadcrumbLevelComponent: BreadcrumbLevelComponent,
        productList: any,
        productDetailList: any
    ) {
        this.dateRangeComponent = dateRangeComponent;
        this.productComponent = productComponent;
        this.weekComponent = weekComponent;
        this.breadcrumbLevelComponent = breadcrumbLevelComponent;
        this.productList = productList;
        this.productDetailList = productDetailList;
    }

    init() {
        this.cacheDOMElements();
        this.bindShowHideFilterEvent();
        this.bindExportExcelEvent();
        this.renderComponents();

        // make fadeOut effect shorter
        //$.blockUI.defaults.fadeOut = 0;
    }

    private cacheDOMElements() {
        this.globalFormData = {
            fromDate: $('#from-date').val(),
            toDate: $('#to-date').val(),
            productIds: $('#product-ids').val(),
            resourceIds: $('#resource-ids').val(),
            custId: $('#cust-id').val(),
            custName: $('#cust-name').val()
        };

        this.betListWeekSelection = $('#betlist-week-selection');
        this.tableBetList = this.betListWeekSelection.find('#tbl-betlist');
    }

    private bindShowHideFilterEvent() {
        const hideFilterText = $('#hidden-hide-filter-text').val();
        const showFilterText = $('#hidden-show-filter-text').val();
        const formFilter = $('#form-filter');
        const iconFilter = $('#icon-filter').click(function () {
            const isHideFilter = iconFilter.hasClass('icon-arrow-up-drop-circle');
            if (isHideFilter) {
                iconFilter
                    .removeClass('icon-arrow-up-drop-circle')
                    .addClass('icon-arrow-down-drop-circle')
                    .attr('title', showFilterText);
                formFilter.addClass('hide');
                $('#week-container').addClass('hide');
            } else {
                iconFilter
                    .removeClass('icon-arrow-down-drop-circle')
                    .addClass('icon-arrow-up-drop-circle')
                    .attr('title', hideFilterText);
                formFilter.removeClass('hide');
                $('#week-container').removeClass('hide');
            }
        });
    }

    private bindExportExcelEvent() {
        $('#icon-excel')
            .off()
            .click((e) => {
                if (this.isInvalidGlobalFormData()) {
                    e.preventDefault();
                    return;
                }

                const $this = $(e.target);
                const excelParams = $.extend({}, this.globalFormData);
                $this.attr('href', `${$this.data('url')}?${$.param(excelParams)}`);
                setTimeout(function () {
                    $this.attr('href', 'javascript:;');
                }, 1000);
            });
    }

    private renderComponents() {
        this.dateRangeComponent.render({ maxDays: 92 }, () => {
            const dateRange = this.dateRangeComponent.getDateRangeValues();
            const productsAndResources = this.productComponent.getCheckedValues();
            $.extend(this.globalFormData, dateRange, productsAndResources);
            if (this.isInvalidGlobalFormData()) {
                return;
            }

            this.getBetListWeekSelection();
        });

        this.productComponent.render(this.productList, this.productDetailList, () => {
            const dateRange = this.dateRangeComponent.getDateRangeValues();
            const productsAndResources = this.productComponent.getCheckedValues();
            $.extend(this.globalFormData, dateRange, productsAndResources);
            if (this.isInvalidGlobalFormData()) {
                return;
            }

            this.getBetListWeekSelection();
        });

        this.weekComponent.init((dateRange) => {
            if (this.isInvalidGlobalFormData()) {
                return;
            }

            $.extend(this.globalFormData, dateRange);
            this.getBetListData();
        });

        const activeWeek = this.weekComponent.getActiveWeek();
        // update fromDate/toDate of globalFormData if activeWeek is NOT empty
        if (activeWeek) {
            this.globalFormData.fromDate = activeWeek.weekFromDate;
            this.globalFormData.toDate = activeWeek.weekToDate;
        }

        this.breadcrumbLevelComponent.init(($element, breadcrumbParam) => {
            if (this.isInvalidGlobalFormData()) {
                return;
            }

            this.tableBetList.block();
            const breadcrumbLevels = this.breadcrumbLevelComponent.getBreadcrumbLevels(breadcrumbParam.custLevelId);
            let reportParams: any = this.weekComponent.getActiveWeek() || {};
            if (breadcrumbLevels.length) {
                reportParams.breadcrumbsJson = breadcrumbLevels;
            }
            reportParams.visibleColumns = $('#hidden-show-columns').val();
            $.extend(reportParams, this.globalFormData, breadcrumbParam, this.dateRangeComponent.getDateRangeValues());

            $element.attr('href', this.buildWinLossReportUrl(reportParams));
        });
    }

    private buildWinLossReportUrl(reportFormData) {
        return `${$('#winloss-detail-report-path').val()}?${$.param(reportFormData)}`;
    }

    private getBetListWeekSelection() {
        //this.addUnselectedProductAndProductDetailForStatistic();
        const formData = $.extend({}, this.globalFormData, {
            breadcrumbsJson: this.breadcrumbLevelComponent.getBreadcrumbLevels()
        });
        AJAXHelper.get(
            `${AJAXHelper.buildURL('WinLoss/BetListWeekSelection')}?${$.param(formData)}`,
            (html) => {
                this.betListWeekSelection.html(html);

                this.tableBetList = this.betListWeekSelection.find('#tbl-betlist');
                this.weekComponent.bindEvents();
                this.breadcrumbLevelComponent.bindEvents();
                this.bindExportExcelEvent();
                this.updateCurrentUrl();

                const activeWeek = this.weekComponent.getActiveWeek();
                // update fromDate/toDate of globalFormData to activeWeek
                if (activeWeek) {
                    this.globalFormData.fromDate = activeWeek.weekFromDate;
                    this.globalFormData.toDate = activeWeek.weekToDate;
                }

                this.betListWeekSelection.unblock();
            },
            this.betListWeekSelection
        );
    }

    private getBetListData() {
        this.tableBetList.block();

        //this.addUnselectedProductAndProductDetailForStatistic();
        //this.addIsWeekSelectionForStatistic();
        AJAXHelper.get(
            `${AJAXHelper.buildURL('WinLoss/BetList')}?${$.param(this.globalFormData)}`,
            (html) => {
                this.tableBetList.find('tbody').html(html);
                this.tableBetList.unblock();
            },
            this.tableBetList
        );
        //.always(() => this.removeIsWeekSelectionForStatistic());
    }

    private isInvalidGlobalFormData() {
        return this.globalFormData.productIds.length === 0;
    }

    private updateCurrentUrl() {
        if (this.globalFormData.productIds) {
            const formData = $.extend({}, this.globalFormData, {
                breadcrumbsJson: JSON.stringify(this.breadcrumbLevelComponent.getBreadcrumbLevels())
            });

            alphaBetLists.updateQueryString($.param(formData));
        }
    }

    // private addUnselectedProductAndProductDetailForStatistic() {
    //     let moreParams: any = {};
    //     const productAndProductDetail = this.productComponent.getUncheckedValues();
    //     if (productAndProductDetail.unselectedProductIds) {
    //         moreParams.unselectedProductIds = productAndProductDetail.unselectedProductIds;
    //     } else {
    //         delete this.globalFormData.unselectedProductIds;
    //     }

    //     if (productAndProductDetail.unselectedResourceIds) {
    //         moreParams.unselectedResourceIds = productAndProductDetail.unselectedResourceIds;
    //     } else {
    //         delete this.globalFormData.unselectedResourceIds;
    //     }

    //     $.extend(this.globalFormData, moreParams);
    // }

    // private addIsWeekSelectionForStatistic() {
    //     this.globalFormData.isWeekSelection = 1;
    // }

    // private removeIsWeekSelectionForStatistic() {
    //     delete this.globalFormData.isWeekSelection;
    // }
}

$(function () {
    new WinLossBetListView(
        new DateRangePickerJQueryComponent(),
        new ProductDropdownComponent(),
        new WeekComponent(),
        new BreadcrumbLevelComponent(),
        window.productList,
        window.productDetailList
    ).init();
});
