namespace AgencyCash.API.Shared.Helpers;

public static class Base64StringHelper
{
    public static bool TryConvertToByte(string base64String, out byte[] result)
    {
        result = null;
        // If the string includes a data URI, strip it out
        var parts = base64String.Split(',', StringSplitOptions.RemoveEmptyEntries);
        var cleanBase64 = parts.Length > 1 ? parts[1] : parts[0];

        try
        {
            result = Convert.FromBase64String(cleanBase64);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}