using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Identity.API.Features.Auth
{
    public class LoginClaimDownlineCommand : INonQueryCommand
    {
        public int CustId { get; init; }

        [SpParam(SqlParamName = "userpwd")]
        public string Password { get; init; }

        public string UserName { get; set; } = string.Empty;

        public string Nickname { get; set; }

        public bool IsNickname { get; init; }

        public int SubAccId { get; init; }

        public string SubUsername { get; set; }

        public DateTime LoginTime { get; init; }

        public string Language { get; init; }

        public string LoginIp { get; init; }

        public string CountryCode { get; init; }

        public string LoginDomain { get; init; }

        public string BrowserName { get; init; }

        public string Platform { get; init; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public int RoleId { get; set; } = 0;

        public int CurrencyId { get; set; }

        public string Currency { get; set; }

        public string SubPageAccess { get; set; }

        public string Site { get; set; }

        public int ServerId { get; set; }

        public bool IsFirstLogin { get; set; }

        public DateTime PwdExpiry { get; set; } = DateTime.Now;

        public string SecurityCode { get; set; }

        public bool IsForcedChangeSecurityCode { get; set; }

        public bool IsProtectedMode { get; set; }

        public string LanguageUsage { get; set; }

        public string GetSettingKey() => "Age_LoginClaimDownline";

        public bool IsValid() => CustId > 0;
    }
}