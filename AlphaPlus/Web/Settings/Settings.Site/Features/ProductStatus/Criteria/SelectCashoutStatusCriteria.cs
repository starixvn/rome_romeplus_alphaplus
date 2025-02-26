using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Site.Features.ProductStatus.Criteria
{
    public class SelectCashOutStatusCriteria : SelectProductStatusCriteria
    {
        private const int CashOutSettingType = 4;

        public SelectCashOutStatusCriteria(int productId, int custId, byte roleId)
            : base(productId, custId)
        {
            RoleId = roleId;
        }

        public byte RoleId { get; }

        public int SettingType { get; } = CashOutSettingType;

        public byte SettingStatus => AppSettings.SettingStatus;

        public override string GetSettingKey() => "Acc_CustSetting_StatusSettingSelect";

        public override bool IsValid()
            => base.IsValid()
            && UserLevel.Member.Value <= RoleId
            && RoleId <= UserLevel.Master.Value;
    }
}