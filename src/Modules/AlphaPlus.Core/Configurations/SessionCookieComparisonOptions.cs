namespace AlphaPlus.Core.Configurations;

public class SessionCookieComparisonOptions
{
    public const string SectionName = "SessionCookieComparison";
    
    public bool EnableSessionCookieComparison { get; set; } = true;
    
    public string[] MonitoredPaths { get; set; } = new[]
    {
        "/settings/creditbalance",
        "/reports/winlossdetail"
    };
    
    public string LogLevel { get; set; } = "Error";
    
    public bool IncludeUserAgentInLog { get; set; } = true;
    
    public bool IncludeRefererInLog { get; set; } = true;
    
    public bool IncludeIpAddressInLog { get; set; } = true;
} 