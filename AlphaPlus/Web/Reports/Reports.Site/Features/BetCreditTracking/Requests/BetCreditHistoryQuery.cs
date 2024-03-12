using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.BetCreditTracking;

public class BetCreditHistoryQuery : DefaultDateRangeQuery, IRequest<BetCreditHistoryViewModel>
{
    public BetCreditHistoryQuery() : base()
        => MinDate = ReportDateService.Instance.GetBalanceUpDay().GetAwaiter().GetResult();

    public string Member { get; set; }

    public long? RefNo { get; set; }

    public int PageIndex { get; set; } = 1;

    public int PageSize { get; set; } = 100;

    public override bool IsValid() => !string.IsNullOrWhiteSpace(Member) && base.IsValid();
}
