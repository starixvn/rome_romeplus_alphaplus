using AlphaPlus.Core.Helpers;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.TableGames;

public class TableGamesResultHandler(
    ITableGamesResult tableGamesResult,
    IBrowserHelper browserHelper) : IRequestHandler<TableGamesResultQuery, TableGamesResultViewModel>
{
    public Task<TableGamesResultViewModel> Handle(TableGamesResultQuery message, CancellationToken cancellationToken)
    {
        var resultPath = tableGamesResult.GetResult(
            message.CustId.ToString(),
            !browserHelper.IsMobile(),
            message.EventDate,
            message.RootUrl,
            message.LanguageKey);
        var viewModel = new TableGamesResultViewModel(resultPath);

        return Task.FromResult(viewModel);
    }
}