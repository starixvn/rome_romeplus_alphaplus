using System.Xml.Serialization;

namespace Alpha.Main.Helpers;

[XmlRoot("siteMap")]
public class SiteMap
{
    [XmlElement("siteMapNode")]
    public SiteMapNode[] ChildNodes { get; set; }

    public bool HasChildNodes => ChildNodes != null && ChildNodes.Length != 0;
}