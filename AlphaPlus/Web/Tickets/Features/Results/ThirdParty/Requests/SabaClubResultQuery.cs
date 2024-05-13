using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class SabaClubResultQuery : IRequest<ThirdPartyProductViewModel>
{
    public SabaClubResultQuery() : this(new UserSessionExt())
    {
    }

    protected SabaClubResultQuery(IUserSession userSession)
        => Language = userSession.Language;

    public string CustId { get; set; }

    public string GameId { get; set; }

    public string GameRoundId { get; set; }

    public int SportId { get; set; }

    public string SiteId { get; set; }

    public string Language { get; }
}