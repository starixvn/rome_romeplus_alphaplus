using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.WinLossDetail;

public class WinLossDetailExcelNewQuery : WinLossDetailNewQuery, IRequest<ExcelFileViewModel>
{
    public bool ShowFullName { get; set; }
}