namespace AlphaPlus.BetLists.Shared.Models;

public record GetProductsParam(
    string SiteName,
    int CurrencyId,
    byte SettingStatus,
    string Language,
    DateTime MinReportDate,
    bool IsSiteLabel777);
