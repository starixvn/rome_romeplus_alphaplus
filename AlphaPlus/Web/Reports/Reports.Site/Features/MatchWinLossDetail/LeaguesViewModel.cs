using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Reports.Site.Features.MatchWinLossDetail;

public record LeaguesViewModel(
    IEnumerable<SelectListItem> Leagues,
    IEnumerable<string> UserSelectedLeagueIds,
    bool IsUserSelectsAllLeagues);
