/// <reference path="../core.ts" />
/// <reference path="index-controller.ts" />

/*jslint node: true */
"use strict";
/*global creditController, alphaViewLogs, viewModel*/
declare var $: JQueryStatic;
declare var initAutoComplete: Function;

var creditView = {
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
        iconExcel: null,
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
        creditView.elelments.btnSubmit = $("#btn-submit");
        creditView.elelments.iconExcel = $("#icon-excel");
        creditView.elelments.ddlMonth = $("#ddlMonth");
        creditView.elelments.iconLoading = $("#icon-loading");
        creditView.elelments.txtUsername = $("#txt-username");
        creditView.elelments.pagination = $("#pagination");
        creditView.elelments.longMonthsInYear = $("#longMonthsInYear");
        creditView.elelments.dataLog = $("#DataLog");
        creditView.elelments.navTabs = $("#nav-tabs");
    },
    initElements: function (viewModel: any) {
        creditView.cacheElements();

        creditView.initMonthPicker(viewModel);

        creditView.elelments.btnSubmit.click(function () {
            creditView.pageNumber = 0;
            creditView.pageSize = 0;

            creditController.viewLogs();
        });

        creditView.elelments.iconExcel.click(function () {
            creditController.exportExcel();
        });

        creditView.elelments.txtUsername.keypress(function (e: any) {
            if (!e) {
                e = window.event;
            }

            var key = e.keyCode ? e.keyCode : e.which;
            if (key === alphaViewLogs.keyCode.enter) {
                creditView.pageNumber = 0;
                creditView.pageSize = 0;
                creditController.viewLogs();

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

        creditView.renderPager({
            totalRecords: viewModel.totalRecords,
            pageSize: viewModel.pageSize,
            pageIndex: viewModel.pageIndex + 1
        });

        creditView.pageNumber = viewModel.pageIndex;
        creditView.pageSize = viewModel.pageSize;
        creditView.elelments.dataLog.stickyTableHeaders({ fixedOffset: creditView.elelments.navTabs });
    },
    initMonthPicker: function (viewModel: any) {
        creditView.elelments.ddlMonth.monPickr({
            longMonthsInYear: creditView.getLongMonthsInYear(),
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
        creditView.elelments.btnSubmit.attr("disabled", "disabled");
        creditView.elelments.iconLoading.removeClass("hide");
    },
    showCompleteLoadingState: function () {
        creditView.elelments.iconLoading.addClass("hide");
        creditView.elelments.btnSubmit.attr("disabled", null);
    },

    getParams: function () {
        return {
            ActionTypeId: creditView.viewModel.actionTypeId,
            PageIndex: creditView.pageNumber,
            PageSize: creditView.pageSize,
            Username: creditView.elelments.txtUsername.val(),
            FromDate: creditView.elelments.ddlMonth.monPickr("getFromDate", alphaViewLogs.format.shortDateWithSlash),
            ToDate: creditView.elelments.ddlMonth.monPickr("getToDate", alphaViewLogs.format.shortDateWithSlash)
        };
    },
    renderPager: function (options: any) {
        if (alphaViewLogs.pageList[0] <= options.totalRecords) {
            creditView.elelments.pagination.pagination({
                pageList: alphaViewLogs.pageList,
                total: options.totalRecords,
                pageSize: options.pageSize,
                pageNumber: options.pageIndex,

                showRefresh: false,
                onSelectPage: function (pageNumber: number, pageSize: number) {
                    creditView.pageNumber = pageNumber - 1;
                    creditView.pageSize = pageSize;

                    creditController.pageLogs();
                },
                beforePageText: creditView.viewModel.Page,
                afterPageText: creditView.viewModel.of + " {pages}",
                displayMsg: creditView.viewModel.Pager_DisplayItems
            });

            creditView.elelments.pagination.removeClass("hide");
        } else {
            creditView.elelments.pagination.addClass("hide");
        }
    },
    getLongMonthsInYear: function () {
        // $.parseJSON: string must start/end with "
        var longMonthsInYear = $.parseJSON(creditView.elelments.longMonthsInYear.val());
        return longMonthsInYear;
    }
};