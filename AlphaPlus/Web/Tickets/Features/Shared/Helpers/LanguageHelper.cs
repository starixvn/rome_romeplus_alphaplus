namespace AlphaPlus.Tickets.Site.Features.Shared.Helpers;

public static class LanguageHelper
{
    // zh-Hant = zh-TW
    // zh-Hans = zh-CN
    private static readonly string[] Languages = { "en-US", "zh-Hant", "zh-Hans", "th-TH", "vi-VN" };

    public static string DefaultLanguage { get; } = Languages[0];

    public static bool IsValidLanguage(string culture) => Languages.Any(l => l.Equals(culture));
}