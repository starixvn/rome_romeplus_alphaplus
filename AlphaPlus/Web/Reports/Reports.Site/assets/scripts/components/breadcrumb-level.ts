// class BreadcrumbLevelOption {
//     constructor(
//         public breadcrumbs = [],
//         public onClick = Function(),
//         public reportTitle: any,
//         public selectorId = 'breadcrumb-level',
//         public reportTitleSelectorId = 'report-title'
//     ) {
//         this.breadcrumbs = breadcrumbs;
//         this.onClick = onClick;
//         this.selectorId = selectorId;
//         this.reportTitleSelectorId = reportTitleSelectorId;
//     }
// }

class BreadcrumbLevelComponent {
    private $reportTitle: JQuery;
    private $breadcrumbLevel: JQuery;
    private breadcrumbs = [];
    private onClick: Function;

    constructor() {
        this.$reportTitle = $('#report-title');
        this.$breadcrumbLevel = $('#breadcrumb-level');
    }

    init(breadcrumbData, onClick: Function) {
        this.breadcrumbs.push(breadcrumbData);
        this.onClick = onClick;
        this.render();
    }

    addBreadcrumbLevel(breadcrumbData: any) {
        this.breadcrumbs.push(breadcrumbData);
        this.render();
    }

    removeBreadcrumbLevel(custLevelId: number) {
        this.breadcrumbs = this.breadcrumbs.filter((item) => item.CustLevelId >= custLevelId);
        this.render();
    }

    resetBreadcrumbLevel(breadcrumbs) {
        this.breadcrumbs = breadcrumbs;
        this.render();
    }

    getBreadcrumbs() {
        return this.breadcrumbs;
    }

    private render() {
        const breadcrumbLength = this.breadcrumbs.length;
        const html = this.breadcrumbs.map(function (item, index) {
            return index + 1 === breadcrumbLength
                ? `<li>${item.CustName}</li>`
                : `<li><a href="javascript:;" class="upline-link" data-custid="${item.CustId}" data-custname="${item.CustName}" data-custlevelid="${item.CustLevelId}">${item.CustName}</a></li>`;
        });

        // this.$reportTitle.text(
        //     this.breadcrumbLevelOption.reportTitle[
        //         this.breadcrumbLevelOption.breadcrumbs[breadcrumbLength - 1].custLevelId
        //     ]
        // );

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
                this.onClick && this.onClick(custId, custName, custLevelId);
            });
    }
}
