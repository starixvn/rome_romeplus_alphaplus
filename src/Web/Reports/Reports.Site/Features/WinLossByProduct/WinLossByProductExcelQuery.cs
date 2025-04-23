using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.WinLossByProduct;

public class WinLossByProductExcelQuery : WinLossByProductQuery, IRequest<ExcelFileViewModel>
{

}