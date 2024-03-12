/// <reference path="../core.ts" />
/// <reference path="../base-report.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/client-pager.ts" />
/// <reference path="../components/link-builder.ts" />
/// <reference path="statement.ts" />

$(function () {
    $.blockUI();
    new StatementView(
        $,
        new DateRangePickerComponent($),
        new ClientPagerComponent($),
        window.top,
        new LinkBuilderComponent($)
    ).init();
    $.unblockUI();
});
