class LastBetsNewView {
    private iconSaveFilter: JQuery;
    private dropdownFilter: JQuery;
    private dropdownMostRecentTicket: JQuery;
    private autoRefreshComponent: AutoRefreshComponent;
    private lastBetsCopyHtml: LastBetsCopyHtml;
    private saveFilterView: SaveFilterView;

    $msgError: JQuery;
    $betListTableContainer: JQuery;

    isCalling = false;
    ajaxRequest: JQueryXHR;

    lastTransIdCacheKeyByFilter = '';
    latestTransIdCacheKeyByFilter = '';

    constructor(
        autoRefreshComponent: AutoRefreshComponent,
        lastBetsCopyHtml: LastBetsCopyHtml,
        saveFilter: SaveFilterView
    ) {
        this.autoRefreshComponent = autoRefreshComponent;
        this.lastBetsCopyHtml = lastBetsCopyHtml;
        this.saveFilterView = saveFilter;
    }

    init() {
        this.cacheElements();
        this.registerEvents();
        this.setTransIdCacheKeyByFilter(
            $('#last-trans-id-cache-key-by-filter').val(),
            $('#latest-trans-id-cache-key-by-filter').val()
        );
        this.setTransId($('#last-trans-id').val(), $('#latest-trans-id').val());

        this.lastBetsCopyHtml.init();
        this.saveFilterView.init(this.iconSaveFilter, () => this.getSelectedFilterParams());
        this.$betListTableContainer.find('[data-toggle="tooltip"]').tooltip();

        const filterId = this.dropdownFilter.find(':selected').val();
        this.iconSaveFilter.toggleClass('hide', filterId !== '0');
        if (!filterId) {
            this.autoRefreshComponent.disable();
        } else if (this.autoRefreshComponent.isOn()) {
            this.autoRefreshComponent.start();
        }

        this.initFeedback();
    }

    private cacheElements() {
        this.$msgError = $('#msg-error');
        this.$betListTableContainer = $('#bet-list-table-container');

        this.iconSaveFilter = $('#icon-save-filter');
    }

    private registerEvents() {
        this.registerToggleFilter();

        this.dropdownFilter = $('#monitoring-bets-filter').select2({
            minimumResultsForSearch: -1,
            dropdownAutoWidth: true
        });
        this.dropdownFilter.on('change.select2', () => {
            this.loadLastBets(true);
            this.autoRefreshComponent.enable();
            this.autoRefreshComponent.start();

            const filterId = this.dropdownFilter.find(':selected').val();
            this.iconSaveFilter.toggleClass('hide', filterId !== '0');

            Common.Utils.UpdateCurrentUrl(this.getSelectedFilterParams());
        });

        this.dropdownMostRecentTicket = $('#most-recent-tickets').select2({
            minimumResultsForSearch: -1
        });
        this.dropdownMostRecentTicket.on('change.select2', () => {
            if (this.dropdownFilter.find(':selected').val()) {
                this.loadLastBets(true);
                Common.Utils.UpdateCurrentUrl(this.getSelectedFilterParams());
            }
        });

        this.autoRefreshComponent.init(
            new AutoRefreshOption(
                $('#hidden-OnText').val(),
                $('#hidden-OffText').val(),
                () => this.loadLastBets(false),
                (onOffStatus) => Common.Utils.UpdateCurrentUrl(this.getSelectedFilterParams(onOffStatus))
            )
        );

        $('.navigation').click((e) => {
            const $this = $(e.target);
            const url = `${$this.attr('href')}?${$.param(this.getSelectedFilterParams())}`;
            $this.attr('href', url);
        });
    }

    private registerToggleFilter() {
        const filterContainer = $('#form-monitoring-bets');
        const iconFilter = $('#icon-filter').click(function () {
            const isHideFilter = iconFilter.hasClass('icon-arrow-up-drop-circle');
            if (isHideFilter) {
                iconFilter
                    .removeClass('icon-arrow-up-drop-circle')
                    .addClass('icon-arrow-down-drop-circle')
                    .attr('title', $('#hidden-ShowFilterText').val());
                filterContainer.addClass('hide');
            } else {
                iconFilter
                    .removeClass('icon-arrow-down-drop-circle')
                    .addClass('icon-arrow-up-drop-circle')
                    .attr('title', $('#hidden-HideFilterText').val());
                filterContainer.removeClass('hide');
            }
        });
    }

    private initFeedback() {
        setTimeout(function () {
            window.top.indexManager.initFeedbackFunction(4, undefined, 450);
        }, 1000);
    }

    private setTransIdCacheKeyByFilter(lastTransId, lastestTransId) {
        this.lastTransIdCacheKeyByFilter = lastTransId;
        this.latestTransIdCacheKeyByFilter = lastestTransId;
    }

    private setTransId(lastTransId, lastestTransId) {
        alphaReports.localStorage.set(this.lastTransIdCacheKeyByFilter, lastTransId);
        alphaReports.localStorage.set(this.latestTransIdCacheKeyByFilter, lastestTransId);
    }

    private loadLastBets = (isResetTransId: boolean) => {
        if (isResetTransId) {
            this.setTransId(0, 0);
        }

        this.isCalling && this.abortRequest();

        this.isCalling = true;
        const url = AJAXHelper.buildURL('LastBets/BetsDataNew');
        this.ajaxRequest = AJAXHelper.post(url, this.getParam(), this.successGetLastBets, this.completeGetLastBets);
    };

    private getParam() {
        let lastTransId = alphaReports.localStorage.get(this.lastTransIdCacheKeyByFilter);
        let latestTransId = alphaReports.localStorage.get(this.latestTransIdCacheKeyByFilter);
        const selectedFilter = this.dropdownFilter.find(':selected');

        const FilterId = selectedFilter.val();
        const MemberList = selectedFilter.data('members');
        const BetTypeList = selectedFilter.data('bet-types');
        const SportTypeList = selectedFilter.data('sport-types');
        const LeagueList = selectedFilter.data('leagues');
        const EvenStatus = selectedFilter.data('event-status');
        const MinStake = selectedFilter.data('stake');
        const FilterByMembers = selectedFilter.data('filter-by-member');
        const LimitRowCount = this.dropdownMostRecentTicket.find(':selected').val();

        return {
            FilterId,
            TransId: lastTransId ? lastTransId : 0,
            MemberList,
            BetTypeList,
            SportTypeList,
            LeagueList,
            EvenStatus,
            MinStake,
            LimitRowCount,
            FilterByMembers,
            LatestTransId: latestTransId
        };
    }

    private abortRequest() {
        this.ajaxRequest && this.ajaxRequest.abort();
    }

    private successGetLastBets = (responseHtml: any) => {
        if (
            (responseHtml.ErrorCode && responseHtml.ErrorCode !== 0) ||
            (responseHtml.ErrCode && responseHtml.ErrCode !== 0)
        ) {
            this.showError();
            return;
        }

        this.$msgError.addClass('hide');

        if (!this.isCalling) {
            return;
        }

        this.$betListTableContainer.html(responseHtml);
        this.$betListTableContainer.find('[data-toggle="tooltip"]').tooltip();

        const lastTransIdCacheKeyByFilter = this.$betListTableContainer.find('#new-last-trans-id-by-filter').val();
        const latestTransIdCacheKeyByFilter = this.$betListTableContainer.find('#new-latest-trans-id-by-filter').val();
        this.setTransIdCacheKeyByFilter(lastTransIdCacheKeyByFilter, latestTransIdCacheKeyByFilter);

        const lastTransId = this.$betListTableContainer.find('#last-trans-id').val();
        const latestTransId = this.$betListTableContainer.find('#latest-trans-id').val();
        this.setTransId(lastTransId, latestTransId);

        this.isCalling = false;
    };

    private showError() {
        this.$msgError.removeClass('hide');
        // protection level, avoid through too many exceptions on server
    }

    private completeGetLastBets = () => this.autoRefreshComponent.isOn() && this.autoRefreshComponent.start();

    private getSelectedFilterParams(autoStart = undefined) {
        return {
            filterName: $.trim(this.dropdownFilter.find(':selected').text()),
            limitRowCount: this.dropdownMostRecentTicket.find(':selected').val(),
            autoStart: autoStart || this.autoRefreshComponent.isOn()
        };
    }
}

$(function () {
    new LastBetsNewView(new AutoRefreshComponent(), new LastBetsCopyHtml(Clipboard), new SaveFilterView()).init();
});
