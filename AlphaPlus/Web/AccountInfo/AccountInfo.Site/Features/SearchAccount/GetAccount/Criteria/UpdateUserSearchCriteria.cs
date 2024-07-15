using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.Criteria
{
    public class UpdateUserSearchCriteria(int userId, string search, string firstName, string lastName, string nickname) : CriteriaBase
    {
        public int UserId { get; } = userId;

        public string Search { get; } = search;

        public string FirstName { get; } = firstName;

        public string LastName { get; } = lastName;

        [SpParam(SqlParamName = "username2")]
        public string Nickname { get; set; } = nickname;

        public string System { get; } = "ACCOUNTING";

        public override string GetSettingKey() => "Acc_CustomerUpdUserSearch";

        public override bool IsValid() => UserId > 0 && !string.IsNullOrWhiteSpace(Search);
    }
}