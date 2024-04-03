/// <reference path="../core.ts" />
/// <reference path="index-controller.ts" />

"use strict";
/*global loginController, alphaViewLogs, viewModel*/
declare var $: JQueryStatic;
declare var initAutoComplete: Function;

var loginView = {
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
        actionTypeId: (<HTMLInputElement>document.getElementById("actionTypeId")).value
    },
    elelments: {
        btnSubmit: null,
        ddlMonth: null,
        iconLoading: null,
        txtUsername: null,
        pagination: null,
        longMonthsInYear: null,
        dataLog: null,
        navTabs: null
    },
    pageNumber: 0,
    pageSize: 0,
    cacheElements: function () {
        loginView.elelments.btnSubmit = $("#btn-submit");
        loginView.elelments.ddlMonth = $("#ddlMonth");
        loginView.elelments.iconLoading = $("#icon-loading");
        loginView.elelments.txtUsername = $("#txt-username");
        loginView.elelments.pagination = $("#pagination");
        loginView.elelments.longMonthsInYear = $("#longMonthsInYear");
        loginView.elelments.dataLog = $("#DataLog");
        loginView.elelments.navTabs = $("#nav-tabs");
    },
    initElements: function (viewModel: any) {
        loginView.cacheElements();

        loginView.initMonthPicker(viewModel);

        loginView.elelments.btnSubmit.click(function () {
            loginView.pageNumber = 0;
            loginView.pageSize = 0;

            loginController.viewLogs();
        });

        loginView.elelments.txtUsername.keypress(function (e: any) {
            if (!e) {
                e = window.event;
            }

            var key = e.keyCode ? e.keyCode : e.which;
            if (key === alphaViewLogs.keyCode.enter) {
                loginView.pageNumber = 0;
                loginView.pageSize = 0;
                loginController.viewLogs();

                return alphaViewLogs.cancelBubbleEvent(e);
            }
        });

        var queryUsernameURL = alphaViewLogs.urls.queryUsername + "?" +
            $.param({
                issub: 0,
                ismember: 0
            });

        initAutoComplete($("#txt-username")[0], queryUsernameURL);

        loginView.renderPager({
            totalRecords: viewModel.totalRecords,
            pageSize: viewModel.pageSize,
            pageIndex: viewModel.pageIndex + 1
        });

        loginView.pageNumber = viewModel.pageIndex;
        loginView.pageSize = viewModel.pageSize;
        loginView.elelments.dataLog.stickyTableHeaders({ fixedOffset: loginView.elelments.navTabs });
    },
    initMonthPicker: function (viewModel: any) {
        loginView.elelments.ddlMonth.monPickr({
            longMonthsInYear: loginView.getLongMonthsInYear(),
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
            toDayLabel: loginView.viewModel.labelTo,
            sortDesc: true,
            log: function (msg: string) {
                console.warn(msg);
            }
        });
    },
    showLoadingState: function () {
        loginView.elelments.btnSubmit.attr("disabled", "disabled");
        loginView.elelments.iconLoading.removeClass("hide");
    },
    showCompleteLoadingState: function () {
        loginView.elelments.iconLoading.addClass("hide");
        loginView.elelments.btnSubmit.attr("disabled", null);
    },

    getParams: function () {
        return {
            ActionTypeId: loginView.viewModel.actionTypeId,
            PageIndex: loginView.pageNumber,
            PageSize: loginView.pageSize,
            Username: loginView.elelments.txtUsername.val(),
            FromDate: loginView.elelments.ddlMonth.monPickr("getFromDate", alphaViewLogs.format.shortDateWithSlash),
            ToDate: loginView.elelments.ddlMonth.monPickr("getToDate", alphaViewLogs.format.shortDateWithSlash)
        };
    },
    renderPager: function (options: any) {
        if (alphaViewLogs.pageList[0] <= options.totalRecords) {
            loginView.elelments.pagination.pagination({
                pageList: alphaViewLogs.pageList,
                total: options.totalRecords,
                pageSize: options.pageSize,
                pageNumber: options.pageIndex,

                showRefresh: false,
                onSelectPage: function (pageNumber: number, pageSize: number) {
                    loginView.pageNumber = pageNumber - 1;
                    loginView.pageSize = pageSize;

                    loginController.pageLogs();
                },
                beforePageText: loginView.viewModel.Page,
                afterPageText: loginView.viewModel.of + " {pages}",
                displayMsg: loginView.viewModel.Pager_DisplayItems
            });

            loginView.elelments.pagination.removeClass("hide");
        } else {
            loginView.elelments.pagination.addClass("hide");
        }
    },
    getLongMonthsInYear: function () {
        var longMonthsInYear = $.parseJSON(loginView.elelments.longMonthsInYear.val());
        return longMonthsInYear;
    }
};