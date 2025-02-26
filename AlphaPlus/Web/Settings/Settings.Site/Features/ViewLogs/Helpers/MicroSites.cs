namespace AlphaPlus.Settings.Site.Features.ViewLogs.Helpers
{
    public interface MicroSites
    {
        string OldMainPath(string virtualPath);

        string ViewLogsPath(string virtualPath);
    }
}