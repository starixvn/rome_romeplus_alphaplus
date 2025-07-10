/// <reference path="menu-bar.ts" />
interface JQueryStatic {
    material: any;
}

declare var popupManager: any;
declare interface String {
    startsWith(text: string, position: string): boolean;
}

declare interface Window {
    bootboxAlert(text: string);
}

if (typeof String.prototype.startsWith != 'function') {
    String.prototype.startsWith = function (prefix) {
        return this.slice(0, prefix.length) == prefix;
    };
}

('use strict');
/*global alpha, briefProfile */

var indexManager = {
    currentTab: null,
    currentUrlCookieName: 'DFFKDHFJKLDHFJDH',
    hidePopupCookies: 'HidePopup',

    requiredNetworkSpeedCalculation: false,
    showOldCustomerList: false,
    reportDomain: '',
    recheckInIntervalMiliseconds: 30000,
    isInternal: true,
    isReloadTopPage: false,
    isEnableReCheckIn: false,
    isBlockPage: true,

    domElements: {
        mainFrame: null,
        partialContents: null,
        content: null,
        accountInfoContent: null,
        pagePopup: null,
        feedbackFunction: null,
        language: null,

        winningThresholdSurveyPopup: null
    },

    urls: {
        reCheckinUrl: alpha.urls.mainRootPath + 'Account/ReCheckIn',
        feedback: alpha.urls.messageRootPath + 'Feedback/Index',
        checkExistFeedback: alpha.urls.messageRootPath + 'Feedback/CheckExist?id=',
        feedbackFunction: alpha.urls.messageRootPath + 'Feedback/Function?id=',
        winningThresholdSurvey: alpha.urls.messageRootPath + 'Feedback/WinningThresholdSurvey',
        cashServiceSetupChecklist: alpha.urls.cashServiceRootPath + 'Common/GetSetupChecklist'
    },

    init: function () {
        indexManager.isBlockPage = true;
        alpha.detectResponsiveInformation();
        if (!alpha.core.IsIe8OrLower()) {
            $.material.init();
        }

        indexManager.cachedDomElements();
        indexManager.loadPartialViews();
        indexManager.registerEvents();
        indexManager.setCurrentUrl();
        setTimeout(() => {
            indexManager.initWinningThresholdSurvey();
        }, 1000);

        indexManager.initCashServiceChecklist();
        indexManager.bindCashServiceChecklistEvents();
    },

    initAfterCheckSurvey: function () {
        indexManager.isBlockPage = false;
        indexManager.initFeedbackGlobal();
        indexManager.initNewFeatureHighlightPopup();

        if (indexManager.requiredNetworkSpeedCalculation) {
            setTimeout(indexManager.calculateUserConnectionSpeed, 8000);
        }

        var messageForReloadSite = indexManager.getCustomCookies('messageForReloadSite');
        if (messageForReloadSite != '') {
            window.bootboxAlert(messageForReloadSite);
            indexManager.removeCustomCookies('messageForReloadSite');
        }
    },

    registerEvents: function () {
        indexManager.domElements.pagePopup.tinyDraggable();

        if (indexManager.isEnableReCheckIn) {
            setTimeout(indexManager.reCheckin, indexManager.recheckInIntervalMiliseconds);
        }

        const eventMethod = window.addEventListener ? 'addEventListener' : 'attachEvent';
        const eventer = window[eventMethod];
        const messageEvent = eventMethod === 'attachEvent' ? 'onmessage' : 'message';
        eventer(
            messageEvent,
            function (e) {
                if (e.data.event_id === 'daily-statement' || e.data.event_id === 'detail-statement') {
                    popupManager.open(e.data.url, e.data.title, 900);
                }

                if (e.data.event_id === 'betlist-result') {
                    popupManager.openMultiple(
                        e.data.url,
                        e.data.title,
                        e.data.width,
                        e.data.height,
                        undefined,
                        undefined,
                        undefined,
                        undefined,
                        undefined,
                        undefined,
                        e.data.options
                    );
                }

                if (e.data.event_id === 'pplivecasino-result') {
                    var left = popupManager.leftPosition();
                    var top = popupManager.topPosition() + 100;
                    var option =
                        'toolbar=no,scrollbars=no,resizable=yes,top=' +
                        top +
                        ',left=' +
                        left +
                        ',width=835,height=400,location=no';
                    var resultPopup = window.open('', '_blank', option);
                    resultPopup.document.title = 'PP Live Casino Result';
                    $.ajax({
                        type: 'POST',
                        url:
                            location.origin.split(':')[0] +
                            '://' +
                            location.hostname +
                            alpha.urls.resultRootPath +
                            e.data.url,
                        success: function (data) {
                            resultPopup.location.href = data.Result;
                        },
                        error: function () {
                            resultPopup.location.href = alpha.urls.mainRootPath + 'Error.aspx';
                        }
                    });
                }

                if (e.data.event_id === 'winlossdetailnew-feedback') {
                    indexManager.initFeedbackFunction(e.data.feedback_id, e.data.popup_width, e.data.popup_height);
                }
            },
            false
        );
    },

    cachedDomElements: function () {
        indexManager.isInternal = $('#isInternal').val() === '1';
        indexManager.domElements.partialContents = $('.partial-content');
        indexManager.domElements.content = $('#content');
        indexManager.domElements.accountInfoContent = $('#accountinfo-content');
        indexManager.domElements.pagePopup = $('#page-popup');
        indexManager.requiredNetworkSpeedCalculation = $('#requiredNetworkSpeedCalculation').val() == '1';
        indexManager.showOldCustomerList = $('#showOldCustomerList').val() == '1';
        indexManager.reportDomain = $('#reportDomain').val();
        indexManager.recheckInIntervalMiliseconds = $('#RecheckinIntervalMiliseconds').val();
        indexManager.domElements.feedbackFunction = $('#feedback-function');
        indexManager.domElements.language = $('#language');
        indexManager.isEnableReCheckIn = $('#isEnableReCheckIn').val() === '1';
    },

    changeMenu: function (url: string) {
        indexManager.domElements.mainFrame.attr('src', url);
    },

    reCheckin: function () {
        // Post with return result in JSON format
        $.cookie('ssl', location.href.indexOf('https://') === 0, {
            expires: 30,
            path: '/'
        });

        $.ajax({
            type: 'POST',
            url: indexManager.urls.reCheckinUrl,
            data: { onid: alpha.user.onUserId },
            success: (result) => {
                try {
                    var errCode = result.code;
                    var errMsg = result.message;
                    if (errCode !== 0) {
                        alpha.core.signOut();
                        if (errMsg) {
                            alert(errMsg);
                        }
                    }
                } catch (e) {}
            },
            complete: () => {
                setTimeout(indexManager.reCheckin, indexManager.recheckInIntervalMiliseconds);
            }
        });
    },

    loadPartialViews: function () {
        indexManager.domElements.partialContents.each(function (index, partialContent) {
            var $partialContent = $(partialContent),
                url = $partialContent.data('url');
            if (url && url.length > 0) {
                $partialContent.load(url + '?' + Math.random(), function () {
                    if ($partialContent.hasClass('customer-profile')) {
                        briefProfile.init();
                    }
                });
            }
        });
    },

    cacheCurrentUrl: function () {
        try {
            $('.modal[id^="page-popup-"]').remove();

            if (!indexManager.isInternal) {
                var mainFrame = <HTMLIFrameElement>document.getElementById('main');
                var mainFrameDocument = mainFrame.contentWindow || mainFrame.contentDocument;
                var currentUrl = (mainFrameDocument && mainFrameDocument.location.href) || mainFrame.src;
                localStorage.setItem(indexManager.currentUrlCookieName, currentUrl);
            }
        } catch (e) {}
    },

    setCurrentUrl: function () {
        const defaultUrl = indexManager.domElements.content.data('default-url');
        let currentUrl = indexManager.isInternal
            ? defaultUrl
            : localStorage.getItem(indexManager.currentUrlCookieName) || defaultUrl;
        currentUrl = indexManager.isIgnoreRedirectUrl(currentUrl)
            ? defaultUrl
            : indexManager.replaceQueryStringValues(currentUrl);

        if (currentUrl.indexOf(headerNav.urls.home) !== -1) {
            currentUrl = currentUrl.replace('Index2', 'Index');
        }

        if (indexManager.domElements.mainFrame && indexManager.domElements.mainFrame.length > 0) {
            indexManager.domElements.mainFrame.attr('src', currentUrl);
            indexManager.showTour();
        } else {
            indexManager.domElements.content.append(
                '<iframe frameborder="0" id="main" src="' +
                    currentUrl +
                    '" width="100%" height="100%" scrolling="auto" name="main" marginwidth="0" marginheight="0" allowtransparency="true" class="iframe-content"></iframe>'
            );
            indexManager.domElements.mainFrame = $('#main');

            var isShowTour = false;
            indexManager.domElements.mainFrame.on('load.newmain', function () {
                if (indexManager.isBlockPage) {
                    return;
                }

                if (isShowTour) {
                    indexManager.showTour();
                    isShowTour = false;
                }

                if (indexManager.isReloadTopPage) {
                    window.top.location.reload();
                    return;
                }

                indexManager.domElements.feedbackFunction.addClass('hide');
                toastMessage.updatePosition();

                var iframeWindow =
                    indexManager.domElements.mainFrame[0].contentWindow ||
                    indexManager.domElements.mainFrame[0].contentDocument;
                indexManager.closeAllOpenDropdowns(iframeWindow);

                indexManager.cacheCurrentUrl();
                try {
                    $(iframeWindow.document).on('click touchend', () => {
                        menuBar.collapseSideBar();
                    });

                    if (alpha.isMobile) {
                        $(iframeWindow.document).find(':focus').blur();
                    }
                } catch (e) {}
            });

            indexManager.domElements.accountInfoContent.load(function () {
                var iframeWindow =
                    indexManager.domElements.accountInfoContent[0].contentWindow ||
                    indexManager.domElements.accountInfoContent[0].contentDocument;
                indexManager.closeAllOpenDropdowns(iframeWindow);
            });
        }
    },

    updateCurrentUrl: function (queryString) {
        if (!indexManager.isInternal) {
            const currentUrl = localStorage.getItem(indexManager.currentUrlCookieName);
            if (currentUrl) {
                const currentUrlArray = currentUrl.split('?');
                localStorage.setItem(indexManager.currentUrlCookieName, currentUrlArray[0] + '?' + queryString);
            }
        }
    },

    getCurrentUrl: function () {
        return indexManager.isInternal ? '' : localStorage.getItem(indexManager.currentUrlCookieName);
    },
    closeAllOpenDropdowns: function (iframeWindow) {
        const closeDropdownSubMenu = () => {
            $('.dropdown-submenu').find('.dropdown-menu').hide();
        };

        $(document).on('click touchend', closeDropdownSubMenu);

        try {
            $(iframeWindow.document).on('click touchend', () => {
                $('.dropdown-toggle').each((index, dropdown) => {
                    var $dropdown = $(dropdown);

                    if ($dropdown.parent().hasClass('open')) {
                        $dropdown.dropdown('toggle');
                    }
                });

                closeDropdownSubMenu();
            });
        } catch (e) {}
    },

    closeAllPopup: function () {
        try {
            var prevUrl = localStorage.getItem(indexManager.currentUrlCookieName) || '';
            var currentUrl = (<HTMLIFrameElement>document.getElementById('main')).contentWindow.location.href || '';

            var prevUrlWithoutQueryString = prevUrl.split('?')[0];
            var currentUrlWithoutQueryString = currentUrl.split('?')[0];

            var isCloseAllPopup = !this.isSameUrl(prevUrlWithoutQueryString, currentUrlWithoutQueryString);

            if (isCloseAllPopup) {
                popupManager.closeAllPopup();
            }
        } catch (e) {}
    },

    isSameUrl: function (url1, url2) {
        url1 = this.removeHashFromTheEndOfURL(url1.toLowerCase());
        url2 = this.removeHashFromTheEndOfURL(url2.toLowerCase());
        if (url1 === url2) {
            return true;
        }

        var minLength = Math.min(url1.length, url2.length);

        var diff1 = url1.substring(minLength);
        diff1 = diff1.startsWith('/') ? diff1.substring(1) : diff1;
        diff1 = diff1 || 'index';

        var diff2 = url2.substring(minLength);
        diff2 = diff2.startsWith('/') ? diff2.substring(1) : diff2;
        diff2 = diff2 || 'index';

        return url1.substring(0, minLength) == url2.substring(0, minLength) && diff1 === 'index' && diff2 === 'index';
    },

    removeHashFromTheEndOfURL: function (url: string) {
        const indexOfHash = url.length - 1;

        return url[indexOfHash] === '#' ? url.substring(0, indexOfHash) : url;
    },

    calculateUserConnectionSpeed: function () {
        var iframe = <HTMLIFrameElement>document.getElementById('connection-speed');

        if (typeof iframe == 'undefined' || iframe == null) {
            iframe = <HTMLIFrameElement>document.createElement('iframe');
            iframe.id = 'connection-speed';
            iframe.style.display = 'none';
            iframe.src = alpha.urls.mainRootPath + 'NetworkSpeed/Index';
            document.body.appendChild(iframe);
        }
    },

    isIgnoreRedirectUrl: function (currentUrl: string): boolean {
        const ignoreUrls = [
            'error.aspx',
            'signin.aspx',
            'securitycode',
            'dashboard',
            'partialum',
            'auth',
            'kickout',
            'signout'
        ];
        const ignoreUrlsLength = ignoreUrls.length;
        const lowerCurrentUrl = currentUrl.toLowerCase();

        for (let i = 0; i < ignoreUrlsLength; i++) {
            if (lowerCurrentUrl.indexOf(ignoreUrls[i]) > -1) {
                return true;
            }
        }

        return false;
    },

    replaceQueryStringValues: function (url: string) {
        if (indexManager.isDCSUrl(url)) {
            return url;
        }

        var newUrl = url.replace(/(isdarkmode=)[^\&]+/i, '$1' + $('#isDarkMode').val());
        return newUrl.replace(/(lng=)[^\&]+/i, '$1' + indexManager.domElements.language.data('currentlanguage'));
    },

    isDCSUrl: function (url: string) {
        return url.indexOf('dcs/viewcustomersetting') > -1;
    },

    initFeedbackGlobal: function () {
        $('#feedback-box').click(function () {
            popupManager.open(indexManager.urls.feedback, undefined, 440, 380, null, null, null, null, null, null, {
                customClass: 'feedback-modal'
            });
        });
    },

    initFeedbackFunction: function (id, width, height) {
        const hideFeedbackFunctionSessionKey = indexManager.getCustomerCookiesName('HideFeedbackFunction') + '_' + id;
        if (sessionStorage.getItem(hideFeedbackFunctionSessionKey) === '1') {
            return;
        }

        $.get(indexManager.urls.checkExistFeedback + id, function (result) {
            if (!result.IsExist) {
                //indexManager.calulateFeedbackFunctionPosition();

                const feedbackBox = indexManager.domElements.feedbackFunction.find('.feedback-box');
                feedbackBox.find('.txt-feedback').text(result.FunctionName || 'Feedback');
                feedbackBox.off().click(function () {
                    popupManager.open(
                        indexManager.urls.feedbackFunction + id,
                        undefined,
                        width || 440,
                        height || 380,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        { customClass: 'feedback-modal' }
                    );
                });
                indexManager.domElements.feedbackFunction
                    .find('.icon-close-feedback')
                    .off()
                    .click(function () {
                        sessionStorage.setItem(hideFeedbackFunctionSessionKey, '1');
                        indexManager.domElements.feedbackFunction.addClass('hide');
                    });
                indexManager.domElements.feedbackFunction.removeClass('hide');
                //$(window).off("resize").resize(indexManager.calulateFeedbackFunctionPosition);
            }

            toastMessage.updatePosition();
        });
    },

    //calulateFeedbackFunctionPosition() {
    //    indexManager.domElements.mainFrame.height() >= indexManager.domElements.mainFrame.contents().height()
    //        && indexManager.domElements.feedbackFunction.css({ right: -20 });

    //    indexManager.domElements.feedbackFunction.css({
    //        top: $(window).height() - 100
    //    });
    //},

    initNewFeatureHighlightPopup: function () {
        // Check for Reward
        const hiddenIsShowRewardTour = $('#hidden-isShowRewardTour');
        if (hiddenIsShowRewardTour.length && hiddenIsShowRewardTour.val() === '1') {
            indexManager.initHighlightPopup($('#menu-TRANSFER'), '221208_RewardProgram', 80);
            hiddenIsShowRewardTour.remove();
        } else {
            // New Feature
            indexManager.initHighlightPopup($('#menu-RISKMANAGEMENT'), '230794_Potential', 80);
        }
    },

    initHighlightPopup: function (mainMenuItem, featureName, paddingTop) {
        const box = $('#boxHighlightPopupNewFeature');
        if (!box.length) {
            return;
        }

        if (!mainMenuItem || !mainMenuItem.length) {
            return;
        }

        const cookiesName = indexManager.hidePopupCookies + featureName;
        const hidePopup = localStorage.getItem(indexManager.getCustomerCookiesName(cookiesName));

        if (hidePopup !== '1') {
            popupManager.closeAllPopup();
            box.css({ top: mainMenuItem.position().top + paddingTop });
            box.removeClass('hide');

            box.find('.icon-close').click(() => box.addClass('hide'));

            box.find('#chkHideHighlightPopupNewFeature').click(function () {
                localStorage.setItem(indexManager.getCustomerCookiesName(cookiesName), '1');
                box.addClass('hide');
            });

            const newFeatureLink = box.find('#linkHighlightPopupCheckOut');
            newFeatureLink.click(() => box.addClass('hide'));
            //$(window).scroll(() => box.addClass("hide"));

            $('#main-menu-tab')['mCustomScrollbar']({
                callbacks: {
                    onScrollStart: () => box.addClass('hide')
                }
            });
        }
    },

    hideFloatingFeedbackIcon: function () {
        indexManager.domElements.feedbackFunction.addClass('hide');
    },

    removeCookies: function () {
        localStorage.removeItem(indexManager.currentUrlCookieName);
    },

    getCustomerCookiesName: function (key) {
        return `${key}_${$('#CustId').val()}_${$('#SubAccId').val()}`;
    },

    addCustomCookies: function (cookieName, value) {
        var key = indexManager.getCustomerCookiesName(cookieName);
        localStorage.setItem(key, value);
    },
    getCustomCookies: function (cookieName) {
        var key = indexManager.getCustomerCookiesName(cookieName);
        return localStorage.getItem(key) || '';
    },
    removeCustomCookies: function (cookieName) {
        var key = indexManager.getCustomerCookiesName(cookieName);
        localStorage.removeItem(key);
    },

    showTour: function () {
        if (indexManager.isBlockPage) {
            return;
        }

        // setTimeout(() => {
        //     popupManager.openMultiple($("#TourURL").val(), null, 800, 495, null, null, null, null, null, null, { customClass: 'popup-tour-intro' })
        // }, 3000);
    },

    initWinningThresholdSurvey: function () {
        if ($('#ShowWinningThresholdSurvey').val() === '0') {
            indexManager.initAfterCheckSurvey();
            return;
        }

        const feedbackId = 12;
        $.get(indexManager.urls.checkExistFeedback + feedbackId, function (result) {
            if (result.IsExist) {
                indexManager.initAfterCheckSurvey();
                return;
            }

            indexManager.domElements.winningThresholdSurveyPopup = popupManager.open(
                indexManager.urls.winningThresholdSurvey,
                undefined,
                440,
                380,
                null,
                null,
                null,
                null,
                null,
                null,
                { customClass: 'feedback-survey-modal', disableDraggable: true }
            );
        });
    },

    initCashServiceChecklist: function (openContent: boolean = false) {
        if ($('#ShowCashServiceChecklist').val() === '0') {
            return;
        }

        $.get(indexManager.urls.cashServiceSetupChecklist, function (result) {
            var checklistDiv = $('#cash-service-checklist');
            if(result.isCompleteAll) {
                checklistDiv.addClass('hide');
                return;
            }

            checklistDiv.removeClass('hide');
            var content = checklistDiv.find('.checklist-content');

            if(result.hasDepositMethods) {
                content.find('.chk-deposit-methods').addClass('complete');
            } else {
                content.find('.chk-deposit-methods').removeClass('complete');
            }

            if(result.hasWithdrawalMethods) {
                content.find('.chk-withdrawal-methods').addClass('complete');
            } else {
                content.find('.chk-withdrawal-methods').removeClass('complete');
            }

            if(result.hasLineID) {
                content.find('.chk-line-id').addClass('complete');
            } else {
                content.find('.chk-line-id').removeClass('complete');
            }

            if(openContent) {
                checklistDiv.addClass('show');
            } else {
                checklistDiv.removeClass('show');
            }
        });
    },
    collapseCashServiceChecklist: function () {
        $('#cash-service-checklist').removeClass('show');
    },
    showHideCashServiceChecklist: function (isShow: boolean) {
        if(isShow) {
            $('#ShowCashServiceChecklist').val(1);
            indexManager.initCashServiceChecklist(true);
        } else {
            $('#cash-service-checklist').addClass('hide');
            $('#ShowCashServiceChecklist').val(0);
            $('#cash-service-checklist').removeClass('show');
        }
    },
    bindCashServiceChecklistEvents: function () {
        var checklistDiv = $('#cash-service-checklist');

        checklistDiv.find('.btn-help').click(() => {
            window.open(alpha.urls.howToUseRootPath + 'HandbookDefault/Index', '_blank');
        });

        // Bind click event on main frame to hide cash-service-checklist
        indexManager.domElements.mainFrame.on('load', function() {
            try {
                const iframeWindow = this.contentWindow || this.contentDocument;
                if (iframeWindow && iframeWindow.document) {
                    $(iframeWindow.document).on('click', (e) => {
                        const checklistDiv = $('#cash-service-checklist');
                        if (!checklistDiv.is(e.target) && checklistDiv.has(e.target).length === 0) {
                            indexManager.collapseCashServiceChecklist();
                        }
                    });
                }
            } catch (e) {
                // Handle cross-origin restrictions
            }
        });

      
        checklistDiv.find('#ico-checklist').click(() => {
            if(checklistDiv.hasClass('show')) {
                checklistDiv.removeClass('show');
            } else {
                checklistDiv.addClass('show');
            }
        });

        // Also bind on window for cases where main frame might not be available
        $(window).on('click', (e) => {
            const checklistDiv = $('#cash-service-checklist');
            if (!checklistDiv.is(e.target) && checklistDiv.has(e.target).length === 0) {
                indexManager.collapseCashServiceChecklist();
            }
        });
    }
};

$(function () {
    indexManager.init();
});
