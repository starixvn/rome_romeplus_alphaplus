using AlphaPlus.Core.Models;
using AlphaPlus.Tickets.Site.Features.Shared;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public record VirtualGamesResultViewModel(string ResultUrl) : KenoLotteryTableGameResultViewModel(
    ResultUrl,
    CacheRefServiceHelper.GetProductName(ProductIdConstants.VirtualGames));
