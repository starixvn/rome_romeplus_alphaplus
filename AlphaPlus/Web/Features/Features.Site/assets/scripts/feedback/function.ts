"use strict";

class FeedbackFunction {
    private $: JQueryStatic;
    private feedbackFormData = null;
    private areaFormData = null;
    private messageLimit: JQuery;
    private messageResult: JQuery;
    private feedbackMsg: JQuery;

    private messageMaxLength: number;
    private feedbackNoInputError: string

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init(): void {
        this.feedbackFormData = {
            rate: 0,
            message: "",
            functionId: +this.$("#hiddenFunctionId").val()
        };

        this.cacheDOMElements();
        this.bindEvents();
    }

    private cacheDOMElements() {
        this.messageLimit = this.$("#message-limit");
        this.messageResult = this.$("#message-result");
        this.feedbackMsg = this.$("#feedback-msg");

        this.messageMaxLength = +this.feedbackMsg.attr("maxlength");
        this.feedbackNoInputError = this.$("#hiddenFeedbackNoInputError").val();

        this.feedbackMsg.focus();
    }

    private bindEvents(): void {
        const self = this;
        const buttonSubmit = this.$("#button-submit");

        const areaElements = this.$("input[type=radio]", ".rate-star");
        if (areaElements.length > 0) {
            this.areaFormData = {};
            areaElements.change(function () {
                const $this = self.$(this);
                self.areaFormData[$this.closest(".rate-star").data("areaid")] = +$this.attr("value");
                buttonSubmit.prop("disabled", self.isInvalidFeedbackFormData());
            });
        }

        this.feedbackMsg.keyup(function () {
            const message = self.$.trim(this.value);
            self.feedbackFormData.message = message;
            buttonSubmit.prop("disabled", self.isInvalidFeedbackFormData());
        });

        const rateBoxes = $(".rate-box");
        rateBoxes.click(function () {
            rateBoxes.removeClass("active");

            const $this = self.$(this);
            $this.addClass("active");
            self.feedbackFormData.rate = +$this.find(".rate").attr("value");
            buttonSubmit.prop("disabled", self.isInvalidFeedbackFormData());
        });

        this.$("#button-later").click(function () {
            window.top.popupManager.close();
        });

        buttonSubmit.click(this.submitFeedback);
    }

    private submitFeedback = () => {
        this.messageLimit.addClass("hide");
        this.messageResult.addClass("hide");

        if (this.feedbackFormData.message.length > this.messageMaxLength) {
            this.messageLimit.removeClass("hide");
            return;
        }

        if (this.isInvalidFeedbackFormData()) {
            this.messageResult.html(this.feedbackNoInputError).removeClass("hide");
            return;
        }

        this.$.blockUI();
        if (this.areaFormData && !this.isEmptyObject(this.areaFormData)) {
            this.feedbackFormData.areas = this.convertObjectToArray(this.areaFormData);
        }
        this.$.post(this.$("#hiddenAddFeedbackUrl").val(), this.feedbackFormData, (result) => {
            if (result.Code) {
                this.messageResult.html(result.Message).removeClass("hide");
            } else {
                this.$("#feedback-box").addClass("hide");
                this.$("#feedback-result").removeClass("hide");
                setTimeout(function () {
                    window.top.indexManager.hideFloatingFeedbackIcon();
                    window.top.popupManager.close();
                }, 3000);
            }
        }).always(this.$.unblockUI);
    }

    private isInvalidFeedbackFormData() {
        return this.areaFormData
            ? this.feedbackFormData.message.length === 0 && this.isEmptyObject(this.areaFormData)
            : this.feedbackFormData.message.length === 0 && this.feedbackFormData.rate === 0;
    }

    private isEmptyObject(object: any) {
        return Object.keys(object).length === 0;
    }

    private convertObjectToArray(object: any) {
        return Object.keys(object).map(function (item) {
            return {
                areaId: item,
                rate: object[item]
            };
        });
    }
}

$(function () {
    new FeedbackFunction($).init();
});
