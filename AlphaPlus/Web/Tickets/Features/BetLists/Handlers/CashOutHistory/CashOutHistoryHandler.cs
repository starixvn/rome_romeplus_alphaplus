using AlphaPlus.BetLists.CashOutHistory;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.CashOutHistory;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.CashOutHistory;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.CashOutHistory;

public class CashOutHistoryHandler(ICashOutHistoryService cashOutHistoryService)
    : IRequestHandler<CashOutHistoryQuery, CashOutHistoryViewModel>
{
    private readonly ICashOutHistoryService cashOutHistoryService = cashOutHistoryService;

    public async Task<CashOutHistoryViewModel> Handle(CashOutHistoryQuery message, CancellationToken cancellationToken)
    {
        var param = new CashOutHistoryParam(message.TransId, message.CustId, message.WinLostDate);
        var cashOutHistory = await cashOutHistoryService.GetHistory(param);

        return new CashOutHistoryViewModel(cashOutHistory);
    }
}
