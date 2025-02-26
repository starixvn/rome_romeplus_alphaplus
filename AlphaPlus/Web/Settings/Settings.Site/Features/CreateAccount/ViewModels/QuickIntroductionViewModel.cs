namespace AlphaPlus.Settings.Site.Features.CreateAccount.ViewModels
{
    public class QuickIntroductionViewModel
    {
        public QuickIntroductionViewModel(string name, string language)
        {
            ActorUsername = name;
            Language = language;
        }

        public string ActorUsername { get; }
        public string Language { get; }
    }
}