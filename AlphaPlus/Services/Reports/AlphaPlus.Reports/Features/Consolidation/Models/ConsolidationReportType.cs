namespace AlphaPlus.Reports.Features.Consolidation;

public static class ConsolidationReportType
{
    private const byte Daily = 2;
    private const byte Product = 3;

    public const string ByUsername = "username";
    public const string ByDate = "date";
    public const string ByProduct = "product";

    public static bool IsValidReportType(string reportType)
        => ByUsername == reportType || ByDate == reportType || ByProduct == reportType;

    public static byte MapReportType(string reportType)
        => ByDate == reportType ? Daily : Product;
}