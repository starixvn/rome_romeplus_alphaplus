class UpdateCustomerInfoView {
    init() {
        this.showTitle();
        const $form = $('#form-update').submit((e) => {
            e.preventDefault();
            const isValidEmail = this.validateEmail($('#Email').val());
            if (!isValidEmail) {
                this.showError($('#hidden-EmailNotValidText').val());
                return;
            }

            this.hideAlert();
            $.blockUI();
            $.ajax({
                headers: {
                    RequestVerificationToken: $("input[name='__RequestVerificationToken']").val(),
                    'X-Requested-With': 'XMLHttpRequest'
                },
                type: 'POST',
                url: $form.attr('action'),
                data: $form.serializeArray(),
                dataType: 'json',
                success: (result) => {
                    if (result.IsSuccessful) {
                        this.showSuccess($('#hidden-UpdatedSuccessfullyText').val());
                        setTimeout(function () {
                            window.location.href = window.location.href;
                        }, 3000);
                    } else {
                        this.showError(result.Message);
                    }
                },
                complete: $.unblockUI
            });
        });

        $('.btn-reset').click(function () {
            $('#FirstName').val('');
            $('#LastName').val('');
            $('#Phone').val('');
            $('#MobilePhone').val('');
            $('#Email').val('');
            $('#Fax').val('');
        });
    }

    private validateEmail(email) {
        let isValid = true;
        if (email.length) {
            const emailRegEx = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            isValid = email.match(emailRegEx);
        }

        return isValid;
    }

    private showError(error: string) {
        $('#msg-alert').removeClass('msg-success').addClass('msg-error').text(error).removeClass('hide');
    }

    private showSuccess(success: string) {
        $('#msg-alert').removeClass('msg-error').addClass('msg-success').text(success).removeClass('hide');
    }

    private hideAlert() {
        $('#msg-alert').addClass('hide').text('');
    }

    // Hide Title on top page, when open Popup.
    private showTitle() {
        let popup = window.parent.$('#product-popup');
        if (!popup.length) {
            popup = window.top.$('div[id^="page-popup-"]');
        }

        popup.length > 0 || $('.page-title').removeClass('hide');
    }
}

$(function () {
    new UpdateCustomerInfoView().init();
});
