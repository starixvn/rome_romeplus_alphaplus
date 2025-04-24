class AlertComponent {
    private elementId;

    constructor(elementId: string = 'msg-alert') {
        this.elementId = elementId;
    }

    show(code: number, message: string) {
        code ? this.showError(message) : this.showSuccess(message);
    }

    showError(error: string) {
        $('#' + this.elementId)
            .removeClass('msg-success')
            .addClass('msg-error')
            .text(error)
            .show();
    }

    showSuccess(success: string) {
        $('#' + this.elementId)
            .removeClass('msg-error')
            .addClass('msg-success')
            .text(success)
            .show();
    }

    hideAlert() {
        $('#' + this.elementId)
            .hide()
            .text('');
    }
}
