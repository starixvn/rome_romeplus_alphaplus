using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.WinLossDetail.Requests;

public class CashFeeExcelQuery : CashFeeQuery, IRequest<ExcelFileViewModel>
{
}
