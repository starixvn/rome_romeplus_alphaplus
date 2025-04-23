"use strict";

class MyThresholdsSettings extends ThresholdInputsBase {
    constructor($: JQueryStatic) {
        super($);
    }

    public init() {
        super.initThresholdControls();

        const formSettings = this.$("#form-settings")
        formSettings.submit(e => {
            e.preventDefault();
            this.handleOnSubmit(formSettings.attr("action"));
        });
    }

    private handleOnSubmit(url: string) {
        this.alert.hide();
        const formData = this.getThresholdInputs();
        formData && AJAXHelper.postForm(url, formData, result => this.handleOnSuccess(result));
    }

    private handleOnSuccess(result) {
        if (result.IsSuccessful) {
            this.alert.showSuccess(result.Message);
            SiteHelper.closePopupAndReloadMainFrame();
        } else {
            this.alert.showError(result.Message);
        }
    }
}

$(function () {
    new MyThresholdsSettings($).init();
})