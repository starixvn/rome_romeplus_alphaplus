declare var Cleave: any;
declare var numeral: any;

interface Math {
    trunc(number: number): number;
}

class CopyAccount {
    $: JQueryStatic;
    encryptor: Encryptor;
    $form: JQuery;
    $result: JQuery;
    $sourceUsername: JQuery;
    $creditRange: JQuery;
    $creditMaxLabel: JQuery;
    $submitBtn: JQuery;
    $firstSuffix: JQuery;
    $secondSuffix: JQuery;
    $thirdSuffix: JQuery;
    $noOfAccounts: JQuery;
    $password: JQuery;
    $showPass: JQuery;

    $creditInput: JQuery;
    credit: number = 0;
    creditMin: number = 0;
    creditMax: number = 0;
    availableCredit: number = 0;
    maxGivenCreditPerMember?: number = null;
    memberMaxCredit: number;
    $memberMaxCreditInput: JQuery;

    $messageContainer: JQuery;
    $messageContent: JQuery;

    topWindow: any;
    cleave: any;
    memberMaxCreditCleave: any;

    isCreateAgent: boolean;
    isCreateMember: boolean;
    inValidSourceAccount = true;
    invalidCredit: boolean = false;
    invalidMemberMaxCredit: boolean = false;
    invalidUsername: boolean = false;

    $btnSwitchVersion: JQuery;
    oldVersionUrl: string;
    hashKey: string;

    $sourceAccountErrMsg: JQuery;
    $creditErrMsg: JQuery;
    $memberMaxCreditErrMsg: JQuery;
    $creditHintTooltip: JQuery;
    $memberCreditRange: JQuery;
    $memberMaxCreditLabel: JQuery;
    
    constructor(
        $: JQueryStatic,
        encryptor: Encryptor,
        topWindow: any) {
        this.$ = $;
        this.topWindow = topWindow;
        this.encryptor = encryptor;
    }

    resource = {
        invalidMaxCreditPerAccount: (<HTMLInputElement>document.getElementById('InvalidMaxCreditPerAccount')).value,
        invalidMemberMaxCredit: (<HTMLInputElement>document.getElementById('InvalidMemberMaxCredit')).value,
        live: (<HTMLInputElement>document.getElementById('Live')).value,
        creditHint: (<HTMLInputElement>document.getElementById('CreditHint')).value,
        memberMaxCreditHint: (<HTMLInputElement>document.getElementById('MemberMaxCreditHint')).value,
        lblCredit: (<HTMLInputElement>document.getElementById('lbl-credit')).value,
        lblUsername: (<HTMLInputElement>document.getElementById('lbl-username')).value,
        lblDomain: (<HTMLInputElement>document.getElementById('lbl-domain')).value,
    }

    init = () => {
        this.cacheDOMElement();
        this.bindingEvent();

        if (this.$sourceUsername.val() !== '') {
            this.blurEventSourceAccount();
        }
    };

    resetValidationState = () => {
        this.inValidSourceAccount = true;
        this.invalidCredit = false;
        this.invalidMemberMaxCredit = false;

        this.removeTooltipError(this.$creditErrMsg);
        this.$('#icon-message-credit').removeClass().addClass("hidden");
        if (this.isCreateAgent) {
            this.removeTooltipError(this.$memberMaxCreditErrMsg);
            this.$('#icon-message-member-max-credit').removeClass().addClass("hidden");
        }
        this.$creditRange.removeClass().addClass('hidden');
    }

    cacheDOMElement = () => {
        this.$form = this.$('#copyAccountForm');
        this.$result = this.$("#result");
        this.$submitBtn = this.$('#btn-submit');

        this.$sourceUsername = this.$('#SourceUsername');
        this.$sourceAccountErrMsg = this.$("#sourceAccountErrMsg");

        this.$creditRange = this.$('#credit-range');
        this.$creditMaxLabel = this.$("#credit-max-label");
        this.$creditInput = this.$("#Credit");
        this.$memberMaxCreditInput = this.$("#MemberMaxCredit");

        this.$messageContainer = this.$('#message');
        this.$messageContent = this.$('#message-content');
        this.$firstSuffix = this.$("#FirstSuffix");
        this.$secondSuffix = this.$("#SecondSuffix");
        this.$thirdSuffix = this.$("#ThirdSuffix");
        this.$noOfAccounts = this.$("#NoOfAccounts");
        this.$password = this.$("#Password");

        this.isCreateAgent = this.$("#IsCreateAgent").val() == "True";
        this.isCreateMember = this.$("#IsCreateMember").val() == "True";
        this.$btnSwitchVersion = this.$("#btn-switch-view");
        this.oldVersionUrl = this.$("#old-version-url").val();

        this.$showPass = this.$(".show-pass");
        this.hashKey = this.$("#HashKey").val();
        this.$creditErrMsg = this.$("#creditErrMsg");
        this.$memberMaxCreditErrMsg = this.$("#memberMaxCreditErrMsg");
        this.$creditHintTooltip = this.$("#credit-hint-tooltip");
        this.$memberCreditRange = this.$("#member-max-credit-range");
        this.$memberMaxCreditLabel = this.$("#member-credit-max-label");

    }

    formatNumber = (number: number) => {
        return (Math.trunc(number * 100) / 100).toLocaleString("en-US");
    }

    showPass = () => {
        this.changeEyeIcon(this.$showPass);
        if (this.$password.hasClass("input-pass")) {
            this.$password.removeClass("input-pass")
        } else {
            this.$password.addClass("input-pass")
        }
    }

    changeEyeIcon = (eyeElm) => {
        if (eyeElm.hasClass("icon-eye-close")) {
            eyeElm.removeClass("icon-eye-close");
            eyeElm.addClass("icon-eye");
        } else {
            eyeElm.removeClass("icon-eye");
            eyeElm.addClass("icon-eye-close");
        }
    }

    bindingEvent = () => {
        this.bidingEasyAutoComplete();
        this.$form.submit(event => this.submitEvent(event));

        this.$firstSuffix.change(() => this.checkExistedUsernameByChangeSuffix());
        this.$secondSuffix.change(() => this.checkExistedUsernameByChangeSuffix());
        this.$thirdSuffix.change(() => this.checkExistedUsernameByChangeSuffix());
        this.$password.keyup(() => this.validatePassword());
        this.$showPass.click(this.showPass);

        this.$creditInput.on("input", () => {
            if (!this.inValidSourceAccount) {
                this.validateCredit();
                this.validateInputs();
                if (this.isCreateAgent) {
                    this.$memberMaxCreditLabel.html(`<strong>${this.formatNumber(this.credit)}`)
                    this.validateMemberMaxCredit();
                }
            }
        });
        this.cleave = new Cleave('#Credit', {
            numeral: true,
            numeralDecimalMark: '.',
            delimiter: ','
        });

        this.$btnSwitchVersion.click(() => {
            this.topWindow.indexManager.changeMenu(this.oldVersionUrl);
        })

        if (this.isCreateAgent) {
            this.memberMaxCreditCleave = new Cleave("#MemberMaxCredit", {
                numeral: true,
                numeralDecimalMark: '.',
                delimiter: ','
            });

            this.$memberMaxCreditInput.on("input", () => {
                if (!this.inValidSourceAccount) {
                    this.validateMemberMaxCredit();
                    this.validateInputs();
                }
            });

            this.$memberMaxCreditInput.focus(() => this.showTooltipError(this.$memberMaxCreditErrMsg));
        }

        window.addEventListener("click", (e) => {
            if (this.$(e.target).parent().hasClass("error-input") || this.$(e.target).attr("id") == "NoOfAccounts") {
                return;
            }

            this.hideTooltips();
        });

        this.$sourceUsername.focus(() => this.showTooltipError(this.$sourceAccountErrMsg));
        this.$creditInput.focus(() => this.showTooltipError(this.$creditErrMsg));

        this.registerTooltip();


    }

    formatUserInfo(username: string, credit: string, domain: string) {
        const lblUsername = this.$('#lbl-username').val();
        const lblCredit = this.$('#lbl-credit').val();
        const lblDomain = this.$('#lbl-domain').val();

        return `${lblUsername}: ${username}\n${lblCredit}: ${credit}\n${lblDomain}: ${domain}\n`;
    }
    
    buildAccountInfoText = (isCompactFormat: boolean) => {
        const userNames = this.$("#usernames").text();
        const givenCredit = this.$("#txtClonedCredit").val();
        const domain = this.$("#txtDomains").text();

        const formatUserInfo = this.formatUserInfo;

        return isCompactFormat
            ? formatUserInfo(userNames, givenCredit, domain)
            : userNames.replace(/\s/g, '')
                .split(',')
                .map((name: string) => formatUserInfo(name, givenCredit, domain))
                .join("\n")
    }

    copyAndShowTooltip(elementSelector: string, textToCopy: string, successMessage = 'Copied!') {
        const element = this.$(elementSelector);
        
        function handleCopy() {
            if (copyToClipboard(textToCopy)) {
                showTooltip(successMessage);
            }
        }

        function createHiddenTextArea(text) {
            const textArea = document.createElement("textarea");
            textArea.value = text;
            textArea.style.position = "absolute";
            textArea.style.left = "-999999px";
            return textArea;
        }
        
        function copyToClipboard(text) {
            try {
                if (window.navigator["clipboard"] && window.isSecureContext) {
                    return window.navigator["clipboard"].writeText(text)
                } else {
                    const textArea = createHiddenTextArea(text);
                    document.body.prepend(textArea);
                    textArea.select();
                    document.execCommand('copy');
                    textArea.remove();
                }
                return true;
            } catch (err) {
                console.log('Failed to copy text:', err);
                return false;
            }
        }
        
        function showTooltip(message) {
            element.attr('data-original-title', message);
            element.tooltip('show');

            setTimeout(() => {
                element.tooltip('hide');
                element.removeAttr('data-original-title');
            }, 1500);
        }
        
        element.on('click', handleCopy);
    }

    registerTooltip = () => {
        this.$('[data-toggle="tooltip"]').tooltip({html: true});

        this.hideTooltipOnTouchOutInMobile();
    }

    hideTooltipOnTouchOutInMobile = () => {
        const $tooltip = this.$('[data-toggle="tooltip"]');

        this.$(window.document).on('touchend', (event) => {
            const target = this.$(event.target);

            $tooltip.tooltip('hide');

            if (target.attr('data-toggle') === 'tooltip') {
                target.tooltip('show');
            } else if (target.parent().attr('data-toggle') === 'tooltip') {
                target.parent().tooltip('show');
            }
        });
    }

    showTooltipError = ($errorContianer: JQuery) => {
        if ($errorContianer.parent().hasClass("error-input")) {
            $errorContianer.removeClass("hidden");
        }
    }

    addTooltipError = ($errorContainer: JQuery, message: string) => {
        $errorContainer.parent().addClass("error-input");
        $errorContainer.html(message);
        $errorContainer.removeClass("hidden");
    }

    removeTooltipError = ($errorContainer: JQuery) => {
        $errorContainer.parent().removeClass("error-input");
        $errorContainer.html("");
        $errorContainer.addClass("hidden");
    }

    hideTooltips = () => {
        this.$(".error-input-text").addClass("hidden");
    }

    validateMemberMaxCredit = () => {
        var memberMaxCredit = parseFloat(this.memberMaxCreditCleave.getRawValue());
        this.invalidMemberMaxCredit = this.isInValidCredit(memberMaxCredit, this.credit);
        if (!this.invalidMemberMaxCredit) {
            this.memberMaxCredit = memberMaxCredit;
            this.removeTooltipError(this.$memberMaxCreditErrMsg);
        } else {
            this.addTooltipError(this.$memberMaxCreditErrMsg, this.resource.invalidMemberMaxCredit.replace("{0}", this.formatNumber(this.credit)));
        }
        const errorClass = this.invalidMemberMaxCredit ? 'icon-close' : 'icon-check';
        this.$('#icon-message-member-max-credit').removeClass().addClass(errorClass);
    }

    bidingEasyAutoComplete = () => {
        const options = {
            url: phrase => {
                return `/settings/UserNameSuggestion?Query=${phrase}`;
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

        this.$sourceUsername.easyAutocomplete(options);
        this.$sourceUsername.blur(() => {
            setTimeout(() => {
                this.blurEventSourceAccount();
            }, 300);
        });

        this.$noOfAccounts.on('input', () => {
            if (!this.inValidSourceAccount) {
                const numberOfAccounts = parseInt(this.$noOfAccounts.val());
                this.creditMax = this.maxGivenCreditPerMember ? Math.min(this.maxGivenCreditPerMember, this.availableCredit / numberOfAccounts) : (this.availableCredit / numberOfAccounts);
                this.$creditRange.removeClass('hidden');
                this.$creditMaxLabel.html(`<strong>${this.formatNumber(this.creditMax)}`);
                this.validateCredit();
                this.validateInputs();
            }
        });

        this.$sourceUsername.on('input', () => {
            this.$("#icon-message-SourceUsername").addClass("hidden");
            if (!this.$sourceUsername.val()) {
                this.inValidSourceAccount = true;
                this.validateInputs();
            }
        });
    }

    validateInputs = () => {
        const password = this.$.trim(this.$password.val());
        const isValidPassword = this.isCreateMember ? this.isValidMemberPassword(password) : this.isValidPassword(password);
        this.$submitBtn.prop('disabled', this.inValidSourceAccount || this.invalidUsername || this.invalidCredit || this.invalidMemberMaxCredit || !isValidPassword);
    }

    submitEvent = (event) => {
        event.preventDefault();

        this.$.blockUI();

        const data = {
            SourceUsername: this.$sourceUsername.val(),
            FirstSuffix: this.$firstSuffix.val(),
            SecondSuffix: this.$secondSuffix.val(),
            ThirdSuffix: this.$thirdSuffix.val(),
            Credit: this.credit,
            CreditMin: this.creditMin,
            CreditMax: this.creditMax,
            NumberOfAccounts: this.$noOfAccounts.val(),
            Password: this.encryptor.encrypt(this.$password.val(), this.hashKey)
        };

        if (this.isCreateAgent) {
            data["MemberMaxCredit"] = this.memberMaxCredit;
        }

        this.$.ajax({
            method: "POST",
            headers: this.getHeadersForPostRequest(),
            data: data,
            dataType: 'text',
            url: "/settings/CopyAccount/Index",
            success: (data) => {
                var result = this.safelyParseJSON(data);
                if (result && result.IsFailed) {
                    if (result.MessageContent) {
                        this.showMessageContent(result.MessageContent);
                    } else {
                        this.showErrorMessage(result.Message);
                    }
                } else {
                    this.$result.append(data);
                    this.$result.removeClass("hidden");
                    this.$form.addClass("hidden");
                    this.registerCopyEvenHandler();
                }
                this.$.unblockUI();
            },
            error: () => {
                this.$.unblockUI();
            }
        });
    }

    registerCopyEvenHandler = () => {
        const compactData = this.buildAccountInfoText(true);
        const individualData = this.buildAccountInfoText(false);
        const tooltipMess = this.$('#lbl-copied').val();
        this.copyAndShowTooltip('#compact-copy-btn', compactData, tooltipMess);
        this.copyAndShowTooltip('#individual-copy-btn', individualData, tooltipMess);
    }

    safelyParseJSON = (json) => {
        var parsed;
        try {
            parsed = JSON.parse(json);
        } catch (e) {
        }

        return parsed;
    }

    checkExistedUsernameByChangeSuffix = () => {
        let username = `${this.$('#ActorUsername').val()}${this.$firstSuffix.val()}${this.$secondSuffix.val()}`;
        if (this.$thirdSuffix.val()) {
            username = `${username}${this.$thirdSuffix.val()}`;
        }

        this.$.ajax({
            method: "POST",
            headers: this.getHeadersForPostRequest(),
            data: {custName: username},
            dataType: 'json',
            url: "/settings/CopyAccount/CheckExistedCustomer",
            success: (result) => {
                this.invalidUsername = result;
                const iconClass = result ? 'icon-close' : 'icon-check';
                this.$("#icon-message-username").removeClass().addClass(iconClass);

                if (result) {
                    this.$("#icon-message-username").parent().addClass("error-input");
                } else {
                    this.$("#icon-message-username").parent().removeClass("error-input");
                }
                this.validateInputs();
            }
        });
    }

    blurEventSourceAccount = () => {
        var sourceUsername: string = this.$sourceUsername.val();
        if (sourceUsername && sourceUsername.length) {
            this.$.ajax({
                method: "POST",
                headers: this.getHeadersForPostRequest(),
                data: {custName: sourceUsername},
                dataType: 'json',
                url: "/settings/CopyAccount/CheckUsername",
                success: (result) => {
                    const iconClass = result.IsOk ? 'icon-check' : 'icon-close';
                    this.$("#icon-message-SourceUsername").removeClass().addClass(iconClass);
                    if (result.IsOk) {
                        this.getCustomerCredit(this.$sourceUsername.val());
                        this.removeTooltipError(this.$sourceAccountErrMsg);
                        this.inValidSourceAccount = false;
                        this.validateCredit();
                    } else {
                        this.addTooltipError(this.$sourceAccountErrMsg, result.Message);
                        this.$creditRange.addClass("hidden");
                        this.resetValidationState();
                    }

                    this.validateInputs();
                }
            });
        } else {
            this.resetValidationState();
        }
    }

    getCustomerCredit = (username: string) => {
        this.$.blockUI();

        this.$.ajax({
            method: "POST",
            headers: this.getHeadersForPostRequest(),
            data: {custName: username},
            dataType: 'json',
            url: "/settings/CopyAccount/GetCredit",
            success: result => {
                this.$.unblockUI();

                this.creditMin = 0;
                this.availableCredit = result.MaxCredit;
                var noOfAccounts = parseInt(this.$noOfAccounts.val());
                this.creditMax = result.MemberMaxCredit ? Math.min(result.MemberMaxCredit, result.MaxCredit / noOfAccounts) : (result.MaxCredit / noOfAccounts);
                this.maxGivenCreditPerMember = result.MemberMaxCredit;
                this.$creditRange.removeClass('hidden');
                this.$creditMaxLabel.html(`<strong>${this.formatNumber(this.creditMax)}`);
                let creditHint = this.resource.creditHint.replace("{0}", this.formatNumber(this.availableCredit));
                if (this.maxGivenCreditPerMember) {
                    creditHint += `<br>${this.resource.memberMaxCreditHint.replace("{0}", this.formatNumber(this.maxGivenCreditPerMember))}`;
                }
                this.$creditHintTooltip.attr("data-original-title", creditHint)

                this.isCreateAgent && this.$memberCreditRange.removeClass("hidden");

                parseFloat(this.cleave.getRawValue()) && this.validateCredit();

                this.$.unblockUI();
            },
            error: () => {
                this.$.unblockUI();
            }
        });
    }

    validateCredit = () => {
        this.credit = parseFloat(this.cleave.getRawValue());

        this.invalidCredit = this.isInValidCredit(this.credit, this.creditMax);
        if (this.invalidCredit && !this.inValidSourceAccount) {
            this.addTooltipError(this.$creditErrMsg, this.resource.invalidMaxCreditPerAccount.replace("{0}", this.formatNumber(this.creditMax)));
        } else {
            this.removeTooltipError(this.$creditErrMsg);
            if (this.isCreateAgent) {
                this.validateMemberMaxCredit();
            }
        }

        const errorClass = this.invalidCredit ? 'icon-close' : 'icon-check';
        this.$('#icon-message-credit').removeClass().addClass(errorClass);
    }

    isInValidCredit = (credit: number, max: number) => {
        return isNaN(credit) || credit < this.creditMin || credit > max;
    }

    getHeadersForPostRequest = () => {
        return {
            RequestVerificationToken: this.$("input[name='__RequestVerificationToken']").val(),
            'X-Requested-With': 'XMLHttpRequest'
        };
    }

    showMessageContent = (messageContent) => {
        var message = `<div class="main-message">${messageContent.MainMessage}</div>`
        if (messageContent.SubMessages && messageContent.SubMessages.length) {
            messageContent.SubMessages.forEach((subMessage) => {
                message += `<div class="sub-message">`
                if (subMessage.IsLive) {
                    message += `<span class="live-badge">${this.resource.live}</span> `;
                }
                message += `${subMessage.Message}</div>`;
            });
        }
        this.$messageContainer.removeClass('hidden').addClass('msg msg-error');
        this.$messageContent.html(message);
    }

    showErrorMessage = (message) => {
        this.$messageContainer.removeClass('hidden').addClass('msg msg-error');
        this.$messageContent.html(message);
    }

    validatePassword = () => {
        var validPassword = this.isCreateMember ? this.isValidMemberPassword(this.$password.val()) : this.isValidPassword(this.$password.val());
        const iconClass = validPassword ? 'icon-check' : 'icon-close';
        this.$('#icon-message-password').removeClass().addClass(iconClass);
        this.validateInputs();

        if (validPassword) {
            this.$password.parent().removeClass("error-input");
        } else {
            this.$password.parent().addClass("error-input");
        }
    }

    isValidPassword = (password) => {
        var whiteSpace = /^\S*$/;
        var whiteSpaceCase = password.search(whiteSpace) != -1;

        var regex1 = /^(?=.{8,100}$).*\d/i;
        var regex2 = /^(?=.{8,100}$).*[a-z]/;
        var regex3 = /^(?=.{8,100}$).*[A-Z]/;
        var regex4 = /^(?=.{8,100}$).*[\[\]\^\$\.\|\?\*\+\(\)\\~`\!@#%&\-_+={}'""<>:;,]/i;

        var case1 = password.search(regex1) != -1 ? 1 : 0;
        var case2 = password.search(regex2) != -1 ? 1 : 0;
        var case3 = password.search(regex3) != -1 ? 1 : 0;
        var case4 = password.search(regex4) != -1 ? 1 : 0;

        return (case1 + case2 + case3 + case4 >= 3) && whiteSpaceCase;
    }

    isValidMemberPassword = (str: string) => {
        const regex = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,10}$/;
        const whiteSpace = /^\S*$/;

        const case5 = str.search(whiteSpace) !== -1;

        return str.search(regex) !== -1 && case5;
    }
}

const copyAccount = new CopyAccount($, new Encryptor(), window.top);

$(() => {
    copyAccount.init();
});