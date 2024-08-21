class WeekComponent {
    private $weekContainer: JQuery;
    private onClick: Function;

    init($weekContainer: JQuery, onClick: Function) {
        this.$weekContainer = $weekContainer;
        this.onClick = onClick;
        this.bindEvent();
    }

    render(html: string) {
        this.$weekContainer.html(html);
        this.bindEvent();
    }

    private bindEvent() {
        const labelWeeks = this.$weekContainer.find('.label-week:not(.disabled)');
        labelWeeks.off().click((e) => {
            const $this = $(e.target);
            labelWeeks.removeClass('active');
            $this.addClass('active');
            this.onClick &&
                this.onClick({
                    weekFromDate: $this.data('fromdate'),
                    weekToDate: $this.data('todate')
                });
        });
    }

    getActiveWeek() {
        const activeWeek = this.$weekContainer.find('.label-week.active');
        if (activeWeek.length) {
            const firstActiveWeek = activeWeek.eq(0);
            return {
                weekFromDate: firstActiveWeek.data('fromdate'),
                weekToDate: firstActiveWeek.data('todate')
            };
        }
    }
}
