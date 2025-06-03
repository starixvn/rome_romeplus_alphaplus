using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Utils;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using AlphaPlus.Reports.Site.Features.WinLossDetail.ViewModels;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.WinLossDetail.Requests;

public class CashFeeQuery : DefaultDateRangeQuery, IRequest<CashFeeViewModel>
{
    public CashFeeQuery()
        => MinDate = LimitedDateToShowInfo == DateTime.MinValue
        ? SystemTime.Today.AddYears(-3).FirstDayOfYear()
        : LimitedDateToShowInfo;

    public bool IsFilterVisible { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }    

    public int CustId { get; set; }
}
