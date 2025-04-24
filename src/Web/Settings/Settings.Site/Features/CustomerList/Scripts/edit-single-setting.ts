'use strict';

class EditSingleSetting {
    private $: JQueryStatic;
    private customerListPopup: CustomerListPopup;
    private customerListServices: CustomerListServices;

    constructor($: JQueryStatic, customerListPopup: CustomerListPopup, customerListServices: CustomerListServices) {
        this.$ = $;
        this.customerListPopup = customerListPopup;
        this.customerListServices = customerListServices;
    }

    public bindingEvent = () =>
        this.$('.edit')
            .unbind()
            .bind('click', (event) => this.openPopup(this.$(event.currentTarget)));

    public openPopup = (element, parentElement = null) => {
        const $element = this.$(element);
        this.customerListPopup.addCursorProgress(parentElement || $element);
        this.$('.edit-multiple').data('custid', '');
        this.$('.edit-multiple').data('url', '');
        this.$('#arrayCustID').val('');
        this.$('#arrayUserName').val('');

        this.customerListServices.getSettings($element.data('url'), (data) => {
            const username = $element.data('username'),
                content = this.buildPopupHtml(data.Settings, username),
                title = `${this.$('#editSettingText').val()} - ${username}`;

            this.customerListPopup.openEditSettingPopup(element, content, title, this.bindingPopupEvent);

            this.customerListPopup.removeCursorProgress(parentElement || $element);
        });
    };

    private buildPopupHtml = (settings, username) => {
        let html = ['<ul>'];
        this.$.each(settings, (key, item) => {
            const attrs = {
                class: `edit-popup ${item.Icon}`,
                'data-method': item.Method,
                'data-params': JSON.stringify(item.QueryStringParameters),
                'data-url': item.Url,
                'data-show-guide-line': item.ShowGuideLine
            };

            const title =
                item.Title ||
                `${item.AddEditTextToPopupTitle ? this.$('#editText').val() : ''} ${item.Text} - ${username}`;

            html.push(`<li ${buildAttributes(attrs)} data-title="${title}">${item.Text}</li>`);
        });

        return html.join('');
    };

    private bindingPopupEvent = () =>
        this.$('.edit-popup').click((event) =>
            this.customerListPopup.openProductPopup(event.currentTarget, this.$(event.currentTarget).data('method'))
        );
}
