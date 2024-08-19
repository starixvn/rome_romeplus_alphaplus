namespace AlphaPlus.Main.Site.Features.Account;

public class InternalLoginParam
{
    public int AdminId { get; set; }

    public string Username { get; set; }

    public DateTime LoginTime { get; set; }

    public string Language { get; set; }

    public RequestInfo.Models.RequestInfo RequestInfo { get; set; }
}