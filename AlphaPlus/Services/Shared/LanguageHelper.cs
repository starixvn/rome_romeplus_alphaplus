public static class LanguageHelper
{
    // zh-Hant = zh-TW
    private const string ChineseTraditional = "zh-TW";
    private const string NewChineseTraditional = "zh-Hant";
    // zh-Hans = zh-CN
    private const string ChineseSimplified = "zh-CN";
    private const string NewChineseSimplified = "zh-Hans";

    public static string MapChineseLanguageCode(string newLanguageCode)
        => newLanguageCode switch
        {
            NewChineseTraditional => ChineseTraditional,
            NewChineseSimplified => ChineseSimplified,
            _ => newLanguageCode
        };
}