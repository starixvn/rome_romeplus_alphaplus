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
}