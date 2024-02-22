using System.Xml.Serialization;

namespace AlphaPlus.Main.Site.Features.Default.MenuBar
{
    public interface ISiteMapReader
    {
        SiteMap GetRootNode();
    }

    public class SiteMapReader(IWebHostEnvironment environment) : ISiteMapReader
    {
        private readonly IWebHostEnvironment environment = environment;

        public SiteMap GetRootNode()
        {
            var serializer = new XmlSerializer(typeof(SiteMap));
            using var reader = new StreamReader(string.Concat(environment.ContentRootPath, "/App_Data/Alpha.sitemap"));
            return (SiteMap)serializer.Deserialize(reader);
        }
    }
}