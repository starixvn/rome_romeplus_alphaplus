using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlphaPlus.Settings.Site.Features.ProductStatus.Criteria
{
    public class UpdateProductStatusForSettingType4Criteria : UpdateDCSProductStatusCriteria
    {
        public UpdateProductStatusForSettingType4Criteria(
            int custId,
            int subAccId,
            int userId,
            string ip,
            string productStatusXml) : base(custId, subAccId, userId, ip, productStatusXml)
        {
        }

        public override string GetSettingKey() => "Acc_CustSetting_DisableProductStatusUpd";
    }
}