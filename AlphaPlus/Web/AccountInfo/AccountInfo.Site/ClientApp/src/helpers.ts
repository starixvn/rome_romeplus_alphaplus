export const buildTooltipProps = (tooltip?: string) => {
    return {
        title: tooltip ?? '',
        position: 'bottom',
        trigger: 'mouseenter',
        arrow: true
    };
};

export const formatMessage = (message: string) => message.replace(/\\n/g, '<br />');

export const resizeBootboxOnMobile = () => {
    const bootBoxElement = window.top?.document.getElementsByClassName('bootbox-confirm')[0];
    if (bootBoxElement) {
        bootBoxElement.classList.add('bootbox-account-info');
    }
};

export const usernameRegExp = (message: string, username: string) => message.replace('<username>', username);

export const resizePopupOnMobile = () => {
    const modalElement = window.top?.document.querySelector('[id^="page-popup-"]');
    if (modalElement) {
        modalElement.classList.add('modal-account-info');
    }
};

export const Scale = 0.7;
export const mobileScale = (value: number) => value / Scale;

export const formatNumber = (value: number) => {
    const number = new Intl.NumberFormat('en-US', { maximumFractionDigits: 2 }).format(value);

    return number;
};

export const closeLeftPanelOnMobile = () => {
    const closeIconElement = window.parent.document.getElementById('closeIconOnMobile');
    if (closeIconElement) {
        closeIconElement.click();
    }
};
