using Fanex.Data.Repository;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.LoginInfo
{
    public class SelectLastTxnCriteria(string username, int uplineId, bool isInternal) : CriteriaBase
    {
        public string Username { get; } = username;

        public int UplineId { get; } = uplineId;

        public bool IsInternalAgent { get; } = isInternal;

        public override string GetSettingKey()
            => "Age_CustomerSelMaxTransDate";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(Username) && UplineId > 0;
    }
}