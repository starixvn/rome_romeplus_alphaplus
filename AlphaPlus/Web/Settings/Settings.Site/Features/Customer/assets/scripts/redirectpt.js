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