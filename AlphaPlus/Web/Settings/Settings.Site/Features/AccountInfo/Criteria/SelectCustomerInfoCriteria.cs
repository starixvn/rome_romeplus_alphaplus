using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Site.Features.AccountInfo.Criteria
{
    public class SelectCustomerInfoCriteria : CriteriaBase
    {
        public SelectCustomerInfoCriteria(string username, int uplineId)
        {
            Username = username;
            UplineId = uplineId;
        }

        public string Username { get; }

        public int UplineId { get; }

        public override string GetSettingKey() => "Age_CustomerSelAllInfo";

        public override bool IsValid() => !string.IsNullOrEmpty(Username) && UplineId > 0;
    }
}