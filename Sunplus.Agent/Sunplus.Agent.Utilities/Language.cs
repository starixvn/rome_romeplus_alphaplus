namespace Sunplus.Agent.Utilities
{
    public static class Language
    {
        private static readonly string[] Languages = new string[] { "en-US", "zh-TW", "ja-JP", "th-TH", "zh-CN", "ko-KR", "vi-VN" };
        private static readonly string[] LanguagesKeys = new string[] { "E", "CT", "J", "TH", "CS", "KO", "VI" };

        public static bool IsValidLanguage(string culture)
        {
            int c = Languages.Length;
            for (int i = 0; i < c; i++)
            {
                if (Languages[i] == culture)
                {
                    return true;
                }
            }

            return false;
        }

        public static string LangKeyFromCulture(string culture)
        {
            int c = Languages.Length;
            for (int i = 0; i < c; i++)
            {
                if (Languages[i] == culture)
                {
                    return LanguagesKeys[i];
                }
            }

            return null;
        }

        public static string CultureFromLangKey(string langKey)
        {
            int c = LanguagesKeys.Length;
            for (int i = 0; i < c; i++)
            {
                if (LanguagesKeys[i] == langKey)
                {
                    return Languages[i];
                }
            }

            return null;
        }
    }
}