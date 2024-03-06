"use strict";

class MyWatchlistAlert {
    $: JQueryStatic;
    divAlert: JQuery;

    constructor($: JQueryStatic) {
        this.$ = $;
        this.cacheDOMElements();
    }

    private cacheDOMElements() {
        this.divAlert = this.$("#alert-message");
    }

    public showSuccess(message: string) {
        this.divAlert.removeClass("show-alert-message").html(`<div class="msg msg-success">${message}</div>`)
    }

    public showError(error: string) {
        this.divAlert.removeClass("show-alert-message").html(`<div class="msg msg-error">${error}</div>`);
    }

    public showMultiple(results: any[]) {
        let html = [];
        results.forEach(function (item) {
            html.push('<div>');
            html.push('<div class="name">');
            html.push(item.Name);
            html.push('</div>');
            html.push(`<div class="msg msg-${item.Code ? 'error' : 'success'}">`);
            html.push(item.message);
            html.push('</div>');
            html.push('</div>');
        });

        this.divAlert.html(html.join("")).addClass("show-alert-message");
    }

    public hide() {
        this.divAlert.removeClass("show-alert-message").html("");
    }
}