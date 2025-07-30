using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class StatementResultQuery : IRequest<ThirdPartyProductViewModel>
{
    public StatementResultQuery() : this(new UserSessionExt())
    {
    }

    protected StatementResultQuery(IUserSession userSession)
    {
        Language = userSession.Language;
        IsDarkMode = !string.IsNullOrWhiteSpace(userSession.DarkModeName);
    }

    public int CustId { get; set; }

    public int ProductId { get; set; }

    public string SiteId { get; set; }

    public int CurrencyId { get; set; }

    public int BetType { get; set; }

    public string EventDate { get; set; }

    public string Language { get; }

    public int SportTypeId { get; set; }

    public string RefNo { get; set; }

    public bool IsDarkMode { get; }
}