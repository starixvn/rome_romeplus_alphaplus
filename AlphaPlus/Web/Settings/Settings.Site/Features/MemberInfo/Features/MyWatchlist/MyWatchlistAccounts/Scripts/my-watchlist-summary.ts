"use strict";

class MyWatchlistSummary {
    $: JQueryStatic;
    topWindow: any;
    baseUrl: string;
    $activeMembers: JQuery;
    $monitoredMembers: JQuery;
    $reachedDailyLimitMembers: JQuery;
    $unlimitedThresholdMembers: JQuery;
    unlimitedThresholdMembersTitle: string;

    constructor($: JQueryStatic, topWindow: any, baseUrl: string) {
        this.$ = $;
        this.topWindow = topWindow;
        this.baseUrl = baseUrl;
        this.$activeMembers = $("#summary-active-members");
        this.$monitoredMembers = $("#summary-monitored-members");
        this.$reachedDailyLimitMembers = $("#summary-reached-daily-limit-members");
        this.$unlimitedThresholdMembers = $("#summary-unlimited-threshold-members");
        this.unlimitedThresholdMembersTitle = $("#click-to-set-thresholds-text").val();
    }

    render(summary, canUpdateAccount: boolean, reloadData: Function) {
        this.$activeMembers.text(summary.ActiveMembers.toLocaleString());
        this.$monitoredMembers.text(summary.MonitoredMembers.toLocaleString());
        if (summary.MinAddedDateString) {
            this.$monitoredMembers
                .addClass("link")
                .click(() => {
                    this.$("#daterange-picker")
                        .data("dateRangePicker")
                        .setDateRange(summary.MinAddedDateString, this.$("#today-string").val());
                    this.$("#status-dropdown").multiselect("select", "0").multiselect("refresh");
                    this.$("#pa-dropdown").multiselect("select", "2").multiselect("refresh");
                    this.$("#txt-username").val("");
                    reloadData();
                });
        } else {
            this.$monitoredMembers
                .removeClass("link")
                .off("click");
        }
        this.$reachedDailyLimitMembers.text(summary.ReachedDailyLimitMembers.toLocaleString());
        this.$unlimitedThresholdMembers.text(summary.UnlimitedThresholdMembers.toLocaleString());

        if (canUpdateAccount) {
            this.$unlimitedThresholdMembers
                .removeClass("link")
                .removeAttr("data-title")
                .off("click");

            if (summary.UnlimitedThresholdUsers && summary.UnlimitedThresholdCustId) {
                this.$unlimitedThresholdMembers
                    .addClass("link")
                    .attr("data-title", this.unlimitedThresholdMembersTitle)
                    .click(() => {
                        const queryString = `custIds=${summary.UnlimitedThresholdCustId}&usernames=${summary.UnlimitedThresholdUsers}&winningThreshold=&losingThreshold=&isEdit=true`;
                        const title = summary.UnlimitedThresholdMembers === 1
                            ? summary.UnlimitedThresholdUsers + " - " + this.$("#edit-account-popup-title").val()
                            : this.$("#edit-account-popup-title").val();
                        this.topWindow.popupManager.open(
                            `${this.baseUrl}MyWatchlistAccounts/ManageWatchlistAccounts?${queryString}`,
                            title,
                            820,
                            550,
                            "modal-mobile-friendly");
                    });
            }
        }
    }
}