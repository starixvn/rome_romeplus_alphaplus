namespace AlphaPlus.Main.Site.Features.Language
{
    using System.Collections.Immutable;
    using AlphaPlus.Main.Site.Shared.Constants;

    public class LanguageViewModel
    {
        public LanguageViewModel(
            ImmutableDictionary<string, string> data,
            string currentLanguage = Language.Default)
        {
            Data = data ?? Language.CodeNames;
            CurrentLanguage = currentLanguage;
        }

        public ImmutableDictionary<string, string> Data { get; }

        public string CurrentLanguage { get; }
    }
}
