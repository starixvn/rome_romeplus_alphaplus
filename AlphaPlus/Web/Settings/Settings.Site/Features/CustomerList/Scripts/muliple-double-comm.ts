class MultipleDoubleComm {
    $: JQueryStatic;
    topWindow: any;
    $messageContainer: JQuery;
    $messageContent: JQuery;

    constructor($: JQueryStatic, topWindow: any) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    init = () => {
        this.$messageContainer = this.$('#message');
        this.$messageContent = this.$('#message-content');
        this.$('.btn-submit').click((event) => this.submitEvent(event));
    };

    showErrorMessage = (message) => {
        this.$messageContainer.removeClass('hidden').addClass('msg msg-error');
        this.$messageContent.html(message);
    };

    reloadFrame = () => {
        const frms = window.top.parent.frames;
        if (frms && frms['main']) {
            frms['main'].location = frms['main'].location;
        }
    };

    submitEvent = (event) => {
        event.preventDefault();

        this.$.blockUI();

        const data = {
            SearchAccountIds: this.$('#searchAccountIds').val(),
            SearchAccountRoleId: this.$('#searchAccountRoleId').val(),
            EnabledDoubleComm: this.$('input[name=mulitpleDoubleComm]:checked').val(),
            Usernames: this.$('#usernames').val(),
            AgentId: this.$('#agentId').val()
        };

        this.$.ajax({
            method: 'POST',
            headers: {
                RequestVerificationToken: this.$("input[name='__RequestVerificationToken']").val(),
                'X-Requested-With': 'XMLHttpRequest'
            },
            data: data,
            dataType: 'json',
            url: 'UpdateMultipleDoubleComm',
            success: (results) => {
                let html = [];

                for (const [username, item] of Object.entries(results)) {
                    html.push(`<div>${username}`);
                    if (item.IsSuccessful) {
                        html.push(`<div class='msg msg-success'>${this.$('#successullyMessage').val()}</div></div>`);
                    } else {
                        html.push(`<div class='msg msg-error'>${item.Message}</div></div>`);
                    }
                }

                this.$messageContent.html(html.join(''));
                this.$messageContainer.removeClass('hidden');

                this.$.unblockUI();
            },
            error: () => {
                this.$.unblockUI();
            }
        });
    };
}

const multipleDoubleComm = new MultipleDoubleComm($, window.top);

$(() => {
    multipleDoubleComm.init();
});
