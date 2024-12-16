declare var clipboard: any;
declare var Cleave: any;
declare var numeral: any;

class QRCodeMemberSignIn {
    $: JQueryStatic;
    $form: JQuery;
    $usernameCopyFrom: JQuery;
    usernameCopyFromInQueryRequest: string;
    $creditRange: JQuery;
    $creditMinLabel: JQuery;
    $creditMaxLabel: JQuery;

    $submitBtn: JQuery;

    $creditInput: JQuery;
    credit: number;
    creditMin: number;
    creditMax: number;
    availableCredit: number;

    $messageContainer: JQuery;
    $messageContent: JQuery;

    memberAccountInvalidMessage: string;
    usernameIsInvalid: string;
    iconMessageUsernameCopyFrom: string;

    topWindow: any;
    username: string;
    memberLink: string;
    cleave: any;
    givenCreditMustBeBetweenMinMaxMessage: string;

    inValidCustomerCopyFrom: boolean;
    inValidUsername: boolean;
    invalidCredit: boolean = true;

    showCreateFromUnopenedMemberMessage: boolean;
    cannotCreateClosedMemberQRCodeMessage: string;
    liveText: string;
    creditHint: string;
    memberMaxCreditHint: string;
    $creditHintTooltip: JQuery;

    constructor(
        $: JQueryStatic,
        topWindow: any) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    init = () => {
        this.cacheDOMElement();
        this.checkingLocalStorage();
        this.bindingEvent();

        if (this.$usernameCopyFrom.val() !== '') {
            this.blurEventCustomerCopyFrom();
        }

        this.checkErrorAndShowMessage();
    };

    registerTooltip = () => {
        this.$('[data-toggle="tooltip"]').tooltip({ html: true });

        this.hideTooltipOnTouchOutInMobile();
    }

    hideTooltipOnTouchOutInMobile = () => {
        const $tooltip = this.$('[data-toggle="tooltip"]');

        this.$(window.document).on('touchend', (event) => {
            const target = this.$(event.target);

            $tooltip.tooltip('hide');

            if (target.attr('data-toggle') === 'tooltip') {
                target.tooltip('show');
            }
            else if (target.parent().attr('data-toggle') === 'tooltip') {
                target.parent().tooltip('show');
            }
        });
    }

    bindingEvent = () => {
        this.bidingEasyAutoComplete();
        this.$('#qrCodeMemberSignInForm').submit(event => this.submitEvent(event));

        this.$('#FirstSuffix').change(() => this.checkExistedUsernameByChangeSuffix());
        this.$('#SecondSuffix').change(() => this.checkExistedUsernameByChangeSuffix());
        this.$('#ThirdSuffix').change(() => this.checkExistedUsernameByChangeSuffix());

        this.$('#Credit').keyup(() => {
            this.validateCredit();
            this.checkErrorAndShowMessage();
        });
        this.cleave = new Cleave('#Credit', {
            numeral: true,
            numeralDecimalMark: '.',
            delimiter: ','
        });

        this.registerTooltip();
    }

    checkExistedUsernameByChangeSuffix = () => {
        const username = `${this.$('#actor-name').text()}${this.$('#FirstSuffix').val()}${this.$('#SecondSuffix').val()}${this.$('#ThirdSuffix').val()}`;
        this.checkExistedCustomer(username, 'icon-message-username', false);
    }

    cacheDOMElement = () => {
        this.$form = this.$('#qrCodeMemberSignInForm');
        this.$usernameCopyFrom = this.$('#UsernameCopyFrom');
        this.usernameCopyFromInQueryRequest = this.$('#MemberNameCopyFrom').val();
        this.$submitBtn = this.$('#btn-submit');

        this.$creditRange = this.$('#credit-range');
        this.$creditMinLabel = this.$("#credit-min-label");
        this.$creditMaxLabel = this.$("#credit-max-label");

        this.$creditInput = this.$("#Credit");

        this.$messageContainer = this.$('#message');
        this.$messageContent = this.$('#message-content');
        this.memberAccountInvalidMessage = this.$('#MemberAccountIsInvalid').val();
        this.usernameIsInvalid = this.$('#UsernameIsInvalid').val();
        this.iconMessageUsernameCopyFrom = 'icon-message-UsernameCopyFrom';
        this.cannotCreateClosedMemberQRCodeMessage = this.$('#CannotCreateClosedMemberQRCode').val();
        this.liveText = this.$("#Live").val();
        this.$creditHintTooltip = this.$("#credit-hint-tooltip");
        this.creditHint = this.$("#CreditHint").val();
        this.memberMaxCreditHint = this.$("#MemberMaxCreditHint").val();
    }

    formatNumber = (number: number) => {
        return (Math.trunc(number * 100) / 100).toLocaleString("en-US");
    }

    getMemberCredit = () => {
        this.$.blockUI();

        this.$.ajax({
            method: "POST",
            headers: this.getHeadersForPostRequest(),
            data: { SearchAccountName: this.$usernameCopyFrom.val() },
            dataType: 'json',
            url: "/settings/QRCodeMemberSignIn/GetMemberCreditAndStatus",
            success: result => {
                this.$.unblockUI();
                this.showCreateFromUnopenedMemberMessage = result.Status !== null && result.Status.toUpperCase() !== 'OPEN';

                this.showCreateFromUnopenedMemberMessage ?
                    this.$(`#${this.iconMessageUsernameCopyFrom}`).removeClass().addClass('icon-close') :
                    this.$messageContainer.removeClass('msg msg-error').addClass('hidden');

                this.creditMin = result.CreditMin;
                this.availableCredit = result.CreditMax
                this.creditMax = result.MemberMaxCredit ? Math.min(result.MemberMaxCredit, result.CreditMax) : result.CreditMax;
                this.givenCreditMustBeBetweenMinMaxMessage = result.GivenCreditMustBeBetweenMinMaxMessage;

                this.$creditRange.removeClass('hidden');
                this.$creditMinLabel.html(this.formatNumber(this.creditMin));
                this.$creditMaxLabel.html(this.formatNumber(this.creditMax));
                this.$creditInput.val(this.formatNumber(result.Credit));

                console.log(result.MemberMaxCredit);
                if (result.MemberMaxCredit) {
                    const creditHint = `${this.creditHint.replace("{0}", this.formatNumber(this.availableCredit))}<br>${this.memberMaxCreditHint.replace("{0}", this.formatNumber(result.MemberMaxCredit))}`;
                    this.$creditHintTooltip.removeClass("hidden").attr("data-original-title", creditHint);
                } else {
                    this.$creditHintTooltip.addClass("hidden");
                }

                this.validateCredit();
                this.checkErrorAndShowMessage();
                this.$.unblockUI();
            },
            error: () => { this.$.unblockUI(); }
        });
    }

    showErrorMessage = (message) => {
        this.$messageContainer.removeClass('hidden').addClass('msg msg-error');
        this.$messageContent.html(message);
    }

    showMessageContent = (messageContent) => {
        var message = `<div class="main-message">${messageContent.MainMessage}</div>`
        if (messageContent.SubMessages && messageContent.SubMessages.length) {
            messageContent.SubMessages.forEach((subMessage) => {
                message += `<div class="sub-message">`
                if (subMessage.IsLive) {
                    message += `<span class="live-badge">${this.liveText}</span> `;
                }
                message += `${subMessage.Message}</div>`;
            });
        }
        this.$messageContainer.removeClass('hidden').addClass('msg msg-error');
        this.$messageContent.html(message);
    }

    checkingLocalStorage = () => {
        this.usernameCopyFromInQueryRequest === '' && JSON.parse(localStorage.getItem('IsOldActor')) ?
            this.$usernameCopyFrom.val(localStorage.getItem('memberNameCopyFrom')) :
            localStorage.removeItem('memberNameCopyFrom');
    }

    getHeadersForPostRequest = () => {
        return {
            RequestVerificationToken: this.$("input[name='__RequestVerificationToken']").val(),
            'X-Requested-With': 'XMLHttpRequest'
        };
    }

    validateCredit = () => {
        this.credit = parseFloat(this.cleave.getRawValue());
        this.invalidCredit = isNaN(this.credit) || this.credit < this.creditMin || this.credit > this.creditMax;
        const errorClass = this.invalidCredit ? 'icon-close' : 'icon-check';
        this.$('#icon-message-credit').removeClass().addClass(errorClass);
    }

    submitEvent = (event) => {
        event.preventDefault();

        if (this.checkErrorAndShowMessage()) {
            return;
        }

        this.$.blockUI();

        const data = {
            UsernameCopyFrom: this.$usernameCopyFrom.val(),
            FirstName: this.$('#FirstName').val(),
            FirstSuffix: this.$('#FirstSuffix').val(),
            SecondSuffix: this.$('#SecondSuffix').val(),
            ThirdSuffix: this.$('#ThirdSuffix').val(),
            Credit: this.credit,
            CreditMin: this.creditMin,
            CreditMax: this.creditMax,
            c_v: this.$("input[name=c_v]").val(),
            c_f: this.$("input[name=c_f]").val()
        };

        this.$.ajax({
            method: "POST",
            headers: this.getHeadersForPostRequest(),
            data: data,
            dataType: 'json',
            url: "/settings/QRCodeMemberSignIn/CreateAccount",
            success: (result, status, xqr) => {
                if (result.ErrCode === 0 || status === "302" || xqr.status === 302) {
                    localStorage.setItem("memberNameCopyFrom", data.UsernameCopyFrom);
                    setTimeout(() => {
                        this.reloadFrame(result);
                    }, 1000);
                }
                else if (result.ErrCode === 998)
                {
                    this.showErrorMessage(result.ErrMsg);
                    this.$.unblockUI();
                }
                else {
                    this.showMessageContent(result.MessageContent);
                    this.$.unblockUI();
                }
            },
            error: () => { this.$.unblockUI(); }
        });
    }

    reloadFrame = (result) => {
        const frms = window.top.parent.frames;
        const showQRCodeLink = `${this.$('#ShowQRCodeLink').val()}?${this.$.param(result)}`;
        if (frms && frms["main"]) {
            frms["main"].frames['frmAddNewMember'] ?
                frms["main"].frames['frmAddNewMember'].location = showQRCodeLink :
                frms["main"].location = showQRCodeLink;
        }
        else {
            window.location.href = showQRCodeLink;
        }
    }

    showMessageIcon = (iconId: string, existedCustomer: boolean, checkCustomerCopyFrom: boolean = true) => {
        this.$(`#${iconId}`).removeClass();

        let iconClass = '';
        if (checkCustomerCopyFrom) {
            this.inValidCustomerCopyFrom = !existedCustomer;
            iconClass = existedCustomer ? 'icon-check' : 'icon-close';
        }
        else {
            this.inValidUsername = existedCustomer;
            iconClass = existedCustomer ? 'icon-close' : 'icon-check';
        }

        this.$(`#${iconId}`).addClass(iconClass);
        this.checkErrorAndShowMessage();
    }

    checkExistedCustomer = (username, iconId: string, checkCustomerCopyFrom: boolean = true) => {
        this.$.ajax({
            method: "POST",
            headers: this.getHeadersForPostRequest(),
            data: { username: username },
            dataType: 'json',
            url: "/settings/QRCodeMemberSignIn/CheckExistedCustomer",
            success: (result) => {
                this.showMessageIcon(iconId, result, checkCustomerCopyFrom);
            }
        });
    }

    blurEventCustomerCopyFrom = () => {
        var sourceUsername: string = this.$usernameCopyFrom.val().trim();
        if (sourceUsername && sourceUsername.length) {
            this.checkExistedCustomer(this.$usernameCopyFrom.val(), this.iconMessageUsernameCopyFrom);
            this.getMemberCredit();
        }
    }

    bidingEasyAutoComplete = () => {
        const options = {
            url: phrase => {
                return `${this.$('#member-info-url').val()}UserNameSuggestion?Query=${phrase}`;
            },
            getValue: 'UserName',
            requestDelay: 200,
            minCharNumber: 2,
            template: {
                type: "custom",
                method: (value, item) => {
                    const nickname = item.UserName2 === null || item.UserName2 === '' ? '' : `(${item.UserName2})`;
                    return `<div class='suggestion'>${value} ${nickname}<div>${item.FirstName} ${item.LastName}</div></div>`;
                }
            },
            list: {
                maxNumberOfElements: 10
            }
        };

        this.$usernameCopyFrom.easyAutocomplete(options);
        this.$usernameCopyFrom.blur(() => {
            setTimeout(() => {
                this.blurEventCustomerCopyFrom();
            }, 300);
        });

        this.$usernameCopyFrom.on('input', () => {
            this.$(`#${this.iconMessageUsernameCopyFrom}`).removeClass().addClass('hidden');
            if (!this.$usernameCopyFrom.val()) {
                this.inValidCustomerCopyFrom = true;
                this.checkErrorAndShowMessage();
            }
        });
    }

    checkErrorAndShowMessage = () => {
        if (!this.inValidCustomerCopyFrom && !this.inValidUsername && !this.invalidCredit) {
            this.$submitBtn.prop('disabled', false);
        }
        else {
            this.$submitBtn.prop('disabled', true);
        }

        if (this.showCreateFromUnopenedMemberMessage && this.cannotCreateClosedMemberQRCodeMessage) {
            this.showErrorMessage(this.cannotCreateClosedMemberQRCodeMessage);
            return true;
        }

        if (this.inValidCustomerCopyFrom && this.memberAccountInvalidMessage) {
            this.showErrorMessage(this.memberAccountInvalidMessage);
            return true;
        }

        if (this.invalidCredit && this.givenCreditMustBeBetweenMinMaxMessage) {
            this.showErrorMessage(this.givenCreditMustBeBetweenMinMaxMessage);
            return true;
        }

        if (this.inValidUsername && this.usernameIsInvalid) {
            this.showErrorMessage(this.usernameIsInvalid);
            return true;
        }

        this.$messageContainer.addClass('hidden');
        this.$messageContent.html('');
        return false;
    }
}

const qrCodeMemberSignIn = new QRCodeMemberSignIn($, window.top);

$(() => {
    qrCodeMemberSignIn.init();
});