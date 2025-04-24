using Fanex.Data.Repository;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.Criteria
{
    public class SelectCustomerInfoCriteria(string username, int uplineId) : CriteriaBase
    {
        public string Username { get; } = username;

        public int UplineId { get; } = uplineId;

        public override string GetSettingKey() => "Age_CustomerSelAllInfo";

        public override bool IsValid() => !string.IsNullOrEmpty(Username) && UplineId > 0;
    }
}