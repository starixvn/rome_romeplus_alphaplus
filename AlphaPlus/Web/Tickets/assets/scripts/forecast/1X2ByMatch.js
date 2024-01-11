'use strict';
(function ($) {
    $(function () {
        const $tblRpt = $('#tblRpt tbody');
        const autoRefreshUrl = $('#hidden-autoRefreshUrl').val();
        $('.auto-refresh').fanexAutoRefresh({
            buttonRefreshTooltip: $('#hidden-autoRefreshRefreshText').val(),
            seconds: [15, 30, 45, 60, 75],
            refreshHandler: function () {
                $.get(autoRefreshUrl, function (html) {
                    $tblRpt.html(html);
                });
            },
            savedSecondKey: '1X2ByMatch'
        });
    });
})(window.jQuery);
