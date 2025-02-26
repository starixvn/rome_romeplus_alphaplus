using AlphaPlus.Core.Models;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class KenoLotteryTableGameResultQuery
{
    public KenoLotteryTableGameResultQuery() : this(new UserSessionExt())
    {
    }

    protected KenoLotteryTableGameResultQuery(IUserSession userSession)
    {
        CustId = userSession.CustId;
        LanguageKey = userSession.LangKey;
        EventDate = DateTime.Today;
        RootUrl = string.Empty;
    }

    public int CustId { get; }

    public string LanguageKey { get; }

    public DateTime EventDate { get; }

    public string RootUrl { get; }
}