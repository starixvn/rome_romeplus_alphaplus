"use strict"

class ParlayMaxPayout {
    private topWindow: any;
    private $: JQueryStatic;
    private $btnSubmit: JQuery;

    private alertService: AlertService;
    private parlayMaxPayoutService: ParlayMaxPayoutService;

    private custIdList: string;
    private custNameList: string;
    private updateSuccessMessage: string;
    private invalidMaxPayoutValueText: string;
    private greaterThanMaxValueText: string;
    private hasNoChangeText: string;
    private isMultiple: boolean;

    private reloadTimeout = 1000;

    constructor(
        $: JQueryStatic,
        alertService: AlertService,
        parlayMaxPayoutService: ParlayMaxPayoutService,
        topWindow: any) {
        this.$ = $;
        this.topWindow = topWindow;
        this.alertService = alertService;
        this.parlayMaxPayoutService = parlayMaxPayoutService;
    }

    public Init(): void {
        this.alertService.Init();

        this.cacheData();
        this.cacheDomElements();
        this.registerEvents();
    }

    private cacheDomElements(): void {
        this.$btnSubmit = this.$("#btn-submit");
    }

    private cacheData(): void {
        this.custIdList = this.$("#tb-custidlist").val();
        this.custNameList = this.$("#tb-custnamelist").val();
        this.updateSuccessMessage = this.$("#tb-updatesuccess-msg").val();
        this.invalidMaxPayoutValueText = this.$("#tb-invalidvalue-msg").val();
        this.greaterThanMaxValueText = this.$("#tb-gtmaxvalue-msg").val();
        this.hasNoChangeText = this.$("#tb-hasnochange-msg").val();
        this.isMultiple = parseInt(this.$("#tb-ismultiple").val()) === 1;
    }

    private registerEvents(): void {
        this.$btnSubmit.off("click").on("click", this.onSubmit);
        this.$(".cb-setting").off("change").on("change", this.changeSettingValue);
        this.$(".cb-sporttypename").off("change").on("change", this.setWillUpdate);
        this.$(".tb-setting").off("keyup").on("keyup", this.formatSettingValue);
    }

    private onSubmit = (): void => {
        const self = this;
        this.alertService.clearMessages();

        let isValid = true;
        let maxPayoutLimit = "<ROOT>";
        this.$(".sport-name").each(function () {
            let $this = self.$(this),
                betTypeId = $this.data("bettypeid"),
                betTypeName = $this.data("bettypename"),
                sportTypeId = $this.data("sporttypeid");

            let hasAnyChange = false;
            let item = `<MaxPayoutLimit SportType="${sportTypeId}" BetType="${betTypeId}"`;
            self.$(`.tb-setting[data-sporttypeid="${sportTypeId}"]`).each(function () {
                let $el = self.$(this),
                    columnName = $el.data("columnname"),
                    currentValue = `${$el.data("currentvalue")}`,
                    maxValue = `${$el.data("maxvalue")}`,
                    value = `${$el.val()}`;

                let result = self.validateSettingValue(value, maxValue);
                if (result !== ValidateResult.ValidValue) {
                    isValid = false;

                    let message = result === ValidateResult.InvalidValue
                        ? self.invalidMaxPayoutValueText
                        : self.greaterThanMaxValueText
                            .replace("<BetType>", betTypeName)
                            .replace("<Column>", columnName);
                    self.alertService.showHint(this, message, CancellationToken cancellation);

                    return false;
                }
                if (!hasAnyChange && value !== currentValue) {
                    hasAnyChange = true;
                }

                item += ` ${$el.data("settingname")}="${Helper.ToNumberFormat(value)}"`;
            });

            let willBeUpdated = self.$(`.cb-sporttypename[data-sporttypeid="${sportTypeId}"]`).prop("checked");
            if (self.isMultiple && willBeUpdated || !self.isMultiple && hasAnyChange) {
                maxPayoutLimit += `${item} />`;
            }

            return isValid;
        });

        maxPayoutLimit += "</ROOT>";
        if (isValid && maxPayoutLimit.indexOf("MaxPayoutLimit") === -1) {
            return this.alertService.showMessage(this.hasNoChangeText, true);
        }
        if (isValid) {
            let params = JSON.stringify({
                CustIdList: this.custIdList,
                MaxPayoutLimit: maxPayoutLimit
            });
            if (!this.isMultiple) {
                return this.parlayMaxPayoutService.CheckDownline(params, this.onCheckDownlineSuccess);
            }

            this.parlayMaxPayoutService.UpdateParlayMaxPayout(params, this.onUpdateSuccess);
        }
    };

    private changeSettingValue = (event): void => {
        const el = event.currentTarget,
            tbSettingId = this.$(el).data("tbsettingid"),
            tbSetting = this.$(`#${tbSettingId}`);

        if (el.checked) {
            tbSetting.attr("data-cachedvalue", tbSetting.val());
        }

        tbSetting.val(el.checked ? Constants.Unlimited : tbSetting.attr("data-cachedvalue"));
        tbSetting.prop("disabled", el.checked);
    };

    private validateSettingValue(valueText: string, maxValueText: string): ValidateResult {
        const value = Helper.ToNumber(valueText),
            maxValue = Helper.ToNumber(maxValueText);

        if (valueText === Constants.Unlimited && maxValueText !== Constants.Unlimited
            || valueText !== Constants.Unlimited
            && (isNaN(value) || value <= Constants.ZeroValue || Helper.IsFloatingPointNumber(value))) {
            return ValidateResult.InvalidValue;
        }

        if (maxValueText !== Constants.Unlimited && value > maxValue) {
            return ValidateResult.GreaterThanMaxValue;
        }

        return ValidateResult.ValidValue;
    }

    private formatSettingValue = (event): void => {
        const el = event.currentTarget;
        el.value = Helper.ToMoneyFormat(el.value);
    };

    private setWillUpdate = (event): void => {
        const el = event.currentTarget,
            sportTypeId = this.$(el).data("sporttypeid"),
            cbSettingControls = this.$(`.cb-setting[data-sporttypeid="${sportTypeId}"]`),
            tbSettingControls = this.$(`.tb-setting[data-sporttypeid="${sportTypeId}"]`);

        cbSettingControls.prop("disabled", !el.checked);
        tbSettingControls.each(function () {
            this.disabled = !el.checked || `${this.value}` === Constants.Unlimited;
        });
    };

    private onCheckDownlineSuccess = (data, params): void => {
        if (this.$.isEmptyObject(data)) {
            return this.alertService.showMessage("Server error(s).", true);
        }

        if (data.Code !== StatusCode.Success) {
            return this.alertService.openBootboxConfirm(
                data.Message,
                () => {
                    this.parlayMaxPayoutService.UpdateParlayMaxPayout(params, this.onUpdateSuccess);
                });
        }

        this.parlayMaxPayoutService.UpdateParlayMaxPayout(params, this.onUpdateSuccess);
    };

    private onUpdateSuccess = (data): void => {
        if (this.$.isEmptyObject(data)) {
            return this.alertService.showMessage("Server error(s).", true);
        }

        if (data.Code && data.Code !== StatusCode.Success) {
            return this.alertService.showMessage(data.Message, true);
        }

        if (data.Code === StatusCode.Success) {
            this.alertService.showMessage(this.updateSuccessmessage, CancellationToken cancellation);
        }

        if (data.length > 0) {
            const custNameList = this.custNameList.split(/\,/g);

            let hasError = false;
            let messages = "";
            data.forEach((item, index) => {
                let custName = custNameList[index];
                let message = item.Code === StatusCode.Success
                    ? this.updateSuccessMessage
                    : item.Message;
                let className = item.Code === StatusCode.Success
                    ? "message-success"
                    : "message-error";
                if (!hasError && item.Code !== StatusCode.Success) {
                    hasError = true;
                }

                messages +=
                    `<div class="message ${className} message-icon">
                        ${custName}: ${message}
                    </div>`;
            });

            this.alertService.showMessages(messages);
            if (hasError) {
                return;
            }
        }

        this.reloadAfterSuccess();
    };

    private reloadAfterSuccess(): void {
        if (!this.isMultiple) {
            setTimeout(function () {
                window.location.reload();
            }, this.reloadTimeout);
        } else {
            this.topWindow.customerListNew
                ? this.topWindow.customerListPopup.successfulUpdatingCallback(this.reloadTimeout)
                : this.topWindow.reloadPopUpSetting(this.reloadTimeout);
        }
    }
}

$(function () {
    const alertService = new AlertService($);
    const parlayMaxPayoutService = new ParlayMaxPayoutService($, alertService);
    const parlayMaxPayout = new ParlayMaxPayout($, alertService, parlayMaxPayoutService, window.top);

    parlayMaxPayout.Init();
});