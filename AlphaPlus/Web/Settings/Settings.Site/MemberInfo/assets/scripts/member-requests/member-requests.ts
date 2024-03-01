/// <reference path="../components/single-selection-dropdown.ts" />
"use strict";

class MemberRequests {
    $: JQueryStatic;
    topWindow: any;
    singleSelectionDropdownComponent: SingleSelectionDropdownComponent;

    $dropdownProduct: JQuery;
    $msgWarning: JQuery;
    $msgInfo: JQuery;
    disabledProductWarningMessage: string;
    noSettingWarning: string;

    memberRequestUrl: string;
    $boxContent: JQuery;

    $accountInfoHeader: JQuery;
    $sideBar: JQuery;
    $accountInfoContent: JQuery;
    viewerId: number;

    viewedCustName: string;

    $tableReport: JQuery;
    guidelinesTitle: string;
    guidelinesUrl: string;

    constructor(
        $: JQueryStatic,
        topWindow: any,
        singleSelectionDropdownComponent: SingleSelectionDropdownComponent) {
        this.$ = $;
        this.topWindow = topWindow;
        this.singleSelectionDropdownComponent = singleSelectionDropdownComponent;
    }

    init = () => {
        this.cacheDOMElements();
        this.singleSelectionDropdownComponent.render(
            new SingleSelectionDropdownOption("dropdown-product", "UserSelectedProductId", this.changeProduct));

        this.registerEvents();
        this.changeProduct();
    };
    registerEvents = () => {
        this.$("#icon-refresh").click(this.refreshPage);
        this.$("#icon-information").on("click", this.showGuidelines);
    };

    cacheDOMElements = () => {
        this.$dropdownProduct = this.$("#dropdown-product");

        this.$msgWarning = this.$("#msg-warning");
        this.$msgInfo = this.$("#msg-info");
        this.disabledProductWarningMessage = this.$("#disabledProductWarningMessage").val();
        this.noSettingWarning = this.$("#noSettingWarning").val();

        this.memberRequestUrl = this.$("#memberRequestUrl").val();
        this.$boxContent = this.$("#box-content");

        this.$accountInfoHeader = this.topWindow.$("#account-info-header");
        this.$sideBar = this.topWindow.$(".sidebar");
        this.viewerId = parseInt(this.$("#viewerId").val(), 0);

        this.guidelinesTitle = this.$('#PlayForFunGuide').val();
        this.guidelinesUrl = this.$('#GuidelineUrl').val();
    };
    changeProduct = () => {
        this.loadMemberRequests();
    };

    convertAttributeToBool = (attrValue: string) => !!attrValue && attrValue.toString().toLowerCase() === "true";
    updateWarningMessage = () => {
        var selectedProduct = this.getSelectedProduct();
        var isDisable = this.convertAttributeToBool(selectedProduct.data("disabled"));

        if (isDisable) {
            this.$msgWarning.html(this.disabledProductWarningmessage, CancellationToken cancellation).removeClass("hide");
            this.$msgInfo.addClass("hide");
            return;
        }

        let errorCode = parseInt(selectedProduct.data("error-code"), 0);

        if (errorCode !== 0) {
            this.$msgWarning.html(selectedProduct.data("error-message")).removeClass("hide");
            this.$msgInfo.addClass("hide");
            return;
        }

        var hasSetting = this.convertAttributeToBool(selectedProduct.data("has-setting"));

        if (!hasSetting) {
            this.$msgWarning.html(this.noSettingWarning).removeClass("hide");
            this.$msgInfo.addClass("hide");
            return;
        }

        this.$msgWarning.addClass("hide");
        this.$msgInfo.removeClass("hide");
    };
    loadMemberRequests = () => {
        this.$.blockUI();

        var selectedProduct = this.getSelectedProduct();
        var param = {
            UserSelectedProductId: selectedProduct.val(),
            DisabledUserSelectedProduct: this.convertAttributeToBool(selectedProduct.data("disabled"))
        };

        this.$.ajax({
            method: "GET",
            data: param,
            url: this.memberRequestUrl,
            cache: false,
            success: this.renderMemberRequests
        });
    };
    renderMemberRequests = (html: string) => {
        this.$boxContent.html(html);

        this.$tableReport = this.$("#tbl-report");
        this.registerEventsForTable();
        this.updateWarningMessage();
        this.$.unblockUI();
    };
    getSelectedProduct = () => this.$dropdownProduct.find("option:selected");
    registerEventsForTable = () => {
        let self = this;

        let $infoLinks = this.$(".view-member-info-link");
        $infoLinks.on("click", function (event: any) {
            self.viewMemberInfo(event, self.$(this));
        });

        let $editSettingLink = this.$(".edit-setting:not([disabled])");
    };
    viewMemberInfo = (event: any, element: JQuery) => {
        let username = element.text();

        if (this.$sideBar.hasClass("collapsed") ||
            this.$sideBar.hasClass("hidden-mobile")) {
            this.topWindow.menuBar.collapseLeftPanel();
        }

        this.$accountInfoHeader.click();
        this.$accountInfoContent = this.topWindow.$("#accountinfo-content");
        this.viewedCustName = username;

        if (this.isLoadedAccountInfo()) {
            setTimeout(() => {
                this.searchCustInfo(username);
            }, 300);
        } else {
            this.$accountInfoContent.on("load", this.bindLoadCustInfoEvent);
        }

        event.stopPropagation();
    };
    isLoadedAccountInfo = () => {
        return this.$accountInfoContent.contents().find("#txtUsername").length !== 0;
    };
    
    refreshPage = () => {
        location.reload();
    };
    bindLoadCustInfoEvent = () => {
        this.searchCustInfo(this.viewedCustName);
        this.$accountInfoContent.off("load", this.bindLoadCustInfoEvent);
    };
    searchCustInfo = (username: string) => {
        var contents = this.$accountInfoContent.contents();
        contents.find("#txtUsername").val(username);
        contents.find(".icon-search").click();
    };
    showGuidelines = () => {
        this.topWindow.popupManager.open(this.guidelinesUrl, this.guidelinesTitle, 900, 600, "popup-guidelines");
    };
}

const memberRequests = new MemberRequests($, window.top, new SingleSelectionDropdownComponent($));

$(() => {
    memberRequests.init();
});
