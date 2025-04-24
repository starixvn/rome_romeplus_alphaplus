using AlphaPlus.Core.Helpers;

namespace AlphaPlus.Core.Utils;

public static class VirtualPathUtility
{
    public static string ToAbsolute(string relativePath)
    {
        var request = AlphaContext.Current.Request;
        return request.PathBase + relativePath[1..];
    }
}
