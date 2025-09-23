/// <reference path="../core.ts" />
/// <reference path="../tab/index.ts" />
/// <reference path="index-view.ts" />

'use strict';
/*global cashserviceView, alphaViewLogs, tabController*/
declare var $: JQueryStatic;
declare var viewModel: any;

var cashserviceController = {
    urls: {
        index: (<HTMLInputElement>document.getElementById('indexPath')).value
    },
    init: function () {
        cashserviceView.initElements(cashserviceView.viewModel);

        tabController.viewLogsParams = cashserviceView.getParams();
        new CompactFeedbackComponent().init(tabController.viewLogsParams);
    },
    viewLogs: function () {
        setVisible('hidden');
        cashserviceView.showLoadingState();

        var viewLogsParams = cashserviceView.getParams();
        tabController.viewLogsParams = viewLogsParams;

        var url = cashserviceController.urls.index + '?' + $.param(viewLogsParams);
        window.location.href = url;
    },
    pageLogs: function () {
        setVisible('hidden');
        settingView.showLoadingState();

        tabController.viewLogsParams.PageIndex = cashserviceView.pageNumber;
        tabController.viewLogsParams.PageSize = cashserviceView.pageSize;

        var url = cashserviceController.urls.index + '?' + $.param(tabController.viewLogsParams);
        window.location.href = url;
    }
};

$(function () {
    cashserviceController.init();
});
