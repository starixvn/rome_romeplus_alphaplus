using System.Xml;
using Microsoft.AspNetCore.Hosting;

namespace Alpha.Main.Helpers.Impl
{

    public class SiteMapReaderImpl(IWebHostEnvironment environment) : SiteMapReader
    {
        private readonly IWebHostEnvironment environment = environment;

        public IEnumerable<SiteMapNode> GetSiteMapNodes()
        {
            var nodes = new List<SiteMapNode>();
            XmlDocument doc = new();
            doc.Load(string.Concat(environment.WebRootPath, "/App_Data/Alpha.sitemap"));

            foreach (XmlNode node in doc.SelectNodes("/siteMap/siteMapNodes/siteMapNode"))
            {
                var siteMapNode = new SiteMapNode
                {
                    Key = node["Key"]?.InnerText,
                    ResourceKey = node["ResourceKey"]?.InnerText
                };

                nodes.Add(siteMapNode);
            }

            return nodes;
        }
    }
}