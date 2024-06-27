namespace AlphaPlus.Settings.Site.Features.ViewLogs.Helpers
{
    using AlphaPlus.Settings.ViewLogs.API.Models;

    public interface TrackingValueFormatter
    {
        string GetFormattedDiffTrackingValue(Log log, bool isShowOldValue);
    }
}