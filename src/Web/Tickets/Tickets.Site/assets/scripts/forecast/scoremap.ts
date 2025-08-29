interface JQuery {
    modal(option: any): void;
}

class ScoreMap {
    $ddlLeague: JQuery = null;
    $iconLoadingLeague: JQuery = null;
    $popupMatches: JQuery = null;
    $popupMatchList: JQuery = null;
    $popupAlertMatchError: JQuery = null;
    $popupBtnSelect: JQuery = null;
    $tblLiveDeadBetType: JQuery = null;
    $btnSubmit: JQuery = null;
    $iconLoadingScoreMapResults: JQuery = null;
    $alertBetTypeError: JQuery = null;
    $content: JQuery = null;
    resources = null;
    isLeagueChanged = true;
    globalFormData = null;
    cacheMatchIds = null;
    compactFeedback = new CompactFeedbackComponent();

    constructor() {
        this.$ddlLeague = $('#ddlLeague');
        this.$iconLoadingLeague = $('#icon-loadingLeague');
        this.$popupMatches = $('#popup-matches');
        this.$popupMatchList = $('#popup-matchList');
        this.$popupAlertMatchError = $('#popup-alertMatchError');
        this.$popupBtnSelect = $('#popup-btnSelect');
        this.$tblLiveDeadBetType = $('#tbl-liveDeadBetType');
        this.$btnSubmit = $('#btn-submit');
        this.$iconLoadingScoreMapResults = $('#icon-loadingScoreMapResults');
        this.$alertBetTypeError = $('#alert-betTypeError');
        this.$content = $('#content');
        this.resources = {
            all: $('#hidden-allText').val()
        };
        this.cacheMatchIds = $('#hidden-matchIds').val();
    }

    init() {
        this.$ddlLeague.change(() => {
            this.isLeagueChanged = true;
            this.resetUIControls();
        });
        $('#icon-refreshLeague').click(() => this.refreshLeagues());
        $('#btn-selectMatch').click(() => this.getMatches());
        this.$popupMatches.on('hidden.bs.modal', () => {
            if ($(document.activeElement).attr('id') === 'popup-btnSelect') {
                this.$btnSubmit.removeClass('hidden');
                this.$tblLiveDeadBetType.removeClass('hidden');
            } else {
                this.resetUIControls();
            }

            this.$popupAlertMatchError.addClass('hidden');
        });
        this.$popupBtnSelect.click(() => {
            if (this.$popupMatchList.find('.match:checked').length) {
                this.$popupMatches.modal('hide');
            } else {
                this.$popupAlertMatchError.removeClass('hidden');
            }
        });
        const $allLiveBetTypeCheckbox = $('#chk-allLiveBetType');
        const $liveBetTypeCheckboxes = $('.live-bettype');
        this.bindLiveDeadBallsEvents($allLiveBetTypeCheckbox, $liveBetTypeCheckboxes);
        const $allDeadBetTypeCheckbox = $('#chk-allDeadBetType');
        const $deadBetTypeCheckboxes = $('.dead-bettype');
        this.bindLiveDeadBallsEvents($allDeadBetTypeCheckbox, $deadBetTypeCheckboxes);
        this.$btnSubmit.click(() => this.getScoreMapResults());
        if (!this.$ddlLeague.length) {
            this.globalFormData = this.getScoreMapResultsFormData();
            this.bindEventsAfterRenderScoreMapSucessfully(this.$content);
            this.handleBetTypeChanged($allLiveBetTypeCheckbox, $liveBetTypeCheckboxes);
            this.handleBetTypeChanged($allDeadBetTypeCheckbox, $deadBetTypeCheckboxes);
            this.initCompactFeedback(this.globalFormData);
        }
        $('.icon-information-outline').click(function (e) {
            e.preventDefault();
            const width = window.top.window.innerWidth * 0.7;
            const url = $(this).attr('href');
            top.popupManager.openMultiple(url, '', width);
        });
    }

    resetUIControls() {
        this.$alertBetTypeError.addClass('hidden');
        this.$content.html('');
        this.$btnSubmit.addClass('hidden');
        this.$tblLiveDeadBetType.addClass('hidden');
        this.compactFeedback.toggleShowHide(false);
    }

    refreshLeagues() {
        this.isLeagueChanged = true;
        this.$iconLoadingLeague.removeClass('hidden');
        $.get('GetScoreMapLeagues', (leagues) => {
            const html = [];
            leagues.forEach(function (item) {
                html.push('<option value="', item.LeagueId, '">', item.LeagueName, '</option>');
            });
            this.$ddlLeague.html(html.join(''));
        }).always(() => this.$iconLoadingLeague.addClass('hidden'));
    }

    getMatches() {
        if (!this.isLeagueChanged) {
            this.$popupMatches.modal('show');
            return;
        }
        this.$iconLoadingLeague.removeClass('hidden');
        $.get('GetScoreMapMatches?leagueId=' + this.$ddlLeague.val(), (matches) => {
            const html = [];
            const matchesCount = matches ? matches.length : 0;
            if (matchesCount > 0) {
                if (matchesCount > 1) {
                    html.push(
                        '<li><input type="checkbox" id="chk-allMatch"/>',
                        '<label for="chk-allMatch">',
                        this.resources.all,
                        '</label></li>'
                    );
                }

                matches.forEach(function (item) {
                    html.push(
                        '<li><input type="checkbox" class="match" id="chk-',
                        item.MatchId,
                        '" value="',
                        item.MatchId,
                        '"/>',
                        '<label for="chk-',
                        item.MatchId,
                        '">',
                        item.Event,
                        '</label></li>'
                    );
                });
            }

            this.$popupMatchList.html(html.join(''));
            const $chkAllMatch = this.$popupMatchList.find('#chk-allMatch');
            const $chkMatches = this.$popupMatchList
                .find('.match')
                .off()
                .change(function () {
                    if (this.checked) {
                        $chkAllMatch.prop('checked', $chkMatches.filter(':not(:checked)').length === 0);
                    } else {
                        $chkAllMatch.prop('checked', false);
                    }
                });
            $chkAllMatch.off().change(function () {
                $chkMatches.prop('checked', this.checked);
            });
            this.$popupMatches.modal('show');
        }).always(() => {
            this.isLeagueChanged = false;
            this.$iconLoadingLeague.addClass('hidden');
        });
    }

    bindLiveDeadBallsEvents($chkAllBetType: JQuery, $chkBetTypes) {
        $chkAllBetType.change(function () {
            $chkBetTypes.prop('checked', this.checked);
        });
        $chkBetTypes.change(() => this.handleBetTypeChanged($chkAllBetType, $chkBetTypes));
    }

    handleBetTypeChanged($chkAllBetType: JQuery, $chkBetTypes) {
        const checkedBetTypesLength = $chkBetTypes.filter(':checked').length;
        const isCheckAll = checkedBetTypesLength > 0 && checkedBetTypesLength === $chkBetTypes.length;
        const isIndeterminate = !isCheckAll && checkedBetTypesLength > 0;
        $chkAllBetType.prop({
            indeterminate: isIndeterminate,
            checked: isCheckAll
        });
    }

    getScoreMapResults() {
        this.$content.html('');
        this.globalFormData = this.getScoreMapResultsFormData();
        if (typeof this.globalFormData !== 'undefined') {
            this.$iconLoadingScoreMapResults.removeClass('hidden');
            $.get('ScoreMapResults?' + $.param(this.globalFormData), (resultHtml) => {
                this.$content.html(resultHtml);
                this.bindEventsAfterRenderScoreMapSucessfully(this.$content);
                this.initCompactFeedback(this.globalFormData);
            }).always(() => {
                this.$iconLoadingScoreMapResults.addClass('hidden');
            });
        }
    }

    getScoreMapResultsFormData() {
        const liveBetTypes = [];
        $('.live-bettype:checked').each(function () {
            liveBetTypes.push(this.value);
        });

        const deadBetTypes = [];
        $('.dead-bettype:checked').each(function () {
            deadBetTypes.push(this.value);
        });

        if (!liveBetTypes.length && !deadBetTypes.length) {
            this.$alertBetTypeError.removeClass('hidden');
            return;
        }

        const matchIds = [];
        this.cacheMatchIds && matchIds.push(this.cacheMatchIds);
        this.$popupMatchList.find('.match:checked').each(function () {
            matchIds.push(this.value);
        });

        this.$alertBetTypeError.addClass('hidden');

        return {
            matchIds: matchIds.join(),
            liveBetTypes: liveBetTypes.join(),
            deadBetTypes: deadBetTypes.join()
        };
    }

    bindEventsAfterRenderScoreMapSucessfully(container: JQuery) {
        container
            .find('.icon-menu-down')
            .off()
            .click((e) => this.toggleScoreMapResult($(e.target)));
        container
            .find('.scoremap-match')
            .off()
            .click((e) => this.toggleScoreMapResult($(e.target).prev()));
        container
            .find('.icon-refresh')
            .off()
            .click((e) => this.refreshScoreMapResult(e));
    }

    toggleScoreMapResult($icon: JQuery) {
        const $result = $icon.parent().next();
        if ($icon.hasClass('open')) {
            $icon.removeClass('open');
            $result.removeClass('hidden');
        } else {
            $icon.addClass('open');
            $result.addClass('hidden');
        }
    }

    refreshScoreMapResult(e) {
        const $this = $(e.target);
        const iconLoading = $this.next();
        iconLoading.removeClass('hidden');
        const refreshFormData = $.extend({}, this.globalFormData, {
            isRefresh: true,
            matchIds: $this.data('matchid')
        });
        $.get('ScoreMapResults?' + $.param(refreshFormData), (resultHtml) => {
            const $scoreMapResult = $this.closest('.scoremap-result');
            $scoreMapResult.html(resultHtml);
            this.bindEventsAfterRenderScoreMapSucessfully($scoreMapResult);
        }).always(() => iconLoading.addClass('hidden'));
    }

    initCompactFeedback(formData) {
        let hasData = false;
        this.$content.find('.tbl-score-result').each((_, table) => {
            hasData = hasData || $(table).find('tbody tr').length > 0;
        });
        this.compactFeedback.init(formData, hasData);
    }
}

$(function () {
    new ScoreMap().init();
});
