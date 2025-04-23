"use strict";
class WinningThresholdSurvey {
    $: JQueryStatic;
    addSurveyUrl = "AddSurvey";

    feedback = {
        rate: 0,
        message: '',
        functionId: 0
    };

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init(): void {
        this.bindEvents();
        this.feedback.functionId = $('#FunctionId').val();
    }

    bindEvents(): void {
        const self: any = this;

        if ($('#IsInternal').val() === '0') {
            $('#btn-not-useful').click(function (): void {
                self.submitFeedback(0);
            });

            $('#btn-useful').click(function (): void {
                self.submitFeedback(1);
            });
        } else {
            $('#btn-close').click(function (): void {
                window.top.indexManager.domElements.winningThresholdSurveyPopup.close();
            });
        }
    }

    submitFeedback(useful: boolean): void {
        const self: any = this;

        self.$.blockUI();
        self.feedback.message = useful ? 1 : 0;

        $('.message-result').addClass('hide');

        $.ajax({
            type: "POST",
            url: self.addSurveyUrl,
            data: self.feedback,
            success: (result) => {
                self.$.unblockUI();
                if (result.IsSuccessful) {
                    $('.feedback-box').addClass('hide');
                    $('.survey-result').removeClass('hide');
                    setTimeout(function (): void {
                        window.top.location.reload();
                        window.top.indexManager.domElements.winningThresholdSurveyPopup.close();
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
    new WinningThresholdSurvey($).init();
}); 