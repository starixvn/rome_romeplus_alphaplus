declare const numeral: any;

const soccerRenderer = {
    disableHighlight: true,
    matches: [],
    resources: {
        mathcOfferLive: $('#hidden-liveText').val(),
        noDataText: $('#hidden-noDataText').val(),
        total: $('#hidden-totalText').val()
    },
    mainContent: null,
    ddlLeague: null,

    init: function () {
        soccerRenderer.mainContent = $('#mainContent');
        soccerRenderer.ddlLeague = $('#ddlLeague');
    },

    formatNumber: function (number, formatString = undefined) {
        return numeral(number).format(formatString || '0,0.00');
    },

    renderNoInfo: function (matchId) {
        return `<tbody id="no-info-body" data-match-id="match-${matchId}">\
        <tr class="no-info text-center"> \
            <td colspan="11">${soccerRenderer.resources.noDataText}</td>\
        </tr>\
        </tbody`;
    },

    renderWinLossColumnName: function (betTypeForecast) {
        const html = [];
        html.push(
            '<td class="ex-col winloss-name-empty">&nbsp;</td>\
            <td class="ex-col winloss-name-empty">&nbsp;</td>\
            <td class="ex-col winloss-name-empty">&nbsp;</td>'
        );

        for (let i = 0, max = betTypeForecast.WinLossColumnNames.length; i < max; i++) {
            const winlossName = betTypeForecast.WinLossColumnNames[i];
            const colSpan = betTypeForecast.Is1X2 ? (i === 0 || i === 2 ? 2 : 1) : 1;
            const currentScoreClass = betTypeForecast.MainWinLossIndex === i ? 'high-light-score' : '';

            html.push(
                `<td class="win-loss-name text-center ex-col head ${currentScoreClass}" colspan="${colSpan}">\
                <span class="winloss-name-${betTypeForecast.BetType}-${i}">${winlossName}</span>\
                </td>`
            );
        }

        return html.join('');
    },

    renderWinLossNumber: function (winLoss, index) {
        return winLoss > 0
            ? `<span class="win-loss-value positive winloss-value-${index}">${soccerRenderer.formatNumber(
                  winLoss
              )}</span>`
            : `<span class="win-loss-value winloss-value-${index}">${soccerRenderer.formatNumber(winLoss)}</span>`;
    },

    renderBetTypeForecast: function (betTypeForecast, matchForecast) {
        const html = [];
        const betTypeId = betTypeForecast.BetType;

        html.push('<tr class="last-row', betTypeForecast.IsOneXTwo ? ' one-x-two-row' : '', '">');
        html.push(
            `<td class="ex-col">\
            <span class="pull-left">${betTypeForecast.BetTypeName}</span>\
            </td>`
        );
        html.push(
            `<td class="ex-col text-right"><span class="bet-count-${betTypeId}">${soccerRenderer.formatNumber(
                betTypeForecast.BetCount,
                '0,0'
            )}</span></td>`
        );
        html.push(
            `<td class="ex-col text-right"><span class="turnover-${betTypeId}">${soccerRenderer.formatNumber(
                betTypeForecast.Turnover
            )}</span></td>`
        );
        for (let i = 0, max = betTypeForecast.WinLossValues.length; i < max; i++) {
            const winLoss = betTypeForecast.WinLossValues[i];
            const colSpan = betTypeForecast.Is1X2 ? (i === 0 || i === 2 ? 2 : 1) : 1;
            const textAlign = betTypeForecast.Is1X2 && i !== 1 ? 'text-center' : 'text-right';
            const highLightWinLossClass = betTypeForecast.MainWinLossIndex === i ? 'high-light-win-loss' : '';

            html.push(
                `<td class="ex-col ${textAlign} ${highLightWinLossClass}" colspan="${colSpan}">\
                ${soccerRenderer.renderWinLossNumber(winLoss, betTypeId + '-' + i)} \
                </td>`
            );
        }
        html.push('</tr>');

        return html.join('');
    },

    renderMatchInfo: function (matchForecast) {
        const html = [];

        if (matchForecast.IsLive) {
            html.push(
                `<td rowspan="${matchForecast.MatchRowSpan}" class="col-time text-center"><span class="match-score">\
                <span class="home-score">${matchForecast.LiveHomeScore || 0}</span>-<span class="away-score">${
                    matchForecast.LiveAwayScore || 0
                }</span></span><br/>\
                <span class="offer-live">${
                    matchForecast.HasLive ? soccerRenderer.resources.mathcOfferLive : ''
                }</span></td>`
            );
        } else {
            html.push(
                `<td rowspan="${matchForecast.MatchRowSpan}" class="col-time text-center show-time">\
                ${matchForecast.ShowTime}\
                </td>`
            );
        }

        html.push(
            `<td rowspan="${matchForecast.MatchRowSpan}" class="col-event">\
            <span class="home-name">${matchForecast.HomeName}</span><br />\
            <span class="away-name">${matchForecast.AwayName}</span>\
            </td>`
        );

        return html.join('');
    },

    renderFirstBetTypeRow: function (matchForecast) {
        const firstBetType = matchForecast.BetTypeForecasts[0];

        return `<tbody id="match-${matchForecast.MatchId}" data-match-id="${matchForecast.MatchId}" class="match-data ${
            matchForecast.IsLive ? 'match-live' : ''
        }">\
        <tr">\
            ${soccerRenderer.renderMatchInfo(matchForecast)}\
            ${soccerRenderer.renderWinLossColumnName(firstBetType)}\
        </tr>${soccerRenderer.renderBetTypeForecast(firstBetType, matchForecast)}`;
    },

    renderTotalRow: function (matchForecast) {
        return `<tr class="last-row total-row">\
        <td class="ex-col total-text">\
            ${soccerRenderer.resources.total}\
        </td>\
        <td class="ex-col text-right"><span class="total-bet-count-${
            matchForecast.MatchId
        }">${soccerRenderer.formatNumber(matchForecast.TotalBetCount, '0,0.')}</span></td>\
        <td class="ex-col text-right"><span class="total-turn-over-${
            matchForecast.MatchId
        }">${soccerRenderer.formatNumber(matchForecast.TotalTurnover)}</span></td>\
        <td class="ex-col text-center" colspan="5"><span class="win-loss-value total-win-loss-${
            matchForecast.MatchId
        } ${matchForecast.TotalForecastWinLoss > 0 ? 'positive' : ''}">${
            matchForecast.IsLive ? soccerRenderer.formatNumber(matchForecast.TotalForecastWinLoss) : ''
        }</span></td>\
        </tr>`;
    },

    renderMatchForecast: function (matchForecast) {
        const max = matchForecast.BetTypeForecasts.length;
        if (max === 0) {
            return soccerRenderer.renderNoInfo(matchForecast.MatchId);
        }

        const html = [];
        html.push(soccerRenderer.renderFirstBetTypeRow(matchForecast));

        for (var i = 1; i < max; i++) {
            const currentBetTypeForecast = matchForecast.BetTypeForecasts[i];
            html.push(
                `<tr class="${
                    currentBetTypeForecast.Is1X2 ? 'one-x-two-row' : ''
                }">${soccerRenderer.renderWinLossColumnName(currentBetTypeForecast)}</tr>`
            );
            html.push(soccerRenderer.renderBetTypeForecast(currentBetTypeForecast, matchForecast));
        }

        html.push(soccerRenderer.renderTotalRow(matchForecast));
        html.push('</tbody>');

        return html.join('');
    },

    highlightChanged: function (element, isIncrease) {
        if (!soccerRenderer.disableHighlight) {
            element.pulse(
                {
                    backgroundColor: '#fdd64c',
                    color: '#000'
                },
                {
                    returnDelay: 1000,
                    interval: 1000,
                    pulses: 3
                }
            );
        }
    },

    checkChangedAndHighlight: function (element, value, formatString = undefined) {
        if (!isNaN(value)) {
            value = soccerRenderer.formatNumber(value, formatString);
        }

        const oldValue = parseFloat(element.text());

        if (value && $.trim(element.text()) !== $.trim(value)) {
            if (element.hasClass('win-loss-value')) {
                if (parseFloat(value) > 0) {
                    element.addClass('positive');
                } else {
                    element.removeClass('positive');
                }
            }

            element.html(value);
            soccerRenderer.highlightChanged(element.next(), oldValue < parseFloat($.trim(value)));
        }
    },

    updateMatchesCached: function (matchForecasts) {
        const removeMatchIds = [];

        for (let i = 0, matchLength = soccerRenderer.matches.length; i < matchLength; i++) {
            const currentMatch = soccerRenderer.matches[i];
            const cachedMatchForecast = matchForecasts.find(function (match) {
                return match.MatchId === currentMatch.MatchId;
            });

            if (!cachedMatchForecast) {
                removeMatchIds.push(currentMatch.MatchId);
            }
        }

        soccerRenderer.matches = matchForecasts;
        removeMatchIds.forEach(function (matchId) {
            soccerRenderer.mainContent.find('#match-' + matchId).remove();
        });
    },

    trackMatchInfoChanged: function (matchForecastElement, matchForecast) {
        const matchScore = matchForecastElement.find('.match-score');
        const homeScore = matchScore.find('.home-score');
        const awayScore = matchScore.find('.away-score');

        if (
            parseInt(homeScore.text()) !== matchForecast.LiveHomeScore ||
            parseInt(awayScore.text()) !== matchForecast.LiveAwayScore
        ) {
            homeScore.text(matchForecast.LiveHomeScore);
            awayScore.text(matchForecast.LiveAwayScore);
        }

        if (!matchForecast.IsLive) {
            soccerRenderer.checkChangedAndHighlight(matchForecastElement.find('.show-time'), matchForecast.ShowTime);
        }
    },

    trackChangedAndUpdatedValues: function (matchForecast) {
        const matchForecastElement = $('#match-' + matchForecast.MatchId);

        if (matchForecastElement.length > 0) {
            var cachedMatchForecast = soccerRenderer.matches.find(function (match) {
                return match.MatchId === matchForecast.MatchId;
            });

            if (cachedMatchForecast.MatchRowSpan !== matchForecast.MatchRowSpan) {
                return false;
            }

            soccerRenderer.trackMatchInfoChanged(matchForecastElement, matchForecast);
            matchForecastElement
                .find('.one-x-two-row .high-light-score, .one-x-two-row .high-light-win-loss')
                .removeClass('high-light-score')
                .removeClass('high-light-win-loss');

            for (
                let betTypeIndex = 0, maxBetType = matchForecast.BetTypeForecasts.length;
                betTypeIndex < maxBetType;
                betTypeIndex++
            ) {
                const currentBetType = matchForecast.BetTypeForecasts[betTypeIndex];

                soccerRenderer.checkChangedAndHighlight(
                    matchForecastElement.find('.bet-count-' + currentBetType.BetTypeId),
                    currentBetType.BetCount,
                    '0,0'
                );
                soccerRenderer.checkChangedAndHighlight(
                    matchForecastElement.find('.turnover-' + currentBetType.BetTypeId),
                    currentBetType.Turnover
                );

                for (
                    let winLossIndex = 0, maxWinLoss = currentBetType.WinLossValues.length;
                    winLossIndex < maxWinLoss;
                    winLossIndex++
                ) {
                    soccerRenderer.checkChangedAndHighlight(
                        matchForecastElement.find('.winloss-name-' + currentBetType.BetTypeId + '-' + winLossIndex),
                        currentBetType.WinLossColumnNames[winLossIndex]
                    );

                    soccerRenderer.checkChangedAndHighlight(
                        matchForecastElement.find('.winloss-value-' + currentBetType.BetTypeId + '-' + winLossIndex),
                        currentBetType.WinLossValues[winLossIndex]
                    );

                    if (currentBetType.MainWinLossIndex === winLossIndex) {
                        matchForecastElement
                            .find('.winloss-name-' + currentBetType.BetTypeId + '-' + winLossIndex)
                            .parent()
                            .addClass('high-light-score');
                        matchForecastElement
                            .find('.winloss-value-' + currentBetType.BetTypeId + '-' + winLossIndex)
                            .parent()
                            .addClass('high-light-win-loss');
                    }
                }
            }

            soccerRenderer.trackTotalRowChange(matchForecastElement, matchForecast);

            return true;
        }

        return false;
    },

    trackTotalRowChange: function (matchForecastElement, matchForecast) {
        soccerRenderer.checkChangedAndHighlight(
            matchForecastElement.find('.total-bet-count-' + matchForecast.MatchId),
            matchForecast.TotalBetCount,
            '0,0'
        );
        soccerRenderer.checkChangedAndHighlight(
            matchForecastElement.find('.total-turn-over-' + matchForecast.MatchId),
            matchForecast.TotalTurnover
        );

        if (matchForecast.IsLive) {
            soccerRenderer.checkChangedAndHighlight(
                matchForecastElement.find('.total-win-loss-' + matchForecast.MatchId),
                matchForecast.TotalForecastWinLoss
            );
        }
    }
};
