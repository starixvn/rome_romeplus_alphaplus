'use strict';
(function ($) {
    const $ddlLeague = $('#ddlLeague');
    const $boxMatch = $('#match-list');
    const resources = {
        all: $('#hidden-allText').val(),
        selectMatch: $('#hidden-selectMatchText').val(),
        hideMatch: $('#hidden-hideMatchText').val()
    };

    function refreshLeagues() {
        //isLeagueChanged = true;
        //showElement(matchBox, false);
        // loadingIcon.addClass('loading');
        // enableButton(dSelMatch, false);
        // enableButton(dSubmit, false);
        // dSelMatch.text(scoreMapResources.SelectMatch);
        $.get('GetScoreMapLeagues', function (leagues) {
            const html = [];
            leagues.forEach((item) => html.push('<option value="', item.LeagueId, '">', item.LeagueName, '</option>'));

            $ddlLeague.html(html.join(''));
        }).always(function () {
            // loadingIcon.removeClass('loading');
            // enableButton(dSelMatch, true);
            // enableButton(dSubmit, true);
        });
    }

    function getMatches() {
        $.get('GetScoreMapMatches?leagueId=' + $ddlLeague.val(), function (matches) {
            const html = [];
            const matchesCount = matches ? matches.length : 0;
            if (matchesCount > 0) {
                if (matchesCount > 1) {
                    html.push(
                        '<li><input type="checkbox" id="chk-allMatch"/>',
                        '<label for="chk-allMatch">',
                        resources.all,
                        '</label></li>'
                    );
                }

                matches.forEach((item) => {
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

            $boxMatch.html(html.join(''));
            const $chkAllMatch = $boxMatch.find('#chk-allMatch');
            const $chkMatches = $boxMatch
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
            //matchBox.removeClass('hide');
            //selectAllMatches();
        }).always(function () {});
    }

    function getResult() {
        const liveBetTypes = [];
        $('.live-bettype:checked').each(function () {
            liveBetTypes.push(this.value);
        });

        const deadBetTypes = [];
        $('.dead-bettype:checked').each(function () {
            deadBetTypes.push(this.value);
        });

        if (!liveBetTypes.length && !deadBetTypes.length) {
            // mainContent.html('');
            // showBettypeError(scoreMapResources.BettypeEmptyError);
            return;
        }

        const matchIds = [];
        $boxMatch.find('.match:checked').each(function () {
            matchIds.push(this.value);
        });

        const queryString = {
            matchIds: matchIds.join(),
            liveBetTypes: liveBetTypes.join(),
            deadBetTypes: deadBetTypes.join()
        };
        $.get('GetScoreMapResults?' + $.param(queryString), function (html) {
            $('#content').html(html);
        });
    }

    $(function () {
        $('#icon-refresh').click(refreshLeagues);
        $('#btn-select-matches').click(getMatches);

        const $chkAllLiveBetType = $('#chk-allLiveBetType');
        const $chkLiveBetTypes = $('.live-bettype').change(function () {
            if (this.checked) {
                $chkAllLiveBetType.prop('checked', $chkLiveBetTypes.filter(':not(:checked)').length === 0);
            } else {
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
            } else {
                $chkAllDeadBetType.prop('checked', false);
            }
        });
        $chkAllDeadBetType.change(function () {
            $chkDeadBetTypes.prop('checked', this.checked);
        });

        $('#btn-submit').click(getResult);
    });
})(window.jQuery);
