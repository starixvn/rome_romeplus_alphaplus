namespace AlphaPlus.BetLists.WinLoss;

public class WinLossParam(
    int custId,
    string productIds,
    DateTime fromDate,
    DateTime toDate,
    string productDetailsXML,
    bool displayVNIP,
    string siteName,
    bool isExcel = false,
    int maxTotalRow = 10000,
    int maxTotalRowConfig = 10000,
    string spSuffix = "")
{
    public int CustId { get; } = custId;

    public DateTime FromDate { get; } = fromDate;

    public DateTime ToDate { get; } = toDate;

    public string Language { get; }

    public string ProductIds { get; } = productIds;

    public DateTime LimitedDate { get; }

    public string ProductDetailsXML { get; } = productDetailsXML;

    public bool DisplayVNIP { get; } = displayVNIP;

    public string SiteName { get; } = siteName;

    public int MaxTotalRow { get; set; } = maxTotalRow;

    public int MaxTotalRowConfig { get; } = maxTotalRowConfig;

    public bool IsExcel { get; } = isExcel;

    public string SpSuffix { get; } = spSuffix;
}