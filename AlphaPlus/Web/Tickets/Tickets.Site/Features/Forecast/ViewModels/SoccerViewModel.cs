using AlphaPlus.Tickets.Site.App_GlobalResources;
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