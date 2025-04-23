"use strict";

interface DataTablePagingOption {
    paging: boolean;
    pageLength: number;
    pageSizes: number[];
    pageSizeText: string;
    pageText: string;
    ofText: string;
}

class DataTableHelper {
    static initDataTable(tableReport: JQuery, options: any, pagingOptions: DataTablePagingOption = undefined) {
        let defaultOptions = {
            serverSide: true,
            responsive: {
                details: { type: "column", target: -1 }
            },
            columnDefs: [{
                className: "control",
                defaultContent: '',
                targets: -1,
                orderable: false
            }],
            stripeClasses: [],
            ordering: true,
            searching: false,
            bInfo: false,
            fixedHeader: true,
            processing: false,
            dom: 'rt<"bottom"iflp<"clear">>',
            drawCallback: _ => tableReport.removeClass("hidden")
        };

        options.language.processing = "";
        if (pagingOptions) {
            options.paging = pagingOptions.paging;
            options.pagingType = "input";
            options.pageLength = pagingOptions.pageLength;

            const pagingLanguage = this.buildDataTablePagingLanguage(pagingOptions);
            options.language.lengthMenu = pagingLanguage.lengthMenu;
            options.language.oPaginate = pagingLanguage.oPaginate;
        }

        const dataTable = tableReport.DataTable($.extend(defaultOptions, options));
        tableReport.find("thead tr th.control").click(event => this.collapseExpandAll(event, tableReport, dataTable));

        return dataTable;
    }

    private static buildDataTablePagingLanguage(options: DataTablePagingOption) {
        let lengthMenu = [`${options.pageSizeText} <select>`];
        options.pageSizes.forEach(function (item) {
            lengthMenu.push(`<option value="${item}">${item}</option>`);
        });
        lengthMenu.push("</select>");

        const oPaginate = {
            sFirst: "",
            sPrevious: "",
            sNext: "",
            sLast: "",
            info: `${options.pageText} _INPUT_ ${options.ofText} _TOTAL_`
        };

        return { lengthMenu: lengthMenu.join(), oPaginate };
    }

    private static collapseExpandAll(event, tableReport: JQuery, dataTable: JQuery) {
        $.blockUI();
        setTimeout(() => {
            const targetParent = $(event.currentTarget).parent();

            if (targetParent.hasClass("parent")) {
                targetParent.removeClass("parent");
                this.triggerClickDetailWhenSwitchAll(tableReport.find("tbody tr.parent"), dataTable);
            }
            else {
                targetParent.addClass("parent");
                this.triggerClickDetailWhenSwitchAll(tableReport.find("tbody tr").not(".parent"), dataTable);
            }

            $.unblockUI();
        }, 100);
    }

    private static triggerClickDetailWhenSwitchAll(controlArray, dataTable: JQuery) {
        controlArray.each((_, detailButton) => $(detailButton).find(".control").click());
        dataTable.fixedHeader.adjust();
    }
}
