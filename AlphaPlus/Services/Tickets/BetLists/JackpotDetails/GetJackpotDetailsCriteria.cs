using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.JackpotDetails;

public class GetJackpotDetailsCriteria(
    long transId,
    int custId,
    int betType,
    DateTime moveBetsDate,
    DateTime balanceUpDate) : CriteriaBase
{
    public long TransId { get; } = transId;

    public int CustId { get; } = custId;

    public int BetType { get; } = betType;

    public DateTime MoveBetsDay { get; } = moveBetsDate;

    public DateTime BalanceUpDay { get; } = balanceUpDate;

    public override string GetSettingKey() => "Acc_BetList_Jackpot_Detail";

    public override bool IsValid() => TransId > 0 && CustId > 0 && BetType > 0;
}