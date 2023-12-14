/// <reference path="../core.ts" />
var ApiKeyStatistics = /** @class */ (function () {
    function ApiKeyStatistics($, topWindow) {
        var _this = this;
        this.init = function () {
            _this.cacheElements();
            _this.initDateTimePicker();
        };
        this.cacheElements = function () {
            _this.$datePicker = _this.$("#dpkContainer");
            _this.$datePickerInput = _this.$("#input-date");
        };
        this.initDateTimePicker = function () {
            var options = {
                allowInputToggle: true,
                minDate: new Date(2022, 1, 1),
                maxDate: new Date(2023, 1, 1),
                format: "MM/DD/YYYY"
            };
            _this.$datePicker.datepicker(options);
        };
        this.$ = $;
        this.topWindow = topWindow;
    }
    return ApiKeyStatistics;
}());
$(function () {
    var apiKeyStatistics = new ApiKeyStatistics($, window.top);
    apiKeyStatistics.init();
});
//# sourceMappingURL=apikey-statistics.js.map