using AlphaPlus.Tickets.Site.App_GlobalResources;

namespace AlphaPlus.Tickets.Site.Features.Results.Keno;

public record BaseResultTab(string ProductName)
{
    public string ResultLinkText => string.Format(Resources.ResultLinkText, ProductName);
}