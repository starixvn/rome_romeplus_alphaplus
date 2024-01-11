'use strict';
(function ($) {
    const $content = $('#content');
    const dataUrl = $('#hidden-dataUrl').val();

    function bindLeagueChangeEvent() {
        $('#ddlLeague')
            .off('change')
            .change(function () {
                $.get(dataUrl + this.value, function (html) {
                    $('#tblRpt tbody').html(html);
                });
            });
    }

    function initAutoRefresh() {
        const autoRefreshSeconds = $('#hidden-autoRefreshSeconds')
            .val()
            .split(',')
            .map(function (second) {
                return +second;
            });
        const autoRefreshUrl = $('#hidden-autoRefreshUrl').val();
        $('.auto-refresh').fanexAutoRefresh({
            buttonRefreshTooltip: $('#hidden-autoRefreshRefreshText').val(),
            seconds: autoRefreshSeconds,
            refreshHandler: function () {
                const $ddlLeague = $('#ddlLeague');
                $.get(autoRefreshUrl + ($ddlLeague.length ? $ddlLeague.val() : ''), function (html) {
                    $content.html(html);
                    bindLeagueChangeEvent();
                });
            },
            savedSecondKey: $('#hidden-autoRefreshSavedSecondKey').val()
        });
    }

    $(function () {
        initAutoRefresh();
        bindLeagueChangeEvent();
    });
})(window.jQuery);
