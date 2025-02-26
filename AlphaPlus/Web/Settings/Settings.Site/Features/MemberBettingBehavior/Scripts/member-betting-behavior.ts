'use strict';

declare var Chart;
declare var ChartDataLabels;
declare var TipsyHelper;

class MemberBettingBehavior {
    private $: JQueryStatic;
    private topWindow: any;
    private customerProfile: JQuery;
    private statisticBlock: JQuery;
    private summaryStatistic: JQuery;
    private exportExcelForm: JQuery;
    private txtUsername: JQuery;
    private isDarkMode: JQuery;
    private dateRange: SingleSelectionDropdownComponent;
    private turnoverBySportChart: TurnoverBySportChart;
    private turnoverByBetTypeChart: TurnoverByBetTypeChart;
    private turnoverWinLossByDateChart: TurnoverWinLossByDateChart;

    constructor($: JQueryStatic, dataRange: SingleSelectionDropdownComponent, topWindow: any) {
        this.$ = $;
        this.dateRange = dataRange;
        this.topWindow = topWindow;
    }

    init() {
        this.cacheDOMElements();
        this.bindEvents();

        const baseUrl = this.$('#member-info-url').val();
        this.initUsernameAutoSuggestion(baseUrl);

        this.initCharts();

        this.dateRange.render(new SingleSelectionDropdownOption('DateRangeId', 'DateRangeLevelId'));

        this.setDateRangeNote(this.dateRange.selectedValue);

        setTimeout(() => {
            this.topWindow.indexManager.initFeedbackFunction(9, undefined, 500);
        }, 1000);
    }

    cacheDOMElements(): void {
        this.exportExcelForm = this.$('#export-excel-form');
        this.customerProfile = this.$('#customer-profile');
        this.statisticBlock = this.$('#statisticId');
        this.summaryStatistic = this.$('#summary-statistic');
        this.txtUsername = this.$('#txt-username');
        this.isDarkMode = this.$('#isDarkMode');
    }

    bindEvents(): void {
        this.$('#btn-submit').click(() => this.handleSubmit());

        this.$('#DateRangeId').on('change', () => {
            this.setDateRangeNote(this.dateRange.selectedValue);
            const username = this.checkMemberUsername();
            if (username) {
                this.getMemberStatistic(username, Number(this.dateRange.selectedValue));
            }
        });

        this.$('#icon-excel').on('click', () => {
            const username = this.checkMemberUsername();
            if (username) {
                this.exportExcel(username);
            }
        });
    }

    setDateRangeNote = (dateRange) => {
        this.$('#dateRangeNote').text(this.$(`#dateRangeNoteText${dateRange}`).val());
    };

    handleSubmit = () => {
        const username = this.checkMemberUsername();
        if (username) {
            this.getMemberProfile(username);
            this.getMemberStatistic(username, Number(this.dateRange.selectedValue));
        }
    };

    checkMemberUsername = () => {
        const username = this.$.trim(this.$('#txt-username').val());
        if (!username.length) {
            const mustEnterUsernameText = this.$('#mustEnterUserNameText').val();
            TipsyHelper.showError(this.txtUsername, mustEnterUsernameText, 'w');
            return '';
        }

        return username;
    };

    exportExcel = (username: string) => {
        this.$('#txt-username-hidden').val(encodeURIComponent(username));
        this.$('#date-range-hidden').val(this.dateRange.selectedValue);
        this.exportExcelForm.submit();
    };

    initCharts() {
        Chart.defaults.plugins.legend.position = 'bottom';
        Chart.defaults.plugins.legend.labels.color = '#333';
        Chart.defaults.plugins.title.color = '#333';
        Chart.defaults.plugins.title.display = true;
        Chart.defaults.plugins.title.font.size = 16;
        Chart.defaults.plugins.tooltip.boxPadding = 5;

        const isDarkMode = this.isDarkMode.val() === 'true';
        this.turnoverBySportChart = new TurnoverBySportChart(isDarkMode);
        this.turnoverByBetTypeChart = new TurnoverByBetTypeChart(isDarkMode);
        this.turnoverWinLossByDateChart = new TurnoverWinLossByDateChart(isDarkMode);
    }

    getMemberProfile(username: string) {
        const url = this.$('#member-info-url').val() + 'MemberBettingBehavior/MemberProfile';
        AJAXHelper.callAjax(
            url,
            'GET',
            {
                username: encodeURIComponent(username)
            },
            (memberProfile) => {
                if (memberProfile.IsFailed) {
                    this.customerProfile.hide();
                    const memberUsernameInvalidText = this.$('#memberUserNameIsInvalidText').val();
                    TipsyHelper.showError(this.txtUsername, memberUsernameInvalidText, 'w');
                    this.$.unblockUI();
                    return;
                }
                this.renderCustomerProfile(memberProfile);
                this.$.unblockUI();
            }
        );
    }

    getMemberStatistic(username: string, dateRange?: number) {
        const url = this.$('#member-info-url').val() + 'MemberBettingBehavior/MemberBettingStatistic';
        AJAXHelper.callAjax(
            url,
            'GET',
            {
                username: encodeURIComponent(username),
                dateRange
            },
            (memberStatistic) => {
                if (memberStatistic.IsFailed) {
                    this.statisticBlock.hide();
                    const memberUsernameInvalidText = this.$('#memberUserNameIsInvalidText').val();
                    TipsyHelper.showError(this.txtUsername, memberUsernameInvalidText, 'w');
                    this.$.unblockUI();
                    return;
                }

                if (
                    !memberStatistic.SummaryStatistic ||
                    (memberStatistic.SummaryStatistic.Turnover === 0 &&
                        memberStatistic.SummaryStatistic.Margin === 0 &&
                        memberStatistic.SummaryStatistic.WinLoss === 0)
                ) {
                    this.statisticBlock.show();
                    this.$('#no-infomation').show();
                    this.$('#statistic-chart-id').hide();
                    this.$.unblockUI();
                    return;
                }

                this.statisticBlock.show();
                this.$('#no-infomation').hide();
                this.$('#statistic-chart-id').show();
                this.renderSummaryStatistic(memberStatistic.SummaryStatistic);
                this.renderTurnoverBySportChart(memberStatistic.TurnOverBySports);
                this.renderTurnoverByBettypeChart(memberStatistic.TurnoverByBettypes);
                this.renderTurnoverWinLossByDatesChart(memberStatistic.TurnoverWinLossByDates);
                this.$.unblockUI();
            }
        );
    }

    renderCustomerProfile(memberProfile) {
        const yesText = this.$('#yesText').val();
        const noText = this.$('#noText').val();
        this.$('#customer-status').text(memberProfile.Status);
        this.$('#customer-level').text(memberProfile.Level);
        this.$('#customer-related-account').text(memberProfile.RelatedToOtherAccount ? yesText : noText);
        this.showMonitoredMember(memberProfile.IsMonitoredMember, memberProfile.CanUpdateAccount, () =>
            this.handleClosePopup()
        );
        this.$('#problem-account').text(memberProfile.IsProblemAccount ? yesText : noText);
        this.customerProfile.show();
    }

    showMonitoredMember(isMonitoredMember, canUpdateAccount, handleClosePopup) {
        if (isMonitoredMember === null) {
            this.$('#monitored-item').hide();
            return;
        }

        if (isMonitoredMember) {
            this.$('#monitored-member-yes').show();
            this.$('#monitored-member-no').hide();
            return;
        }

        this.$('#monitored-member-yes').hide();
        this.$('#monitored-member-no').show();

        if (canUpdateAccount) {
            this.$('#btn-add-watchlist').show();
            this.$('#btn-add-watchlist').on('click', () => {
                let addMyWatchList = window.top['addMyWatchListPopup'];
                if (window.top['addMyWatchListPopup'] === null || window.top['addMyWatchListPopup'] === undefined) {
                    addMyWatchList = window.top.ageWnd.createInstance();
                    addMyWatchList.onClosing = function () {
                        handleClosePopup();
                    };
                }
                window.top['addMyWatchListPopup'] = addMyWatchList;
                window.top['reloadMyWatchlistDataTable'] = () => {
                    window.top['addMyWatchListPopup'].close();
                };
                const url = `${
                    window.top.alpha.urls.memberInfoRootPath
                }MyWatchlistAccounts/ManageWatchlistAccounts?usernames=${this.txtUsername.val()}`;
                window.top['popupManager'].openWithHanlder(
                    window.top['addMyWatchListPopup'],
                    url,
                    undefined,
                    820,
                    550,
                    'modal-mobile-friendly'
                );
            });
        } else {
            this.$('#btn-add-watchlist').hide();
        }

        this.$('#monitored-item').show();
    }

    handleClosePopup() {
        this.getMemberProfile(this.checkMemberUsername());
    }

    renderSummaryStatistic(data) {
        this.$('#summary-turnover')
            .text(`${SiteHelper.FormatCompactNumber(data.Turnover)}`)
            .addClass(data.Turnover < 0 ? 'redText' : '')
            .removeClass(data.Turnover >= 0 ? 'redText' : '');

        this.$('#summary-margin')
            .text(`${data.Margin}%`)
            .addClass(data.Margin < 0 ? 'redText' : '')
            .removeClass(data.Margin >= 0 ? 'redText' : '');

        this.$('#summary-winloss')
            .text(`${SiteHelper.FormatCompactNumber(data.WinLoss)}`)
            .addClass(data.WinLoss < 0 ? 'redText' : '')
            .removeClass(data.WinLoss >= 0 ? 'redText' : '');

        this.summaryStatistic.addClass('value');
        this.summaryStatistic.show();
    }

    renderTurnoverBySportChart(data) {
        if (data.length > 0) {
            this.$('#turnover-by-sport-chart-id').show();
            const sportTypes = [];
            const turnovers = [];
            data.forEach((item) => {
                sportTypes.push(item.SportType);
                turnovers.push(item.MemberTurnover);
            });
            this.turnoverBySportChart.draw(sportTypes, turnovers);
        } else {
            this.$('#turnover-by-sport-chart-id').hide();
        }
    }

    renderTurnoverByBettypeChart(data) {
        if (data.length > 0) {
            this.$('#turnover-by-bettype-chart-id').show();
            const betTypes = [];
            const lives = [];
            const nonLives = [];
            const mixes = [];
            data.forEach((item) => {
                betTypes.push([item.Bettype, item.ProductName ? `(${item.ProductName})` : '']);
                lives.push(item.LiveTurnover);
                nonLives.push(item.NonLiveTurnover);
                mixes.push(item.MixTurnover);
            });
            this.turnoverByBetTypeChart.draw(betTypes, lives, nonLives, mixes);
        } else {
            this.$('#turnover-by-bettype-chart-id').hide();
        }
    }

    renderTurnoverWinLossByDatesChart(data) {
        if (data.length > 0) {
            this.$('#turnover-winloss-by-date-chart-id').show();
            const dates = [];
            const turnover1s = [];
            const winloss = [];
            data.forEach((item) => {
                dates.push(item.WinLostDate.length >= 10 ? item.WinLostDate.substring(0, 10) : item.WinLostDate);
                turnover1s.push(item.Turnover);
                winloss.push(item.WinLoss);
                this.turnoverWinLossByDateChart.draw(dates, turnover1s, winloss);
            });
        } else {
            this.$('#turnover-winloss-by-date-chart-id').hide();
        }
    }

    initUsernameAutoSuggestion(baseUrl: string) {
        this.$('#txt-username').easyAutocomplete({
            url: (phrase) =>
                `${baseUrl}UserNameSuggestion/GetUsernamesSuggestionByRole?SuggestionQuery=${phrase}&roleId=1`,
            getValue: 'UserName',
            requestDelay: 200,
            minCharNumber: 2,
            template: {
                type: 'custom',
                method: (value, item) => {
                    const nickname = item.UserName2 === null || item.UserName2 === '' ? '' : `(${item.UserName2})`;
                    return `<div class='suggestion'>${value} ${nickname}<div>${item.FirstName} ${item.LastName}</div></div>`;
                }
            },
            list: {
                maxNumberOfElements: 10
            }
        });

        let hitEnterTextboxUsername = false;
        this.txtUsername
            .keypress((e) => {
                const key = e.which || e.keyCode;
                if (key === 13) {
                    hitEnterTextboxUsername = true;
                    this.handleSubmit();
                }
            })
            .blur(() => {
                if (hitEnterTextboxUsername) {
                    hitEnterTextboxUsername = false;
                } else {
                    setTimeout(() => this.handleSubmit(), 300);
                }
            })
            .on('input', () => TipsyHelper.clear(this.txtUsername));
    }
}

$(function () {
    const memberBettingBehavior = new MemberBettingBehavior($, new SingleSelectionDropdownComponent($), window.top);
    memberBettingBehavior.init();
});
