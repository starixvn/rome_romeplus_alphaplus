/// <reference path="../core.ts" />
/// <reference path="../tab/index.ts" />
/// <reference path="index-view.ts" />

'use strict';
/*global statusView, alphaViewLogs, tabController*/
declare var $: JQueryStatic;
declare var viewModel: any;

var statusController = {
    urls: {
        index: (<HTMLInputElement>document.getElementById('indexPath')).value
    },
    init: function () {
        statusView.initElements(statusView.viewModel);

        tabController.viewLogsParams = statusView.getParams();
        new CompactFeedbackComponent().init(tabController.viewLogsParams);
    },
    viewLogs: function () {
        setVisible('hidden');
        statusView.showLoadingState();

        var viewLogsParams = statusView.getParams();
        tabController.viewLogsParams = viewLogsParams;

        var url = statusController.urls.index + '?' + $.param(viewLogsParams);
        window.location.href = url;
    },
    pageLogs: function () {
        setVisible('hidden');
        statusView.showLoadingState();

        tabController.viewLogsParams.PageIndex = statusView.pageNumber;
        tabController.viewLogsParams.PageSize = statusView.pageSize;

        var url = statusController.urls.index + '?' + $.param(tabController.viewLogsParams);
        window.location.href = url;
    }
};

$(function () {
    statusController.init();
});
