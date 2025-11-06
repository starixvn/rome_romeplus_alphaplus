namespace AlphaPlus.Settings.Transfer.API.Criteria.TransferAllAndClearCredit;

using Core.Criteria;
using Core.Helpers;

public class GetNetPositionCriteria(int custId, byte roleId, int siteDbGroupId) : SiteDbBaseCommand(siteDbGroupId)
{
    public int CustId { get; set; } = custId;

    public byte RoleId { get; set; } = roleId;

    public override string GetSettingKey()
        => SpNameHelpers.SiteDbPrefix("Age_GetCustomerNetPosition", SiteDbGroupId);

    public override bool IsValid()
        => RoleId > 0 && CustId > 0;
}