using AlphaPlus.Tickets.Site.App_GlobalResources;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public record KenoLotteryTableGameResultViewModel(string ResultUrl, string ProductName)
{
    public string ResultLinkText => string.Format(Resources.ResultLinkText, ProductName);
}