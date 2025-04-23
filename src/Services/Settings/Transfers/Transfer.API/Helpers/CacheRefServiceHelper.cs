using Fanex.RemotingRef;

namespace AlphaPlus.Settings.Transfer.API.Helpers;

public static class CacheRefServiceHelper
{
    public static IRemotingRefService Instance { get; set; }

    public static string GetProgramName(int resourceId)
        => GetResourceDataByRefIdAndResourceId("RewardProgram", resourceId);

    private static string GetResourceDataByRefIdAndResourceId(string refId, int resourceId)
        => Instance.GetResourceDataByRefIdAndResourceId(
            refId,
            resourceId.ToString(),
            LanguageCode,
            string.Empty);

    private static string LanguageCode
        => Thread.CurrentThread.CurrentCulture.Name;
}