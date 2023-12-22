namespace Alpha.Main.Helpers
{
    public interface SiteMapReader
    {
        IEnumerable<SiteMapNode> GetSiteMapNodes();
    }
}