"use strict";

class AJAXHelper {
    static postJSON(url: string, formData: any, onSuccess) {
        $.blockUI();
        const xhr = $.post(url, formData, onSuccess);
        xhr.fail(jqXHR => this.handleOnFail(jqXHR));
        xhr.always($.unblockUI);
    }

    static postForm(url: string, formData: any, onSuccess) {
        $.blockUI();
        $.ajax({
            url: url,
            headers: {
                __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val(),
                'X-Requested-With': 'XMLHttpRequest'
            },
            dataType: "json",
            type: "POST",
            data: formData,
            success: onSuccess,
            error: jqXHR => this.handleOnFail(jqXHR),
            complete: $.unblockUI
        });
    }

    private static handleOnFail(jqXHR) {
        if (jqXHR) {
            if (jqXHR.status === 401) {
                window.top["age"].SignOut();
            } else if (jqXHR.status === 500 && jqXHR.responseText) {
                const jsonData = JSON.parse(jqXHR.responseText);
                if (jsonData.UMFrom && jsonData.UMTo) {
                    location.href = `${window.top["alpha"].urls.mainRootPath}PartialUM/Index?umfrom=${jsonData.UMFrom}&umto=${jsonData.UMTo}`;
                }
            }
        }
    }
}