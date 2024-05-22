using AlphaPlus.Tickets.Site.App_GlobalResources;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public record KenoResultViewModel(string ResultUrl) : KenoLotteryTableGameResultViewModel(ResultUrl, Resources.Keno);
