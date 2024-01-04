(function ($) {
    function FanexAutoRefresh($element, options) {
        this._init($element, options);
    }

    FanexAutoRefresh.prototype = {
        /* Public Functions */
        /* Private Functions */
        _defaults: {
            buttonRefreshTooltip: 'Refresh',
            seconds: [15, 30, 45, 60, 75],
            refreshHandler: null,
            savedSecondKey: 'auto-refresh'
        },

        _init: function ($element, options) {
            this.options = $.extend({}, this._defaults, options);
            const savedSecond = sessionStorage.getItem(this.options.savedSecondKey);
            if (savedSecond) {
                this.options.selectedSecond = +savedSecond;
            } else if (!this.options.selectedSecond) {
                this.options.selectedSecond = this.options.seconds[0];
            }

            $element.css('position', 'relative');
            $element.html(this._buildAutoRefresh());
            this.$autoRefresh = $element;
            this.$btnRefresh = $element.find('.refresh-button');
            this._registerEvents();
            this._start();
        },

        _buildAutoRefresh: function () {
            const html = [
                '<div class="refresh-countdown">',
                '<div class="refresh-button" title="',
                this.options.buttonRefreshTooltip,
                '"></div>',
                '<div class="refresh-arrow-down"></div></div>'
            ];
            html.push('<ul class="refresh-dropdown">');
            this.options.seconds.forEach((second) => {
                html.push('<li class="refresh-dropdown-item');
                if (this.options.selectedSecond === second) {
                    html.push(' selected');
                }
                html.push('">', second, '</li>');
            });
            html.push('</ul>');
            return html.join('');
        },

        _registerEvents: function () {
            const $dropdown = this.$autoRefresh.find('.refresh-dropdown');
            const $dropdownItems = $dropdown.find('.refresh-dropdown-item');

            this.$btnRefresh.click(() => {
                this.options.refreshHandler && this.options.refreshHandler();
                this._start();
            });

            this.$autoRefresh.find('.refresh-arrow-down').click(function () {
                $dropdown.toggleClass('open');
            });

            $dropdownItems.click((e) => {
                const $this = $(e.target);
                $dropdownItems.removeClass('selected');
                $this.addClass('selected');
                $dropdown.removeClass('open');

                const second = $this.text();
                sessionStorage.setItem(this.options.savedSecondKey, second);
                this.options.selectedSecond = +second;
                this._start();
            });

            $(document).click((e) => this.$autoRefresh[0].contains(e.target) || $dropdown.removeClass('open'));
        },

        _start: function () {
            clearTimeout(this.timeout);
            this.$btnRefresh.text(this.options.selectedSecond);
            this.timeout = setTimeout(() => this._tick(), 1000);
        },

        _tick: function () {
            let value = +this.$btnRefresh.text();
            if (isNaN(value)) {
                this.$btnRefresh.text(this.options.selectedSecond);
            } else if (value === 1) {
                clearTimeout(this.timeout);
                this.$btnRefresh.text('...');
                this.options.refreshHandler && this.options.refreshHandler();
            } else {
                this.$btnRefresh.text(--value);
            }

            this.timeout = setTimeout(() => this._tick(), 1000);
        }
    };

    $.fn.fanexAutoRefresh = function (option, parameter) {
        let data;
        if (typeof option === 'string') {
            data = this.data('fanexAutoRefresh');
            return data[option](parameter);
        }

        return this.each(function () {
            const $this = $(this);
            data = $this.data('fanexAutoRefresh');

            if (!data) {
                const options = typeof option === 'object' && option;
                data = new FanexAutoRefresh($this, options);
                $this.data('fanexAutoRefresh', data);
            }
        });
    };

    $.fn.fanexAutoRefresh.Constructor = FanexAutoRefresh;
})(window.jQuery);
