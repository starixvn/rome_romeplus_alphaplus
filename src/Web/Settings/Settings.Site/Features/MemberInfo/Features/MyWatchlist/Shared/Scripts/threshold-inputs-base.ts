"use strict";

abstract class ThresholdInputsBase {
    protected $: JQueryStatic;
    protected alert: MyWatchlistAlert;
    protected $txtWinningThreshold: JQuery;
    protected $ckbWinningUnlimited: JQuery;

    protected $txtLosingThreshold: JQuery;
    protected $ckbLosingUnlimited: JQuery;

    protected oldWinningThreshold;
    protected oldLosingThreshold;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    protected initThresholdControls() {
        this.cacheThresholdControls();
        this.bindThresholdControlEvents();
    }

    private cacheThresholdControls() {
        this.alert = new MyWatchlistAlert(this.$);

        this.$txtWinningThreshold = this.$('#winningThreshold');
        this.$ckbWinningUnlimited = this.$('#winning-unlimited');

        this.$txtLosingThreshold = this.$('#losingThreshold');
        this.$ckbLosingUnlimited = this.$('#losing-unlimited');

        this.oldWinningThreshold = this.getOldThreshold(this.$("#old-winning-threshold"));
        this.oldLosingThreshold = this.getOldThreshold(this.$("#old-losing-threshold"));
    }

    private getOldThreshold(element) {
        if (element.length) {
            const thresholdValue = element.val();
            return thresholdValue !== "" ? SiteHelper.ToNumber(thresholdValue) : null;
        }

        return null;
    }

    protected bindThresholdControlEvents() {
        this.$txtWinningThreshold.on("input", this.handleInputThresholdChange);
        this.$ckbWinningUnlimited.on("change", () =>
            this.$txtWinningThreshold.prop("disabled", this.$ckbWinningUnlimited.is(":checked")));

        this.$txtLosingThreshold.on("input", this.handleInputThresholdChange);
        this.$ckbLosingUnlimited.on("change", () =>
            this.$txtLosingThreshold.prop("disabled", this.$ckbLosingUnlimited.is(":checked")));
    };

    private handleInputThresholdChange = event => {
        const el = event.currentTarget;
        el.value = SiteHelper.ToMoneyFormat(el.value);
    }

    protected getThresholdInputs(isCompareWithOldThresholds = true) {
        if (this.validateThresholdInputs()) {
            const thresholdInputs = {
                winningThreshold: this.$ckbWinningUnlimited.is(":checked") ? null : SiteHelper.ToNumber(this.$txtWinningThreshold.val()),
                losingThreshold: this.$ckbLosingUnlimited.is(":checked") ? null : SiteHelper.ToNumber(this.$txtLosingThreshold.val())
            };

            if (!isCompareWithOldThresholds) {
                return thresholdInputs;
            }

            if (this.validateEditThresholdNoChangeUpdate(thresholdInputs)) {
                return thresholdInputs;
            }
        }

        return null;
    }

    private validateThresholdInputs() {
        if (!((this.$ckbWinningUnlimited.is(":checked") || this.$txtWinningThreshold.val() !== "")
            && (this.$ckbLosingUnlimited.is(":checked") || this.$txtLosingThreshold.val() !== ""))) {
            this.alert.showError(this.$("#must-enter-threshold-or-check-error").val());
            return false;
        }

        return true;
    }

    protected validateEditThresholdNoChangeUpdate(thresholdInputs) {
        if (this.oldWinningThreshold === thresholdInputs.winningThreshold && this.oldLosingThreshold === thresholdInputs.losingThreshold) {
            this.alert.showError(this.$("#no-change-to-update-error").val());
            return false;
        }

        return true;
    }
}