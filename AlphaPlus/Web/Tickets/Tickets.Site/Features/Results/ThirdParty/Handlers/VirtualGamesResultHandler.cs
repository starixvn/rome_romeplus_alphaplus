using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Models;
using AlphaPlus.Tickets.Site.Features.Shared;
using Fanex.Result.Core.KenoLotteryTG;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public interface IVirtualGamesResult : IKenoLotteryTGResult
{
}

public class VirtualGamesResult(KenoLotteryTGConfigs configs) : KenoLotteryTGResult(configs), IVirtualGamesResult
{
}

public class VirtualGamesResultHandler(
    IVirtualGamesResult virtualGamesResult,
    IBrowserHelper browserHelper,
    ICacheRefService cacheRefService) : IRequestHandler<VirtualGamesResultQuery, KenoLotteryTableGameResultViewModel>
{
    public Task<KenoLotteryTableGameResultViewModel> Handle(VirtualGamesResultQuery message, CancellationToken cancellationToken)
    {
        var resultPath = virtualGamesResult.GetResult(
            message.CustId.ToString(),
            !browserHelper.IsMobile(),
            message.EventDate,
            message.RootUrl,
            message.LanguageKey);
        var viewModel = new KenoLotteryTableGameResultViewModel(resultPath, cacheRefService.GetProductName(ProductIdConstants.VirtualGames));

        return Task.FromResult(viewModel);
    }
}