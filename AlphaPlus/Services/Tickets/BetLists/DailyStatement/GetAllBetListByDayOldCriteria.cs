using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.DailyStatement;

public class GetAllBetListByDayOldCriteria(int custId, DateTime winlossDate, int transType) : CriteriaBase
{
    public int CustId { get; } = custId;

    public DateTime FromDate { get; } = winlossDate;

    public int TransType { get; } = transType;

    public override string GetSettingKey() => "Age_BetList_SelByDayAll_Mem_WH";

    public override bool IsValid() => CustId > 0;
}