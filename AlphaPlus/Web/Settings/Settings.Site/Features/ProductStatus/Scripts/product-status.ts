'use strict';

interface BettypeStatus {
    SportType: number;
    BetType: number;
    IsDisabled: boolean;
}

interface ProductStatus {
    ProductId: number;
    CustId: number;
    CustRoleId: number;
    BetTypeStatus: Array<BettypeStatus>;
    IsGroup: boolean;
}

const ProductConstants = {
    AllBet: 21,
    Keno: 8
};

class ProductStatusPopup {
    $: JQueryStatic;
    topWindow: any;

    $msgServerResult: JQuery;
    $formProductStatus: JQuery;
    productId: number;
    custId: number;
    custName: string;
    custRoleId: number;
    token: string;
    isGroup: boolean;

    constructor($: JQueryStatic, topWindow: any) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    init = () => {
        this.cacheDOMElements();
        this.registerEvents();
    };

    cacheDOMElements = () => {
        this.$msgServerResult = this.$('#msg-serverResult');
        this.$formProductStatus = this.$('#formProductStatus');
        this.productId = parseInt(this.$('#hiddenProductId').val());
        this.custId = this.$('#hiddenCustId').val();
        this.custName = this.$('#hiddenCustName').val();
        this.custRoleId = this.$('#hiddenCustRoleId').val();
        this.token = this.$('input[name="__RequestVerificationToken"]', this.$formProductStatus).val();
        this.isGroup = this.$('#hiddenIsGroup').val() == '1';
    };

    registerEvents = () => {
        this.$formProductStatus.submit(this.updateProductStatus);
    };

    updateProductStatus = (event) => {
        event.preventDefault();

        this.$.blockUI();
        const formData = this.getProductStatusData();

        if (!formData) {
            this.addNoDataChange();
            this.$.unblockUI();
            return;
        }

        this.hideMessage();
        const requestVerificationToken = this.token;

        this.$.ajax({
            type: 'POST',
            url: this.$formProductStatus.attr('action'),
            data: { message: formData },
            dataType: 'json',
            beforeSend: (xhr) => xhr.setRequestHeader('RequestVerificationToken', requestVerificationToken)
        })
            .done(this.afterUpdateStatusSuccessfully)
            .always(this.$.unblockUI);
    };

    getProductStatusData = (): ProductStatus => {
        let checkbox: JQuery, isDisabled: boolean, oldValue: boolean;
        const betTypeStatus = [];

        this.$("input[type='checkbox']").each((_index, element) => {
            checkbox = this.$(element);
            isDisabled = !checkbox.prop('checked');
            oldValue = checkbox.data('oldvalue');

            if (isDisabled !== oldValue) {
                betTypeStatus.push({
                    SportType: checkbox.data('sporttypeid'),
                    BetType: checkbox.data('bettypeid'),
                    IsDisabled: isDisabled
                });
            }
        });

        return betTypeStatus.length !== 0
            ? {
                  ProductId: this.productId,
                  CustId: this.custId,
                  CustRoleId: this.custRoleId,
                  BetTypeStatus: betTypeStatus,
                  IsGroup: this.isGroup
              }
            : null;
    };

    afterUpdateStatusSuccessfully = (data) => {
        var html = [];
        var isSuccess = true;
        if (this.$.type(data.ErrCode) !== 'undefined') {
            html.push(this.buildErrorMessage(data.ErrMsg));
            isSuccess = false;
        } else {
            const self = this;
            this.$.each(data, function (index, item) {
                if (item.Code !== 0) {
                    html.push(self.buildErrorMessage(item.Message));
                    isSuccess = false;
                } else {
                    html.push(self.buildSuccessMessage(item.Message));
                }
            });
        }

        this.showMessage(html.join(''));

        if (
            isSuccess &&
            this.productId !== ProductConstants.Keno &&
            this.productId !== ProductConstants.AllBet &&
            this.topWindow.updateProductStatusPopup &&
            this.topWindow.updateProductStatusPopup.onClosing &&
            typeof this.topWindow.updateProductStatusPopup.onClosing === 'function'
        ) {
            this.topWindow.updateProductStatusPopup.onClosing(this.custName, true);
        }
    };

    addNoDataChange = () => {
        const html = this.buildWarningMessage(this.$('#hiddenNoDataChange').val());
        this.showMessage(html);
    };

    showMessage = (html) => {
        this.$msgServerResult.removeClass('hide').html(html);
    };

    hideMessage = () => {
        this.$msgServerResult.addClass('hide').html('');
    };

    buildSuccessMessage = (message: string) => {
        return '<div class="msg msg-success">' + message + '</div>';
    };

    buildErrorMessage = (message: string) => {
        return '<div class="msg msg-error">' + message + '</div>';
    };

    buildWarningMessage = (message: string) => {
        return '<div class="msg msg-warning">' + message + '</div>';
    };
}

$(() => {
    const productStatusPopup = new ProductStatusPopup($, window.top);
    productStatusPopup.init();
});
