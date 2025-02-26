using AlphaPlus.Core.Configurations;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Site.Features.ProductStatus.Criteria
{
    public class SelectCustomerInformationCriteria(int custId) : CriteriaBase
    {
        public int CustId { get; } = custId;

        public byte SettingStatus => AppSettings.SettingStatus;

        public override string GetSettingKey() => "Age_CustomerSelUserInfo";

        public override bool IsValid() => CustId > 0;
    }
}