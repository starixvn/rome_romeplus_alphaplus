using System.Xml.Serialization;

namespace Alpha.Main.Site.Features.Default.MenuBar;

public class SiteMapNode
{
    [XmlAttribute("roles")]
    public string Roles { get; set; }

    [XmlAttribute("hideForSites")]
    public string HideForSites { get; set; }

    [XmlAttribute("resourceKey")]
    public string ResourceKey { get; set; }

    [XmlAttribute("byPassAuthorization")]
    public bool ByPassAuthorization { get; set; }

    [XmlAttribute("byPassProtectedMode")]
    public bool ByPassProtectedMode { get; set; }

    [XmlAttribute("byPassProtectedModeInternal")]
    public bool ByPassProtectedModeInternal { get; set; }

    [XmlAttribute("isBeta")]
    public bool IsBeta { get; set; }

    [XmlAttribute("isNew")]
    public bool IsNew { get; set; }

    [XmlAttribute("isOld")]
    public bool IsOld { get; set; }

    [XmlAttribute("isMobileView")]
    public bool IsMobileView { get; }

    [XmlAttribute("isHiddenInMobile")]
    public bool IsHiddenInMobile { get; set; }

    [XmlAttribute("url")]
    public string Url { get; set; }

    [XmlAttribute("menuItemType")]
    public string MenuItemType { get; set; }

    [XmlElement("siteMapNode")]
    public SiteMapNode[] ChildNodes { get; set; }

    public bool HasChildNodes => ChildNodes != null && ChildNodes.Length != 0;
}