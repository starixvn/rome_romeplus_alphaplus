using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace AlphaPlus.Core.Extensions;

public static class HttpSessionExtensions
{
    public static void Set<T>(this ISession session, string key, T value)
    {
        session.SetString(key, JsonSerializer.Serialize(value));
    }

    public static T Get<T>(this ISession session, string key, T defaultValue = default)
    {
        var value = session.GetString(key);
        return value == null ? defaultValue : JsonSerializer.Deserialize<T>(value);
    }
}
