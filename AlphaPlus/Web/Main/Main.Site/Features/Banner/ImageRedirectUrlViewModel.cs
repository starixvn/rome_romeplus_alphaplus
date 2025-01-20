namespace AlphaPlus.Main.Site.Features.Banner
{
    public class ImageRedirectUrlViewModel(string imageSource, string redirectUrl, bool logClick, string target)
    {
        public string ImageSource { get; } = imageSource;

        public string RedirectUrl { get; } = redirectUrl;

        public bool LogClick { get; } = logClick;

        public string Target { get; } = target;
    }
}