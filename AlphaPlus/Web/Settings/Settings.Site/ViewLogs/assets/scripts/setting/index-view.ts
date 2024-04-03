/// <reference path="../core.ts" />
/// <reference path="index-controller.ts" />

"use strict";
/*global settingController, alphaViewLogs, viewModel*/
declare var $: JQueryStatic;
declare var initAutoComplete: Function;

var settingView = {
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
        ddlLogActionType: null,
        btnSubmit: null,
        ddlMonth: null,
        iconLoading: null,
        chkOldValues: null,
        txtUsername: null,
        pagination: null,
        longMonthsInYear: null,
        dataLog: null,
        navTabs: null
    },
    showOldValuesCookieName: "",
    pageNumber: 0,
    pageSize: 0,
    cacheElements: function () {
        settingView.elelments.ddlLogActionType = $("#ddl-logactiontype");
        settingView.elelments.btnSubmit = $("#btn-submit");
        settingView.elelments.ddlMonth = $("#ddlMonth");
        settingView.elelments.iconLoading = $("#icon-loading");
        settingView.elelments.chkOldValues = $("#chk-oldvalues");
        settingView.elelments.txtUsername = $("#txt-username");
        settingView.elelments.pagination = $("#pagination");
        settingView.elelments.longMonthsInYear = $("#longMonthsInYear");
        settingView.elelments.dataLog = $("#DataLog");
        settingView.elelments.navTabs = $("#nav-tabs");

    },
    initElements: function (viewModel: any) {
        settingView.cacheElements();

        settingView.elelments.ddlLogActionType.change(function () {
            settingView.pageNumber = 0;
            settingView.pageSize = 0;

            settingController.viewLogs();
        });

        settingView.initMonthPicker(viewModel);

        settingView.elelments.btnSubmit.click(function () {
            settingView.pageNumber = 0;
            settingView.pageSize = 0;

            settingController.viewLogs();
        });

        settingView.elelments.chkOldValues.click(function () {
            var isShowOldValues = settingView.elelments.chkOldValues.prop("checked") ? 1 : 0;
            $.cookie(viewModel.showOldValuesCookieName, isShowOldValues, { path: "/" });
            settingController.viewLogs();
        });

        settingView.elelments.txtUsername.keypress(function (e: any) {
            if (!e) {
                e = window.event;
            }

            var key = e.keyCode ? e.keyCode : e.which;
            if (key === alphaViewLogs.keyCode.enter) {
                settingView.pageNumber = 0;
                settingView.pageSize = 0;
                settingController.viewLogs();

                return alphaViewLogs.cancelBubbleEvent(e);
            }
        });

        var queryUsernameURL = alphaViewLogs.urls.queryUsername + "?" +
            $.param({
                issub: 0,
                ismember: 0
            });

        initAutoComplete($("#txt-username")[0], queryUsernameURL);

        settingView.renderPager({
            totalRecords: viewModel.totalRecords,
            pageSize: viewModel.pageSize,
            pageIndex: viewModel.pageIndex + 1
        });

        settingView.pageNumber = viewModel.pageIndex;
        settingView.pageSize = viewModel.pageSize;

        settingView.elelments.dataLog.stickyTableHeaders({ fixedOffset: settingView.elelments.navTabs });
    },
    initMonthPicker: function (viewModel: any) {
        settingView.elelments.ddlMonth.monPickr({
            longMonthsInYear: settingView.getLongMonthsInYear(),
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
            toDayLabel: viewModel.labelTo,
            sortDesc: true,
            log: function (msg: string) {
                console.warn(msg);
            }
        });
    },
    showLoadingState: function () {
        settingView.elelments.btnSubmit.attr("disabled", "disabled");
        settingView.elelments.iconLoading.removeClass("hide");
    },
    showCompleteLoadingState: function () {
        settingView.elelments.iconLoading.addClass("hide");
        settingView.elelments.btnSubmit.attr("disabled", null);
    },

    getParams: function () {
        return {
            PageIndex: settingView.pageNumber,
            PageSize: settingView.pageSize,
            Username: settingView.elelments.txtUsername.val(),
            FromDate: settingView.elelments.ddlMonth.monPickr("getFromDate", alphaViewLogs.format.shortDateWithSlash),
            ToDate: settingView.elelments.ddlMonth.monPickr("getToDate", alphaViewLogs.format.shortDateWithSlash),
            ActionTypeId: settingView.elelments.ddlLogActionType.val()
        };
    },
    renderPager: function (options: any) {
        if (alphaViewLogs.pageList[0] <= options.totalRecords) {
            settingView.elelments.pagination.pagination({
                pageList: alphaViewLogs.pageList,
                total: options.totalRecords,
                pageSize: options.pageSize,
                pageNumber: options.pageIndex,

                showRefresh: false,
                onSelectPage: function (pageNumber: number, pageSize: number) {
                    settingView.pageNumber = pageNumber - 1;
                    settingView.pageSize = pageSize;

                    settingController.viewLogs();
                },
                beforePageText: settingView.viewModel.Page,
                afterPageText: settingView.viewModel.of + " {pages}",
                displayMsg: settingView.viewModel.Pager_DisplayItems
            });

            settingView.elelments.pagination.removeClass("hide");
        } else {
            settingView.elelments.pagination.addClass("hide");
        }
    },
    getLongMonthsInYear: function () {
        var longMonthsInYear = $.parseJSON(settingView.elelments.longMonthsInYear.val());
        return longMonthsInYear;
    }
};