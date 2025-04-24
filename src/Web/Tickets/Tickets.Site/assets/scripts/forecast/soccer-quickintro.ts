const soccerQuickIntro = {
    domElements: {
        beginModel: null,
        carouselMain: null,
        btnLetGo: null,
        carouseles: null,
        leftCarousel: null,
        rightCarousel: null,
        bothCarousel: null,
        btnClose: null,
        quickIntroductionModal: null,
        btnShowUserguide: null,
        helpModal: null
    },

    readQuickIntroductionCookieName: 'readQuickIntroduction',

    cacheElements: function () {
        soccerQuickIntro.domElements.beginModel = $('#beginModal');
        soccerQuickIntro.domElements.carouselMain = $('#carousel-main');
        soccerQuickIntro.domElements.btnLetGo = $('.js-let-go-btn');

        soccerQuickIntro.domElements.leftCarousel = $('.left.carousel-control');
        soccerQuickIntro.domElements.rightCarousel = $('.right.carousel-control');
        soccerQuickIntro.domElements.bothCarousel = $('.carousel-control');
        soccerQuickIntro.domElements.btnClose = $('#btnClose');
        soccerQuickIntro.domElements.quickIntroductionModal = $('#quickIntroductionModal');
        soccerQuickIntro.domElements.btnShowUserguide = $('#btnShowUserguide');
        soccerQuickIntro.domElements.helpModal = $('#helpModal');
    },
    registerEvents: function () {
        var cookieExpire = { expires: 365, path: '/' };

        soccerQuickIntro.domElements.btnLetGo.click(function () {
            soccerQuickIntro.domElements.beginModel.on('hidden.bs.modal', function () {
                soccerQuickIntro.domElements.quickIntroductionModal.modal('show');
            });
            soccerQuickIntro.domElements.beginModel.modal('hide');

            $.cookie(soccerQuickIntro.readQuickIntroductionCookieName, 'true', cookieExpire);
        });

        soccerQuickIntro.domElements.btnClose.click(function () {
            $.cookie(soccerQuickIntro.readQuickIntroductionCookieName, 'true', cookieExpire);
        });

        soccerQuickIntro.domElements.btnShowUserguide.click(function () {
            soccerQuickIntro.domElements.quickIntroductionModal.on('hidden.bs.modal', function () {
                soccerQuickIntro.domElements.helpModal.modal('show');
            });
            soccerQuickIntro.domElements.quickIntroductionModal.modal('hide');
        });

        soccerQuickIntro.domElements.carouselMain.carousel({
            interval: false
        });

        soccerQuickIntro.domElements.carouselMain
            .on('slid.bs.carousel', '', soccerQuickIntro.checkItem)
            .trigger('slid.bs.carousel');
    },
    checkItem: function () {
        if (soccerQuickIntro.domElements.carouselMain.find('.carousel-inner .item:first').hasClass('active')) {
            soccerQuickIntro.domElements.leftCarousel.hide();
            soccerQuickIntro.domElements.rightCarousel.show();
        } else if (soccerQuickIntro.domElements.carouselMain.find('.carousel-inner .item:last').hasClass('active')) {
            soccerQuickIntro.domElements.leftCarousel.show();
            soccerQuickIntro.domElements.rightCarousel.hide();
        } else {
            soccerQuickIntro.domElements.bothCarousel.show();
        }
    },
    init: function () {
        var isShowQuickIntroduction = $.cookie(soccerQuickIntro.readQuickIntroductionCookieName);

        if (isShowQuickIntroduction) {
            return;
        }

        soccerQuickIntro.cacheElements();

        soccerQuickIntro.domElements.beginModel.modal({ backdrop: 'static' }).modal('show');

        soccerQuickIntro.registerEvents();
    }
};

$(function () {
    soccerQuickIntro.init();
});
