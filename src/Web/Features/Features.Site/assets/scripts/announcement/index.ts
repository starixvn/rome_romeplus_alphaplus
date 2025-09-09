class Announcement {
    private $: JQueryStatic;
    constraintFromTo = 6;
    dateRangePickerComponent: DateRangePickerComponent;
    compactFeedback: CompactFeedbackComponent;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        compactFeedback: CompactFeedbackComponent
    ) {
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.$ = $;
        this.compactFeedback = compactFeedback;
    }

    init = () => {
        this.registerEvents();
        this.compactFeedback.init(this.$('#report-form'));
    };

    registerEvents = () => {
        const this$ = this.$;

        this.$('.nav-tabs > li').on('click', function () {
            this$(this).find('span').val('');
        });
        this.$('.btn-submit').on('click', () => {
            this.$.blockUI();
        });

        this.dateRangePickerComponent.render(
            new DateRangePickerOption('daterange-picker', 'report-form', this.constraintFromTo)
        );

        this.$('#icon-information').on('click', this.showGuidelines);
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
    const message = new Announcement($, new DateRangePickerComponent($), new CompactFeedbackComponent());
    message.init();
});
