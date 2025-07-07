using System.Text.RegularExpressions;

namespace AlphaPlus.CashService.API.Features.Shared.Helpers;

public static class ValidatorHelper
{
    public static bool IsValidAccountHolderName(string accountHolderName) {
        return !string.IsNullOrWhiteSpace(accountHolderName) && accountHolderName.Length <= 255;
    }

    public static bool IsValidAccountNumber(string accountNumber) {
        return !string.IsNullOrWhiteSpace(accountNumber) && accountNumber.Length <= 100 && Regex.IsMatch(accountNumber, @"^\d+$");
    }

    public static bool IsValidCountryCode(string countryCode) {
        return !string.IsNullOrWhiteSpace(countryCode) && Regex.IsMatch(countryCode, @"^\d+$");
    }

    public static bool IsValidMobileNumber(string mobileNumber) {
        return !string.IsNullOrWhiteSpace(mobileNumber) && Regex.IsMatch(mobileNumber, @"^\d+$");
    }

    public static bool IsValidAccountNickname(string accountNickname) {
        return accountNickname.Length <= 255;
    }
}