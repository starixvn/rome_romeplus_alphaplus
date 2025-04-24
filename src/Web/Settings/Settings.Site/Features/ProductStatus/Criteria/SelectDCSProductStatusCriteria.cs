using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Site.Features.ProductStatus.Criteria
{
    public class SelectDCSProductStatusCriteria(int productId, int custId, byte roleId) : SelectProductStatusCriteria(productId, custId)
    {
        private const short DefaultDcsSettingType = 13;

        public byte RoleId { get; } = roleId;

        public virtual short SettingType { get; } = DefaultDcsSettingType;

        public byte SettingStatus => AppSettings.SettingStatus;

        public override string GetSettingKey() => "Acc_CustSetting_StatusSettingSelect";

        public override bool IsValid()
            => base.IsValid()
            && UserLevel.Member.Value <= RoleId
            && RoleId <= UserLevel.Master.Value;
    }
}