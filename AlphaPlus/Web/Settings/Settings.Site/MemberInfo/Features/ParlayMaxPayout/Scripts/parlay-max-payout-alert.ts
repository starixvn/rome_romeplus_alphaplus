"use strict";

interface Window {
    bootboxConfirm: Function;
    customerListNew: boolean;
}

class AlertService {
    private $: JQueryStatic;

    private $divMessageContainer: JQuery;
    private $divMessageText: JQuery;
    private $divPmpContainer: JQuery;
    private $divHint: JQuery;
    private $divHintText: JQuery;
    private $divHintPointer: JQuery;

    private hideHintTimeout = 3000;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    public Init(): void {
        this.cacheDomElements();
    }

    private cacheDomElements(): void {
        this.$divMessageContainer = this.$("#message-container");
        this.$divMessageText = this.$("#message-text");
        this.$divPmpContainer = this.$("#pmp-container");
        this.$divHint = this.$("#hint");
        this.$divHintText = this.$("#hint-text");
        this.$divHintPointer = this.$("#hint-pointer");
    }

    public showHint(el: HTMLElement, message: string): void {
        this.$divHintText.text(message);
        this.$divHint.css("display", "block");

        this.adjustHintPosition(el);
        this.setHideHint();
    }

    private adjustHintPosition(el: HTMLElement): void {
        const hintHeight = 30,
            hintPointerHeight = 12,
            inputWidth = 120,
            hintWidth = this.$divHint.outerWidth(),
            elOffset = this.$(el).offset(),
            containerOffset = this.$divPmpContainer.offset(),
            containerWidth = this.$divPmpContainer.width(),
            top = elOffset.top - containerOffset.top - hintHeight - hintPointerHeight;

        if (elOffset.left + hintWidth > containerWidth) {
            const right = containerOffset.left + containerWidth - elOffset.left - inputWidth;
            this.$divHint.css({ top, right, left: "auto" });
            this.$divHintPointer.css({ "margin-left": "auto", "margin-right": 0 });
        } else {
            this.$divHint.css({ top, right: "auto", left: elOffset.left });
            this.$divHintPointer.css({ "margin-left": 0, "margin-right": "auto" });
        }
    }

    private setHideHint(): void {
        setTimeout(() => {
            this.$divHint.css({ display: "none", left: 0, right: "auto" });
        }, this.hideHintTimeout);
    }

    public showMessage(message: string, isError: boolean = false): void {
        this.$divMessageText.html(message);
        this.$divMessageText.addClass(isError ? "message-error" : "message-success");
        this.$divMessageContainer.css("display", "block");
    }

    public showMessages(messages: string): void {
        this.$divMessageContainer.empty();
        this.$divMessageContainer.html(messages);
        this.$divMessageContainer.css("display", "block");
    }

    public clearMessages(): void {
        this.$divMessageContainer.css("display", "none");
    }

    public openBootboxConfirm(message: string, onAcceptCallback: Function): void {
        window.top.bootboxConfirm(message, null, onAcceptCallback);
    }
}