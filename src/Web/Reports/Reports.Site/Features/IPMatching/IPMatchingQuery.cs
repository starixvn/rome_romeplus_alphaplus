using AlphaPlus.Core.Utils;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.IPMatching;

public class IPMatchingQuery : BaseQuery, IRequest<IPMatchingViewModel>
{
    public IPMatchingQuery()
    {
        MinDate = ReportDateService.Instance.GetBalanceUpDay().GetAwaiter().GetResult();
        IsAdminCanViewVNIP = Viewer.IsAdminCanViewVNIP;
        SRecommendId = Viewer.SRecommendId;
        System = Viewer.System;
    }

    public DateTime Date { get; set; } = SystemTime.Today;

    public int MemberCount { get; set; } = 2;

    public bool IsAdminCanViewVNIP { get; }

    public override bool IsValid() => MinDate <= Date && base.IsValid();

    public int SRecommendId { get; }
    public string System { get; }
}
