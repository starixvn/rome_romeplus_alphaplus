class WeekComponent {
    private $weekContainer: JQuery;

    constructor() {
        this.$weekContainer = $('#week-container');
    }

    render(weeks, onClick: Function) {
        const html = [];
        weeks.forEach(function (week) {
            html.push(
                `<a href="javascript:;" class="label-week${week.IsEnable ? '' : ' disabled'}" data-fromdate="${
                    week.FromDate
                }" data-todate="${week.ToDate}">${week.Display}</a>`
            );
        });

        this.$weekContainer.html(html.join(''));
        const labelWeeks = this.$weekContainer.find('.label-week:not(.disabled)');
        labelWeeks.off().click((e) => {
            const $this = $(e.target);
            labelWeeks.removeClass('active');
            $this.addClass('active');
            onClick &&
                onClick({
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
