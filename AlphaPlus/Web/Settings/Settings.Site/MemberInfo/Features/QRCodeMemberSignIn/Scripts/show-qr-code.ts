/// <reference path="../../../assets/scripts/shared/top-windows-interface.ts" />

declare var clipboard: any;
declare var numeral: any;

interface Window {
    alpha: any,
    updateGivenCreditPopup: any,
    updateInformationPopup: any,
    ageWnd: any
}

class ShowQRCode {
    $: JQueryStatic;
    topWindow: any;
    username: string;

    constructor(
        $: JQueryStatic,
        topWindow: any) {
        this.$ = $;
        this.topWindow = topWindow;
   }

    init = () => {
        this.username = this.$('#SearchAccountName').val();

        this.$('#btn-copy-link').click(() => {
            clipboard.writeText(this.$('#QRCodeLink').val());
        });

        this.$('#domains').change(() => this.domainOnChange());

        this.$('#btn-update-information').click(() => this.updateInformation());

        this.$('#btn-update-credit').click(() => this.updateCredit());
    };

    resizePopup = () => {
        if (top.alpha.isScreenLower768) {
            const modalElement = window.top.document.querySelector('[id^="page-popup-"]');
            if (modalElement) {
                modalElement.classList.add('modal-account-info');
            }
        }
    };

    givenPopupOnclosing = (username = null, updateSuccessfully = false, credit = 0) => {
        if (updateSuccessfully === true) {
            const creditFormat = numeral(credit);
            this.$('#credit-label').text(creditFormat.format('0,0.##'));
            setTimeout(() => {
                window.top.updateGivenCreditPopup.close();
            }, 1000);
		}
		else {
			window.top.updateGivenCreditPopup.close();
		}
    };

    informationPopupOnclosing = (updateSuccessfully = false, firstName = '') => {
        if (updateSuccessfully === true) {
            this.$('#first-name-label').text(firstName);
            setTimeout(() => {
                window.top.updateInformationPopup.close();
            }, 1000);
        }
        else {
            window.top.updateInformationPopup.close();
        }
    };

    domainOnChange = () => {
        this.$.blockUI();

        const params = {
            SearchAccountName: this.username,
            SearchAccountId: this.$('#SearchAccountId').val(),
            Domain: this.$('#domains').val()
        };

        const headers = {
            __RequestVerificationToken: this.$("input[name='__RequestVerificationToken']").val(),
            'X-Requested-With': 'XMLHttpRequest'
        };

        this.$.ajax({
            method: "POST",
            headers: headers,
            data: params,
            dataType: 'json',
            url: "GenerateQrCode",
			success: (result) => {
				if (result.QRCodeLink === null || result.QRCodeLink === '') {
					this.$('#message').removeClass('hidden').addClass('msg msg-error');
					this.$('#message').html(this.$('#cannot-generate-message').val());
				}
				else {
					this.$('#qr-code-image').attr('src', result.QRCodeImage);
					this.$('#QRCodeLink').val(result.QRCodeLink);
				}

                this.$.unblockUI();
			},
			error: () => { this.$.unblockUI(); }
        });
    }

    updateInformation = () => {
        if (window.top.updateInformationPopup === null || window.top.updateInformationPopup === undefined) {
            window.top.updateInformationPopup = window.top.ageWnd.createInstance();
        }

        window.top.updateInformationPopup.onClosing = this.informationPopupOnclosing;
        window.top.popupManager.openWithHanlder(
            top.updateInformationPopup,
            this.$('#UpdateInformationUrl').val(),
            `${this.$('#update-infor-text').val()} - ${this.username}`,
            700,
            400);

        this.resizePopup();
    };

    updateCredit = () => {
        if (window.top.updateGivenCreditPopup === null || window.top.updateGivenCreditPopup === undefined) {
            window.top.updateGivenCreditPopup = window.top.ageWnd.createInstance();
        }

        window.top.updateGivenCreditPopup.onClosing = this.givenPopupOnclosing;
        window.top.popupManager.openWithHanlder(
            top.updateGivenCreditPopup,
            `${this.$('#UpdateCreditUrl').val()}&CurrentGivenCredit=${this.$('#credit-label').text()}`,
            `${this.$('#update-credit-text').val()} - ${this.username}`,
            500,
            300);

        this.resizePopup();
    };
}

const showQRCode = new ShowQRCode($, window.top);

$(() => {
    showQRCode.init();
});
