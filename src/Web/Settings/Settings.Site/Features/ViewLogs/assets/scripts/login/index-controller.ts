/// <reference path="../core.ts" />
/// <reference path="../tab/index.ts" />
/// <reference path="index-view.ts" />

'use strict';
/*global loginView, alphaViewLogs, tabController*/
declare var $: JQueryStatic;
declare var viewModel: any;

var loginController = {
    urls: {
        index: (<HTMLInputElement>document.getElementById('indexPath')).value
    },
    init: function () {
        loginView.initElements(loginView.viewModel);

        tabController.viewLogsParams = loginView.getParams();
        new CompactFeedbackComponent().init(tabController.viewLogsParams);
    },
    viewLogs: function () {
        setVisible('hidden');
        loginView.showLoadingState();

        var viewLogsParams = loginView.getParams();
        tabController.viewLogsParams = viewLogsParams;

        var url = loginController.urls.index + '?' + $.param(viewLogsParams);
        window.location.href = url;
    },
    pageLogs: function () {
        setVisible('hidden');
        loginView.showLoadingState();

        tabController.viewLogsParams.PageIndex = loginView.pageNumber;
        tabController.viewLogsParams.PageSize = loginView.pageSize;

        var url = loginController.urls.index + '?' + $.param(tabController.viewLogsParams);
        window.location.href = url;
    }
};

$(function () {
    loginController.init();
});
