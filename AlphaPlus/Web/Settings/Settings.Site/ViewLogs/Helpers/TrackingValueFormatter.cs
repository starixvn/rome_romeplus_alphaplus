namespace AlphaPlus.Settings.Site.ViewLogs.Helpers
{
    using AlphaPlus.Settings.ViewLogs.API.Models;

    public interface TrackingValueFormatter
    {
        string GetFormattedDiffTrackingValue(Log log, bool isShowOldValue);
    }
}