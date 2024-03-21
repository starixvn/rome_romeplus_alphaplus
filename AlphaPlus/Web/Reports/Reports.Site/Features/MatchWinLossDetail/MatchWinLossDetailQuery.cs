using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.MatchWinLossDetail;

public class MatchWinLossDetailQuery : DefaultDateRangeQuery, IRequest<MatchWinLossDetailViewModel>
{
    public MatchWinLossDetailQuery() : base()
    {
        MinDate = ReportDateService.Instance.GetBalanceUpDay().GetAwaiter().GetResult();
        LanguageKey = Viewer.LangKey;
    }

    public string UserSelectedProductIds { get; set; }

    public string UserSelectedLeagueIds { get; set; }

    public bool IsUserSelectsAllLeagues { get; set; }

    public bool IsStackMode { get; set; }

    public bool IsSetDefaultStackMode => string.IsNullOrWhiteSpace(UserSelectedProductIds);

    public string LanguageKey { get; }
}