using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.WinLossByProduct;

public class WinLossByProductExcelQuery : WinLossByProductQuery, IRequest<ExcelFileViewModel>
{

}