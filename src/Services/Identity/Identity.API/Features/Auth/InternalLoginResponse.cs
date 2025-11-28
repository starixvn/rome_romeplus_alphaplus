using AlphaPlus.Identity.API.Features.Auth.SiteDb;

namespace AlphaPlus.Identity.API.Features.Auth;

public class InternalLoginResponse
{
    public int AdminId { get; set; }

    public int CustId { get; set; }

    public string Username { get; set; }

    public int RoleId { get; set; }

    public int On_UserId { get; set; }

    public int Code { get; set; }

    public string Message { get; set; }

    public string Site { get; set; }

    public string Currency { get; set; }

    public int CurrencyId { get; set; }

    public int ServerId { get; set; }

    public string AccUsername { get; set; }

    public string LanguageUsage { get; set; }

    public bool IsProtectedMode { get; set; }

    public IDictionary<string, string> Status { get; set; }

    public bool IsShowConsolidatedReport { get; set; }

    public bool IsShowBetChoiceLimit { get; set; }

    public SiteDBConfig SiteDB { get; set; }

    public bool IsAllowTotalBalanceTransfer { get; set; }

    public DateTime DateLimit { get; set; }

    public bool IsAdminCanUpdateSubAccount { get; set; }

    public bool IsAdminCanViewIPVN { get; set; }

    public bool IsEnabledSecurityCodeInBetLists { get; set; }

    public bool IsEnabledSecurityCodeInStatement { get; set; }

    public bool AutoFollowBet { get; set; }

    public IList<int> OfferedJackpotProducts { get; set; }

    public string LoginIp { get; set; }

    public int[] OfferedProductGroupIds { get; set; }

    public bool IsForcedChangeSecurityCode { get; set; }

    public string SecurityCode { get; set; }
}