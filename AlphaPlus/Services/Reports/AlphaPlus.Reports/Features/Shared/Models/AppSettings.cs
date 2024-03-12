namespace AlphaPlus.Reports.Features.Shared.Models;

public class AppSettings
{
    public bool LocalTest { get; init; }

    public byte SettingStatus { get; init; }

    public string[] AllowedSitesToViewBonusDetail { get; init; }
}
