
namespace AlphaPlus.Core.Helpers;

public class IPHelper
{
    public static string[] GetClientIPs()
    {
        var context = AlphaContext.Current;
        var actualIP = context.Request.Headers["X-Actual-IP"].ToString();

        actualIP = string.IsNullOrWhiteSpace(actualIP) ? context.Request.Headers["X-Forwarded-For"].ToString() : actualIP;

        actualIP = string.IsNullOrWhiteSpace(actualIP) ? context.Connection.RemoteIpAddress.ToString() : actualIP;

        return string.IsNullOrWhiteSpace(actualIP) || actualIP.Equals("localhost") || actualIP.Equals("::1") ?
            ["127.0.0.1"] : actualIP.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
    }

    public static string GetClientIP()
    {
        string[] ips = GetClientIPs();
        return ips[0].Trim();
    }
}
