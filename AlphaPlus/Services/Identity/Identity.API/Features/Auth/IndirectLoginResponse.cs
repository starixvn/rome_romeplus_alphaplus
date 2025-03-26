using AlphaPlus.Identity.API.Features.Auth.SiteDb;

namespace AlphaPlus.Identity.API.Features.Auth;

public class IndirectLoginResponse
{
    public int CustId { get; set; }

    public string Username { get; set; }

    public string UplineUsername { get; set; }

    public int UplineCustId { get; set; }

    public int UplineSubAccId { get; set; }

    public int SubAccId { get; set; }

    public int RoleId { get; set; }

    public int Code { get; set; }

    public string Message { get; set; }

    public int ServerId { get; set; }

    public string Site { get; set; }

    public int CurrencyId { get; set; }

    public string Currency { get; set; }

    public string SubPageAccess { get; set; }

    public bool IsNickname { get; set; }

    public bool NicknameLoginOnly { get; set; }

    public bool IsProtectedMode { get; set; }

    public string LanguageUsage { get; set; }

    public IDictionary<string, string> Status { get; set; }

    public bool IsShowConsolidatedReport { get; set; }

    public bool IsShowBetChoiceLimit { get; set; }

    public SiteDBConfig SiteDB { get; set; }

    public bool IsAllowTotalBalanceTransfer { get; set; }

    public DateTime DateLimit { get; set; }

    public bool IsEnabledSecurityCodeInBetLists { get; set; }

    public bool IsEnabledSecurityCodeInStatement { get; set; }

    public bool AutoFollowBet { get; set; }

    public IList<int> OfferedJackpotProducts { get; set; }

    public bool IsFirstLogin { get; set; }

    public bool IsForcedChangeSecurityCode { get; set; }

    public string SecurityCode { get; set; }

    public DateTime PwdExpiry { get; set; }

    public string Nickname { get; set; }

    public string LoginIp { get; set; }

    public int[] OfferedProductGroupIds { get; set; }
}