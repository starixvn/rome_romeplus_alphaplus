namespace AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.WinLoss;

public record WinLossWeekViewModel(
    string FromDate,
    string ToDate,
    string Display,
    bool IsEnable,
    bool IsActive);
