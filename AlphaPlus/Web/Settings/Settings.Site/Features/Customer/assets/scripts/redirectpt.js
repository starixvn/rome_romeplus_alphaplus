var redirectPT = {
    productList: null,
    txtSearch: null,
    showAutoComplete: function () {
        $("#frmHeader").addClass("hide");
        $("#search-product").addClass("show");

        var txtSearch = $("#txt-search");
        txtSearch.val("");
        txtSearch.focus();
        txtSearch.autocomplete("search", txtSearch.val());
    },
    bindAutoComplete: function (productList) {
        redirectPT.productList = productList;

        var txtSearch = $("#txt-search");

        txtSearch.autocomplete({
            source: productList,
            minLength: 0,
            delay: 20,
            close: function (event, ui) {
                var selectedValue = $(this).val();

                if (redirectPT.findIndexOfTab(selectedValue) != -1) {
                    txtSearch.blur();
                }

                redirectPT.goToTab(selectedValue);
            }
        });

        txtSearch.click(function () {
            txtSearch.autocomplete("search", txtSearch.val());
        });

        txtSearch.keyup(function (e) {
            var key = e.charCode || e.keyCode || 0;
            if (key == 13) {
                if (txtSearch.val() == "") {
                    $("#search-product").removeClass("show");
                    $("#frmHeader").removeClass("hide");
                    frmHeader.tabPT.iconMore.removeClass("hide");

                    return;
                }

                if (redirectPT.findIndexOfTab(txtSearch.val()) == -1) {
                    return;
                }

                txtSearch.autocomplete("close");
            }
        });

        redirectPT.txtSearch = txtSearch;
    },
    goToTab: function (tabName) {
        if (redirectPT.findIndexOfTab(tabName) == -1) {
            return;
        }

        $("#search-product").removeClass("show");
        $("#frmHeader").removeClass("hide");
        frmHeader.tabPT.showTabs(tabName);
    },
    findIndexOfTab: function (tabName) {
        var index = -1;
        for (var i = 0; i < redirectPT.productList.length; i++) {
            var tag = redirectPT.productList[i].toLowerCase();
            if (tag == tabName.toLowerCase()) {
                return i;
            }
        }

        return index;
    }
};

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
        tabPT.iconPrev = $(".icon-previous");
        tabPT.iconNext = $(".icon-next");
        tabPT.iconMore = $("#icon-more");
        tabPT.tabListContainer = $("#tab-list");
        tabPT.tabsList = tabPT.tabListContainer.find("> li");

        tabPT.numberOfProducts = tabPT.tabsList.length;
    },
    bindEvents: function () {
        $(window).resize(tabPT.disablePrevNext);

        parent.onload = function () {
            tabPT.iconMore.click(function () {
                parent.redirectPT.showAutoComplete();
                $(this).addClass("hide");
            });

            parent.redirectPT.bindAutoComplete(productList);
        };
    },
    getCurrentTabIndex: function () {
        var current = tabPT.tabListContainer.find("> li.current");
        return tabPT.tabsList.index(current);
    },
    disablePrevNext: function () {
        tabPT.iconPrev.unbind("click");
        tabPT.iconNext.unbind("click");

        var isDisablePrev = tabPT.isOnScreen($("#IdCurr0"));
        if (isDisablePrev) {
            tabPT.iconPrev.attr("disabled", "disabled");
        } else {
            tabPT.iconPrev.attr("disabled", null);
            tabPT.iconPrev.bind("click", tabPT.prevProduct);
        }

        var isDisableNext = tabPT.isOnScreen($("#IdCurr" + (tabPT.numberOfProducts - 1)));
        if (isDisableNext) {
            tabPT.iconNext.attr("disabled", "disabled");
        } else {
            tabPT.iconNext.attr("disabled", null);
            tabPT.iconNext.bind("click", tabPT.nextProduct);
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

        var tab = $("#IdCurr" + index);
        tabPT.tabsList.removeClass("current");
        tab.addClass("current");

        tabPT.setReadProduct(index);

        var href = tab.find("> a");
        parent.frmmain.location.href = href.attr("data-url") + "&isedit=" + $("#" + href.attr("data-isEdit")).val();
    },
    setReadProduct: function (index) {
        var key = "ReviewedSettingProduct_" + customerId;
        var products = $.cookie(key) || "";
        if ((products + ",").indexOf("," + index + ",") == -1) {
            $.cookie(key, products + "," + index);
        }

        tabPT.setReadProductStyle();
    },
    setReadProductStyle: function () {
        var key = "ReviewedSettingProduct_" + customerId;
        var products = $.cookie(key) || "";
        for (var i = 0; i < tabPT.numberOfProducts; i++) {
            if ((products + ",").indexOf("," + i + ",") != -1) {
                $("#IdCurr" + i).addClass("read");
            }
        }
    },
    showTabs: function (tabName) {
        tabPT.iconMore.removeClass("hide");
        tabPT.rotateProduct(tabName);
    },
    rotateProduct: function (tabName) {
        tabPT.tabListContainer.scrollLeft(0);
        for (var i = 0; i < tabPT.numberOfProducts; i++) {
            var tab = $(tabPT.tabsList[i]);

            if ($.trim(tab.find(">a>span").html()).toLowerCase() != tabName.toLowerCase()) {
                tab.remove();
                tabPT.tabListContainer.append(tab);
                continue;
            }

            tabPT.tabsList.removeClass("current");
            tab.addClass("current");
            tabPT.setReadProduct(tabPT.findIndexOfTab(tabName));
            var href = tab.find("> a");
            parent.frmmain.location.href = href.attr("data-url") + "&isedit=" + $("#" + href.attr("data-isEdit")).val();

            break;
        }

        tabPT.tabsList = tabPT.tabListContainer.find("> li");
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