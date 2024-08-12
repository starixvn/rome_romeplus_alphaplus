namespace AlphaPlus.Tickets.Site.Features.Shared.Models;

public class AppSettings
{
    public bool LocalTest { get; init; }

    public string SiteLabel { get; init; }

    public bool IsSiteLabel777 => SiteLabel.Equals("777");

    public bool EnableBetsListMonitoring { get; init; }

    public HashSet<int> UsersAlwaysAllowAccessBetsListMonitoring { get; init; }

    public int RunningMatchCacheTime { get; init; }

    public int[] MonitoringBetsBetTypes { get; init; }

    public byte SettingStatus { get; init; }

    public HashSet<int> ProductOfferProductDetail { get; init; }

    public int BetListMaxTotalRowView { get; init; }

    public int BetListMaxTotalRowExportExcel { get; init; }

    public string AllbetResultImagePath { get; init; }
}
