class TotalEnteredNonEnteredComponent {
    private nonEnteredYesterdayContainer: JQuery;
    private eventDateYesterday: JQuery;
    private nonEnteredYesterday: JQuery;
    private enteredYesterdayContainer: JQuery;
    private eventDateToday: JQuery;
    private nonEnteredToday: JQuery;

    constructor() {
        this.cacheDOMElements();
    }

    private cacheDOMElements() {
        this.nonEnteredYesterdayContainer = $('#total-non-entered-yesterday');
        this.eventDateYesterday = $('.event-date-yesterday');
        this.nonEnteredYesterday = this.nonEnteredYesterdayContainer.find('#total-non-entered-yesterday-number');
        this.enteredYesterdayContainer = $('#total-entered-yesterday');
        const nonEnteredTodayContainer = $('#total-non-entered-today');
        this.eventDateToday = nonEnteredTodayContainer.find('#total-non-entered-today-text');
        this.nonEnteredToday = nonEnteredTodayContainer.find('#total-non-entered-today-number');
    }

    render(totalEnteredResults) {
        const totalEnteredResultYesterday = totalEnteredResults.EnteredResultYesterday;
        const hasNonEnteredYerterday = totalEnteredResultYesterday.NonEntered > 0;

        this.eventDateYesterday.text(totalEnteredResultYesterday.EventDateText);

        if (hasNonEnteredYerterday) {
            this.enteredYesterdayContainer.hide();

            this.nonEnteredYesterday.text(alphaReports.formatNumber(totalEnteredResultYesterday.NonEntered));
            this.nonEnteredYesterdayContainer.show();
        } else {
            this.nonEnteredYesterdayContainer.hide();
            this.enteredYesterdayContainer.show();
        }

        this.eventDateToday.text(totalEnteredResults.EnteredResultToday.EventDateText);
        this.nonEnteredToday.text(alphaReports.formatNumber(totalEnteredResults.EnteredResultToday.NonEntered));
    }
}
