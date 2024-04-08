class Message {
    private $: JQueryStatic;
    dateRangePickerComponent: DateRangePickerComponent;
    constraintFromTo = 6;
    pagerComponent: PagerComponent;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        pagerComponent: PagerComponent) {
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.pagerComponent = pagerComponent;
        this.$ = $;
    }

    registerEvents = () => {
        var self = this;
        this.$('.nav-tabs > li').on('click', function () {
            self.$.blockUI();
            self.$(this).find('span').val('');
        });
        this.$('.btn-submit').on('click', () => {
            this.$.blockUI();
        });
        this.dateRangePickerComponent.render(new DateRangePickerOption("daterange-picker", "report-form", this.constraintFromTo));
        this.pagerComponent.render();
        this.$("#report-form").on("click", this.resetPaging);
        this.$("#icon-information").on("click", this.showGuidelines);
    }

    resetPaging = () => {
        this.$("#PageIndex").val(1);
    };

    showGuidelines = () => {
        const iconInformation = this.$("#icon-information");
        const guideNote = this.$("#guide-note");
        guideNote.modal({ show: true, backdrop: false });
        const iconHelpPosition = iconInformation.position();
        guideNote.css({
            bottom: "auto",
            top: iconHelpPosition.top + 40,
        });
    };
}

$(() => {
    const message = new Message($,
        new DateRangePickerComponent($),
        new PagerComponent($));
    message.registerEvents();
});