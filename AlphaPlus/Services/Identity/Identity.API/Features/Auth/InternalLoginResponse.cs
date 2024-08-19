namespace AlphaPlus.Identity.API.Features.Auth;

public class InternalLoginResponse
{
    public int AdminId { get; set; }

    public int CustId { get; set; }

    public string Username { get; set; }

    public byte RoleId { get; set; }

    public int On_UserId { get; set; }

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public string Site { get; set; }

    public string Currency { get; set; }

    public int CurrencyId { get; set; }

    public int ServerId { get; set; }

    public string AccUsername { get; set; }

    public string LanguageUsage { get; set; }

    public bool IsProtectedMode { get; set; }
}