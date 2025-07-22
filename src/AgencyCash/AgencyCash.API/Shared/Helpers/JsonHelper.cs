using System.Text.Json;

namespace AgencyCash.API.Shared.Helpers;

public static class JsonHelper
{
    private static readonly JsonSerializerOptions CachedOptions = new()
    {
        PropertyNamingPolicy = null,
        WriteIndented = false
    };

    public static string ToJson(object value)
        => JsonSerializer.Serialize(value, CachedOptions);
}