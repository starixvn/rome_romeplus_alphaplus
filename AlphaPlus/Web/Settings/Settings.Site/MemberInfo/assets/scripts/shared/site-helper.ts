"use strict";

interface Window {
    closePopupAndReloadMainFrameAfterUpdateSuccess: Function
}

class SiteHelper {
    static AllowedCharsRegex: RegExp = /\,/g;
    static LeadingZerosRegex: RegExp = /^0+(?!$)/;
    static NonDigitalCharsRegex: RegExp = /\D/g;
    static ThousandsSeparatorRegex: RegExp = /\B(?=(\d{3})+(?!\d))/g;

    static EmptyChar = "";
    static SeparatorChar = ",";

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
}
