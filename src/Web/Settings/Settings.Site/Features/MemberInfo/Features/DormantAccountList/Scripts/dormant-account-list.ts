class DormantAccountList {
    $: JQueryStatic;
    topWindow: unknown;
    $formFilter: JQuery;
    levelComponent: SingleSelectionDropdownComponent;
    statusComponent: SingleSelectionDropdownComponent;
    $exportExcelForm: JQuery;
    pager: PagerComponent;
    closedTooltipTitle: string;
    compactFeedback: CompactFeedbackComponent;

    constructor(
        $: JQueryStatic,
        topWindow: unknown,
        levelComponent: SingleSelectionDropdownComponent,
        statusComponent: SingleSelectionDropdownComponent,
        pager: PagerComponent,
        compactFeedback: CompactFeedbackComponent
    ) {
        this.$ = $;
        this.topWindow = topWindow;
        this.levelComponent = levelComponent;
        this.statusComponent = statusComponent;
        this.pager = pager;
        this.compactFeedback = compactFeedback;
    }

    init(): void {
        this.cacheDOMElements();
        this.bindEvents();

        this.levelComponent.render(new SingleSelectionDropdownOption('LevelId', 'HiddenLevelId'));
        this.statusComponent.render(new SingleSelectionDropdownOption('StatusId', 'HiddenStatusId'));
        const pageSizeOptions = [10, 50, 100, 500];
        this.pager.render(
            new PagerOption(
                () => this.$formFilter.submit(),
                'report-pager',
                'form-filter',
                'PageIndex',
                'PageSize',
                pageSizeOptions
            )
        );

        this.$('#tbl-report').stickyTableHeaders();

        this.$('#popup-archiveProcess').css({
            bottom: 'auto',
            top: this.$('#icon-archiveProcess').position().top + 35
        });

        $('[data-toggle="tooltip"]').tooltip({
            placement: 'right',
            template: '<div class="tooltip" role="tooltip"><div class="tooltip-inner"></div></div>',
            title: this.closedTooltipTitle
        });
        this.compactFeedback.init(this.$formFilter);
    }

    cacheDOMElements(): void {
        this.$formFilter = this.$('#form-filter');
        this.$exportExcelForm = this.$('#export-excel-form');

        const $closedTooltip = $('#closed-tooltip-title');
        this.closedTooltipTitle = $closedTooltip.val();
        $closedTooltip.remove();
    }

    bindEvents(): void {
        this.$formFilter.submit(this.submitForm);
        this.$('#btn-submit').click(() => this.$('#PageIndex').val('1'));
        this.$('#icon-excel').on('click', this.exportExcel);
    }

    submitForm = () => {
        this.$.blockUI();
    };

    exportExcel = () => {
        this.$exportExcelForm.submit();
    };
}

$(() => {
    new DormantAccountList(
        $,
        window.top,
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new PagerComponent($),
        new CompactFeedbackComponent()
    ).init();
});
