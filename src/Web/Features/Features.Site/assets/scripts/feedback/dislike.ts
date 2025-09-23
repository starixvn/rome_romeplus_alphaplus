'use strict';

declare interface Window {
    CompactFeedbackManager: {
        init: (window, $boxIconTitle, $like, $dislike, formData) => void;
        closeDislikePopup: () => void;
    };
    afterDislikeSubmitCallback: () => void;
}

class CompactFeedback {
    init() {
        const formFeedback = $('#formFeedback');
        formFeedback.submit((e) => {
            e.preventDefault();
            this.submitFeedback(formFeedback);
        });
        $('#buttonClose').click(() => window.top.CompactFeedbackManager.closeDislikePopup());
        const btnSubmit = $('#buttonSubmit');
        $('#feedback-msg')
            .keyup(function () {
                btnSubmit.prop('disabled', $.trim(this.value).length === 0);
            })
            .focus();
    }

    submitFeedback(formFeedback: JQuery) {
        $.blockUI();
        const msgError = $('#msg-error').addClass('hide');
        const message = $.trim($('#feedback-msg').val());
        if (!message.length) {
            this.showError(msgError, $('#Feedback_NoInput').val());
            $.unblockUI();
            return;
        }

        $.post(
            formFeedback.attr('action'),
            { message: message, rate: 1, functionId: $('#FunctionId').val(), extraInfo: $('#ExtraInfo').val() },
            (result) => {
                if (result.Code) {
                    this.showError(msgError, result.Message);
                } else {
                    window.top.afterDislikeSubmitCallback && window.top.afterDislikeSubmitCallback();

                    formFeedback.addClass('hide');
                    $('#msg-success').html(result.Message).removeClass('hide');
                }
            }
        ).always($.unblockUI);
    }

    showError(errorElement, message: string) {
        errorElement.html(message).removeClass('hide');
    }
}

$(function () {
    new CompactFeedback().init();
});
