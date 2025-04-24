class AccountInfoTab {
    $: JQueryStatic;
    $userNames: JQuery;
    $givenCredit: JQuery;
    $domains: JQuery;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init = () => {
        this.cacheDOMElement();
        this.bindingEvent();
    };

    bindingEvent() {
        const compactData = this.buildAccountInfoText(true);
        const tooltipMess = this.$('#lbl-copied').val();
        this.copyAndShowTooltip('#compact-copy-btn', compactData, tooltipMess);
    }

    cacheDOMElement() {
        this.$userNames = this.$('#usernames');
        this.$givenCredit = this.$('#txtClonedCredit');
        this.$domains = this.$('#txtDomains');
    }

    formatUserInfo(username: string, credit: string, domain: string) {
        const lblUsername = this.$('#lbl-username').val();
        const lblCredit = this.$('#lbl-credit').val();
        const lblDomain = this.$('#lbl-domain').val();

        return `${lblUsername}: ${username}\n${lblCredit}: ${credit}\n${lblDomain}: ${domain}\n`;
    }

    buildAccountInfoText = (isCompactFormat: boolean) => {
        const userNames = this.$('#usernames').text();
        const givenCredit = this.$('#txtClonedCredit').val();
        const domain = this.$('#txtDomains').text();

        const formatUserInfo = this.formatUserInfo;

        return isCompactFormat
            ? formatUserInfo(userNames, givenCredit, domain)
            : userNames
                  .replace(/\s/g, '')
                  .split(',')
                  .map((name: string) => formatUserInfo(name, givenCredit, domain))
                  .join('\n');
    };

    copyAndShowTooltip(elementSelector: string, textToCopy: string, successMessage = 'Copied!') {
        const element = this.$(elementSelector);

        function handleCopy() {
            if (copyToClipboard(textToCopy)) {
                showTooltip(successMessage);
            }
        }

        function createHiddenTextArea(text) {
            const textArea = document.createElement('textarea');
            textArea.value = text;
            textArea.style.position = 'absolute';
            textArea.style.left = '-999999px';
            return textArea;
        }

        function copyToClipboard(text) {
            try {
                if (window.navigator['clipboard'] && window.isSecureContext) {
                    return window.navigator['clipboard'].writeText(text);
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
}

const accountInfoTab = new AccountInfoTab($);
$(() => {
    accountInfoTab.init();
});
