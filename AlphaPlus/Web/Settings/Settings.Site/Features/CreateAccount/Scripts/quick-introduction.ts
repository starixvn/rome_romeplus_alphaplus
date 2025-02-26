class QuickIntroduction {
    $: JQueryStatic;
    beginModel: JQuery;
    carouselMain: any;
    btnLetGo: JQuery;
    leftCarousel: JQuery;
    rightCarousel: JQuery;
    bothCarousel: JQuery;
    btnClose: JQuery;
    quickIntroductionModal: JQuery;
    btnShowUserguide: JQuery;
    helpModal: JQuery;
    chkDoNotShowAgain: JQuery;
    slideIndex: JQuery;
    localStorageKey = 'quickIntroduction_localStorageKey';
    sessionStorageKey = 'quickIntroduction_sessionStorageKey';
    interval: boolean;

    constructor($: JQueryStatic) {
        this.$ = $;

    }

    updatePaging(pageIndex, maxPage) {
        const sentence = this.$('#PageOf').val();
        return sentence.replace(/\{0}/g, pageIndex).replace(/\{1}/g, maxPage);
    }

    checkItem = () => {
        const currentActiveItem = this.carouselMain.find('.item.active');
        const items = currentActiveItem.parent().children('.item')
        const pageIndex = items.index(currentActiveItem) + 1;


        if (this.carouselMain.find(".carousel-inner .item:first").hasClass("active")) {
            this.leftCarousel.hide();
            this.rightCarousel.show();
        } else if (this.carouselMain.find(".carousel-inner .item:last").hasClass("active")) {
            this.leftCarousel.show();
            this.rightCarousel.hide();
        } else {
            this.bothCarousel.show();
        }
        this.slideIndex.text(this.updatePaging(pageIndex, items.length))
    }
    cacheElements = () => {
        this.beginModel = this.$('#beginModal');
        this.carouselMain = this.$('#carousel-main');
        this.btnLetGo = this.$(".js-let-go-btn");
        this.leftCarousel = this.$(".left.carousel-control");
        this.rightCarousel = this.$(".right.carousel-control");
        this.bothCarousel = this.$(".carousel-control");
        this.btnClose = this.$('#btnClose');
        this.quickIntroductionModal = this.$('#quickIntroductionModal');
        this.btnShowUserguide = this.$('#btnShowUserguide');
        this.chkDoNotShowAgain = this.$('#chkHideQuickIntroPopup');
        this.slideIndex = this.$('#slideIndex');
    }
    registerEvents = () => {
        this.btnLetGo.click(() => {
            this.beginModel.on('hidden.bs.modal', () => {
                this.quickIntroductionModal.modal('show');
            });
            this.beginModel.modal('hide');
        });

        this.chkDoNotShowAgain.click(() => {
            localStorage.setItem(this.getCustomerCookiesName(), "1");
            this.beginModel.modal('hide');
        });

        this.btnClose.click(() => {
            sessionStorage.setItem(this.sessionStorageKey, "1")
        });

        this.btnShowUserguide.click(() => {
            this.quickIntroductionModal.on('hidden.bs.modal', () => {
                this.helpModal.modal('show');
            });
            this.quickIntroductionModal.modal('hide');
        });

        this.carouselMain.carousel({
            interval: false
        });

        this.carouselMain.on("slid.bs.carousel", "", quickIntroduction.checkItem).trigger('slid.bs.carousel');
    }
    getCustomerCookiesName = () => {
        return `${this.localStorageKey}_${this.$("#ActorUsername").val()}`;
    }

    init = () => {
        const hidePopup = localStorage.getItem(quickIntroduction.getCustomerCookiesName()) || sessionStorage.getItem(this.sessionStorageKey);
        if (hidePopup === "1") {
            return
        }
        this.cacheElements();

        this.beginModel.modal({backdrop: 'static'}).modal('show');

        this.registerEvents();
    }
}

const quickIntroduction = new QuickIntroduction($);
$(() => {
    quickIntroduction.init();
});