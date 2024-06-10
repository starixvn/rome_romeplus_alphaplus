using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.CashOutHistory;

public class GetCashOutHistoryCriteria(long transId, long custId, DateTime winlossDate, DateTime balanceUpDay) : CriteriaBase
{
    public long TransId { get; } = transId;

    public long CustId { get; } = custId;

    public DateTime WinLostDate { get; } = winlossDate;

    public DateTime BalanceUpDay { get; } = balanceUpDay;

    public override string GetSettingKey() => "Acc_BetList_CashOut_History";

    public override bool IsValid() => TransId > 0 && CustId > 0;
}