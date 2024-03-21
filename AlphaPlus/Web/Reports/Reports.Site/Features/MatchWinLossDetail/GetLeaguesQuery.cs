using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.MatchWinLossDetail;

public class GetLeaguesQuery : BaseQuery, IRequest<LeaguesViewModel>
{
    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public string Products { get; set; }

    public string LanguageKey => Viewer.LangKey;
}