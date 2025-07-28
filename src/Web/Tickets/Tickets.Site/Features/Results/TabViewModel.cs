namespace AlphaPlus.Tickets.Site.Features.Results;

public record TabViewModel(
    string SelectedTabName,
    bool IsEnableVirtualGames,
    bool IsEnableSportsLottery,
    Func<int, string> GetProductName);
