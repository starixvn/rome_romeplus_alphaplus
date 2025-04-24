/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/productmultiselection.ts" />
/// <reference path="../components/league-multiselection.ts" />

class MatchWinLossDetailView extends BaseReport {
    productComponent: ProductMultiSelectionComponent;
    dateRangePickerComponent: DateRangePickerComponent;
    leagueComponent: LeagueMultiSelectionComponent;
    reportTooltip: ReportTooltip;

    $leagueContainer: JQuery;
    $buttonLoadLeague: JQuery;
    $buttonLeague: JQuery;
    $userSelectedLeagueIds: JQuery;
    $isUserSelectsAllLeagues: JQuery;
    loadLeagueURL: string;

    isFirstSwitchMode = true;

    constructor(
        productComponent: ProductMultiSelectionComponent,
        dateRangePickerComponent: DateRangePickerComponent,
        leagueComponent: LeagueMultiSelectionComponent,
        reportTooltip: ReportTooltip
    ) {
        super();

        this.dateRangePickerComponent = dateRangePickerComponent;
        this.productComponent = productComponent;
        this.leagueComponent = leagueComponent;
        this.reportTooltip = reportTooltip;
    }

    init() {
        $.blockUI();

        this.cacheElements();
        this.initComponent();
        this.registerEvents();

        $.unblockUI();
    }

    cacheElements() {
        this.$leagueContainer = $('#dropdown-league-container');
        this.$buttonLoadLeague = $('#btn-loadleague');
        this.$userSelectedLeagueIds = $('#league-ids');
        this.$isUserSelectsAllLeagues = $('#IsUserSelectsAllLeagues');

        const $loadLeagueURL = $('#LoadLeagueURL');
        this.loadLeagueURL = $loadLeagueURL.attr('href');
        $loadLeagueURL.remove();
    }

    registerEvents() {
        $('.ic-switchmode').on('click', this.onSwitchMode);
        this.$buttonLoadLeague.on('click', this.loadLeagues);
    }

    initComponent() {
        this.productComponent.render(this.showButtonLoadLeague);
        this.dateRangePickerComponent.render(
            new DateRangePickerOption('daterange-picker', 'report-form', this.showButtonLoadLeague)
        );
        if (
            this.$buttonLoadLeague.hasClass('hide') &&
            (this.$userSelectedLeagueIds.val() !== '' || this.$isUserSelectsAllLeagues.val().toLowerCase() === 'true')
        ) {
            this.leagueComponent.render();
            this.$buttonLeague = this.leagueComponent.$multiSelectButton;
        }

        if (
            $('#IsSetDefaultStackMode').val().toString().toLowerCase() === 'true' &&
            alphaReports.screenSmallThan768()
        ) {
            this.setStackMode();
            this.reportTooltip.show(new ReportTooltipOption('icon-normal-column', 'matchwinlossdetail'));
        }
    }

    onSubmitReport = (event: any) => {
        if (!this.productComponent.validateProductIds()) {
            event.preventDefault();
            return;
        }

        if (this.$buttonLoadLeague.hasClass('hide') && !this.leagueComponent.validateLeagueIds()) {
            event.preventDefault();
            return;
        }

        super.onSubmitReport(event);
    };

    onSwitchMode = () => {
        $.blockUI();
        const currentStackModeState = this.isStackMode();

        if (currentStackModeState) {
            this.$pageTitle.removeClass('stack-mode');
            this.$tableReport.removeClass('stack-mode');
        } else {
            this.$pageTitle.addClass('stack-mode');
            this.$tableReport.addClass('stack-mode');
        }

        if (this.isFirstSwitchMode) {
            if (currentStackModeState) {
                alphaReports.splitCell('col-turnover', '');
                this.levels.forEach(function (element: string, index: number) {
                    alphaReports.splitCell(`col-${element}-winloss`, index % 2 ? ' altercol' : '');
                });
                alphaReports.splitCell('col-company-winloss', '');
            } else {
                alphaReports.combineConsecutiveCells($('td.col-turnover'), 2, 'col-turnover-combined');
                this.levels.forEach(function (element: string, index: number) {
                    const cellCss = `col-${element}-winloss-combined` + (index % 2 ? ' altercol' : '');
                    alphaReports.combineConsecutiveCells($(`td.col-${element}-winloss`), 3, cellCss);
                });
                alphaReports.combineConsecutiveCells($('td.col-company-winloss'), 2, 'col-company-winloss-combined');
            }

            this.isFirstSwitchMode = false;
        }

        $.unblockUI();
    };

    setStackMode = () => {
        $.blockUI();

        this.$pageTitle.addClass('stack-mode');
        this.$tableReport.addClass('stack-mode');

        alphaReports.combineConsecutiveCells($('td.col-turnover'), 2, 'col-turnover-combined');

        this.levels.forEach(function (element: string, index: number) {
            const cellCss = `col-${element}-winloss-combined${index % 2 ? ' altercol' : ''}`;
            alphaReports.combineConsecutiveCells($(`td.col-${element}-winloss`), 3, cellCss);
        });

        alphaReports.combineConsecutiveCells($('td.col-company-winloss'), 2, 'col-company-winloss-combined');

        this.isFirstSwitchMode = false;
        $.unblockUI();
    };

    loadLeagues = () => {
        $.blockUI();

        $.ajax({
            method: 'GET',
            url: this.loadLeagueURL,
            cache: false,
            data: {
                fromDate: $('#from-date').val(),
                toDate: $('#to-date').val(),
                products: this.productComponent.getProductIds()
            },
            success: this.renderLeagues,
            complete: $.unblockUI
        });
    };

    renderLeagues = (html: string) => {
        this.$leagueContainer.html($(html).html());
        this.leagueComponent.render();

        this.$buttonLoadLeague = $('#btn-loadleague');
        this.$buttonLoadLeague.on('click', this.loadLeagues);

        this.$buttonLeague = this.leagueComponent.$multiSelectButton;
        this.$userSelectedLeagueIds = $('#league-ids');

        this.$userSelectedLeagueIds.val('');
        this.$buttonLoadLeague.addClass('hide');

        this.$isUserSelectsAllLeagues = $('#IsUserSelectsAllLeagues');
        this.$buttonLeague.click();
    };

    showButtonLoadLeague = () => {
        this.$userSelectedLeagueIds.val('');
        this.$buttonLoadLeague.removeClass('hide');
        this.$leagueContainer.find('> .btn-group:not(.btn-loadleague-container)').addClass('hide');
        this.$isUserSelectsAllLeagues.val('false');
    };
}

$(function () {
    new MatchWinLossDetailView(
        new ProductMultiSelectionComponent(),
        new DateRangePickerComponent(),
        new LeagueMultiSelectionComponent(),
        new ReportTooltip()
    ).init();
});
