using AlphaPlus.Core.Configurations;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Site.Features.ProductStatus.Criteria;

public class GetProductSettingCriteria(int custId, int roleId) : CriteriaBase
{
    public int CustId => custId;

    public int RoleId => roleId;

    public static byte SettingStatus => AppSettings.SettingStatus;

    public override string GetSettingKey() => "Acc_CustSetting_StatusSettingSelect";

    public override bool IsValid() => CustId > 0 && RoleId > 1 && RoleId < 5;
}