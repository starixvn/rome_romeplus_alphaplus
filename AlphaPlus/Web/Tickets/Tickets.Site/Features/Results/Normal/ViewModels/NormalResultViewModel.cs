using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Utils;
using AlphaPlus.Tickets.Site.App_GlobalResources;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Tickets.Site.Features.Results.Normal;

public record NormalResultViewModel(
    DateTime ResultDate,
    int SportId,
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

    public bool HasInformation => !string.IsNullOrWhiteSpace(HtmlResults);
}