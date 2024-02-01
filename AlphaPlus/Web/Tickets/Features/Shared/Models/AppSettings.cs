namespace AlphaPlus.Tickets.Site.Features.Shared.Models;

public sealed class AppSettings
{
    public bool LocalTest { get; init; }

    public string AssetAppPath { get; init; }

    public string JsVersion { get; init; }
}