class WeekComponent {
    private onClick: Function;
    private $weekContainer: JQuery;

    init(onClick: Function) {
        this.onClick = onClick;
        this.$weekContainer = $('#week-container');
        this.bindEvents();
    }

    reRender(html: string) {
        this.$weekContainer.html(html);
        this.bindEvents();
    }

    bindEvents() {
        const labelWeeks = this.$weekContainer.find('.label-week:not(.disabled)');
        labelWeeks.off().click((e) => {
            const $this = $(e.target);
            labelWeeks.removeClass('active');
            $this.addClass('active');
            this.onClick && this.onClick({ fromDate: $this.data('fromdate'), toDate: $this.data('todate') });
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
