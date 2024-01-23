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
}