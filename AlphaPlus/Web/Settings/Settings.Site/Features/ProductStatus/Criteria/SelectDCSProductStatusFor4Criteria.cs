using AlphaPlus.Core.Configurations;

namespace AlphaPlus.Settings.Site.Features.ProductStatus.Criteria
{
    public class SelectDCSProductStatusFor4Criteria(int productId, int custId)
        : SelectProductStatusCriteria(productId, custId)
    {
        public byte SettingStatus => AppSettings.SettingStatus;

        public override string GetSettingKey() => "Acc_CustSetting_DisableProductStatusSelect";
    }
}