namespace AlphaPlus.Settings.Transfer.API.Criteria.TransferAllAndClearCredit;

using Core.Criteria;
using Core.Helpers;

public class GetTotalAmtCriteria(int downlineId, byte downlineRoleId) : SiteDbBaseCriteria
{
    public int CustId { get; set; } = downlineId;

    public byte RoleId { get; set; } = downlineRoleId;

    public override string GetSettingKey()
        => SpNameHelpers.SiteDbPrefix("Age_Customer_TotalAmount_Sel", SiteDbGroupId);

    public override bool IsValid()
        => CustId > 0 && RoleId > 0;
}