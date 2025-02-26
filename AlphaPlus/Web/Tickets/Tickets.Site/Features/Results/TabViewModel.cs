namespace AlphaPlus.Tickets.Site.Features.Results;

public record TabViewModel(
    string SelectedTabName,
    bool IsEnabledKenoTab,
    bool IsEnableLottery,
    bool IsEnableVirtualGames,
    bool IsEnableTableGames,
    bool IsEnableSportsLottery,
    Func<int, string> GetProductName);
