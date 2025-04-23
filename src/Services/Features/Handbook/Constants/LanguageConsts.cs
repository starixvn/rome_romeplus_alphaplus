namespace AlphaPlus.Features.Handbook.API.Constants
{
    public static class LanguageConsts
    {
        public const string EnglishKey = "e";
        public const string VietnameseKey = "vi";
        public const string Default = EnglishKey;

        public static Dictionary<string, string> LanguageNames = new Dictionary<string, string>
        {
            ["e"] = "English",
            ["ct"] = "繁體中文",
            ["cs"] = "简体中文",
            ["j"] = "Japanese",
            ["th"] = "ภาษาไทย",
            ["ko"] = "Korean",
            ["vi"] = "Tiếng Việt"
        };

        public static Dictionary<string, string> Codes = new Dictionary<string, string>
        {
            ["e"] = "en-US",
            ["ct"] = "zh-TW",
            ["cs"] = "zh-CN",
            ["j"] = "ja-JP",
            ["th"] = "th-TH",
            ["ko"] = "ko-KR",
            ["vi"] = "vi-VN"
        };

        public static Dictionary<string, string> Locales = new Dictionary<string, string>
        {
            ["e"] = "en",
            ["ct"] = "zh-Hant",
            ["cs"] = "zh-Hans",
            ["j"] = "en",
            ["th"] = "th-TH",
            ["ko"] = "en",
            ["vi"] = "vi-VN"
        };
    }
}