using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Transfer.API.Criteria.CashBalanceTransfer;

public class GetCashBalanceTransferCriteria(int custId, bool isDirectMember) : CriteriaBase
{
    public int CustId { get; } = custId;

    public override string GetSettingKey()
        => isDirectMember ? "SiteDB_Age_Transfer_CashBalance_DirectMember_Sel" : "SiteDB_Age_Transfer_CashBalance_Sel";

    public override bool IsValid() => CustId > 0;
}