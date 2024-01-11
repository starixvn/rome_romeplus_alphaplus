namespace AlphaPlus.BetsForecast.Infrastructure.Shared.Helper;

public static class LanguageHelper
{
    // zh-Hant = zh-TW
    // zh-Hans = zh-CN
    public static IDictionary<string, string> Languages { get; } = new Dictionary<string, string>
    {
        ["en-US"] = "en",
        ["zh-Hant"] = "ch",
        ["zh-Hans"] = "cs",
        ["th-TH"] = "th",
        ["vi-VN"] = "vi",
        ["zh-TW"] = "ch",
        ["zh-CN"] = "cs"
    };

    public static string GetLangCode(string language)
        => Languages.TryGetValue(language, out var langCode)
        ? langCode
        : "en";
}