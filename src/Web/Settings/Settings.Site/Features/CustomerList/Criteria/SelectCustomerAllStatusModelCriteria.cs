using AlphaPlus.Settings.MemberInfo.Domain.Configurations;
using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Site.Features.CustomerList.Criteria
{
    public class SelectCustomerAllStatusModelCriteria : CriteriaBase
    {
        public SelectCustomerAllStatusModelCriteria(int searchAccountId)
        {
            SearchAccountId = searchAccountId;
        }

        [SpParam(SqlParamName = "CustId")]
        public int SearchAccountId { get; }

        public byte SettingStatus => AppSettings.SettingStatus;

        public override string GetSettingKey() => "Age_CustomerSelAllStatus";

        public override bool IsValid() => SearchAccountId > 0;
    }
}