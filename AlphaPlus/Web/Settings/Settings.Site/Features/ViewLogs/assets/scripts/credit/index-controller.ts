/// <reference path="../core.ts" />
/// <reference path="../tab/index.ts" />
/// <reference path="index-view.ts" />

/*jslint node: true */
"use strict";
/*global creditView, alphaViewLogs, tabController*/
declare var $: JQueryStatic;
declare var viewModel: any;

var creditController = {
    urls: {
        index: (<HTMLInputElement>document.getElementById("indexPath")).value
    },
    init: function () {
        creditView.initElements(creditView.viewModel);

        tabController.viewLogsParams = creditView.getParams();
    },
    viewLogs: function () {
        setVisible("hidden");
        creditView.showLoadingState();

        var viewLogsParams = creditView.getParams();
        tabController.viewLogsParams = viewLogsParams;

        var url = creditController.urls.index + "?" + $.param(viewLogsParams);
        window.location.href = url;
    },
    pageLogs: function () {
        setVisible("hidden");
        creditView.showLoadingState();

        tabController.viewLogsParams.PageIndex = creditView.pageNumber;
        tabController.viewLogsParams.PageSize = creditView.pageSize;

        var url = creditController.urls.index + "?" + $.param(tabController.viewLogsParams);
        window.location.href = url;
    }
};

$(function () {
    creditController.init();
});
