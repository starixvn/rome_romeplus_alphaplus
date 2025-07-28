namespace AlphaPlus.Auth.Site.Features.Account;

using AlphaPlus.RequestInfo.Models;

public class SignInParam
{
    public int CustId { get; init; }

    public string Password { get; init; }

    public int SubAccId { get; init; }

    public string Nickname { get; init; }

    public bool IsNickname { get; init; }

    public DateTime LoginTime { get; init; }

    public string Language { get; init; }

    public RequestInfo RequestInfo { get; init; }

    public string DarkModeName { get; set; }

    public string AssetAppPath { get; set; }

    public string DCSStaticAppPath { get; set; }
} 