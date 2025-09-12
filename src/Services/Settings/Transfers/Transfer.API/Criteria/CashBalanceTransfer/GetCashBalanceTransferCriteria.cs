namespace AlphaPlus.Settings.Transfer.API.Criteria.CashBalanceTransfer;

using Core.Criteria;
using Core.Helpers;

public class GetCashBalanceTransferCriteria(int custId, bool isDirectMember, int siteDbGroupId) : SiteDbBaseCriteria(siteDbGroupId)
{
    public int CustId { get; } = custId;

    public override string GetSettingKey()
        => isDirectMember ? "SiteDB_Age_Transfer_CashBalance_DirectMember_Sel" : SpNameHelpers.SiteDbPrefix("Age_Transfer_CashBalance_Sel", SiteDbGroupId);

    public override bool IsValid() => CustId > 0;
}