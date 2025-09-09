namespace AlphaPlus.Core.Configurations;

public class SessionCookieComparisonOptions
{
    public const string SectionName = "SessionCookieComparison";
    
    public bool EnableSessionCookieComparison { get; set; } = true;
    
    public string[] MonitoredPaths { get; set; } = new[]
    {
        "/settings/CreditBalance",
        "/reports/winlossdetail"
    };
    
    public string LogLevel { get; set; } = "Error";
} 