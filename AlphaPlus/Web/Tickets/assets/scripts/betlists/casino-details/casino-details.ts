class CasionDetailsView {
    pagerComponent: PagerComponent;
    pageList = [100, 200, 300, 500, 1000];

    constructor(pagerComponent: PagerComponent) {
        this.pagerComponent = pagerComponent;
    }

    init() {
        if (this.isiOS()) {
            const maxHeight = window.top.window.innerHeight - 260;
            $('#table-container').css({ 'max-height': maxHeight, overflow: 'auto' });
        }

        this.pagerComponent.pageList = this.pageList;
        this.pagerComponent.render();
    }

    isiOS() {
        return /iPhone|iPad|iPod/i.test(navigator.userAgent);
    }
}

$(function () {
    new CasionDetailsView(new PagerComponent()).init();
});
