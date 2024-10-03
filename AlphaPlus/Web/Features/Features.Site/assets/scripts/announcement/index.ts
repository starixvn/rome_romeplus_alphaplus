class Announcement {
    private $: JQueryStatic;
    constraintFromTo = 6;   
    dateRangePickerComponent: DateRangePickerComponent;

    constructor($: JQueryStatic, dateRangePickerComponent: DateRangePickerComponent) {
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.$ = $;
    }

    init = () => {        
        this.registerEvents();
    }

    registerEvents = () => {
        var self = this;

        this.$('.nav-tabs > li').on('click', function () {
            self.$(this).find('span').val('');
        });
        this.$('.btn-submit').on('click', () => {
            this.$.blockUI();
        });

        this.dateRangePickerComponent.render(new DateRangePickerOption("daterange-picker", "report-form", this.constraintFromTo));

        this.$("#icon-information").on("click", this.showGuidelines);
    }

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
    const message = new Announcement($, new DateRangePickerComponent($));
    message.init();
});