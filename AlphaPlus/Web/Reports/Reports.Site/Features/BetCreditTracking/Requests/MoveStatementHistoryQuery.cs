using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.BetCreditTracking;

public class MoveStatementHistoryQuery : DefaultDateRangeQuery, IRequest<MoveStatementHistoryViewModel>
{
    public string Member { get; set; }

    public int PageIndex { get; set; } = 1;

    public int PageSize { get; set; } = 100;

    public bool IsMainReport { get; set; } = true;
}