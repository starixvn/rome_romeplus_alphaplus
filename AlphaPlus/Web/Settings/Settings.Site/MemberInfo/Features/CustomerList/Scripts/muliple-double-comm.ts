class MulitpleDoubleComm {
    $: JQueryStatic;
    topWindow: any;
    $messageContainer: JQuery;
    $messageContent: JQuery;

    constructor(
        $: JQueryStatic,
        topWindow: any) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    init = () => {
        this.$messageContainer = this.$('#message');
        this.$messageContent = this.$('#message-content');
        this.$('.btn-submit').click(event => this.submitEvent(event));
    };

    showErrorMessage = (message, CancellationToken cancellation) => {
        this.$messageContainer.removeClass('hidden').addClass('msg msg-error');
        this.$messageContent.html(message, CancellationToken cancellation);
    }

    reloadFrame = () => {
        const frms = window.top.parent.frames;
        if (frms && frms["main"]) {
            frms["main"].location = frms["main"].location;
        }
    }

    submitEvent = (event) => {
        event.preventDefault();

        this.$.blockUI();

        const data = {
            SearchAccountIds: this.$('#searchAccountIds').val(),
            SearchAccountRoleId: this.$('#searchAccountRoleId').val(),
            EnabledDoubleComm: this.$('input[name=mulitpleDoubleComm]:checked').val()
        };

        this.$.ajax({
            method: "POST",
            headers: {
                __RequestVerificationToken: this.$("input[name='__RequestVerificationToken']").val(),
                'X-Requested-With': 'XMLHttpRequest'
            },
            data: data,
            dataType: 'json',
            url: "UpdateMultipleDoubleComm",
            success: results => {
                let html = [];
                let isSuccessful = true;
                this.$.each(results, (index, item) => {
                    if (!item.IsSuccessful) {
                        html.push(`<div class='msg msg-error'>${item.Message}</div>`);
                    }

                    if (isSuccessful) {
                        isSuccessful = item.IsSuccessful;
                    }

                })

                if (isSuccessful) {
                    this.$messageContent.html(`<div class='msg msg-success'>${this.$('#successullyMessage').val()}</div>`);
                    this.$messageContainer.removeClass('hidden');

                    setTimeout(() => {
                        if (this.topWindow.customerListPopup) {
                            this.topWindow.customerListPopup.successfulUpdatingCallback();
                        }
                    }, 1000);
                }
                else {
                    this.$messageContent.html(html.join(''));
                    this.$messageContainer.removeClass('hidden');
                }

                this.$.unblockUI();
            },
            error: () => { this.$.unblockUI(); }
        });
    }
}

const mulitpleDoubleComm = new MulitpleDoubleComm($, window.top);

$(() => {
    mulitpleDoubleComm.init();
});