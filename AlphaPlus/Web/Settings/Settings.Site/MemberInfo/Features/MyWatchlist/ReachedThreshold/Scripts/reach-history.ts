"use strict";

interface Window {
    reachHistoryData: any;
}

class ReachHistory {
    private $: JQueryStatic;

    private $selectedCustId: JQuery;
    private $fromDate: JQuery;
    private $toDate: JQuery;
    private $limitType: JQuery;

    private reachHistoryUrl: string;
    private reachHistoryData: any;

    constructor($: JQueryStatic, reachHistoryData: any) {
        this.$ = $;
        this.reachHistoryData = reachHistoryData;
    }

    public init() {
        this.$selectedCustId = this.$('#selected-cust-id');
        this.$fromDate = this.$('#from-date');
        this.$toDate = this.$('#to-date');
        this.$limitType = this.$('#limit-type');
        this.reachHistoryUrl = this.$('#reach-history-url').val();

        DataTableHelper.initDataTable(this.$('#tblRpt'), this.buildDataTableOptions());
    }

    private buildDataTableOptions() {
        const sortDefinitions = {
            DateTime: 1,
            ThresholdType: 2,
            WinningLosingAmount: 3,
            WinningThreshold: 4,
            LosingThreshold: 5
        };

        return {
            ajax: (data, callback) => {
                if (this.reachHistoryData) {
                    callback({ data: this.reachHistoryData });
                    delete this.reachHistoryData;
                } else {
                    const columnName = data.columns[data.order[0].column].name;
                    const sortType = sortDefinitions[columnName];
                    const formData = {
                        selectedCustId: this.$selectedCustId.val(),
                        fromDate: this.$fromDate.val(),
                        toDate: this.$toDate.val(),
                        limitType: this.$limitType.val(),
                        sortType,
                        isDesc: data.order[0].dir === 'desc'
                    };
                    AJAXHelper.postJSON(this.reachHistoryUrl, formData, jsonData => callback({ data: jsonData }));
                }
            },
            responsive: false,
            paging: false,
            columns: this.buildDataTableColumns(),
            order: [[1, 'desc']],
            language: {
                emptyTable: this.$("#no-info").val()
            }
        };
    }

    private buildDataTableColumns() {
        return [
            { "className": 'center', "data": "RowNumber", "orderable": false },
            { "className": 'center', "data": "CreatedDateString", "name": "CreatedDate" },
            { "className": 'center', "data": "TranslatedLimitTypeReach", "name": "ThresholdType" },
            { "className": 'right', "name": "WinningLosingAmount", "data": item => `<div class="${item.ReachType === 1 ? 'winning-reach' : 'losing-reach'}">${item.WinLossAmountString}</div>` },
            { "className": 'right', "name": "WinningThreshold", "data": item => `<div class="${item.ReachType === 1 ? 'winning-reach' : ''}">${item.WinningThresholdString}</div>` },
            { "className": 'right', "name": "LosingThreshold", "data": item => `<div class="${item.ReachType === 2 ? 'losing-reach' : ''}">${item.LosingThresholdString}</div>` }
        ];
    }
}

$(function () {
    new ReachHistory($, window.reachHistoryData).init();
});