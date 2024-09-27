namespace AlphaPlus.Identity.API.Features.Auth
{
    public class AuthenticateResponse
    {
        public int Code { get; init; }

        public string Message { get; init; }

        public int CustId { get; init; }

        public string UserName { get; init; }

        public int SubAccId { get; init; }

        public string SubUsername { get; init; }

        public string SubPageAccess { get; init; }

        public int RoleId { get; init; }

        public string Site { get; init; }

        public string Currency { get; init; }

        public int CurrencyId { get; init; }

        public int ServerId { get; init; }

        public int AdminId { get; init; } = 0;

        public string AccUserName { get; init; } = string.Empty;

        public bool IsFirstLogin { get; init; }

        public bool IsForcedChangeSecurityCode { get; init; }

        public string SecurityCode { get; init; }

        public DateTime PwdExpiry { get; init; }

        public string Nickname { get; init; }

        public bool IsNickName { get; init; }

        public IDictionary<string, string> Status { get; set; }

        public bool IsShowConsolidatedReport { get; set; }

        public bool IsShowBetChoiceLimit { get; set; }

        public SiteDB SiteDB { get; set; }

        public bool IsAllowTotalBalanceTransfer { get; set; }

        public DateTime DateLimit { get; set; }

        public bool IsAdminCanUpdateSubAccount { get; set; }

        public bool IsEnabledSecurityCodeInBetLists { get; set; }

        public bool IsEnabledSecurityCodeInStatement { get; set; }

        public bool AutoFollowBet { get; set; }

        public IList<int> OfferedJackpotProducts { get; set; }

        public string LanguageUsage { get; set; }

        public string LoginIp { get; set; }

        public int[] OfferedProductGroupIds { get; set; }
    }
}