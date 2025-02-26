using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Models;
using AlphaPlus.Tickets.Site.Features.Shared;
using Fanex.Result.Core.KenoLotteryTG;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public interface ITableGamesResult : IKenoLotteryTGResult
{
}

public class TableGamesResult(KenoLotteryTGConfigs configs) : KenoLotteryTGResult(configs), ITableGamesResult
{
}

public class TableGamesResultHandler(
    ITableGamesResult tableGamesResult,
    IBrowserHelper browserHelper,
    ICacheRefService cacheRefService) : IRequestHandler<TableGamesResultQuery, KenoLotteryTableGameResultViewModel>
{
    public Task<KenoLotteryTableGameResultViewModel> Handle(TableGamesResultQuery message, CancellationToken cancellationToken)
    {
        var resultPath = tableGamesResult.GetResult(
            message.CustId.ToString(),
            !browserHelper.IsMobile(),
            message.EventDate,
            message.RootUrl,
            message.LanguageKey);
        var viewModel = new KenoLotteryTableGameResultViewModel(resultPath, cacheRefService.GetProductName(ProductIdConstants.TableGames));

        return Task.FromResult(viewModel);
    }
}