class Message {
    private $: JQueryStatic;
    dateRangePickerComponent: DateRangePickerComponent;
    constraintFromTo = 6;
    pagerComponent: PagerComponent;
    compactFeedback: CompactFeedbackComponent;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        pagerComponent: PagerComponent,
        compactFeedback: CompactFeedbackComponent
    ) {
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.pagerComponent = pagerComponent;
        this.$ = $;
        this.compactFeedback = compactFeedback;
    }

    registerEvents = () => {
        const this$ = this.$;
        const $reportForm = this$('#report-form');
        this$('.nav-tabs > li').on('click', function () {
            this$(this).find('span').val('');
        });
        this$('.btn-submit').on('click', () => this$.blockUI());
        this.dateRangePickerComponent.render(
            new DateRangePickerOption('daterange-picker', 'report-form', this.constraintFromTo)
        );
        this.pagerComponent.render();
        $reportForm.on('click', this.resetPaging);
        this$('#icon-information').on('click', this.showGuidelines);
        this.compactFeedback.init($reportForm);
    };

    resetPaging = () => {
        this.$('#PageIndex').val(1);
    };

    showGuidelines = () => {
        const iconInformation = this.$('#icon-information');
        const guideNote = this.$('#guide-note');
        guideNote.modal({ show: true, backdrop: false });
        const iconHelpPosition = iconInformation.position();
        const paddingTop = 40;
        guideNote.css({
            bottom: 'auto',
            top: iconHelpPosition.top + paddingTop
        });
    };
}

$(() => {
    const message = new Message(
        $,
        new DateRangePickerComponent($),
        new PagerComponent($),
        new CompactFeedbackComponent()
    );
    message.registerEvents();
});
