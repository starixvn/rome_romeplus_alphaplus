namespace Alpha.Main.Site.Features.Banner
{
    public class ImageRedirectUrlViewModel(string imageSource, string redirectUrl = "")
    {
        public string ImageSource { get; } = imageSource;

        public string RedirectUrl { get; } = redirectUrl;
    }
}