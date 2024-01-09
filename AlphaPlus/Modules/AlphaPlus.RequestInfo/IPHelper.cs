using System.Net;
using System.Net.Sockets;
using Microsoft.AspNetCore.Http;

namespace AlphaPlus.RequestInfo;

public interface IPHelper
{
    string[] GetClientIPs();

    string GetClientIP();

    bool IsIPV6(string clientIP);

    ulong ConvertIPV4ToNumber(string ipAddress);

    ulong ConvertIPV6ToNumber(string ipAddress);
}

public class IPHelperImpl : IPHelper
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public IPHelperImpl(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public string[] GetClientIPs()
    {
        var context = httpContextAccessor.HttpContext;
        var actualIP = context.Request.Headers["X-Actual-IP"].ToString();

        actualIP = string.IsNullOrWhiteSpace(actualIP) ? context.Request.Headers["X-Forwarded-For"].ToString() : actualIP;

        actualIP = string.IsNullOrWhiteSpace(actualIP) ? context.Connection.RemoteIpAddress.ToString() : actualIP;

        return string.IsNullOrWhiteSpace(actualIP) || actualIP.Equals("localhost") || actualIP.Equals("::1") ?
            new[] { "127.0.0.1" } : actualIP.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
    }

    public string GetClientIP()
    {
        string[] ips = GetClientIPs();
        return ips[0].Trim();
    }


    public bool IsIPV6(string clientIP)
    {
        return IPAddress.TryParse(clientIP, out IPAddress address)
            && address.AddressFamily == AddressFamily.InterNetworkV6;
    }

    public ulong ConvertIPV4ToNumber(string ipAddress)
    {
        if (!IPAddress.TryParse(ipAddress, out IPAddress parsedIpAddress))
        {
            return 0;
        }

        var addressBytes = parsedIpAddress.GetAddressBytes();
        Array.Reverse(addressBytes);

        return BitConverter.ToUInt32(addressBytes, 0);
    }

    public ulong ConvertIPV6ToNumber(string ipAddress)
    {
        if (IPAddress.TryParse(ipAddress, out IPAddress address))
        {
            byte[] addrBytes = address.GetAddressBytes();
            if (BitConverter.IsLittleEndian)
            {
                List<byte> byteList = new(addrBytes);
                byteList.Reverse();
                addrBytes = byteList.ToArray();
            }

            var ipNum = BitConverter.ToUInt64(addrBytes, 8);
            ipNum += BitConverter.ToUInt64(addrBytes, 0);

            return ipNum;
        }

        return 0;
    }

}
