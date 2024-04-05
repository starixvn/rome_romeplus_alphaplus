namespace AlphaPlus.Features.Messages.Utils
{
    using System;

    public static class MessageFormatter
    {
        private const string BoldTexten_US = "which affected are considered VOID";
        private const string BoldTextzh_CN = "只作废受影响的注单";
        private const string BoldTextzh_TW = "只作廢受影響的注單";

        public static string MarkBoldText(string message, string language)
        {
            var boldText = GetBoldText(language);

            return message.IndexOf(boldText, StringComparison.OrdinalIgnoreCase) == -1
                ? message
                : message.Replace(boldText, $"<strong>{boldText}</strong>");
        }

        private static string GetBoldText(string language)
        {
            switch (language)
            {
                case "zh-CN":
                    return BoldTextzh_CN;

                case "zh-TW":
                    return BoldTextzh_TW;

                default:
                    return BoldTexten_US;
            }
        }
    }
}
