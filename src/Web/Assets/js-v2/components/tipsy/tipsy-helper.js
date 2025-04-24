"use strict";
var TipsyHelper = (function () {
    return {
        /*
         * Southwest    South	Southeast
         * East                 West
         * Northwest    North   Northeast
         */
        // gravity: sw | s | se | e | w | nw | n | ne
        showError: function ($element, message, gravity, autoFocus) {
            $element.attr('title', message).addClass('error');
            $element.tipsy({ trigger: 'manual', gravity: gravity || 'nw', className: 'error' }).tipsy('show');

			if (typeof autoFocus === 'undefined') {
				autoFocus = true;
			}
			
            // FireFox will not re-focus to the control until blur event finshes.
            autoFocus && setTimeout(function () { $element.focus(); }, 0);

            // Hide error when changing the field.
            $element.is('input') && $element.off("change").change(() => this.clear($element));
        },
        clear: function ($element) {
            $element.tipsy('hide');
            $element.removeAttr('original-title').removeClass('error');
        },
        clearAll: function () {
            $('[original-title]').each((_, item) => this.clear($(item)));
        }
    };
}());