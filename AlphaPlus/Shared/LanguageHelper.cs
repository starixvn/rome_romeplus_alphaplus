public static class LanguageHelper
{
    // zh-Hant = zh-TW
    private const string ChineseTraditional = "zh-TW";
    private const string NewChineseTraditional = "zh-Hant";
    // zh-Hans = zh-CN
    private const string ChineseSimplified = "zh-CN";
    private const string NewChineseSimplified = "zh-Hans";
    private static readonly string[] Languages = ["en-US", "zh-Hant", "zh-Hans", "th-TH", "vi-VN"];

    public static string DefaultLanguage { get; } = Languages[0];

    public static string MapChineseLanguageCode(string newLanguageCode)
        => newLanguageCode switch
        {
            NewChineseTraditional => ChineseTraditional,
            NewChineseSimplified => ChineseSimplified,
            _ => newLanguageCode
        };

    public static bool IsValidLanguage(string culture) => Languages.Any(l => l.Equals(culture));
}