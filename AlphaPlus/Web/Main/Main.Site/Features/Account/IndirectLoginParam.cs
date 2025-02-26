namespace AlphaPlus.Main.Site.Features.Account;

public class IndirectLoginParam
{
    public int CustId { get; set; }

    public string UplineUsername { get; set; }

    public int UplineCustId { get; set; }

    public int UplineSubAccId { get; set; }

    public int SubAccId { get; set; }

    public string Username { get; set; }

    public DateTime LoginTime { get; set; }

    public string Language { get; set; }

    public RequestInfo.Models.RequestInfo RequestInfo { get; set; }

    public string DarkModeName { get; set; }

    public string AssetAppPath { get; set; }
}