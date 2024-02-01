/// <reference path="soccer-renderer.ts" />
interface JQueryStatic {
    blockUI(): void;
    unblockUI(): void;
    cookie(key: string): string;
    cookie(key: string, value: string, options): void;
}

interface JQuery {
    select2(): void;
    select2(options): void;
    select2(method: string, options): void;
}

interface Window {
    alpha: {
        urls: {
            mainRootPath: string;
        };
    };
}

const soccer = {
    countDownSeconds: 30,
    countDownDeadBallSeconds: 60,
    countDownLiveBallSeconds: 30,
    loadMatchesInterval: 5 * 60 * 1000,
    isStartCountdown: false,
    loadingMatches: [],
    maxNumOfEvents: null,
    currentRequest: null,

    urls: {
        getLeagues: 'GetSoccerLeagues',
        getMatches: 'GetSoccerMatches',
        getMatchForecasts: 'GetSoccerResults'
    },

    cookies: {
        status: 'forecast_match_status',
        league: 'forecast_league',
        match: 'forecast_match'
    },
    cookieExpireTime: 1,

    domElements: {
        btnSubmit: null,
        ddlDeadBall: null,
        ddlLeague: null,
        ddlMatch: null,
        mainContent: null,
        browserWarning: null,
        closeWarning: null,
        btnMatchSelect: null,
        blockIconSmall: null,
        refreshValues: null,
        ddlRefresh: null
    },

    init: function () {
        $(document).ajaxStop($.unblockUI);
        soccer.cachedDOM();
        soccer.registerEvents();

        soccerRenderer.init();

        setInterval(soccer.loadLeagues, soccer.loadMatchesInterval);
    },

    cachedDOM: function () {
        soccer.domElements.btnSubmit = $('#btnSubmit');
        soccer.domElements.ddlMatch = $('#ddlMatch');
        soccer.domElements.ddlLeague = $('#ddlLeague');
        soccer.domElements.ddlDeadBall = $('#ddlLiveStatus');
        soccer.domElements.mainContent = $('#mainContent');
        soccer.domElements.browserWarning = $('#browser-warning');
        soccer.domElements.closeWarning = $('#close-warning');
        soccer.domElements.blockIconSmall = $('#blockIconSmall');
        soccer.domElements.refreshValues = $('#refresh-value a');
        soccer.domElements.ddlRefresh = $('#ddlRefresh');
    },

    registerEvents: function () {
        soccer.checkIEBrowserAndShowWarning();

        const matchStatusCache = $.cookie(soccer.cookies.status);
        if (matchStatusCache) {
            soccer.domElements.ddlDeadBall.val(matchStatusCache);
        }

        soccer.domElements.ddlDeadBall
            .off('change.forecast')
            .on('change.forecast', function () {
                soccer.domElements.mainContent.parent().hide().find('tbody').remove();
                const statusValue = soccer.domElements.ddlDeadBall.val();
                const isLive = statusValue === 'true';

                soccer.stopCountdown();
                soccer.countDownSeconds = isLive ? soccer.countDownLiveBallSeconds : soccer.countDownDeadBallSeconds;
                soccer.domElements.ddlRefresh.text(soccer.countDownSeconds);
                soccer.loadLeagues();

                $.cookie(soccer.cookies.status, statusValue, { expires: soccer.cookieExpireTime });
            })
            .trigger('change');

        soccer.domElements.btnSubmit.off('click.forecast').on('click.forecast', soccer.loadMatchForecasts);

        soccer.domElements.refreshValues.off('click.forecast').on('click.forecast', function () {
            soccer.countDownSeconds = parseInt($(this).data('refresh-value'));
            soccer.domElements.ddlRefresh.text(soccer.countDownSeconds);
            if (soccer.isStartCountdown) {
                soccer.stopCountdown();
                soccer.startCountdown();
            }
        });
    },

    loadMatchForecasts: function () {
        if (soccer.currentRequest) {
            soccer.currentRequest.abort();
        }

        soccer.stopCountdown();
        const matchIds = soccer.domElements.ddlMatch.val();

        if (matchIds == null) {
            soccer.domElements.btnMatchSelect
                .attr('title', soccer.domElements.ddlMatch.data('select-at-least'))
                .tooltip('show');
            soccer.domElements.mainContent.parent().hide().find('tbody').remove();
            return;
        }
        soccer.domElements.blockIconSmall.show();

        soccer.currentRequest = $.ajax({
            url: soccer.urls.getMatchForecasts,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: {
                matchIds: matchIds.join(','),
                leagueId: soccer.domElements.ddlLeague.val(),
                isLive: soccer.domElements.ddlDeadBall.val() === 'true',
                version: Math.random()
            }
        })
            .done(function (matchForecasts) {
                if (matchForecasts) {
                    soccer.removeLoadingIcon();

                    if (matchForecasts.length === 0) {
                        const noInfoHtml = soccerRenderer.renderNoInfo('no-info-body');
                        soccer.addNoInfoRow(noInfoHtml);
                    } else {
                        for (let i = 0; i < matchForecasts.length; i++) {
                            const matchForecast = matchForecasts[i];

                            if (matchForecast) {
                                const isUpdated = soccerRenderer.trackChangedAndUpdatedValues(matchForecast);

                                if (!isUpdated) {
                                    const html = soccerRenderer.renderMatchForecast(matchForecast);
                                    const newMatchForecast = $(html);

                                    soccer.addMatchForecast(newMatchForecast, html);
                                }
                            }
                        }

                        soccerRenderer.updateMatchesCached(matchForecasts);
                    }

                    soccer.domElements.mainContent.parent().show();
                }
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                soccer.removeLoadingIcon();
                soccer.processAjaxError(jqXHR);
            });

        $.cookie(soccer.cookies.match, matchIds.join(','), { expires: soccer.cookieExpireTime });
    },

    removeLoadingIcon: function () {
        soccer.domElements.ddlRefresh.removeClass('block-icon');
        soccer.domElements.blockIconSmall.hide();
        if (!soccer.isStartCountdown) {
            soccer.startCountdown();
        }
    },

    addMatchForecast: function (newMatchForecast, rawHtml) {
        const currentMatchForecast = soccer.domElements.mainContent.find(
            'tbody[id=' + newMatchForecast.attr('id') + ']'
        );

        currentMatchForecast.length > 0
            ? currentMatchForecast.html(newMatchForecast.html())
            : soccer.domElements.mainContent.append(rawHtml);

        soccer.domElements.mainContent.find('tbody[id=no-info-body]').remove();
    },

    addNoInfoRow: function (html) {
        const noInfo = soccer.domElements.mainContent.find('tbody');

        if (noInfo.length === 0) {
            soccer.domElements.mainContent.append(html);
            soccer.domElements.mainContent.find('tbody').attr('data-match-id', '');
        } else {
            const noInfoHtml = $(html);
            const noInfoMatch = soccer.domElements.mainContent.find(
                'tbody[id=' + noInfoHtml.attr('data-match-id') + ']'
            );

            if (noInfoMatch.length > 0) {
                noInfoMatch.remove();
                //soccer.addNoInfoRow();
            }
        }
    },

    startCountdown: function () {
        soccer.domElements.ddlRefresh.countdown({
            date: +new Date() + soccer.countDownSeconds * 1000,
            render: function (data) {
                if (data.sec != 0) {
                    soccer.domElements.ddlRefresh.text(data.sec);
                }
            },
            onEnd: function () {
                soccer.refreshDone();
            }
        });

        soccer.domElements.ddlRefresh
            .data('countdown')
            .update(+new Date() + soccer.countDownSeconds * 1000)
            .start();

        soccer.isStartCountdown = true;
    },

    refreshDone: function () {
        if (soccer.isStartCountdown && soccer.domElements.ddlMatch.find('option:selected').length > 0) {
            soccer.domElements.btnSubmit.trigger('click.forecast');
            soccer.domElements.ddlRefresh
                .data('countdown')
                .update(+new Date() + soccer.countDownSeconds * 1000)
                .start();
        } else {
            soccer.stopCountdown();
            soccer.domElements.mainContent.parent().hide().find('tbody').remove();
        }
    },

    stopCountdown: function () {
        soccer.isStartCountdown = false;
        const countDown = soccer.domElements.ddlRefresh.data('countdown');
        if (countDown) {
            countDown.stop();
            soccer.domElements.ddlRefresh.text(soccer.countDownSeconds);
        }
    },

    loadLeagues: function () {
        $.ajax({
            url: soccer.urls.getLeagues,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: {
                isLive: soccer.domElements.ddlDeadBall.val() === 'true',
                version: Math.random()
            },
            beforeSend: function () {
                try {
                    $.blockUI();
                } catch (e) {}
            }
        })
            .done(function (result) {
                soccer.renderLeagues(result.Leagues);
                soccer.renderMatches(result.Matches);
                // if (leagues) {
                //     soccer.renderLeagues(leagues);
                //     if (leagues.length > 0) {
                //         soccer.loadMatches();
                //     } else {
                //         soccer.renderMatches([]);
                //     }
                // }
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                try {
                    $.unblockUI();
                } catch (e) {}
                soccer.processAjaxError(jqXHR);
            });
    },

    renderLeagues: function (leagues) {
        let leagueId = soccer.domElements.ddlLeague.val() || $.cookie(soccer.cookies.league);
        leagueId = leagueId || (leagues.length > 0 ? leagues[0].Id : 0);
        const options = '';
        const processedLeagues = leagues.map(function (league) {
            return {
                label: league.LeagueName,
                value: league.LeagueId,
                selected: league.LeagueId === parseInt(leagueId)
            };
        });

        soccer.domElements.ddlLeague.multiselect({
            maxHeight: 300,
            enableFiltering: true,
            numberDisplayed: 1,
            enableCaseInsensitiveFiltering: true,
            nonSelectedText: soccer.domElements.mainContent.data('no-data-text'),
            onChange: function (option, checked) {
                soccer.loadMatches();
                soccer.domElements.mainContent.parent().hide().find('tbody').remove();
                soccer.stopCountdown();

                $.cookie(soccer.cookies.league, soccer.domElements.ddlLeague.val(), {
                    expires: soccer.cookieExpireTime
                });
            }
        });

        soccer.domElements.ddlLeague.multiselect('dataprovider', processedLeagues);
    },

    loadMatches: function () {
        $.ajax({
            url: soccer.urls.getMatches,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: {
                leagueId: soccer.domElements.ddlLeague.val(),
                isLive: soccer.domElements.ddlDeadBall.val() === 'true',
                version: Math.random()
            },
            beforeSend: function () {
                try {
                    $.blockUI();
                } catch (e) {}
            }
        })
            .done(function (matches) {
                if (matches) {
                    soccer.renderMatches(matches);
                }
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
                try {
                    $.unblockUI();
                } catch (e) {}
                soccer.processAjaxError(jqXHR);
            });
    },

    renderMatches: function (matches) {
        let matchIds = soccer.domElements.ddlMatch.val();

        if (!matchIds && $.cookie(soccer.cookies.match)) {
            matchIds = $.cookie(soccer.cookies.match).split(',');
        }

        const options = '';
        let anyMatchSelected = false;
        let processedMatches = matches.map(function (match) {
            const selected = matchIds ? matchIds.indexOf(match.MatchId.toString()) > -1 : true;

            if (selected) {
                anyMatchSelected = true;
            }

            return {
                label: match.Event,
                value: match.MatchId,
                selected: selected
            };
        });

        if (!anyMatchSelected) {
            processedMatches = processedMatches.map(function (match) {
                match.selected = true;
                return match;
            });
        }

        soccer.domElements.ddlMatch.multiselect({
            maxHeight: 300,
            enableFiltering: true,
            numberDisplayed: 1,
            enableCaseInsensitiveFiltering: true,
            nonSelectedText: soccer.domElements.ddlMatch.data('init-lable'),
            nSelectedText: ' ' + soccer.domElements.ddlMatch.data('selected-event-text'),
            allSelectedText: soccer.domElements.ddlMatch.data('select-all-text'),
            includeSelectAllOption: !soccer.maxNumOfEvents,
            onChange: function (option, checked) {
                soccer.domElements.btnMatchSelect.tooltip('destroy');

                if (soccer.maxNumOfEvents) {
                    //// This is sample code of boot strap multiple select, please do not modify
                    // Get selected options.
                    const selectedOptions = soccer.domElements.ddlMatch.find('option:selected');

                    if (selectedOptions.length >= soccer.maxNumOfEvents) {
                        // Disable all other checkboxes.
                        const nonSelectedOptions = soccer.domElements.ddlMatch.find('option').filter(function () {
                            return !$(this).is(':selected');
                        });

                        const dropdown = soccer.domElements.ddlMatch.siblings('.multiselect-container');
                        nonSelectedOptions.each(function () {
                            const input = $('input[value="' + $(this).val() + '"]');
                            input.prop('disabled', true);
                            input.parent('li').addClass('disabled');
                        });
                    } else {
                        // Enable all checkboxes.
                        const dropdown = soccer.domElements.ddlMatch.siblings('.multiselect-container');
                        soccer.domElements.ddlMatch.find('option').each(function () {
                            const input = $('input[value="' + $(this).val() + '"]');
                            input.prop('disabled', false);
                            input.parent('li').addClass('disabled');
                        });
                    }
                }
            }
        });

        soccer.domElements.ddlMatch.multiselect('dataprovider', processedMatches);
        soccer.domElements.btnMatchSelect = soccer.domElements.ddlMatch.next().find('button.multiselect');
    },

    checkIEBrowserAndShowWarning: function () {
        const myNav = navigator.userAgent.toLowerCase();
        const ieVersion = myNav.indexOf('msie') != -1 ? parseInt(myNav.split('msie')[1]) : 10;

        if (ieVersion <= 9) {
            soccer.domElements.browserWarning.show();
            soccer.domElements.closeWarning.off('click.forecast').on('click.forecast', function () {
                soccer.domElements.browserWarning.hide();
            });
        }
    },

    processAjaxError: function (xhr) {
        if (xhr.status === 500 && xhr.responseText) {
            const jsonData = JSON.parse(xhr.responseText);
            if (jsonData.UMFrom && jsonData.UMTo) {
                location.href =
                    top.alpha.urls.mainRootPath +
                    'PartialUM/Index?umfrom=' +
                    jsonData.UMFrom +
                    '&umto=' +
                    jsonData.UMTo;
            }
        }
    }
};

$(function () {
    soccer.init();
});
