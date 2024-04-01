using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.SuperWinLossDetailByPT;

public class SuperWinLossDetailByPTExcelQuery : SuperWinLossDetailByPTQuery, IRequest<ExcelFileViewModel>
{
}