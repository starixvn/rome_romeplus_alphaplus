namespace AlphaPlus.Settings.Site.ViewLogs.Helpers
{
    public interface MicroSites
    {
        string OldMainPath(string virtualPath);

        string ViewLogsPath(string virtualPath);
    }
}