using AlphaPlus.Core.Models;

namespace Alpha.Main.Models;

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

    public int OnlineUserId { get; init; }

    public CustomerStatus Status { get; init; }

    public bool IsShowConsolidatedReport { get; init; }

    public bool IsShowBetChoiceLimit { get; init; }

    public SiteDB SiteDB { get; init; }

    public bool IsAllowTotalBalanceTransfer { get; init; }
}

public class CustomerStatus
{
    public bool DoubleCommLosing { get; init; }

    public bool DisableAutoMPT { get; init; }

    public int SRecommend { get; init; }

    public int MRecommend { get; init; }

    public int ARecommend { get; init; }

    public bool IsAdminCanViewVNIP { get; init; }

    public bool IsMyanmarOdds { get; init; }
}

public class SiteDB
{
    public int SiteDBGroupId { get; init; }

    public string LinkedServerName { get; init; }

    public string ClientAccessName { get; init; }

    public string AGIP { get; init; }
}