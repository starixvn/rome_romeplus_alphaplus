namespace AlphaPlus.Settings.Site.ViewLogs.Helpers
{
    public class MicroSitesImpl : MicroSites
    {
        public string OldMainPath(string virtualPath)
        {
            return AppPathHelper.GetAppPath("/settings", virtualPath);
        }

        public string ViewLogsPath(string virtualPath)
        {
            return AppPathHelper.GetAppPath("/settings", virtualPath);
        }
    }
}
