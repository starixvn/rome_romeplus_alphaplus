namespace Sunplus.Agent.Utilities
{
    public static class Language
    {
        private static readonly string[] Languages = new string[] { "en-US", "zh-TW", "ja-JP", "th-TH", "zh-CN", "ko-KR", "vi-VN" };

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
    }
}