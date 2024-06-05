using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.Parlay;

public class GetComboParlayDetailCriteria(
    long refNo,
    int custId,
    DateTime balanceUpDate,
    DateTime moveBetDate) : CriteriaBase
{
    public long RefNo { get; } = refNo;

    public int CustId { get; } = custId;

    public DateTime BalanceUpDay { get; } = balanceUpDate;

    public DateTime MoveBetDay { get; } = moveBetDate;

    public decimal? Outstanding { get; set; }

    public bool IsParlayBoost { get; set; }

    public override string GetSettingKey() => "Acc_BetlistCombinationParlay_Details";

    public override bool IsValid() => RefNo > 0;
}