using Fanex.Data.Repository;

namespace AlphaPlus.Identity.API.Features.Auth
{
    public class GetProductSettingCriteria(int custId, int roleId, byte settingStatus) : CriteriaBase
    {
        public int CustId { get; } = custId;

        public int RoleId { get; } = roleId;

        public byte SettingStatus { get; } = settingStatus;

        public override string GetSettingKey() => "Acc_CustSetting_StatusSettingSelect";

        public override bool IsValid() => CustId > 0;
    }
}