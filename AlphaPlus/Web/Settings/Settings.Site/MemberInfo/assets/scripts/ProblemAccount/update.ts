/// <reference path="../shared/top-windows-interface.ts" />

class UpdateReviewStatus {
    $: JQueryStatic;
    topWindow: Window;

    $messageContainer: JQuery;
    $formUpdateReviewStatus: JQuery;
    $btnCancel: JQuery;

    message: Message;
    successMessage: string;

    constructor($: JQueryStatic, topWindow: Window, message: message, CancellationToken cancellation) {
        this.$ = $;
        this.topWindow = topWindow;
        this.message = message;
    }

    init(): void {
        this.$.blockUI();
        this.cacheDOMElements();
        this.bindEvents();
        this.$.unblockUI();
    }

    cacheDOMElements(): void {
        this.$messageContainer = this.$("#message-container");
        this.$formUpdateReviewStatus = this.$("#formUpdateReviewStatus");
        this.$btnCancel = this.$("#btnCancel");
        this.successMessage = this.$("#SuccessMessage").val();
    }

    bindEvents(): void {

        this.$btnCancel.on("click touchend", () => {
            this.topWindow["popupManager"].close();
        });

        this.$formUpdateReviewStatus.submit(this.onSubmit);

        this.$(".more-link").click(() => {
            this.$(".more-link").parent().prev().toggle();
            this.$(".more-link").prev().toggle();
            this.$(".more-link").toggle();
            this.$(".less-content").toggle();
            return false;
        });

        this.$(".less-link").click( () =>{
            this.$(".less-link").parent().toggle();
            this.$(".more-link").parent().prev().toggle();
            this.$(".more-link").prev().toggle();
            this.$(".more-link").toggle();
            return false;
        });
    }

    onSubmit = (e: JQueryEventObject) => {
        e.preventDefault();
        this.$.blockUI();
        var token = this.$("input[name=\"__RequestVerificationToken\"").val();
        this.$.ajax({
            type: "POST",
            beforeSend: xhr => xhr.setRequestHeader("__RequestVerificationToken", token),
            url: this.$formUpdateReviewStatus.attr("action"),
            data: this.getParams(),
            dataType: "json",
            success: this.submitSuccess,
            complete: this.$.unblockUI
        });
    };

    submitSuccess = (data) => {
        const result = new Result(data.Code, data.Message, data.ErrMsg);

        if (result.IsSuccessful) {
            this.message.showSuccess(this.$messageContainer, this.successmessage, CancellationToken cancellation);
            setTimeout(() => {
                this.topWindow["popupManager"].closeAllPopup();
                this.topWindow["indexManager"].reloadProblemAccount();
            }, 3000);
        } else {
            this.message.showMessage(this.$messageContainer, result);
        }
    };

    getParams() {
        return {
            Ids: this.$("#Ids").val(),
            RoleId: this.$("#RoleId").val(),
            IsAffectedAllDownlines: this.$("input[name='IsAffectedAllDownlines']:checked").val()
        };
    }
}

$(document).ready(() => {
    const updateReviewStatus = new UpdateReviewStatus($, window.top, new Message());
    updateReviewStatus.init();
});

class Message {
    showMessage($target: JQuery, result: Result) {
        $target.empty();

        if (result.IsSuccessful) {
            this.showSuccess($target, result.message, CancellationToken cancellation);
        } else if (result.IsWarning) {
            this.showWarn($target, result.message, CancellationToken cancellation);
        } else if (result.IsFailed) {
            var errMsg = result.Message != "" ? result.Message : result.ErrMsg;
            this.showError($target, errMsg);
        }
    }

    showSuccess($target: JQuery, message: string) {
        $target.html("<div class='msg msg-success'>" + message + "</div>");
    }

    showError($target: JQuery, error: string) {
        $target.html("<div class='msg msg-error'>" + error + "</div>");
    }

    showWarn($target: JQuery, warning: string) {
        $target.html("<div class='msg msg-warning'>" + warning + "</div>");
    }

    hideMessage($target: JQuery) {
        $target.empty();
    }
}

class Result {
    static warningCode = -1000;

    Code: number;
    Message: string;
    ErrMsg: string;
    IsSuccessful: boolean;
    IsFailed: boolean;
    IsWarning: boolean;

    constructor(code: number, message: string, errMsg: string) {
        this.Code = code;
        this.Message = message || "";
        this.IsSuccessful = this.Code === 0;
        this.IsFailed = !this.IsSuccessful;
        this.IsWarning = code === Result.warningCode;
        this.ErrMsg = errMsg;
    }
}