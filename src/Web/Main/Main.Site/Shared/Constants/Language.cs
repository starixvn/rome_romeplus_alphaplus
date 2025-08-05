using System.Collections.Immutable;

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

        public static readonly ImmutableDictionary<string, string> CodeKeys = new Dictionary<string, string>
        {
            [English] = "E",
            [ChineseTraditional] = "CT",
            [ChineseSimplified] = "CS",
            [Japanese] = "J",
            [Thai] = "TH",
            [Korean] = "KO",
            [Vietnamese] = "VI"
        }.ToImmutableDictionary();

        public static readonly ImmutableDictionary<string, string> CodeNames = new Dictionary<string, string>
        {
            [English] = "English",
            [ChineseTraditional] = "繁體中文",
            [ChineseSimplified] = "简体中文",
            [Japanese] = "日本語",
            [Thai] = "ภาษาไทย",
            [Korean] = "한국어",
            [Vietnamese] = "Tiếng Việt"
        }.ToImmutableDictionary();

        public static readonly ImmutableDictionary<string, string> TelegramChatBotCodeNames = new Dictionary<string, string>
        {
            [English] = "English",
            [NewChineseTraditional] = "繁體中文",
            [NewChineseSimplified] = "简体中文",
            [Thai] = "ภาษาไทย",
            [Vietnamese] = "Tiếng Việt"
        }.ToImmutableDictionary();

        public static readonly ImmutableDictionary<string, string> TelegramChatBotCodeNamesWithoutVietNamese = TelegramChatBotCodeNames
            .Where(langauge => langauge.Key != Vietnamese)
            .ToImmutableDictionary(language => language.Key, language => language.Value);

        public static readonly ImmutableDictionary<string, string> CodeNamesWithoutVietNamese = CodeNames
            .Where(langauge => langauge.Key != Vietnamese)
            .ToImmutableDictionary(language => language.Key, language => language.Value);

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