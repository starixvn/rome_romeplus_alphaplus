'use strict';

interface Window {
    closePopupAndReloadMainFrameAfterUpdateSuccess: Function;
}

class SiteHelper {
    static AllowedCharsRegex: RegExp = /\,/g;
    static LeadingZerosRegex: RegExp = /^0+(?!$)/;
    static NonDigitalCharsRegex: RegExp = /\D/g;
    static ThousandsSeparatorRegex: RegExp = /\B(?=(\d{3})+(?!\d))/g;

    static EmptyChar = '';
    static SeparatorChar = ',';

    static closePopupAndReloadMainFrame(reloadTimeout: number = 3000) {
        window.top.closePopupAndReloadMainFrameAfterUpdateSuccess(reloadTimeout);
    }

    static ToMoneyFormat(value) {
        return value
            .replace(SiteHelper.NonDigitalCharsRegex, SiteHelper.EmptyChar)
            .replace(SiteHelper.LeadingZerosRegex, SiteHelper.EmptyChar)
            .replace(SiteHelper.ThousandsSeparatorRegex, SiteHelper.SeparatorChar);
    }

    static ToNumber(value) {
        return Number(value.replace(SiteHelper.AllowedCharsRegex, SiteHelper.EmptyChar));
    }

    static FormatCompactNumber(number: number) {
        let prefix = '';
        let suffix = '';

        if (number < 0) {
            prefix = '-';
            number *= -1;
        }

        if (number >= 1_000 && number < 1_000_000) {
            number /= 1_000;
            suffix = 'K';
        } else if (number >= 1_000_000 && number < 1_000_000_000) {
            number /= 1_000_000;
            suffix = 'M';
        } else if (number >= 1_000_000_000 && number < 1_000_000_000_000) {
            number /= 1_000_000_000;
            suffix = 'B';
        } else if (number >= 1_000_000_000_000 && number < 1_000_000_000_000_000) {
            number /= 1_000_000_000_000;
            suffix = 'T';
        }

        return `${prefix}${number.toLocaleString('en-US', { maximumFractionDigits: 2 })}${suffix}`;
    }
}
