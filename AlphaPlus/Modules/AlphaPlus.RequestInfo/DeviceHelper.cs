using AlphaPlus.RequestInfo.Models;
using Microsoft.AspNetCore.Http;
using UAParser;

namespace AlphaPlus.RequestInfo;

public interface IDeviceHelper
{
    string GetUserAgent();
    ClientInfo GetClientInfo();
    DeviceInfo GetDeviceInfo(string browserSize = null);
    Task<Models.RequestInfo> GetRequestInfo();
}

public class DeviceHelper : IDeviceHelper
{
    private readonly IPHelper ipHelper;
    private readonly IPService ipService;
    private readonly IHttpContextAccessor httpContextAccessor;

    public DeviceHelper(IPHelper ipHelper, IPService ipService, IHttpContextAccessor httpContextAccessor)
    {
        this.ipHelper = ipHelper;
        this.ipService = ipService;
        this.httpContextAccessor = httpContextAccessor;
    }

    public DeviceInfo GetDeviceInfo(string browserSize = null)
    {
        var clientInfo = GetClientInfo();
        var ua = clientInfo.UA;
        var os = clientInfo.OS;
        var device = clientInfo.Device;
        var operateSystem = $"{os.Family} {os.Major ?? "0"}.{os.Minor ?? "0"}.{os.Patch ?? "0"}";
        var browserName = $"{ua.Family} {ua.Major}.{ua.Minor}.{ua.Patch ?? "0"}";
        if (!string.IsNullOrWhiteSpace(browserSize))
        {
            browserName += $", {browserSize}";
        }

        return new DeviceInfo
        {
            IP = ipHelper.GetClientIP(),
            BrowserName = browserName,
            Platform = $"{operateSystem}, Device: {device.Family}, IsCrawler: {device.IsSpider}"
        };
    }

    public ClientInfo GetClientInfo()
    {
        var userAgent = GetUserAgent();
        var parser = Parser.GetDefault();
        var clientInfo = parser.Parse(userAgent);
        return clientInfo;
    }

    public string GetUserAgent()
        => httpContextAccessor.HttpContext.Request.Headers.UserAgent.ToString() ?? string.Empty;

    public async Task<Models.RequestInfo> GetRequestInfo()
    {
        var deviceInfo = GetDeviceInfo();

        return new Models.RequestInfo
        {
            LoginIp = deviceInfo.IP,
            CountryCode = await ipService.GetCountryCode(deviceInfo.IP),
            LoginDomain = httpContextAccessor.HttpContext.Request.Host.Value,
            BrowserName = deviceInfo.BrowserName,
            Platform = deviceInfo.Platform
        };
    }
}