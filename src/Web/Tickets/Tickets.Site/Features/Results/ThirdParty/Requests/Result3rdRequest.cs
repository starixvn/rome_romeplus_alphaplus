using AlphaPlus.Core.Models;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class Result3rdRequest
{
    public Result3rdRequest() : this(new UserSessionExt())
    {
    }

    protected Result3rdRequest(IUserSession userSession)
    {
        Language = userSession.Language;
        IsDarkMode = !string.IsNullOrWhiteSpace(userSession.DarkModeName);
    }

    public int CustId { get; set; }

    public int ProductId { get; set; }

    public string SiteId { get; set; }

    public string TransId3rd { get; set; }

    public string GameId3rd { get; set; }

    public int CurrencyId { get; set; }

    public int BetType { get; set; }

    public string EndTime { get; set; }

    public string Language { get; }

    public bool IsDarkMode { get; }
}