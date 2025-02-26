/*globals document: false, $: false, top: false, window: false*/
/*jshint strict:true */
var alphaViewLogs = {
    format: {
        monthFormat: "MMMM - yyyy",
        shortDateWithSlash: "MM/dd/yyyy"
    },
    urls: {
        queryUsername: (<HTMLInputElement>document.getElementById("usernameAutoSuggestionPath")).value
    },
    keyCode: {
        enter: 13
    },
    pageList: [10, 50, 100, 500],
    callRequest: function (options: any) {
        $.ajax({
            type: "POST",
            url: options.url,
            data: options.data,
            success: options.success
        });
    },
    cancelBubbleEvent: function (event: any) {
        if (alphaViewLogs.isIE()) {
            window.event.cancelBubble = true;
            window.event.returnValue = false;
        } else {
            event.preventDefault();
            event.stopPropagation();
        }

        return false;
    },
    isIE: function () {
        return /msie/i.test(window.navigator.userAgent) &&
            !/opera/i.test(window.navigator.userAgent);
    }
};