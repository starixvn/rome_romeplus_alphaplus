namespace AlphaPlus.Tickets.Site.Features.Shared.Models;

public class AppSettings
{
    public bool LocalTest { get; init; }

    public string SiteLabel { get; init; }

    public bool IsSiteLabel777 => SiteLabel.Equals("777");
}