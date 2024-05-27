using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Models;
using AlphaPlus.Tickets.Site.Features.Shared;
using Fanex.Result.Core.KenoLotteryTG;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public interface ILotteryResult : IKenoLotteryTGResult
{
}

public class LotteryResult(KenoLotteryTGConfigs configs) : KenoLotteryTGResult(configs), ILotteryResult
{
}

public class LotteryResultHandler(
    ILotteryResult lotteryResult,
    IBrowserHelper browserHelper,
    ICacheRefService cacheRefService)
    : IRequestHandler<LotteryResultQuery, KenoLotteryTableGameResultViewModel>
{
    public Task<KenoLotteryTableGameResultViewModel> Handle(LotteryResultQuery message, CancellationToken cancellationToken)
    {
        var result = lotteryResult.GetResult(
            message.CustId.ToString(),
            !browserHelper.IsMobile(),
            message.EventDate,
            message.RootUrl,
            message.LanguageKey);
        var viewModel = new KenoLotteryTableGameResultViewModel(result, cacheRefService.GetProductName(ProductIdConstants.Lottery));

        return Task.FromResult(viewModel);
    }
}