using AlphaPlus.Core.Utils;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using Fanex.Common.Extensions;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.WhiteLabel;

public class SettlementGetDataQuery : BaseQuery, IRequest<SettlementGetDataViewModel>
{
    public SettlementGetDataQuery()
    {
        MinDate = SystemTime.Today.AddYears(-1).FirstDayOfMonth();
        var lastMonth = SystemTime.Today.AddMonths(-1);
        FromDate = lastMonth.FirstDayOfMonth();
        ToDate = lastMonth.LastDayOfMonth();
    }

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public string WhiteLabels { get; set; }

    public string Username { get; set; }

    public override bool IsValid() => MinDate <= FromDate && ViewerId > 0;
}