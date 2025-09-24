class DormantAccountList {
    $: JQueryStatic;
    topWindow: Window;
    $formFilter: JQuery;
    levelComponent: SingleSelectionDropdownComponent;
    statusComponent: SingleSelectionDropdownComponent;
    $exportExcelForm: JQuery;
    pager: PagerComponent;
    closedTooltipTitle: string;
    compactFeedback: CompactFeedbackComponent;

    constructor(
        $: JQueryStatic,
        topWindow: Window,
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
        const pageSizeFirst = 10;
        const pageSizeSecond = 50;
        const pageSizeThird = 100;
        const pageSizeLast = 500;
        const pageSizeOptions = [pageSizeFirst, pageSizeSecond, pageSizeThird, pageSizeLast];
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
        const topPadding = 35;
        this.$('#popup-archiveProcess').css({
            bottom: 'auto',
            top: this.$('#icon-archiveProcess').position().top + topPadding
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
