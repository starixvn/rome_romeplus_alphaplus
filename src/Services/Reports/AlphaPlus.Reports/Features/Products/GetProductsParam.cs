namespace AlphaPlus.Reports.Features.Products;

public record GetProductsParam(
    string SiteName,
    int CurrencyId,
    byte SettingStatus,
    string Language,
    DateTime? MinReportDate = null);
