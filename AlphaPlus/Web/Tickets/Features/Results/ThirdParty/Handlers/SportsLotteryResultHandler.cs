using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Models;
using AlphaPlus.Tickets.Site.Features.Shared;
using Fanex.Result.Core.SportsLottery;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class SportsLotteryResultHandler(
    ISportsLotteryResult sportsLotteryResult,
    IBrowserHelper browserHelper,
    ICacheRefService cacheRefService) : IRequestHandler<SportsLotteryResultQuery, KenoLotteryTableGameResultViewModel>
{
    public Task<KenoLotteryTableGameResultViewModel> Handle(SportsLotteryResultQuery message, CancellationToken cancellationToken)
    {
        var resultPath = sportsLotteryResult.GetResultPage(
            !browserHelper.IsMobile(),
            message.Language,
            message.IsDarkMode);
        var viewModel = new KenoLotteryTableGameResultViewModel(resultPath, cacheRefService.GetProductName(ProductIdConstants.SportsLottery));

        return Task.FromResult(viewModel);
    }
}