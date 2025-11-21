namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.GivenCredit.Update;

using AlphaPlus.Core.Criteria;
using Core.Helpers;

public class GetTotalAmtCommand(int downlineId) : SiteDbBaseCommand
{
    public int CustId { get; } = downlineId;

    public decimal TotalAmt { get; set; }

    public override string GetSettingKey()
        => SpNameHelpers.SiteDbPrefix("Acc_CustomerTotalAmountSel", SiteDbGroupId);

    public override bool IsValid() => CustId > 0;
}