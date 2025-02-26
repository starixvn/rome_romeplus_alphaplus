using AlphaPlus.Core.Models;
using AlphaPlus.Core.Utils;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.Outright;

public class OutrightResultQuery : IRequest<OutrightResultViewModel>
{
    public OutrightResultQuery() : this(new UserSessionExt())
    {
    }

    protected OutrightResultQuery(IUserSession userSession)
    {
        Language = userSession.Language;
        SiteName = userSession.Site;
    }

    public DateTime FromDate { get; set; } = SystemTime.Today;

    public DateTime ToDate { get; set; } = SystemTime.Today;

    public int SportType { get; set; }

    public int LeagueId { get; set; }

    public string Language { get; }

    public string SiteName { get; }
}