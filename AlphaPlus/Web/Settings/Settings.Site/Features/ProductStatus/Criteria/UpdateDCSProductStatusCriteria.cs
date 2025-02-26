using AlphaPlus.Core.Configurations;
using Fanex.Data;

namespace AlphaPlus.Settings.Site.Features.ProductStatus.Criteria
{
    public class UpdateDCSProductStatusCriteria : NonQueryProductStatusCriteria
    {
        public UpdateDCSProductStatusCriteria(
            int custId,
            int subAccId,
            int userId,
            string ip,
            string productStatusXml) : base(custId, subAccId, userId, ip)
        {
            ProductStatusXml = productStatusXml;
        }

        [SpParam(SqlParamName = "IsDisabled")]
        public string ProductStatusXml { get; }

        public byte SettingStatus => AppSettings.SettingStatus;

        public override string GetSettingKey() => "Acc_CustSetting_DisableNewProductStatusUpd";

        public override bool IsValid() => base.IsValid() && !string.IsNullOrEmpty(ProductStatusXml);
    }
}