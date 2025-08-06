namespace AlphaPlus.Main.Site.Shared.Constants
{
    public static class Language
    {
        public const string English = "en-US";
        public const string ChineseTraditional = "zh-TW";
        public const string ChineseSimplified = "zh-CN";
        public const string Japanese = "ja-JP";
        public const string Thai = "th-TH";
        public const string Korean = "ko-KR";
        public const string Vietnamese = "vi-VN";

        public const string NewChineseTraditional = "zh-Hant";
        public const string NewChineseSimplified = "zh-Hans";

        public const string Default = English;

        private static readonly Dictionary<string, string> _codeKeys = new()
        {
            [English] = "E",
            [ChineseTraditional] = "CT",
            [ChineseSimplified] = "CS",
            [Japanese] = "J",
            [Thai] = "TH",
            [Korean] = "KO",
            [Vietnamese] = "VI"
        };

        private static readonly Dictionary<string, string> _codeNames = new()
        {
            [English] = "English",
            [ChineseTraditional] = "繁體中文",
            [ChineseSimplified] = "简体中文",
            [Japanese] = "日本語",
            [Thai] = "ภาษาไทย",
            [Korean] = "한국어",
            [Vietnamese] = "Tiếng Việt"
        };

        private static readonly Dictionary<string, string> _telegramChatBotCodeNames = new()
        {
            [English] = "English",
            [NewChineseTraditional] = "繁體中文",
            [NewChineseSimplified] = "简体中文",
            [Thai] = "ภาษาไทย",
            [Vietnamese] = "Tiếng Việt"
        };

        public static readonly Dictionary<string, string> CodeKeys = _codeKeys;

        public static readonly Dictionary<string, string> CodeNames = _codeNames;

        public static readonly Dictionary<string, string> TelegramChatBotCodeNames = _telegramChatBotCodeNames;

        public static readonly Dictionary<string, string> TelegramChatBotCodeNamesWithoutVietNamese = _telegramChatBotCodeNames
            .Where(langauge => langauge.Key != Vietnamese)
            .ToDictionary(language => language.Key, language => language.Value);

        public static readonly Dictionary<string, string> CodeNamesWithoutVietNamese = _codeNames
            .Where(langauge => langauge.Key != Vietnamese)
            .ToDictionary(language => language.Key, language => language.Value);

        public static string MapNewChineseLanguageCode(string languageCode)
        {
            return languageCode switch
            {
                ChineseTraditional => NewChineseTraditional,
                ChineseSimplified => NewChineseSimplified,
                _ => languageCode,
            };
        }

        public static string MapToMainLanguageCode(string languageCode)
        {
            return languageCode switch
            {
                ChineseTraditional or ChineseSimplified or Thai or Vietnamese => languageCode,
                _ => English,
            };
        }
    }
}