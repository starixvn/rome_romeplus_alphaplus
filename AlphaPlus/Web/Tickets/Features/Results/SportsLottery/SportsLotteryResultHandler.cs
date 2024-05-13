using AlphaPlus.Core.Helpers;
using Fanex.Result.Core.SportsLottery;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.SportsLottery;

public class SportsLotteryResultHandler(
    ISportsLotteryResult sportsLotteryResult,
    IBrowserHelper browserHelper) : IRequestHandler<SportsLotteryResultQuery, SportsLotteryResultViewModel>
{
    public Task<SportsLotteryResultViewModel> Handle(SportsLotteryResultQuery message, CancellationToken cancellationToken)
    {
        var resultPath = sportsLotteryResult.GetResultPage(
            !browserHelper.IsMobile(),
            message.Language,
            message.IsDarkMode);
        var viewModel = new SportsLotteryResultViewModel(resultPath);

        return Task.FromResult(viewModel);
    }
}