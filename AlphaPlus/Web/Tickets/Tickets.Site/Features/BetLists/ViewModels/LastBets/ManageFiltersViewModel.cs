using AlphaPlus.BetLists.LastBets;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.LastBets;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.LastBets;

public class ManageFiltersViewModel(IEnumerable<FilterConfiguration> filterConfiguration, ManageFiltersQuery message)
{
    public IEnumerable<FilterConfiguration> FilterConfiguration { get; } = filterConfiguration;

    public string FilterName { get; } = message.FilterName;

    public int LimitRowCount { get; } = message.LimitRowCount;

    public bool AutoStart { get; } = message.AutoStart;
}
