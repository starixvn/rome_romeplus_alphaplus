using AlphaPlus.Core.Models;
using AlphaPlus.Tickets.Site.Features.Results.Keno;
using AlphaPlus.Tickets.Site.Features.Shared;

namespace AlphaPlus.Tickets.Site.Features.Results.Lottery;

public record LotteryResultViewModel(string ResultUrl) : KenoLotteryTableGameResultViewModel(
    ResultUrl,
    CacheRefServiceHelper.GetProductName(ProductIdConstants.Lottery));