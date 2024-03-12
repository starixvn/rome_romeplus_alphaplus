using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.IPMatching;

public class IPMatchingQuery : BaseQuery, IRequest<IPMatchingViewModel>
{
    public IPMatchingQuery() : base()
    {
        MinDate = ReportDateService.Instance.GetBalanceUpDay().GetAwaiter().GetResult();
        IsAdminCanViewVNIP = Viewer.IsAdminCanViewVNIP;
    }

    public DateTime Date { get; set; } = DateTime.Today;

    public int MemberCount { get; set; } = 2;

    public bool IsAdminCanViewVNIP { get; }

    public override bool IsValid()
        => base.IsValid() && MinDate <= Date && ViewerLevel.IsSuperLevel();
}
