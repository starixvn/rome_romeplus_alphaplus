"use strict";

enum ValidateResult {
    ValidValue,
    InvalidValue,
    GreaterThanMaxValue
};

class Constants {
    public static EmptyChar: string = "";
    public static SeparatorChar: string = ",";
    public static Unlimited: string = "Unlimited";
    public static ZeroValue: number = 0;
}

class Helper {
    public static IsFloatingPointNumber(value: number): boolean {
        return value % 1 !== Constants.ZeroValue;
    }

    public static ToNumber(value: string): number {
        return Number(value.replace(Regex.AllowedCharsRegex, Constants.EmptyChar));
    }

    public static ToMoneyFormat(value: string): string {
        return value
            .replace(Regex.NonDigitalCharsRegex, Constants.EmptyChar)
            .replace(Regex.LeadingZerosRegex, Constants.EmptyChar)
            .replace(Regex.ThousandsSeparatorRegex, Constants.SeparatorChar);
    }

    public static ToNumberFormat(value: string): number {
        return value === Constants.Unlimited
            ? Constants.ZeroValue
            : this.ToNumber(value);
    }
}

class Regex {
    public static AllowedCharsRegex: RegExp = /\,/g;
    public static LeadingZerosRegex: RegExp = /^0+(?!$)/;
    public static NonDigitalCharsRegex: RegExp = /\D/g;
    public static ThousandsSeparatorRegex: RegExp = /\B(?=(\d{3})+(?!\d))/g;
}

class StatusCode {
    public static Forbidden: number = 403;
    public static Success: number = 0;
}