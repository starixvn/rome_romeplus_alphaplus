window.isProductOptionClicked = false;
function ptView(url) {
    if (window.isProductOptionClicked) {
        window.location.href = window.rootUrl + url;
    }

    window.isProductOptionClicked = !window.isProductOptionClicked;
}

function ptProductDropdownBlur() {
    window.isProductOptionClicked = false;
}

function ptProductChanged(element) {
    var isRedirect = navigator.userAgent.match(/iPad/i) != null || navigator.userAgent.indexOf("Safari") > -1;
    if (isRedirect) {
        window.isProductOptionClicked = false;
        var url = $(element).val();
        location.href = window.rootUrl + url;
    }
}

function correctCheckValueOnDropdown(dropdownId) {
    var dropdown = $("#" + dropdownId)[0];
    if (!dropdown) {
        return;
    }

    var findSelected = false;
    for (var i = 0; i < dropdown.length; i++) {
        var option = dropdown[i];
        var attributeLength = option.attributes.length;
        for (var j = 0; j < attributeLength; j++) {
            var attr = option.attributes[j];
            if (attr.name == "selected") {
                if (attr.value == "selected") {
                    $("#" + dropdownId).val(option.value);
                    findSelected = true;
                }

                break;
            }
        }

        if (findSelected) {
            break;
        }
    }
}

window.top.successfulUpdatingCallback = function (isPartialUpdate, isMultipleUpdate) {
    window.top.reloadPopUpSetting();
};

$(document).ready(function () {
    correctCheckValueOnDropdown("dropdown-product");
});
