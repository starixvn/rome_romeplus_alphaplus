using System.Xml;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Hosting;

namespace Alpha.Main.Helpers.Impl
{

    public class SiteMapReaderImpl(IWebHostEnvironment environment) : SiteMapReader
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