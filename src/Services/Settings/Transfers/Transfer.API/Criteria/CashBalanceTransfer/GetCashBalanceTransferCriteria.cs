namespace AlphaPlus.Settings.Transfer.API.Criteria.CashBalanceTransfer;

using Core.Criteria;
using Core.Helpers;

public class GetCashBalanceTransferCriteria(int custId, bool isDirectMember) : SiteDbBaseCriteria
{
    public int CustId { get; } = custId;

    public override string GetSettingKey()
        => isDirectMember ? SpNameHelpers.SiteDbPrefix("Age_Transfer_CashBalance_DirectMember_Sel", SiteDbGroupId) :
            SpNameHelpers.SiteDbPrefix("Age_Transfer_CashBalance_Sel", SiteDbGroupId);

    public override bool IsValid() => CustId > 0;
}