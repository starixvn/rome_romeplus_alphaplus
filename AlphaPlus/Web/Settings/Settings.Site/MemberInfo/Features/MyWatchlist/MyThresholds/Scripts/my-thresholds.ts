/// <reference path="../../Shared/Scripts/my-watchlist-base.ts" />

"use strict";
interface Window {
    myThresholdData: any;
}

class MyThresholds extends MyWatchlistBase {
    private topWindow: any;
    private dateRangePickerComponent: DateRangePickerComponent;

    private $fromDate: JQuery;
    private $toDate: JQuery;

    private $dataTable: JQuery;
    private myThresholdData: any;

    private myThresholdsUrl: string;
    private isMobile: boolean;

    constructor(
        $: JQueryStatic,
        topWindow: any,
        dateRangePickerComponent: DateRangePickerComponent,
        myThresholdData: any) {
        super($);
        this.topWindow = topWindow;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.myThresholdData = myThresholdData;
    }

    public init() {
        super.initInfoLegend();

        this.cacheDOMElements();
        this.bindEvents();
    }

    private cacheDOMElements() {
        this.$fromDate = this.$("#from-date");
        this.$toDate = this.$("#to-date");
        this.myThresholdsUrl = this.$("#my-thresholds-url").val();
        this.isMobile = this.$("#is-mobile").val() === "1";
    }

    private bindEvents() {
        const $reportForm = this.$("#report-form");

        this.dateRangePickerComponent.render(new DateRangePickerOption());
        this.$dataTable = DataTableHelper.initDataTable(
            this.$("#tblRpt"),
            this.buildDataTableOptions($reportForm.attr("action")));

        $reportForm.submit(event => {
            event.preventDefault();
            this.$dataTable.ajax.reload();
        });
        this.$("#btn-my-thresholds").click(() =>
            this.topWindow.popupManager.open(
                `${this.myThresholdsUrl}/Settings`,
                undefined,
                600,
                310,
                "modal-mobile-friendly"));
    }

    private buildDataTableOptions(myThresholdsUrl: string) {
        const sortDefinitions = {
            CreatedDate: 1,
            WinningLosingAmount: 2,
            WinningThreshold: 3,
            LosingThreshold: 4
        };

        return {
            ajax: (data, callback) => {
                if (this.myThresholdData) {
                    callback({ data: this.myThresholdData });
                    delete this.myThresholdData;
                } else {
                    const columnName = data.columns[data.order[0].column].name;
                    const sortType = sortDefinitions[columnName];
                    const formData = {
                        fromDate: this.$fromDate.val(),
                        toDate: this.$toDate.val(),
                        sortType,
                        isDesc: data.order[0].dir === "desc"
                    };
                    AJAXHelper.postJSON(myThresholdsUrl, formData, jsonData => callback({ data: jsonData }));
                }
            },
            columns: this.buildDataTableColumns(),
            order: [this.isMobile ? 0 : 1, "desc"],
            language: {
                zeroRecords: this.$("#no-info").val()
            },
            paging: false
        };
    }

    private buildDataTableColumns() {
        let columns = [];

        !this.isMobile && columns.push({ className: "center", data: "RowNumber", orderable: false });

        columns.push(
            { className: "center", data: "CreatedDateString", name: "CreatedDate" },
            {
                className: "right",
                data: "WinLossAmountString",
                name: "WinningLosingAmount",
                render: function (data, type, row) {
                    return type === "display"
                        ? `<span class="${row.ReachType === 1 ? "winning-reach" : "losing-reach"}">${data}</span>`
                        : data;
                }
            },
            {
                className: "right",
                data: "WinningThresholdString",
                name: "WinningThreshold",
                render: function (data, type, row) {
                    return type === "display"
                        ? `<span class="${row.ReachType === 1 ? "winning-reach" : ""}">${data}</span>`
                        : data;
                }
            },
            {
                className: "right",
                data: "LosingThresholdString",
                name: "LosingThreshold",
                render: function (data, type, row) {
                    return type === "display"
                        ? `<span class="${row.ReachType === 2 ? "losing-reach" : ""}">${data}</span>`
                        : data;
                }
            },
            { data: null }
        );

        return columns;
    }
}

$(function () {
    const dateRangePickerComponent = new DateRangePickerComponent($);
    new MyThresholds($, window.top, dateRangePickerComponent, window.myThresholdData).init();
})