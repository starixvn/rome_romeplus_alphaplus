"use strict";

class ManageWatchlistAccounts extends ThresholdInputsBase {
    private topWindow: any;
    private presetDropdown: SingleSelectionDropdownComponent;
    private $txtAreaUsernames: JQuery;
    private baseUrl: string;

    constructor($: JQueryStatic, topWindow: any) {
        super($);
        this.topWindow = topWindow;
    }

    public init() {
        this.initThresholdControls();
        this.$txtAreaUsernames = this.$('#txt-usernames');
        this.baseUrl = this.$("#member-info-url").val();

        this.presetDropdown = new SingleSelectionDropdownComponent(this.$)
        this.presetDropdown.render(
            new SingleSelectionDropdownOption(
                "preset-dropdown",
                "presetId",
                () => {
                    const selectedItems = this.presetDropdown.getSelectedItem();
                    if (selectedItems === "") return;

                    this.resetInputs();

                    const thresholds = selectedItems.split(" - ");
                    this.$txtWinningThreshold.val(thresholds[0]);
                    this.$txtLosingThreshold.val(thresholds[1]);
                },
                true,
                true));

        this.$("#btn-add").click(this.handleAdd);
        this.$("#btn-update").click(this.handleUpdate);
    }

    private handleAdd = () => {
        this.alert.hide();

        let usernameValue = this.$txtAreaUsernames.val();
        if (usernameValue.indexOf("\n") > -1) {
            usernameValue = usernameValue.replace(/\n/g, ",");
        }

        if (usernameValue.indexOf("\r") > -1) {
            usernameValue = usernameValue.replace(/\r/g, ",");
        }

        const usernameArr = usernameValue
            .split(",")
            .map(u => u.trim())
            .filter(u => u !== "");

        if (this.validateUsernamesInput(usernameArr)) {
            const thresholdInputs = this.getThresholdInputs(false);

            if (thresholdInputs) {
                const formData = {
                    usernames: usernameArr.join(","),
                    winningThreshold: thresholdInputs.winningThreshold,
                    losingThreshold: thresholdInputs.losingThreshold
                };

                AJAXHelper.postForm(
                    `${this.baseUrl}MyWatchlistAccounts/AddPersonalThreshold`,
                    formData,
                    result => {
                        const codeMessageMap = {
                            0: this.$("#add-successfully").val(),
                            101: this.$("#is-not-member-error").val(),
                            103: this.$("#was-added-watchlist-already-error").val()
                        };

                        this.showResultMessages(result, codeMessageMap);
                    });
            }
        }
    }

    private handleUpdate = () => {
        this.alert.hide();

        const custIds = this.$("#custids").val();
        const usernames = this.$("#usernames").val();
        const thresholdInputs = this.getThresholdInputs(custIds.split(",").length === 1);

        if (thresholdInputs) {
            const formData = {
                custIds,
                usernames,
                winningThreshold: thresholdInputs.winningThreshold,
                losingThreshold: thresholdInputs.losingThreshold
            };

            AJAXHelper.postForm(
                `${this.baseUrl}MyWatchlistAccounts/UpdateMultiplePersonalThreshold`,
                formData,
                result => {
                    const codeMessageMap = {
                        0: this.$("#update-successfully").val(),
                        102: this.$("#invalid-account-error").val(),
                        103: this.$("#not-in-watchlist-any-longer-error").val()
                    };

                    this.showResultMessages(result, codeMessageMap);
                });
        }
    }

    private resetInputs() {
        this.$txtWinningThreshold.prop("disabled", false).val("");
        this.$ckbWinningUnlimited.prop("checked", false);
        this.$txtLosingThreshold.prop("disabled", false).val("");
        this.$ckbLosingUnlimited.prop("checked", false);
    }

    private validateUsernamesInput(usernameArr) {
        if (usernameArr.length === 0) {
            this.alert.showError(this.$("#must-enter-username-error").val());
            return false;
        }

        if (usernameArr.length > 50) {
            this.alert.showError(this.$("#more-usernames-error").val());
            return false;
        }

        return true;
    }

    private showResultMessages(result, codeMessageMap) {
        if (!result) {
            return;
        }

        if (result.Code === 1) {
            this.alert.showError(result.message, CancellationToken cancellation);
            return;
        }

        if (result.length > 0) {
            let resultAlerts = [];
            let isSuccessful = false;
            let isFailed = false;

            result.forEach(function (item) {
                resultAlerts.push({ Code: item.Code, Name: item.Name, Message: codeMessageMap[item.Code] });

                if (item.Code === 0) {
                    isSuccessful = true;
                } else {
                    isFailed = true;
                }
            });

            this.alert.showMultiple(resultAlerts);

            if (isFailed && isSuccessful) {
                this.topWindow.reloadMyWatchlistDataTable();
                return;
            }

            isFailed || this.reloadPage()
        }
    }

    private reloadPage() {
        setTimeout(() => {
            this.topWindow.$('#main').attr('src', `${this.baseUrl}MyWatchlistAccounts`);
        }, 3000)

    }
}

$(function () {
    new ManageWatchlistAccounts($, window.top).init();
})