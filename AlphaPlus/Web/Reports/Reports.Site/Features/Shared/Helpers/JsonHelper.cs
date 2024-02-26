using System.Text.Json;

namespace AlphaPlus.Reports.Site.Features.Shared.Helpers;

public static class JsonHelper
{
    [ThreadStatic]
    private static Func<object, string> toJsonFunc;

    public static Func<object, string> ToJsonFunc
    {
        get
        {
            toJsonFunc ??= ToJson;
            return toJsonFunc;
        }
        set
        {
            toJsonFunc = value;
        }
    }

    internal static string ToJson(object data) => JsonSerializer.Serialize(data).Replace("\"", "'");
}
