using AlphaPlus.Core.Models;

namespace AlphaPlus.Main.Site.Features.Account
{
    public class SignInResult : Result
    {
        public int CustId { get; init; }

        public string UserName { get; init; }

        public int SubAccId { get; init; }

        public string SubUsername { get; init; }

        public string SubPageAccess { get; init; }

        public byte RoleId { get; init; }

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

        public IDictionary<string, string> Status { get; init; }

        public bool IsShowConsolidatedReport { get; init; }

        public bool IsShowBetChoiceLimit { get; init; }

        public SiteDB SiteDB { get; init; }

        public bool IsAllowTotalBalanceTransfer { get; init; }

        public DateTime DateLimit { get; init; }

        public bool IsAdminCanUpdateSubAccount { get; init; }

        public bool IsEnabledSecurityCodeInBetLists { get; init; }

        public bool IsEnabledSecurityCodeInStatement { get; init; }

        public bool AutoFollowBet { get; init; }

        public IList<int> OfferedJackpotProducts { get; init; }

        public string UplineUsername { get; set; }

        public int UplineCustId { get; set; }

        public int UplineSubAccId { get; set; }

        public string DarkModeName { get; set; }

        public string AssetAppPath { get; set; }

        public string LanguageUsage { get; set; }

        public string LoginIp { get; set; }

        public int[] OfferedProductGroupIds { get; set; }

        public string DCSStaticAppPath { get; set; }
    }
}