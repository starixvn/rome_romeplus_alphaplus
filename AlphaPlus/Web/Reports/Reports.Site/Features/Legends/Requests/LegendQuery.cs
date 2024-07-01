using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.Core.Utils;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.Legends;

public class LegendQuery : IRequest<LegendViewModel>
{
    public LegendQuery() : this(new UserSessionExt())
    {
    }

    protected LegendQuery(IUserSession viewer)
    {
        ViewerLevel = Enumeration.FromValue<UserLevel>(viewer.RoleId);
        Site = viewer.Site;
        CurrencyId = viewer.CurrencyId;
        Language = viewer.Language;
        MinDate = IsHistoricReport
            ? SystemTime.Today.AddYears(-3).FirstDayOfYear()
            : SystemTime.Today.AddMonths(-3).FirstDayOfMonth();
    }

    public UserLevel ViewerLevel { get; }

    public string Site { get; }

    public int CurrencyId { get; }

    public string Language { get; }

    public DateTime MinDate { get; set; }

    public bool IsHistoricReport { get; set; }
}
