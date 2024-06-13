namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.LastBets;

public class FilterConfigurationQuery
{
    public string FilterName { get; set; }

    public int LimitRowCount { get; set; } = 100;

    public bool AutoStart { get; set; } = true;

    public bool Welcome { get; set; }
}