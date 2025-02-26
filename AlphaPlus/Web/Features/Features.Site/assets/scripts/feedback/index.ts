declare interface Window {
    popupManager: any;
    indexManager: any;
}

"use strict";
class Feedback {
    $: JQueryStatic;
    messageLimit = 4000;
    addFeedbackUrl = "Add";

    feedback = {
        rate: 0,
        message: ''
    };

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init(): void {
        $('.feedback-msg').focus();
        this.bindEvents();
    }

    bindEvents(): void {
        const self: any = this;

        $('.rate').click(function (): void {
            $('.rate').parents('.rate-box').removeClass('active');
            var $this = $(this);
            $this.parents('.rate-box').addClass('active');

            var r = $this.attr('value');
            self.feedback.rate = r;
        });

        $('#buttonLater').click(function (): void {
            window.top.popupManager.close();
        });

        $('#buttonSubmit').click(function (): void {
            self.submitFeedback();
        });
    }

    submitFeedback(): void {
        const self: any = this;

        self.$.blockUI();
        self.feedback.message = $('.feedback-msg').val();

        $('.message-limit').addClass('hide');
        $('.message-result').addClass('hide');

        if (self.feedback.message.length > self.messageLimit) {
            $('.message-limit').removeClass('hide');
            self.$.unblockUI();
            return;
        }

        if (self.feedback.message.length === 0 && self.feedback.rate === 0) {
            var text = self.$('#Feedback_NoInput').val();
            $('.message-result').html(text);
            $('.message-result').removeClass('hide');
            self.$.unblockUI();
            return;
        }

        $.ajax({
            type: "POST",
            url: self.addFeedbackUrl,
            data: self.feedback,
            success: (result) => {
                self.$.unblockUI();
                if (result.Code === 0) {
                    $('.feedback-box').addClass('hide');
                    $('.feedback-result').removeClass('hide');
                    setTimeout(function (): void {
                        window.top.popupManager.close();
                    }, 3000);

                    return;
                }

                $('.message-result').html(result.Message);
                $('.message-result').removeClass('hide');
            },
            complete: () => {
            }
        });
    }
}

$(function (): void {
    new Feedback($).init();
});