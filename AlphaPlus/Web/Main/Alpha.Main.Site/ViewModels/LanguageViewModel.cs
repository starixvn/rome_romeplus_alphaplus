using Alpha.Main.Constants;

namespace Alpha.Main.Site.ViewModels
{
    public class LanguageViewModel
    {
        public LanguageViewModel(
            Dictionary<string, string> data,
            string currentLanguage = Language.Default)
        {
            Data = data ?? Language.CodeNames;
            CurrentLanguage = currentLanguage;
        }

        public Dictionary<string, string> Data { get; }

        public string CurrentLanguage { get; }
    }
}
