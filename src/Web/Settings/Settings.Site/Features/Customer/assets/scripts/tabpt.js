var tabPT = {
    tabListContainer: null,
    tabsList: null,
    iconPrev: null,
    iconNext: null,
    iconMore: null,
    currentIndex: 0,
    numberOfProducts: 0,

    init: function () {
        tabPT.cacheElements();
        var currentIndex = tabPT.getCurrentTabIndex();
        tabPT.bindEvents();
        tabPT.setReadProduct(currentIndex);
        tabPT.disablePrevNext();
    },
    cacheElements: function () {
        tabPT.iconPrev = $('.icon-previous');
        tabPT.iconNext = $('.icon-next');
        tabPT.iconMore = $('#icon-more');
        tabPT.tabListContainer = $('#tab-list');
        tabPT.tabsList = tabPT.tabListContainer.find('> li');

        tabPT.numberOfProducts = tabPT.tabsList.length;
    },
    bindEvents: function () {
        $(window).resize(tabPT.disablePrevNext);

        onload = function () {
            tabPT.iconMore.click(function () {
                window.redirectPT.showAutoComplete();
                $(this).addClass('hide');
            });

            window.redirectPT.bindAutoComplete(productList);
        };
    },
    getCurrentTabIndex: function () {
        var current = tabPT.tabListContainer.find('> li.current');
        return tabPT.tabsList.index(current);
    },
    disablePrevNext: function () {
        tabPT.iconPrev.unbind('click');
        tabPT.iconNext.unbind('click');

        var isDisablePrev = tabPT.isOnScreen($('#IdCurr0'));
        if (isDisablePrev) {
            tabPT.iconPrev.attr('disabled', 'disabled');
        } else {
            tabPT.iconPrev.attr('disabled', null);
            tabPT.iconPrev.bind('click', tabPT.prevProduct);
        }

        var isDisableNext = tabPT.isOnScreen($('#IdCurr' + (tabPT.numberOfProducts - 1)));
        if (isDisableNext) {
            tabPT.iconNext.attr('disabled', 'disabled');
        } else {
            tabPT.iconNext.attr('disabled', null);
            tabPT.iconNext.bind('click', tabPT.nextProduct);
        }
    },
    prevProduct: function () {
        tabPT.scrollTabList(false);
    },
    nextProduct: function () {
        tabPT.scrollTabList(true);
    },
    scrollTabList: function (isNext) {
        var scrollLength = tabPT.tabListContainer.width();
        var scrollSign = isNext ? 1 : -1;
        var left = tabPT.tabListContainer.scrollLeft() + scrollSign * scrollLength;
        tabPT.tabListContainer.scrollLeft(left);
        tabPT.disablePrevNext();
    },
    chooseProduct: function (index) {
        tabPT.currentIndex = parseInt(index, 0);

        var tab = $('#IdCurr' + index);
        tabPT.tabsList.removeClass('current');
        tab.addClass('current');

        tabPT.setReadProduct(index);

        var href = tab.find('> a');
        window.frmmain.location.href = href.attr('data-url');
    },
    setReadProduct: function (index) {
        var key = 'ReviewedSettingProduct_' + customerId;
        var products = $.cookie(key) || '';
        if ((products + ',').indexOf(',' + index + ',') == -1) {
            $.cookie(key, products + ',' + index);
        }

        tabPT.setReadProductStyle();
    },
    setReadProductStyle: function () {
        var key = 'ReviewedSettingProduct_' + customerId;
        var products = $.cookie(key) || '';
        for (var i = 0; i < tabPT.numberOfProducts; i++) {
            if ((products + ',').indexOf(',' + i + ',') != -1) {
                $('#IdCurr' + i).addClass('read');
            }
        }
    },
    showTabs: function (tabName) {
        tabPT.iconMore.removeClass('hide');
        tabPT.rotateProduct(tabName);
    },
    rotateProduct: function (tabName) {
        tabPT.tabListContainer.scrollLeft(0);
        for (var i = 0; i < tabPT.numberOfProducts; i++) {
            var tab = $(tabPT.tabsList[i]);

            if ($.trim(tab.find('>a>span').html()).toLowerCase() != tabName.toLowerCase()) {
                tab.remove();
                tabPT.tabListContainer.append(tab);
                continue;
            }

            tabPT.tabsList.removeClass('current');
            tab.addClass('current');
            tabPT.setReadProduct(tabPT.findIndexOfTab(tabName));
            var href = tab.find('> a');
            window.frmmain.location.href = href.attr('data-url');

            break;
        }

        tabPT.tabsList = tabPT.tabListContainer.find('> li');
    },
    findIndexOfTab: function (tabName) {
        var index = -1;
        for (var i = 0; i < productList.length; i++) {
            var tag = productList[i].toLowerCase();
            if (tag == tabName.toLowerCase()) {
                return i;
            }
        }

        return index;
    },
    isOnScreen: function (element) {
        var win = tabPT.tabListContainer;

        var viewport = {
            left: win.scrollLeft()
        };

        viewport.right = viewport.left + win.width();

        var bounds = element.offset();
        bounds.left += viewport.left;
        bounds.right = bounds.left + element.outerWidth();

        return viewport.left <= bounds.left && bounds.right <= viewport.right;
    }
};

$(document).ready(tabPT.init);
