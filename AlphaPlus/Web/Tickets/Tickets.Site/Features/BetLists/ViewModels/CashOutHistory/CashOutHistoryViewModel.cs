using AlphaPlus.Shared.AppResources.Betlist.Services;
using AlphaPlus.BetLists.CashOutHistory;

namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.CashOutHistory;

public class CashOutHistoryViewModel(CashOutHistoryModel model)
{
    public CashOutHistoryModel CashOutModel { get; } = model;

    public string Title { get; } = $"{Resources.CashOutHistoryTitle} - {model.TransId}";

    public int TotalColumn { get; } = 4;
}