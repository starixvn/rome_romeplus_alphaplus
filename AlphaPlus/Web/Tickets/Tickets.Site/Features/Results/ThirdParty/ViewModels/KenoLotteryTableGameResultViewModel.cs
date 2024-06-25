using AlphaPlus.Shared.AppResources.Betlist.Site;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public record KenoLotteryTableGameResultViewModel(string ResultUrl, string ProductName)
{
    public string ResultLinkText => string.Format(Resources.ResultLinkText, ProductName);
}