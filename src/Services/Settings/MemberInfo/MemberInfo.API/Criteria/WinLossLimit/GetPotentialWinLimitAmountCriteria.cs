namespace AlphaPlus.Settings.MemberInfo.API.Criteria.WinLossLimit;

using AlphaPlus.Core.Criteria;
using Core.Helpers;

public class GetPotentialWinLimitAmountCriteria(string custIds) : SiteDbBaseCriteria()
{
    public string CustIds { get; set; } = custIds;

    public override string GetSettingKey()
        => SpNameHelpers.SiteDbPrefix("Age_CustomerSelPotentialWinlimitAmount", SiteDbGroupId);

    public override bool IsValid()
        => true;
}