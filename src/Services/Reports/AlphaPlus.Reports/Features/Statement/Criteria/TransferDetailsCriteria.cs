using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.Statement;

internal class TransferDetailsCriteria(
    int custId,
    DateTime transferDate,
    string storeMappingSuffix) : CriteriaBase
{
    public int CustId { get; } = custId;

    [SpParam(SqlParamName = "WinLostDate")]
    public DateTime TransferDate { get; } = transferDate;

    [SpParam(Ignored = true)]
    public string StoreMappingSuffix { get; } = storeMappingSuffix;

    public override string GetSettingKey() => $"Statement_Report_TransferDetails{StoreMappingSuffix}";

    public override bool IsValid() => CustId > 0;
}