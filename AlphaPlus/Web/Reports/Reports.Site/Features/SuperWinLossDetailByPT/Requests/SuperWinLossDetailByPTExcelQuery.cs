using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.SuperWinLossDetailByPT;

public class SuperWinLossDetailByPTExcelQuery : SuperWinLossDetailByPTQuery, IRequest<ExcelFileViewModel>
{
}