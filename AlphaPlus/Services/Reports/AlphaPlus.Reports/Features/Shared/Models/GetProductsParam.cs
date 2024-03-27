namespace AlphaPlus.Reports.Features.Shared.Models;

public record GetProductsParam(
    string SiteName,
    int CurrencyId,
    byte SettingStatus,
    string Language,
    DateTime? MinReportDate = null);
