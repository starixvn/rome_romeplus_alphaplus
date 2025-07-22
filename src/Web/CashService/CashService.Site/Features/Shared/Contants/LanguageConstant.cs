namespace AlphaPlus.CashService.Site.Features.Shared.Contants
{
    public static class LanguageConstant
    {
        public const string English = "en";
        public const string ChineseTraditional = "zh-tW";
        public const string ChineseSimplified = "zh-ch";
        public const string Japanese = "jp";
        public const string Thai = "th";
        public const string Korean = "ko";
        public const string Vietnamese = "vi";

        public const string Default = English;

        public static readonly Dictionary<string, string> CodeNames = new Dictionary<string, string>
        {
            [English] = "English",
            [ChineseTraditional] = "繁體中文",
            [ChineseSimplified] = "简体中文",
            [Japanese] = "日本語",
            [Thai] = "ภาษาไทย",
            [Korean] = "한국어",
            [Vietnamese] = "Tiếng Việt"
        };
        
        public static readonly Dictionary<string, string> CodeKeysWithoutCN3 = CodeNames
            .Where(langauge => langauge.Key != Vietnamese)
            .ToDictionary(language => language.Key, language => language.Value);
    }
}
