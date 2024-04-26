namespace AlphaPlus.Settings.MemberInfo.API.Criteria.IndirectLoginAccount
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class NewAccountCriteria : NonQueryCommand
    {
        public NewAccountCriteria(
            string username,
            string password,
            int actorId,
            int actorSubAccId,
            byte actorLevelId,
            string language,
            string loginIP,
            string loginDomain,
            string browserName,
            string platform,
            string? countryCode)
        {
            Username = username;
            UserPwd = password;
            ActorId = actorId;
            ActorSubAccId = actorSubAccId;
            ActorLevelId = actorLevelId;
            IP = loginIP;
            Language = language;
            LoginDomain = loginDomain;
            BrowserName = browserName;
            Platform = platform;
            CountryCode = countryCode;
        }

        public string Username { get; }

        public string UserPwd { get; }

        [SpParam(SqlParamName = "custid")]
        public int ActorId { get; }

        [SpParam(SqlParamName = "subaccid")]
        public int ActorSubAccId { get; }

        [SpParam(SqlParamName = "roleid")]
        public byte ActorLevelId { get; }

        public string IP { get; }

        public string Language { get; }

        public string LoginDomain { get; }

        public string BrowserName { get; }

        public string Platform { get; }

        public string? CountryCode { get; }

        public int ErrCode { get; set; }

        public string? ErrMsg { get; set; }

        public override string GetSettingKey()
            => "Age_CustomerIndirectLoginAccountIns";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(Username)
            && !string.IsNullOrWhiteSpace(UserPwd)
            && ActorId > 0
            && ActorSubAccId >= 0;
    }
}
