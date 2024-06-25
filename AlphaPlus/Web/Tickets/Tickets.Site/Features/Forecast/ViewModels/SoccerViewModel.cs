using AlphaPlus.Shared.AppResources.Betlist.Site;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Tickets.Site.Features.Forecast.ViewModels;

public class SoccerViewModel(bool isLive, string language)
{
    public IEnumerable<SelectListItem> LiveStatus
        => [
            new SelectListItem(Resources.Live, "true", isLive),
            new SelectListItem(Resources.NonLive, "false", !isLive)
        ];

    public string Language { get; } = language;
}