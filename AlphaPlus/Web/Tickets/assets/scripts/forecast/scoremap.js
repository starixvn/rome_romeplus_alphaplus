"use strict";
class ScoreMap {
    constructor() {
        this.$ddlLeague = null;
        this.$matchList = null;
        this.$iconLoadingLeague = null;
        this.$popupMatchesError = null;
        this.$popupBtnSelect = null;
        this.$tblLiveDeadBetType = null;
        this.$btnSubmit = null;
        this.$iconLoadingScoreMapResults = null;
        this.$alertBetTypeError = null;
        this.$content = null;
        this.resources = null;
        this.isLeagueChanged = true;
        this.globalFormData = null;
        this.$ddlLeague = $('#ddlLeague');
        this.$matchList = $('#match-list');
        this.$iconLoadingLeague = $('#icon-loadingLeague');
        this.$popupMatchesError = $('#popup-matchesError');
        this.$popupBtnSelect = $('#popup-btnSelect');
        this.$tblLiveDeadBetType = $('#tbl-liveDeadBetType');
        this.$btnSubmit = $('#btn-submit');
        this.$iconLoadingScoreMapResults = $('#icon-loadingScoreMapResults');
        this.$alertBetTypeError = $('#alert-betTypeError');
        this.$content = $('#content');
        this.resources = {
            all: $('#hidden-allText').val(),
            selectMatch: $('#hidden-selectMatchText').val(),
            hideMatch: $('#hidden-hideMatchText').val()
        };
    }
    init() {
        this.$ddlLeague.change(() => {
            this.isLeagueChanged = true;
            this.$alertBetTypeError.addClass('hidden');
            this.$content.html('');
            this.$btnSubmit.addClass('hidden');
            this.$tblLiveDeadBetType.addClass('hidden');
        });
        $('#icon-refreshLeague').click(() => this.refreshLeagues());
        $('#btn-selectMatch').click(() => this.getMatches());
        $('#popup-matches').on('hidden.bs.modal', () => {
            this.$popupMatchesError.addClass('hidden');
            this.$tblLiveDeadBetType.removeClass('hidden');
            this.$btnSubmit.removeClass('hidden');
        });
        this.$popupBtnSelect.click(() => {
            if (this.$matchList.find('.match:checked').length) {
                $('#popup-matches').modal('hide');
            }
            else {
                this.$popupMatchesError.removeClass('hidden');
            }
        });
        const $chkAllLiveBetType = $('#chk-allLiveBetType');
        const $chkLiveBetTypes = $('.live-bettype').change(function () {
            if (this.checked) {
                $chkAllLiveBetType.prop('checked', $chkLiveBetTypes.filter(':not(:checked)').length === 0);
            }
            else {
                $chkAllLiveBetType.prop('checked', false);
            }
        });
        $chkAllLiveBetType.change(function () {
            $chkLiveBetTypes.prop('checked', this.checked);
        });
        const $chkAllDeadBetType = $('#chk-allDeadBetType');
        const $chkDeadBetTypes = $('.dead-bettype').change(function () {
            if (this.checked) {
                $chkAllDeadBetType.prop('checked', $chkDeadBetTypes.filter(':not(:checked)').length === 0);
            }
            else {
                $chkAllDeadBetType.prop('checked', false);
            }
        });
        $chkAllDeadBetType.change(function () {
            $chkDeadBetTypes.prop('checked', this.checked);
        });
        this.$btnSubmit.click(() => this.getScoreMapResults());
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
            $('#popup-matches').modal('show');
            return;
        }
        this.$iconLoadingLeague.removeClass('hidden');
        $.get('GetScoreMapMatches?leagueId=' + this.$ddlLeague.val(), (matches) => {
            const html = [];
            const matchesCount = matches ? matches.length : 0;
            if (matchesCount > 0) {
                if (matchesCount > 1) {
                    html.push('<li><input type="checkbox" id="chk-allMatch"/>', '<label for="chk-allMatch">', this.resources.all, '</label></li>');
                }
                matches.forEach(function (item) {
                    html.push('<li><input type="checkbox" class="match" id="chk-', item.MatchId, '" value="', item.MatchId, '"/>', '<label for="chk-', item.MatchId, '">', item.Event, '</label></li>');
                });
            }
            this.$matchList.html(html.join(''));
            const $chkAllMatch = this.$matchList.find('#chk-allMatch');
            const $chkMatches = this.$matchList
                .find('.match')
                .off()
                .change(function () {
                if (this.checked) {
                    $chkAllMatch.prop('checked', $chkMatches.filter(':not(:checked)').length === 0);
                }
                else {
                    $chkAllMatch.prop('checked', false);
                }
            });
            $chkAllMatch.off().change(function () {
                $chkMatches.prop('checked', this.checked);
            });
            $('#popup-matches').modal('show');
        }).always(() => {
            this.isLeagueChanged = false;
            this.$iconLoadingLeague.addClass('hidden');
        });
    }
    getScoreMapResults() {
        this.globalFormData = this.getScoreMapResultsFormData();
        if (typeof this.globalFormData !== 'undefined') {
            this.$iconLoadingScoreMapResults.removeClass('hidden');
            $.get('ScoreMapResults?' + $.param(this.globalFormData), (resultHtml) => {
                this.$content.html(resultHtml);
                this.bindEventsAfterRenderScoreMapSucessfully(this.$content);
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
            this.$content.html('');
            this.$alertBetTypeError.removeClass('hidden');
            return;
        }
        const matchIds = [];
        this.$matchList.find('.match:checked').each(function () {
            matchIds.push(this.value);
        });
        return {
            matchIds: matchIds.join(),
            liveBetTypes: liveBetTypes.join(),
            deadBetTypes: deadBetTypes.join()
        };
    }
    bindEventsAfterRenderScoreMapSucessfully(container) {
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
    toggleScoreMapResult($icon) {
        const $result = $icon.parent().next();
        if ($icon.hasClass('open')) {
            $icon.removeClass('open');
            $result.removeClass('hidden');
        }
        else {
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
}
$(function () {
    new ScoreMap().init();
});
