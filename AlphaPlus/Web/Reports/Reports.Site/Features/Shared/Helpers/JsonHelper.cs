using System.Text.Json;
using System.Text.Json.Serialization;

namespace AlphaPlus.Reports.Site.Features.Shared.Helpers;

public static class JsonHelper
{
    public static bool TryParse<T>(string json, out T value)
    {
        if (!string.IsNullOrWhiteSpace(json))
        {
            try
            {
                value = JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return true;
            }
            catch (Exception)
            {
            }
        }

        value = default;
        return false;
    }

    public static string ToJson(object data)
        => JsonSerializer.Serialize(data, new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
        });
}
