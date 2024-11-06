using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Criteria.IndirectLoginAccount
{
    public class UpdatePasswordCriteria : NonQueryCommand
    {
        public UpdatePasswordCriteria(
            int indirectCustId,
            int indirectSubAccId,
            string indirectUsername,
            byte indirectLevelId,
            string password,
            int actorId,
            int actorSubAccId,
            string actorUsername,
            string language,
            string loginIP,
            string loginDomain,
            string browserName,
            string platform,
            string countryCode)
        {
            IndirectCustId = indirectCustId;
            IndirectSubAccId = indirectSubAccId;
            IndirectUsername = indirectUsername;
            IndirectLevelId = indirectLevelId;
            UserPwd = password;
            ActorId = actorId;
            ActorSubAccId = actorSubAccId;
            ActorUsername = actorUsername;
            Language = language;
            IP = loginIP;
            LoginDomain = loginDomain;
            BrowserName = browserName;
            Platform = platform;
            CountryCode = countryCode;
        }

        [SpParam(SqlParamName = "custidindirect")]
        public int IndirectCustId { get; }

        [SpParam(SqlParamName = "subaccidindirect")]
        public int IndirectSubAccId { get; }

        [SpParam(SqlParamName = "downlineusername")]
        public string IndirectUsername { get; }

        [SpParam(SqlParamName = "roleidindirect")]
        public byte IndirectLevelId { get; }

        public string UserPwd { get; }

        [SpParam(SqlParamName = "custid")]
        public int ActorId { get; }

        [SpParam(SqlParamName = "subaccid")]
        public int ActorSubAccId { get; }

        [SpParam(SqlParamName = "uplineusername")]
        public string ActorUsername { get; }

        public string IP { get; }

        public string Language { get; }

        public string LoginDomain { get; }

        public string BrowserName { get; }

        public string Platform { get; }

        public string CountryCode { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
            => "Age_CustomerIndirectLoginAccountPwdCheck";

        public override bool IsValid()
            => IndirectCustId > 0
            && IndirectSubAccId >= 0
            && !string.IsNullOrWhiteSpace(IndirectUsername)
            && IndirectLevelId > 0;
    }
}
