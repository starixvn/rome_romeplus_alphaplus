public static class LanguageHelper
{
    private static readonly string[] Languages = ["en-US", "zh-TW", "zh-CN", "ja-JP", "th-TH", "ko-KR", "vi-VN"];

    public static string DefaultLanguage { get; } = Languages[0];

    public static bool IsValidLanguage(string culture) => Languages.Any(l => l.Equals(culture));
}