class BreadcrumbLevelComponent {
    private $breadcrumbLevel: JQuery;
    private breadcrumbs = [];
    private onClick: Function;

    constructor() {
        this.$breadcrumbLevel = $('#breadcrumb-level');
    }

    init(breadcrumbData, onClick: Function) {
        this.onClick = onClick;
        if (Array.isArray(breadcrumbData)) {
            this.resetBreadcrumb(breadcrumbData);
        } else {
            this.addBreadcrumb(breadcrumbData);
        }
    }

    addBreadcrumb(breadcrumbData: any, isRender = true) {
        this.breadcrumbs.push(breadcrumbData);
        isRender && this.render();
    }

    removeBreadcrumb(custLevelId: number) {
        this.breadcrumbs = this.breadcrumbs.filter((item) => item.CustLevelId >= custLevelId);
        this.render();
    }

    resetBreadcrumb(breadcrumbs) {
        this.breadcrumbs = breadcrumbs;
        this.render();
    }

    toStringBreadcrumb() {
        return JSON.stringify(this.breadcrumbs);
    }

    private render() {
        const breadcrumbLength = this.breadcrumbs.length;
        const html = this.breadcrumbs.map(function (item, index) {
            return index + 1 === breadcrumbLength
                ? `<li>${item.CustName}</li>`
                : `<li><a href="javascript:;" class="upline-link" data-custid="${item.CustId}" data-custname="${item.CustName}" data-custlevelid="${item.CustLevelId}">${item.CustName}</a></li>`;
        });

        this.$breadcrumbLevel
            .html(html.join(''))
            .find('.upline-link')
            .off()
            .click((e) => {
                e.preventDefault();
                const $this = $(e.target);
                const custId = $this.data('custid');
                const custName = $this.data('custname');
                const custLevelId = $this.data('custlevelid');
                this.onClick && this.onClick({ custId, custName, custLevelId });
            });
    }
}
