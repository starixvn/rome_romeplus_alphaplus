/// <reference path="../core.ts" />
var Kickout = /** @class */ (function () {
    function Kickout() {
        var _this = this;
        this.init = function () {
            _this.message = document.getElementById('message') != null
                ? document.getElementById('message').value
                : "Sorry, there was an unexpected issue on the server. Please seek help from our customer support. [5003]";
            alpha.core.signOut(_this.message);
            alert(_this.message);
        };
    }
    return Kickout;
}());
$(function () {
    var kickOut = new Kickout();
    kickOut.init();
});
//# sourceMappingURL=kick-out.js.map