namespace AlphaPlus.Tickets.Site.Features.Shared.Models;

public class AppSettings
{
    public bool LocalTest { get; init; }

    public string SiteLabel { get; init; }

    public bool IsSiteLabel777 => SiteLabel.Equals("777");

    public bool EnableBetsListMonitoring { get; init; }

    public int[] UsersAlwaysAllowAccessBetsListMonitoring { get; init; }

    public int RunningMatchCacheTime { get; init; }

    public int[] MonitoringBetsBetTypes { get; init; }
}