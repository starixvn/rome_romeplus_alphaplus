namespace AlphaPlus.Identity.API.Features.Auth;

public class IndirectLoginResponse
{
    public int CustId { get; set; }

    public string Username { get; set; }

    public string UplineUsername { get; set; }

    public int UplineCustId { get; set; }

    public int UplineSubAccId { get; set; }

    public int SubAccId { get; set; }

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public string ServerId { get; set; }

    public string Site { get; set; }

    public int CurrencyId { get; set; }

    public string Currency { get; set; }

    public string SubPageAccess { get; set; }

    public bool IsNickname { get; set; }

    public bool NicknameLoginOnly { get; set; }

    public bool IsProtectedMode { get; set; }

    public string LanguageUsage { get; set; }
}