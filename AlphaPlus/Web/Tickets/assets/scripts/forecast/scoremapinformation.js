'use strict';
(function ($) {
    $(function () {
        $('.bettype-container .header').click(function () {
            $(this).toggleClass("collapse");
            $(this).next().stop().slideToggle();
        });
    });
})(window.jQuery);
