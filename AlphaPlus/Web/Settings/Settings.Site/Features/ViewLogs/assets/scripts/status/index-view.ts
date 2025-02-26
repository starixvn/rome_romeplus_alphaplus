/// <reference path="../core.ts" />
/// <reference path="index-controller.ts" />

"use strict";
/*global statusController, alphaViewLogs*/
declare var $: JQueryStatic;
declare var initAutoComplete: Function;
declare var setVisible: any;

var statusView = {
    viewModel: {
        labelTo: (<HTMLInputElement>document.getElementById("labelTo")).value,
        noInfo: (<HTMLInputElement>document.getElementById("noInfo")).value,
        Page: (<HTMLInputElement>document.getElementById("Page")).value,
        of: (<HTMLInputElement>document.getElementById("of")).value,
        Pager_DisplayItems: (<HTMLInputElement>document.getElementById("Pager_DisplayItems")).value,

        maxReportDate: (<HTMLInputElement>document.getElementById("maxReportDate")).value,
        minReportDate: (<HTMLInputElement>document.getElementById("minReportDate")).value,
        fromDate: (<HTMLInputElement>document.getElementById("viewmodal-fromdate")).value,
        toDate: (<HTMLInputElement>document.getElementById("viewmodal-todate")).value,
        dayInMonth: (<HTMLInputElement>document.getElementById("dayInMonth")).value,

        totalRecords: (<HTMLInputElement>document.getElementById("totalRecords")).value,
        pageSize: (<HTMLInputElement>document.getElementById("viewmodal-pagesize")).value,
        pageIndex: parseInt((<HTMLInputElement>document.getElementById("pageIndex")).value, 0),
        actionTypeId: (<HTMLInputElement>document.getElementById("actionTypeId")).value,

        showOldValuesCookieName: (<HTMLInputElement>document.getElementById("showOldValuesCookieName")).value
    },
    elelments: {
        btnSubmit: null,
        ddlMonth: null,
        iconLoading: null,
        chkOldValues: null,
        txtUsername: null,
        linkChangTabs: null,
        pagination: null,
        dataLog: null,
        navTabs: null,
        longMonthsInYear: null
    },
    showOldValuesCookieName: "",
    pageNumber: 0,
    pageSize: 0,
    cacheElements: function () {
        statusView.elelments.btnSubmit = $("#btn-submit");
        statusView.elelments.ddlMonth = $("#ddlMonth");
        statusView.elelments.iconLoading = $("#icon-loading");
        statusView.elelments.chkOldValues = $("#chk-oldvalues");
        statusView.elelments.txtUsername = $("#txt-username");
        statusView.elelments.pagination = $("#pagination");
        statusView.elelments.dataLog = $("#DataLog");
        statusView.elelments.navTabs = $("#nav-tabs");
        statusView.elelments.longMonthsInYear = $("#longMonthsInYear");
    },
    initElements: function (viewModel: any) {
        statusView.cacheElements();

        statusView.initMonthPicker(viewModel);

        statusView.elelments.btnSubmit.click(function () {
            statusView.pageNumber = 0;
            statusView.pageSize = 0;

            statusController.viewLogs();
        });

        statusView.elelments.chkOldValues.click(function () {
            var isShowOldValues = statusView.elelments.chkOldValues.prop("checked") ? 1 : 0;
            $.cookie(viewModel.showOldValuesCookieName, isShowOldValues, { path: "/" });
            statusController.viewLogs();
        });

        statusView.elelments.txtUsername.keypress(function (e: any) {
            if (!e) {
                e = window.event;
            }

            var key = e.keyCode ? e.keyCode : e.which;
            if (key === alphaViewLogs.keyCode.enter) {
                statusView.pageNumber = 0;
                statusView.pageSize = 0;
                statusController.viewLogs();

                return alphaViewLogs.cancelBubbleEvent(e);
            }
        });

        var queryUsernameURL = alphaViewLogs.urls.queryUsername + "?" +
            $.param({
                IsAccInfo: false,
                DirectDownlines: true,
                CustId: parseInt($("#CustId").val())
            });

        initAutoComplete($("#txt-username")[0], queryUsernameURL);

        statusView.renderPager({
            totalRecords: viewModel.totalRecords,
            pageSize: viewModel.pageSize,
            pageIndex: viewModel.pageIndex + 1
        });

        statusView.pageNumber = viewModel.pageIndex;
        statusView.pageSize = viewModel.pageSize;

        statusView.elelments.dataLog.stickyTableHeaders({ fixedOffset: statusView.elelments.navTabs });
    },
    initMonthPicker: function (viewModel: any) {
        statusView.elelments.ddlMonth.monPickr({
            longMonthsInYear: statusView.getLongMonthsInYear(),
            maxDate: viewModel.maxReportDate,
            minDate: viewModel.minReportDate,
            month: new Date(viewModel.dayInMonth),

            fromDay: viewModel.fromDate,
            toDay: viewModel.toDate,

            showDateRange: true,
            monthFormat: alphaViewLogs.format.monthFormat,
            fromDayId: "ddl-fromday",
            toDayId: "ddl-today",
            fromDayLabel: "",
            toDayLabel: statusView.viewModel.labelTo,
            sortDesc: true,
            log: function (msg: string) {
                console.warn(msg);
            }
        });
    },
    showLoadingState: function () {
        statusView.elelments.btnSubmit.attr("disabled", "disabled");
        statusView.elelments.iconLoading.removeClass("hide");
    },
    showCompleteLoadingState: function () {
        statusView.elelments.iconLoading.addClass("hide");
        statusView.elelments.btnSubmit.attr("disabled", null);
    },
    getParams: function () {
        return {
            ActionTypeId: statusView.viewModel.actionTypeId,
            PageIndex: statusView.pageNumber,
            PageSize: statusView.pageSize,
            Username: statusView.elelments.txtUsername.val(),
            FromDate: statusView.elelments.ddlMonth.monPickr("getFromDate", alphaViewLogs.format.shortDateWithSlash),
            ToDate: statusView.elelments.ddlMonth.monPickr("getToDate", alphaViewLogs.format.shortDateWithSlash)
        };
    },
    renderPager: function (options: any) {
        if (alphaViewLogs.pageList[0] <= options.totalRecords) {
            statusView.elelments.pagination.pagination({
                pageList: alphaViewLogs.pageList,
                total: options.totalRecords,
                pageSize: options.pageSize,
                pageNumber: options.pageIndex,

                showRefresh: false,
                onSelectPage: function (pageNumber: number, pageSize: number) {
                    statusView.pageNumber = pageNumber - 1;
                    statusView.pageSize = pageSize;

                    statusController.pageLogs();
                },
                beforePageText: statusView.viewModel.Page,
                afterPageText: statusView.viewModel.of + " {pages}",
                displayMsg: statusView.viewModel.Pager_DisplayItems
            });

            statusView.elelments.pagination.removeClass("hide");
        } else {
            statusView.elelments.pagination.addClass("hide");
        }
    },
    getLongMonthsInYear: function () {
        var longMonthsInYear = $.parseJSON(statusView.elelments.longMonthsInYear.val());
        return longMonthsInYear;
    }
};
