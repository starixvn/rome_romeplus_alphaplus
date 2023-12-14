using AlphaPlus.Core.Helpers;
using Microsoft.AspNetCore.Http.Extensions;

namespace AlphaPlus.Core.Utils;

public static class VirtualPathUtility
{
    public static string ToAbsolute(string relativePath)
    {
        var request = AlphaContext.Current.Request;
        return UriHelper.BuildAbsolute(request.Scheme, request.Host, relativePath);
    }
}
