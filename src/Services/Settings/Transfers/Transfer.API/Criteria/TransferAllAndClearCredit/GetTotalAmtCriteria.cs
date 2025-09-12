namespace AlphaPlus.Settings.Transfer.API.Criteria.TransferAllAndClearCredit;

using Core.Criteria;

public class GetTotalAmtCriteria(int downlineId, byte downlineRoleId, int siteDbGroupId) : SiteDbBaseCriteria(siteDbGroupId)
{
    public int CustId { get; set; } = downlineId;

    public byte RoleId { get; set; } = downlineRoleId;

    public override string GetSettingKey()
        => "SiteDB_Age_Customer_TotalAmount_Sel";

    public override bool IsValid()
        => CustId > 0 && RoleId > 0;
}