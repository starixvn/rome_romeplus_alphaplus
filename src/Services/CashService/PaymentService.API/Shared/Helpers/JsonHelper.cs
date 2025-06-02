using System.Text.Json;

namespace AlphaPlus.PaymentService.API.Shared.Helpers;

public static class JsonHelper
{
    public static string ToJson(object value)
        => JsonSerializer.Serialize(value, new JsonSerializerOptions
        {
            PropertyNamingPolicy = null,
            WriteIndented = false
        });
}