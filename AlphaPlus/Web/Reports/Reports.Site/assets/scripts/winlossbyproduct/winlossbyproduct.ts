/// <reference path="../core.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/productmultiselection.ts" />
/// <reference path="../components/tooltip.ts" />

class WinLossByProductView extends BaseReport {
    isAgentLevel: boolean;
    isFirstSwitchMode = true;

    productComponent: ProductMultiSelectionComponent;
    dateRangePickerComponent: DateRangePickerComponent;
    reportTooltip: ReportTooltip;

    constructor(
        dateRangePickerComponent: DateRangePickerComponent,
        productComponent: ProductMultiSelectionComponent,
        reportTooltip: ReportTooltip
    ) {
        super();

        this.productComponent = productComponent;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.reportTooltip = reportTooltip;
    }

    init() {
        $.blockUI();

        this.isAgentLevel = $('#IsAgentLevel').val().toLowerCase() === 'true';
        this.initComponents();
        this.registerEvents();

        $.unblockUI();
    }

    initComponents() {
        if (
            $('#IsSetDefaultStackMode').val().toString().toLowerCase() === 'true' &&
            alphaReports.screenSmallThan768()
        ) {
            this.setCssBySwitchMode(false);
            this.combineColumns(this.isAgentLevel);
            this.isFirstSwitchMode = false;
            this.reportTooltip.show(new ReportTooltipOption('icon-normal-column', 'winlossbyproduct'));

            this.hideFilter();
        }

        this.productComponent.render();
        this.dateRangePickerComponent.render();
    }

    public registerEvents() {
        $('.icon-switchmode').on('click', this.onSwitchMode);

        const self = this;
        $('.product-row').on('click', function () {
            self.onClickProduct($(this));
        });
        $('.sport-row').on('click', function () {
            self.onClickSport($(this));
        });
    }

    onSwitchMode = () => {
        const isStackMode = this.isStackMode();
        this.setCssBySwitchMode(isStackMode);

        if (this.isFirstSwitchMode) {
            this.renderAtFirstSwitchMode(isStackMode);
            this.isFirstSwitchMode = false;
        }
    };

    onClickProduct = (productRow: JQuery) => {
        const productId = productRow.data('product-id');
        const isHide = productRow.data('is-hide');

        const columnUsername = productRow.find('.col-username');
        const sportRows = $('.sport-of-' + productId);
        const betTypeRows = $('.bet-type-of-product-' + productId);

        if (isHide) {
            columnUsername.addClass('active');
            columnUsername.find('.icon-arrow-right').removeClass('icon-arrow-right').addClass('icon-arrow-down');

            sportRows.find('.col-username').addClass('active');
            sportRows
                .find('.col-username .icon-arrow-right')
                .removeClass('icon-arrow-right')
                .addClass('icon-arrow-down');

            sportRows.data('is-hide', 'false');
            sportRows.removeClass('hide');
            betTypeRows.removeClass('hide');
        } else {
            columnUsername.removeClass('active');
            columnUsername.find('.icon-arrow-down').removeClass('icon-arrow-down').addClass('icon-arrow-right');

            sportRows.find('.col-username').removeClass('active');
            sportRows
                .find('.col-username .icon-arrow-down')
                .removeClass('icon-arrow-down')
                .addClass('icon-arrow-right');

            sportRows.data('is-hide', 'true');
            sportRows.addClass('hide');
            betTypeRows.addClass('hide');
        }

        productRow.data('is-hide', !isHide);
    };

    onClickSport = (sportRow: JQuery) => {
        const productId = sportRow.data('product-id');
        const sportType = sportRow.data('sporttype');
        const isHide = sportRow.data('is-hide');

        const columnUsername = sportRow.find('.col-username');
        const betTypeRows = $(`.bet-type-of-product-${productId}.bet-type-of-sport-${sportType}`);

        if (isHide) {
            columnUsername.addClass('active');
            columnUsername.find('.icon-arrow-right').removeClass('icon-arrow-right').addClass('icon-arrow-down');

            betTypeRows.removeClass('hide');
        } else {
            columnUsername.removeClass('active');
            columnUsername.find('.icon-arrow-down').removeClass('icon-arrow-down').addClass('icon-arrow-right');

            betTypeRows.addClass('hide');
        }

        sportRow.data('is-hide', !isHide);
    };

    renderAtFirstSwitchMode = (isSwitchToNormalMode: boolean) =>
        isSwitchToNormalMode ? this.splitColumns() : this.combineColumns(this.isAgentLevel);

    setCssBySwitchMode = (isSwitchToNormalMode: boolean) => {
        if (isSwitchToNormalMode) {
            this.$pageTitle.removeClass('stack-mode');
            this.$tableReport.removeClass('stack-mode');
        } else {
            this.$pageTitle.addClass('stack-mode');
            this.$tableReport.addClass('stack-mode');
        }
    };

    splitColumns = () => {
        alphaReports.splitCell('col-turnover');

        this.levels.forEach(function (element: string, index: number) {
            if (element === 'master') {
                alphaReports.splitCell(`col-${element}-winloss`, index % 2 ? '' : ' altercol');
            } else {
                alphaReports.splitCell(`col-${element}-winloss`, index % 2 ? ' altercol' : '');
            }
        });
    };

    combineColumns = (isAgentLevel: boolean) => {
        isAgentLevel ? this.combineToTurnOverColumnAgentLevel() : this.combineToTurnOverColumnSuperMasterLevel();
        this.levels.forEach((element: string, index: number) => this.combineToWinLossColumn(element, index));
    };

    combineToTurnOverColumnAgentLevel = () =>
        alphaReports.combineConsecutiveCells($('td.col-turnover'), 2, 'text-right col-turnover-combined');

    combineToTurnOverColumnSuperMasterLevel = () =>
        alphaReports.combineConsecutiveCells($('td.col-turnover'), 3, 'text-right col-turnover-combined');

    combineToWinLossColumn = (level: string, index: number) => {
        let cellCss = `text-right col-${level}-winloss-combined`;

        if (level === 'master') {
            cellCss = cellCss + (index % 2 ? '' : ' altercol');
        } else {
            cellCss = cellCss + (index % 2 ? ' altercol' : '');
        }

        alphaReports.combineConsecutiveCells($(`td.col-${level}-winloss`), 3, cellCss);
    };

    onSubmitReport = (event: any) => {
        if (!this.productComponent.validateProductIds()) {
            event.preventDefault();
            return;
        }

        super.onSubmitReport(event);
    };
}

$(function () {
    new WinLossByProductView(
        new DateRangePickerComponent(),
        new ProductMultiSelectionComponent(),
        new ReportTooltip()
    ).init();
});
