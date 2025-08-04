/// <reference path="../core.ts" />
/// <reference path="index-controller.ts" />

"use strict";
/*global cashserviceController, alphaViewLogs, viewModel*/
declare var $: JQueryStatic;
declare var initAutoComplete: Function;

var cashserviceView = {
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
        cashserviceView.elelments.ddlLogActionType = $("#ddl-logactiontype");
        cashserviceView.elelments.btnSubmit = $("#btn-submit");
        cashserviceView.elelments.ddlMonth = $("#ddlMonth");
        cashserviceView.elelments.iconLoading = $("#icon-loading");
        cashserviceView.elelments.chkOldValues = $("#chk-oldvalues");
        cashserviceView.elelments.txtUsername = $("#txt-username");
        cashserviceView.elelments.pagination = $("#pagination");
        cashserviceView.elelments.longMonthsInYear = $("#longMonthsInYear");
        cashserviceView.elelments.dataLog = $("#DataLog");
        cashserviceView.elelments.navTabs = $("#nav-tabs");

    },
    initElements: function (viewModel: any) {
        cashserviceView.cacheElements();

        cashserviceView.elelments.ddlLogActionType.change(function () {
            cashserviceView.pageNumber = 0;
            cashserviceView.pageSize = 0;

            cashserviceController.viewLogs();
        });

        cashserviceView.initMonthPicker(viewModel);

        cashserviceView.elelments.btnSubmit.click(function () {
            cashserviceView.pageNumber = 0;
            cashserviceView.pageSize = 0;

            cashserviceController.viewLogs();
        });

        cashserviceView.elelments.chkOldValues.click(function () {
            var isShowOldValues = cashserviceView.elelments.chkOldValues.prop("checked") ? 1 : 0;
            $.cookie(viewModel.showOldValuesCookieName, isShowOldValues, { path: "/" });
            cashserviceController.viewLogs();
        });

        cashserviceView.elelments.txtUsername.keypress(function (e: any) {
            if (!e) {
                e = window.event;
            }

            var key = e.keyCode ? e.keyCode : e.which;
            if (key === alphaViewLogs.keyCode.enter) {
                cashserviceView.pageNumber = 0;
                cashserviceView.pageSize = 0;
                cashserviceController.viewLogs();

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

        cashserviceView.renderPager({
            totalRecords: viewModel.totalRecords,
            pageSize: viewModel.pageSize,
            pageIndex: viewModel.pageIndex + 1
        });

        cashserviceView.pageNumber = viewModel.pageIndex;
        cashserviceView.pageSize = viewModel.pageSize;

        cashserviceView.elelments.dataLog.stickyTableHeaders({ fixedOffset: cashserviceView.elelments.navTabs });
    },
    initMonthPicker: function (viewModel: any) {
        cashserviceView.elelments.ddlMonth.monPickr({
            longMonthsInYear: cashserviceView.getLongMonthsInYear(),
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
        cashserviceView.elelments.btnSubmit.attr("disabled", "disabled");
        cashserviceView.elelments.iconLoading.removeClass("hide");
    },
    showCompleteLoadingState: function () {
        cashserviceView.elelments.iconLoading.addClass("hide");
        cashserviceView.elelments.btnSubmit.attr("disabled", null);
    },

    getParams: function () {
        return {
            PageIndex: cashserviceView.pageNumber,
            PageSize: cashserviceView.pageSize,
            Username: cashserviceView.elelments.txtUsername.val(),
            FromDate: cashserviceView.elelments.ddlMonth.monPickr("getFromDate", alphaViewLogs.format.shortDateWithSlash),
            ToDate: cashserviceView.elelments.ddlMonth.monPickr("getToDate", alphaViewLogs.format.shortDateWithSlash),
            ActionTypeId: cashserviceView.elelments.ddlLogActionType.val(),
            IsShowOldValue: cashserviceView.elelments.chkOldValues.prop("checked") ? 1 : 0
        };
    },
    renderPager: function (options: any) {
        if (alphaViewLogs.pageList[0] <= options.totalRecords) {
            cashserviceView.elelments.pagination.pagination({
                pageList: alphaViewLogs.pageList,
                total: options.totalRecords,
                pageSize: options.pageSize,
                pageNumber: options.pageIndex,

                showRefresh: false,
                onSelectPage: function (pageNumber: number, pageSize: number) {
                    cashserviceView.pageNumber = pageNumber - 1;
                    cashserviceView.pageSize = pageSize;

                    cashserviceController.viewLogs();
                },
                beforePageText: cashserviceView.viewModel.Page,
                afterPageText: cashserviceView.viewModel.of + " {pages}",
                displayMsg: cashserviceView.viewModel.Pager_DisplayItems
            });

            cashserviceView.elelments.pagination.removeClass("hide");
        } else {
            cashserviceView.elelments.pagination.addClass("hide");
        }
    },
    getLongMonthsInYear: function () {
        var longMonthsInYear = $.parseJSON(cashserviceView.elelments.longMonthsInYear.val());
        return longMonthsInYear;
    }
};