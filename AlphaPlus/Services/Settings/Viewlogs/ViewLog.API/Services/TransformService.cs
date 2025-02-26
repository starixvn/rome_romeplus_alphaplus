using AlphaPlus.Settings.ViewLogs.API.Params;

namespace AlphaPlus.Settings.ViewLogs.API.Services
{
    public interface TransformService
    {
        string TransformTrackingValue(string trackingValue, int logActionId, TransformParam transformParam, bool isHideNickname = false);
    }
}
