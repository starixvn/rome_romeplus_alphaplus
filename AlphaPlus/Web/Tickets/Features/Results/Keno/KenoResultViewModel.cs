using AlphaPlus.Tickets.Site.App_GlobalResources;

namespace AlphaPlus.Tickets.Site.Features.Results.Keno;

public record KenoResultViewModel(string ResultUrl) : KenoLotteryTableGameResultViewModel(
    ResultUrl,
    Resources.Keno);
