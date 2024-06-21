namespace AlphaPlus.Features.Messages.API.Models.Announcement
{
    using AlphaPlus.Shared.AppResources.Features.Messages;
    using System.Globalization;

    public interface ResourceManager
    {
        string GetString(string name, CultureInfo culture);
    }

    public class SiteResourceManager : ResourceManager
    {
        public string GetString(string name, CultureInfo culture)
            => Resources.ResourceManager.GetString(name, culture) ?? string.Empty;
    }
}
