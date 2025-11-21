using AlphaPlus.Reports.Features.Shared;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.BetChoiceMatching;

public class BetChoiceMatchingJsonQuery : BetChoiceMatchingQuery, IRequest<BetChoiceMatchingJsonModel>
{
    public BetChoiceMatchingJsonQuery()
    {
        IsAdminCanViewVNIP = Viewer.IsAdminCanViewVNIP;
        SRecommendId = Viewer.SRecommendId;
        System = Viewer.System;
        MinDate = ReportDateService.Instance.GetBalanceUpDay().GetAwaiter().GetResult();
    }

    public bool IsAdminCanViewVNIP { get; }
    public int SRecommendId { get; }
    public string System { get; }

    public override bool IsValid() => MinDate <= EventDate && base.IsValid();
}