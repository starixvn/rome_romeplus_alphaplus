using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.CasinoDetails;

public class GetCasinoDetailsCriteria(
    long refNo,
    int custId,
    byte transType,
    int productId,
    int pageIndex,
    int pageSize,
    DateTime moveBetsDate,
    DateTime balanceUpDate) : CriteriaBase
{
    public long RefNo { get; } = refNo;

    public int CustId { get; } = custId;

    public byte TransType { get; } = transType;

    public int PageIndex { get; } = pageIndex;

    public int PageSize { get; } = pageSize;

    public int TotalRows { get; set; }

    public DateTime MoveBetsDay { get; } = moveBetsDate;

    public DateTime BalanceUpDay { get; } = balanceUpDate;

    public override string GetSettingKey() => $"Acc_BetList_Detail_{productId}";

    public override bool IsValid() => RefNo > 0 && CustId > 0 && productId > 0;
}
