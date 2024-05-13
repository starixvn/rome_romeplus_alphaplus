namespace AlphaPlus.Tickets.Site.Features.Results.Keno;

public record KenoLotteryTableGameResultViewModel(
    string ResultUrl, string ProductName) : BaseResultTab(ProductName)
{
}