/// <reference path="../core.ts" />
/// <reference path="../tab/index.ts" />
/// <reference path="index-view.ts" />

'use strict';
/*global settingView, alphaViewLogs, tabController*/
declare var $: JQueryStatic;
declare var viewModel: any;

var settingController = {
    urls: {
        index: (<HTMLInputElement>document.getElementById('indexPath')).value
    },
    init: function () {
        settingView.initElements(settingView.viewModel);

        tabController.viewLogsParams = settingView.getParams();
        new CompactFeedbackComponent().init(tabController.viewLogsParams);
    },
    viewLogs: function () {
        setVisible('hidden');
        settingView.showLoadingState();

        const viewLogsParams = settingView.getParams();
        tabController.viewLogsParams = viewLogsParams;

        const url = settingController.urls.index + '?' + $.param(viewLogsParams);
        window.location.href = url;
    },
    pageLogs: function () {
        setVisible('hidden');
        settingView.showLoadingState();

        tabController.viewLogsParams.PageIndex = settingView.pageNumber;
        tabController.viewLogsParams.PageSize = settingView.pageSize;

        const url = settingController.urls.index + '?' + $.param(tabController.viewLogsParams);
        window.location.href = url;
    }
};

$(function () {
    settingController.init();
});
