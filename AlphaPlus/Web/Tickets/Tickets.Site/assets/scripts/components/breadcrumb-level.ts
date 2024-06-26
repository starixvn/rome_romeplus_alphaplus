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
                        custId: uplineLink.data('custid'),
                        custName: uplineLink.data('custname'),
                        custLevelId: +uplineLink.data('custlevelid')
                    });
                    uplineLink.off().click((event) => this.handleBreadcrumbClick(event));
                } else {
                    this.breadcrumbs.push({
                        custName: item.innerText
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
        event.preventDefault();
        const $this = $(event.target);
        const custId = $this.data('custid');
        const custName = $this.data('custname');
        const custLevelId = $this.data('custlevelid');
        this.onClick && this.onClick($this, { custId, custName, custLevelId });
    }

    getBreadcrumbLevels(custLevelId = 0): string {
        const breadcrumbLevels = custLevelId
            ? this.breadcrumbs.filter((item) => item.custLevelId >= custLevelId)
            : this.breadcrumbs;
        return breadcrumbLevels.length ? JSON.stringify(breadcrumbLevels) : '';
    }
}
