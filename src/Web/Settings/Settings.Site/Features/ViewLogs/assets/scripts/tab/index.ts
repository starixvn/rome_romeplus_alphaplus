"use strict";

var tabController = {
    viewLogsParams: null,
    init: function () {
        $(".lnk-changetab").click(function () {
            var url = $(this).data("tablink") + "?" + $.param({
                Username: tabController.viewLogsParams.Username,
                FromDate: tabController.viewLogsParams.FromDate,
                ToDate: tabController.viewLogsParams.ToDate,
                ActionTypeId: tabController.viewLogsParams.ActionTypeId
            });

            window.location.href = url;
        });
    }
};

$(function () {
    tabController.init();
});