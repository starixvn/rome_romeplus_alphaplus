namespace AlphaPlus.Settings.Site.Features.Accounts.ViewModels
{
    public class Link(string text, string url)
    {
        public string Text { get; } = text;

        public string Url { get; } = url;
    }
}