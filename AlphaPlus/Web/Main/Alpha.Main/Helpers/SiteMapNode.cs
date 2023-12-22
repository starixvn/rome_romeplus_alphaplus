namespace Alpha.Main.Helpers;

public class SiteMapNode
{
    public string Key { get; set; }

    public string ResourceKey { get; set; }

    public string Link { get; set; }

    public string AppPath { get; set; }

    public bool ByPassAuthorization { get; set; }

    public bool ByPassProtectedMode { get; set; }

    public bool ByPassProtectedModeInternal { get; set; }

    public bool IsBeta { get; set; }

    public bool IsNew { get; set; }

    public bool IsOld { get; set; }

    public bool IsMobileView { get; }

    public bool IsHiddenInMobile { get; set; }

    IEnumerable<SiteMapNode> ChildNodes { get; set; }

    public bool HasChildNodes => ChildNodes != null && ChildNodes.Any();
}