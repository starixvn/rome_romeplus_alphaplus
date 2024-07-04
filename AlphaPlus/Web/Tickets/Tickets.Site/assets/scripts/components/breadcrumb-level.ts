class BreadcrumbLevelComponent {
    private breadcrumbs = [];
    private onClick: Function;

    init(onClick: Function) {
        this.onClick = onClick;
        this.breadcrumbs = [];
        $('#breadcrumb-level')
            .find('li')
            .each((_, item: any) => {
                const uplineLinkList = $(item).find('.upline-link');

                if (uplineLinkList.length) {
                    const uplineLink = uplineLinkList.eq(0);
                    this.breadcrumbs.push({
                        CustId: uplineLink.data('custid'),
                        CustName: uplineLink.data('custname'),
                        CustLevelId: +uplineLink.data('custlevelid')
                    });
                    uplineLink.off().click((event) => this.handleBreadcrumbClick(event));
                } else {
                    this.breadcrumbs.push({
                        CustName: item.innerText
                    });
                }
            });
    }

    bindEvents() {
        $('#breadcrumb-level')
            .find('.upline-link')
            .off()
            .click((event) => this.handleBreadcrumbClick(event));
    }

    private handleBreadcrumbClick(event) {
        const $this = $(event.target);
        const CustId = $this.data('custid');
        const CustName = $this.data('custname');
        const CustLevelId = $this.data('custlevelid');
        this.onClick && this.onClick($this, { CustId, CustName, CustLevelId });
    }

    getBreadcrumbLevels(custLevelId = 0) {
        return custLevelId ? this.breadcrumbs.filter((item) => item.CustLevelId >= custLevelId) : this.breadcrumbs;
    }
}
