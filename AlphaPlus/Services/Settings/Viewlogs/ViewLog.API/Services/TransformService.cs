namespace AlphaPlus.Settings.ViewLogs.API.Services
{
    using Params;

    public interface TransformService
    {
        string? TransformTrackingValue(string trackingValue, int logActionId, TransformParam transformParam, bool isHideNickname = false);
    }
}
