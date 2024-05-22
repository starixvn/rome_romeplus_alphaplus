"use strict";
class Unlock {
    private $: JQueryStatic;
    private topWindow: any;
    private customerListServices: CustomerListServices;
    private isMobile: boolean;

    constructor(
        $: JQueryStatic,
        topWindow: any,
        isMobile: boolean,
        customerListServices: CustomerListServices) {
        this.$ = $;
        this.topWindow = topWindow;
        this.isMobile = isMobile;
        this.customerListServices = customerListServices;
    }

    public bindingEvent = () => this.$('.icon-locked').unbind().bind('click', this.openPopup);

    private openPopup = event => {
        const confirmMessage = this.$(event.currentTarget).data("message");
        confirmMessage && this.topWindow.bootboxConfirm(
            confirmMessage,
            this.isMobile ? null : event.currentTarget,
            () => this.customerListServices.updateCustomerStatus(
                event.currentTarget,
                null,
                this.updateCustomerStatusSuccess));
    }

    private updateCustomerStatusSuccess = (currentTarget, parentTarget, data, reloadcustomerlist) => {
        if (data.IsSuccessful) {
            if (reloadcustomerlist && this.topWindow.customerListNew) {
                this.topWindow.successfulUpdatingCallbackAndReloadMainFrame(true);
            }
            else {
                this.$.unblockUI();
            }
        }
        else {
            this.$.unblockUI();
            this.topWindow.bootboxAlert(data.Message, this.isMobile ? currentTarget : parentTarget);
        }
    }
}