class MoveStatementHistory extends BaseReport {
    monthPickerComponent: MonthPickerComponent;
    pagerComponent: PagerComponent;

    constructor(
        $: JQueryStatic,
        monthPickerComponent: MonthPickerComponent,
        pagerComponent: PagerComponent,
        linkBuilderComponent: LinkBuilderComponent
    ) {
        super($, linkBuilderComponent);
        this.monthPickerComponent = monthPickerComponent;
        this.pagerComponent = pagerComponent;
    }

    public init() {
        this.$.blockUI();

        this.initComponents();
        this.bindEvents();

        this.$.unblockUI();
    }

    public initComponents() {
        this.monthPickerComponent.render();

        this.pagerComponent.pageList = [10, 50, 100, 500];
        this.pagerComponent.render();
    }

    public bindEvents() {
        this.$reportForm.submit(() => this.$.blockUI());

        this.$('.btn-submit').click(() => {
            this.$('#IsMainReport').val('true');
            this.$('#PageIndex').val('');
            this.$('#PageSize').val('');
        });

        this.$('#back-link').click(() => {
            this.$('#IsMainReport').val('true');
            this.$reportForm.submit();
        });

        this.$('#detail-link').click(() => {
            this.$('#IsMainReport').val('false');
            this.$reportForm.submit();
        });
    }
}

$(function () {
    new MoveStatementHistory($, new MonthPickerComponent($), new PagerComponent($), new LinkBuilderComponent($)).init();
});
