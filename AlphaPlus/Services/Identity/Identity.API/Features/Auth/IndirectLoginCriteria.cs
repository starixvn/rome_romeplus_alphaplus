using Fanex.Data.Repository;

namespace AlphaPlus.Identity.API.Features.Auth;

public class IndirectLoginCriteria : NonQueryCommand
{
    public int CustId { get; set; }

    public string UplineUsername { get; set; }

    public int UplineCustId { get; set; }

    public int UplineSubAccId { get; set; }

    public int SubAccId { get; set; }

    public string Username { get; set; }

    public DateTime LoginTime { get; set; }

    public string Language { get; set; }

    public string LoginIp { get; set; }

    public string CountryCode { get; set; }

    public string LoginDomain { get; set; }

    public string BrowserName { get; set; }

    public string Platform { get; set; }

    public string SessionId { get; set; }

    public byte RoleId { get; set; }

    public int On_UserId { get; set; }

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public override string GetSettingKey() => "Age_IndirectLoginClaimDownline";

    public override bool IsValid() => CustId > 0 || SubAccId > 0 || !string.IsNullOrWhiteSpace(Username);
}