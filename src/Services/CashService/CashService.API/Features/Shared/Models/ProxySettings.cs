namespace AlphaPlus.CashService.API.Features.Shared.Models;

public class ProxySettings
{
    public bool IsUseProxy { get; set; }
    public string ProxyAddress { get; set; }
    public bool BypassOnLocal { get; set; }
}