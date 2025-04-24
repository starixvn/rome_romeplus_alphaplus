using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Utils;
using AlphaPlus.Shared.AppResources.Betlist.Site;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Tickets.Site.Features.Results.Outright;

public record OutrightResultViewModel(
    DateTime FromDate,
    DateTime ToDate,
    int SportType,
    IEnumerable<SelectListItem> Sports,
    int LeagueId,
    IEnumerable<SelectListItem> Leagues,
    string HtmlResults,
    DateTime MinDate)
{
    public DateTime Today { get; } = SystemTime.Today;

    public DateTime Yesterday { get; } = SystemTime.Yesterday;

    public string MinDateMessage
        => string.Format(Resources.ViewDataFromDate, MinDate.ToShortDateWithSlash());
}
