using AlphaPlus.Core.Models;
using AlphaPlus.Core.Utils;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.Normal;

public class NormalResultQuery : IRequest<NormalResultViewModel>
{
    public NormalResultQuery() : this(new UserSessionExt())
    {
    }

    protected NormalResultQuery(IUserSession userSession)
    {
        Language = userSession.Language;
        SiteName = userSession.Site;
    }

    public DateTime ResultDate { get; set; } = SystemTime.Today;

    public string TransDesc { get; set; }

    public string BetCheck { get; set; }

    public int SportId { get; set; }

    public int LeagueId { get; set; }

    public int BetTypeId { get; set; }

    public string Language { get; }

    public string SiteName { get; }

    public IDictionary<int, bool> SportsStatus
        => new Dictionary<int, bool>
        {
            [SportTypeConstants.NumberGame] = UserPermission.IsEnableProduct(ProductIdConstants.NumberGame),
            [SportTypeConstants.VirtualSports] = UserPermission.HasPermissionOnProduct(ProductIdConstants.VirtualSports)
        };
}
