namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.Balance.Criteria;

using Core.Criteria;
using Core.Helpers;

public class SelectMemberBalanceCriteria(int custId, int siteDbGroupId) : SiteDbBaseCommand(siteDbGroupId)
{
    public int CustId { get; } = custId;

    public decimal Balance { get; set; }

    public decimal NetPosition { get; set; }

    public decimal PendingTransfer3rd { get; set; }

    public override string GetSettingKey() => SpNameHelpers.SiteDbPrefix("Acc_BalanceMemberSel", SiteDbGroupId);

    public override bool IsValid() => CustId > 0;
}