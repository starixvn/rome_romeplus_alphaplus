namespace AgencyCash.API.Shared.Helpers;

public class Base64StringHelper
{
    public static byte[] ConvertToByte(string base64String)
    {
        if (string.IsNullOrWhiteSpace(base64String))
        {
            return null;
        }

        // If the string includes a data URI, strip it out
        var parts = base64String.Split(',', StringSplitOptions.RemoveEmptyEntries);
        var cleanBase64 = parts.Length > 1 ? parts[1] : parts[0];

        return Convert.FromBase64String(cleanBase64);
    }
}