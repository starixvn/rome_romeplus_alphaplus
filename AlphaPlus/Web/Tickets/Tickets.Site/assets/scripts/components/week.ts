class WeekComponent {
    private onClick: Function;

    init(onClick: Function) {
        this.onClick = onClick;
        this.bindEvents();
    }

    bindEvents() {
        const labelWeeks = $('#week-container').find('.label-week:not(.disabled)');
        labelWeeks.off().click((e) => {
            const $this = $(e.target);
            labelWeeks.removeClass('active');
            $this.addClass('active');
            this.onClick && this.onClick({ fromDate: $this.data('fromdate'), toDate: $this.data('todate') });
        });
    }

    getActiveWeek() {
        const activeWeek = $('#week-container').find('.label-week.active');
        if (activeWeek.length) {
            const firstActiveWeek = activeWeek.eq(0);
            return {
                weekFromDate: firstActiveWeek.data('fromdate'),
                weekToDate: firstActiveWeek.data('todate')
            };
        }
    }
}
