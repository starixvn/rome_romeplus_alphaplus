using AlphaPlus.Core.Models;
using AlphaPlus.Tickets.Site.Features.Shared;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public record TableGamesResultViewModel(string ResultUrl) : KenoLotteryTableGameResultViewModel(
    ResultUrl,
    CacheRefServiceHelper.GetProductName(ProductIdConstants.TableGames));