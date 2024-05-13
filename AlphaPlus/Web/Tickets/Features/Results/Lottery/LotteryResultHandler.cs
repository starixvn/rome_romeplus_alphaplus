using AlphaPlus.Core.Helpers;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.Lottery;

public class LotteryResultHandler(
    ILotteryResult lotteryResult,
    IBrowserHelper browserHelper) : IRequestHandler<LotteryResultQuery, LotteryResultViewModel>
{
    public Task<LotteryResultViewModel> Handle(LotteryResultQuery message, CancellationToken cancellationToken)
    {
        var result = lotteryResult.GetResult(
            message.CustId.ToString(),
            !browserHelper.IsMobile(),
            message.EventDate,
            message.RootUrl,
            message.LanguageKey);
        var viewModel = new LotteryResultViewModel(result);

        return Task.FromResult(viewModel);
    }
}