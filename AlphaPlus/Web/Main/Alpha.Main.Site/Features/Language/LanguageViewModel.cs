namespace Alpha.Main.Site.Features.Language
{
    public class LanguageViewModel
    {
        public LanguageViewModel(
            Dictionary<string, string> data,
            string currentLanguage = Constants.Language.Default)
        {
            Data = data ?? Constants.Language.CodeNames;
            CurrentLanguage = currentLanguage;
        }

        public Dictionary<string, string> Data { get; }

        public string CurrentLanguage { get; }
    }
}
