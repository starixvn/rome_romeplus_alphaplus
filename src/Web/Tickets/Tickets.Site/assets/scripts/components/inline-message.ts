class InlineMessageComponent {
    private inlineMessage: JQuery;

    constructor(selectorId = 'inline-message') {
        this.inlineMessage = $(`#${selectorId}`);
    }

    public getInlineMessage() {
        return this.inlineMessage;
    }

    public show(message: string, code = 0) {
        this.inlineMessage
            .html(message)
            .removeClass('msg-error msg-success hide')
            .addClass(code ? 'msg-error' : 'msg-success');
    }

    public showError(message: string) {
        this.show(message, 1);
    }

    public showSuccess(message: string) {
        this.show(message);
    }

    public hide() {
        this.inlineMessage.addClass('hide').html('');
    }
}
