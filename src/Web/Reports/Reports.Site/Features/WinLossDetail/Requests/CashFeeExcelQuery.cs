using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.WinLossDetail.Requests;

public class CashFeeExcelQuery : DefaultDateRangeQuery, IRequest<ExcelFileViewModel>
{
    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }    

    public int CustId { get; set; }
}
