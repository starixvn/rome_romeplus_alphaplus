using AlphaPlus.Core.Helpers;
using AlphaPlus.Shared.AppResources.Betlist.Site;
using Fanex.Result.Core.KenoLotteryTG;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public interface IKenoResult : IKenoLotteryTGResult
{
}

public class KenoResult(KenoLotteryTGConfigs configs) : KenoLotteryTGResult(configs), IKenoResult
{
}

public class KenoResultHandler(IKenoResult kenoResult, IBrowserHelper browserHelper)
    : IRequestHandler<KenoResultQuery, KenoLotteryTableGameResultViewModel>
{
    public Task<KenoLotteryTableGameResultViewModel> Handle(KenoResultQuery message, CancellationToken cancellationToken)
    {
        var result = kenoResult.GetResult(
            message.CustId.ToString(),
            !browserHelper.IsMobile(),
            message.EventDate,
            message.RootUrl,
            message.LanguageKey);
        var viewModel = new KenoLotteryTableGameResultViewModel(result, Resources.Keno);

        return Task.FromResult(viewModel);
    }
}