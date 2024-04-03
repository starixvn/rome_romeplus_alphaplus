namespace AlphaPlus.Settings.Site.ViewLogs.ViewModels
{
    using AlphaPlus.Settings.ViewLogs.API.Constants;
    using AlphaPlus.Settings.ViewLogs.API.Models;

    public class LogActionTypeViewModel
    {
        public LogActionTypeViewModel(LogActionType logActionType, string language)
        {
            if (logActionType == null)
            {
                return;
            }

            Id = logActionType.Id;
            Name = GetName(logActionType, language);
        }

        public LogActionTypeViewModel(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }

        public string Name { get; }

        private string GetName(LogActionType logActionType, string language)
        {
            switch (language)
            {
                case LanguageCode.English:
                    return logActionType.EnglishName;
                case LanguageCode.ChineseTraditional:
                    return logActionType.ChineseTraditionalName;
                case LanguageCode.ChineseSimplified:
                    return logActionType.ChineseSimplifiedName;
                case LanguageCode.Japanese:
                    return logActionType.JapaneseName;
                case LanguageCode.Thai:
                    return logActionType.ThaiName;
                case LanguageCode.Korean:
                    return logActionType.KoreanName;
                case LanguageCode.Vietnamese:
                    return logActionType.VietnameseName;
                default:
                    return logActionType.EnglishName;
            }
        }
    }
}