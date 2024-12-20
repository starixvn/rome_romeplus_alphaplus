using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.MatchWinLossDetail;

public class MatchWinLossDetailQuery : BalanceUpDateRangeQuery, IRequest<MatchWinLossDetailViewModel>
{
    public MatchWinLossDetailQuery()
        => LanguageKey = Viewer.LangKey;

    public string UserSelectedProductIds { get; set; } = "*";

    public string UserSelectedLeagueIds { get; set; }

    public bool IsUserSelectsAllLeagues { get; set; }

    public bool IsStackMode { get; set; }

    public bool IsSetDefaultStackMode => "*" == UserSelectedProductIds;

    public string LanguageKey { get; }
}