using AlphaPlus.Core.Models;

namespace AlphaPlus.Auth.Site.Features.Account
{
    public class SignInResult : Result
    {
        public int CustId { get; init; }

        public string UserName { get; init; }

        public int SubAccId { get; init; }

        public string SubUserName { get; init; }

        public string SubPageAccess { get; init; }

        public byte RoleId { get; init; }

        public string Site { get; init; }

        public string Currency { get; init; }

        public int CurrencyId { get; init; }

        public int ServerId { get; init; }

        public int AdminId { get; init; } = 0;

        public string AdminName { get; init; } = string.Empty;

        public bool IsLogged { get; init; }

        public bool IsPasswordExpired { get; init; }

        public bool IsForcedSecurityCode { get; init; }

        public string SecurityCode { get; init; }

        public string NickName { get; init; }

        public bool IsNickname { get; init; }

        public string UserLoginIp { get; init; }

        public string LangKey { get; init; }

        public int SRecommendId { get; init; }

        public int MRecommendId { get; init; }

        public bool IsProtectedMode { get; init; }

        public bool IsOfferProtectedMode { get; init; }

        public SiteDB SiteDB { get; init; }

        public bool IsAdminCanUpdateSubAccount { get; init; }

        public DateTime DateLimit { get; init; }

        public bool IsEnabledSecurityCodeInBetLists { get; init; }

        public bool IsEnabledSecurityCodeInStatement { get; init; }

        public bool AutoFollowBetIsDummyAgent { get; init; }

        public IList<int> OfferedJackpotProducts { get; init; }

        public string UplineUsername { get; set; }

        public int UplineCustId { get; set; }

        public int UplineSubAccId { get; set; }

        public string DarkModeName { get; set; }

        public string AssetAppPath { get; set; }
    }
} 