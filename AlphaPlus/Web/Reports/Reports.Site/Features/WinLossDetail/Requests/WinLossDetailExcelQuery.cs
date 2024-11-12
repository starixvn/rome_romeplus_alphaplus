using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.WinLossDetail;

public class WinLossDetailExcelQuery : WinLossDetailQuery, IRequest<ExcelFileViewModel>
{
    public bool ShowFullName { get; set; }
}