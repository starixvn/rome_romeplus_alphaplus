namespace AlphaPlus.Features.Messages.API.Models.Announcement
{
    using AlphaPlus.Features.Messages.API.AppResources;
    using System.Globalization;

    public interface ResourceManager
    {
        string GetString(string name, CultureInfo culture);
    }

    public class SiteResourceManager : ResourceManager
    {
        public string GetString(string name, CultureInfo culture)
            => Resources.ResourceManager.GetString(name, culture);
    }
}
