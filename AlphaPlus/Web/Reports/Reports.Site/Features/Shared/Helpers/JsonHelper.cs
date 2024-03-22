using System.Text.Json;

namespace AlphaPlus.Reports.Site.Features.Shared.Helpers;

public static class JsonHelper
{
    public static bool TryParse<T>(string json, out T value)
    {
        if (!string.IsNullOrWhiteSpace(json))
        {
            try
            {
                var jsonDocument = JsonDocument.Parse(json);
                value = JsonSerializer.Deserialize<T>(jsonDocument);
                return true;
            }
            catch (Exception)
            {
            }
        }

        value = default;
        return false;
    }

    public static string ToJson(object data) => JsonSerializer.Serialize(data);
}
