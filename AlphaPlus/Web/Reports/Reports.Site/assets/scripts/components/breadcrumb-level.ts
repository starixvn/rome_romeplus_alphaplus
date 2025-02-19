class BreadcrumbLevelComponent {
    private onClick: Function;

    init(onClick: Function) {
        this.onClick = onClick;
        this.bindEvents();
    }

    bindEvents() {
        $('#breadcrumb-level')
            .find('.upline-link')
            .off()
            .click((e) => {
                e.preventDefault();
                const $this = $(e.target);
                const custId = $this.data('custid');
                const custName = $this.data('custname').toString();
                const custLevelId = $this.data('custlevelid');
                const breadcrumbs = this.getBreadcrumbs().filter((item) => item.custLevelId > custLevelId);
                const breadcrumbsJson = JSON.stringify(breadcrumbs);
                this.onClick && this.onClick({ custId, custName, custLevelId, breadcrumbsJson });
            });
    }

    getBreadcrumbs() {
        return JSON.parse($('#hidden-breadcrumbsJson').val()).map(function (item) {
            return {
                custId: item.CustId,
                custName: item.CustName,
                custLevelId: item.CustLevelId
            };
        });
    }
}
