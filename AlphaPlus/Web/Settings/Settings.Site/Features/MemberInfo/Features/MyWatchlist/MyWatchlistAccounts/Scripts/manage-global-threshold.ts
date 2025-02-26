"use strict";

class ManageGlobalThreshold extends ThresholdInputsBase {
    private baseUrl: string;

    constructor($: JQueryStatic) {
        super($);
    }

    public init() {
        this.baseUrl = this.$("#member-info-url").val();
        this.initThresholdControls();
        this.$("#btn-update").click(this.handleUpdate);
    }

    private handleUpdate = () => {
        this.alert.hide();
        const formData = this.getThresholdInputs();

        if (formData) {
            AJAXHelper.postForm(
                `${this.baseUrl}MyWatchlistAccounts/UpdateGlobalThreshold`,
                formData,
                result => {
                    if (result.IsSuccessful) {
                        this.alert.showSuccess(this.$("#update-successfully").val());
                        SiteHelper.closePopupAndReloadMainFrame();
                    } else if (result.Code === 1) {
                        this.alert.showError(result.Message);
                    }
            });
        }
    }
}

$(function () {
    new ManageGlobalThreshold($).init();
})