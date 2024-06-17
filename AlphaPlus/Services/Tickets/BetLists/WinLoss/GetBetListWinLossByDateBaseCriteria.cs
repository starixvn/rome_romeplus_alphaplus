using System.Data.SqlTypes;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.WinLoss;

internal abstract class GetBetListWinLossByDateBaseCriteria(
    int custId,
    string productIds,
    string productDetails,
    DateTime fromDate,
    DateTime toDate,
    bool showProgressiveBonus = false,
    int maxTotalRow = 10000,
    bool isExportExcel = false) : CriteriaBase
{
    public int CustId { get; } = custId;

    public DateTime FromDate { get; } = fromDate;

    public DateTime ToDate { get; } = toDate;

    public string ProductIds { get; } = productIds;

    public string ProductDetails { get; } = productDetails;

    public bool ShowProgressiveBonus { get; } = showProgressiveBonus;

    public int MaxTotalRow { get; set; } = maxTotalRow;

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public bool IsExportExcel { get; } = isExportExcel;

    public override bool IsValid()
        => CustId > 0
        && !string.IsNullOrWhiteSpace(ProductIds)
        && ToDate >= FromDate
        && FromDate > SqlDateTime.MinValue.Value;
}
