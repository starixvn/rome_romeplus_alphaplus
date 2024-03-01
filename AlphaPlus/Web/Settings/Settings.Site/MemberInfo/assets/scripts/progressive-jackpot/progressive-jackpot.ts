/// <reference path="../components/productmultiselection.ts" />
"use strict";

class ProgressiveJackpot {
    $: JQueryStatic;
    topWindow: any;
    productMultiSelectionComponent: ProductMultiSelectionComponent;

    $dropdownProduct: JQuery;
    $tableReport: JQuery;
    $iconInformation: JQuery;

    handbookUrl: string;
    handbookPopupTitle: string;

    constructor(
        $: JQueryStatic,
        topWindow: any,
        productMultiSelectionComponent: ProductMultiSelectionComponent) {
        this.$ = $;
        this.topWindow = topWindow;
        this.productMultiSelectionComponent = productMultiSelectionComponent;
    }

    init = () => {
        this.cacheDOMElements();
        this.productMultiSelectionComponent.render();
        this.$iconInformation.click(this.showHandbook);
    };

    cacheDOMElements = () => {
        this.$dropdownProduct = this.$("#dropdown-product");
        this.$iconInformation = this.$("#icon-information");
        this.handbookUrl = this.$("#HandbookUrl").val();
        this.handbookPopupTitle = this.$("#HandbookPopupTitle").val();
    };

    showHandbook = () => {
        this.topWindow.popupManager.open(this.handbookUrl, this.handbookPopupTitle, 900, 600)
    };
}

const progressiveJackpot = new ProgressiveJackpot($, window.top, new ProductMultiSelectionComponent($));

$(() => {
    progressiveJackpot.init();
});