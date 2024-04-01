using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Reports.Site.Features.MatchWinLossDetail;

public record LeagueViewModel(
    IEnumerable<SelectListItem> Leagues,
    IEnumerable<string> UserSelectedLeagueIds,
    bool IsUserSelectsAllLeagues,
    string Language);
