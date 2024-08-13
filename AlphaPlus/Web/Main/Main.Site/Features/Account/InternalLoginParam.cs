namespace AlphaPlus.Main.Site.Features.Account;

public class InternalLoginParam
{
    public int AdminId { get; set; }

    public string Username { get; set; }

    public DateTime LoginTime { get; set; }

    public string Language { get; set; }

    public string LoginIp { get; set; }

    public string CountryCode { get; set; }

    public string LoginDomain { get; set; }

    public string BrowserName { get; set; }

    public string Platform { get; set; }

    public string SessionId { get; set; }
}