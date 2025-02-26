interface JQuery {
    pagination(option: any): void;
}

class PagerOption {
    constructor(
        public onSelectPage: () => void = null,
        public pagerId: string = 'betlist-pager',
        public formId: string = 'betlist-form',
        public pageIndexId: string = 'PageIndex',
        public pageSizeId: string = 'PageSize'
    ) {}
}

class PagerComponent {
    $pageIndex: JQuery;
    $pageSize: JQuery;
    $betlistForm: JQuery;

    totalRecords: number;
    pageSize: number;
    pageIndex: number;

    pageList = [1000, 2000, 3000, 5000];
    smallPageList = [10, 50, 100, 500];

    render = (options: PagerOption = new PagerOption()) => {
        const pagerOption = <PagerOption>$.extend({}, this.options, options);
        const pager = $('#' + pagerOption.pagerId);
        this.totalRecords = pager.data('totalrecords');

        if (this.isShow()) {
            this.pageSize = pager.data('pagesize');
            this.pageIndex = pager.data('pageindex');
            this.$pageIndex = $(
                `<input type="hidden" id="${pagerOption.pageIndexId}" name="${pagerOption.pageIndexId}" value="${this.pageIndex}" />`
            );
            this.$pageSize = $(
                `<input type="hidden" id="${pagerOption.pageSizeId}" name="${pagerOption.pageSizeId}" value="${this.pageSize}" />`
            );

            this.$betlistForm = $('#' + this.options.formId);
            this.$betlistForm.append(this.$pageIndex).append(this.$pageSize);

            pager.pagination({
                pageList: this.pageList,
                total: this.totalRecords,
                pageSize: this.pageSize,
                pageNumber: this.pageIndex,

                showRefresh: false,
                onSelectPage: (pageNumber: number, pageSize: number) => {
                    this.updateHiddenFields(pageNumber, pageSize);
                    this.onChange();
                },
                beforePageText: pager.data('lablepage'),
                afterPageText: pager.data('labelof') + ' {pages}',
                displayMsg: pager.data('labeldisplayitems')
            });

            pager.removeClass('hide');
        } else {
            pager.remove();
        }

        return pager;
    };

    onChange = () => this.$betlistForm.trigger('submit');

    updateHiddenFields = (pageNumber: number, pageSize: number) => {
        this.$pageIndex.val(pageNumber);
        this.$pageSize.val(pageSize);
    };

    // total records less than min page size => do not need to show pager
    isShow = () => this.pageList[0] <= this.totalRecords;

    hasAllDataOnClient = () => this.totalRecords <= this.pageSize;

    options = new PagerOption(this.onChange);
}
