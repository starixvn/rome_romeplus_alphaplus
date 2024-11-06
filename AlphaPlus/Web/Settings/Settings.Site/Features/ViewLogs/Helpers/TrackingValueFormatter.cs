using AlphaPlus.Settings.ViewLogs.API.Models;

namespace AlphaPlus.Settings.Site.Features.ViewLogs.Helpers
{
    public interface TrackingValueFormatter
    {
        string GetFormattedDiffTrackingValue(Log log, bool isShowOldValue);
    }
}