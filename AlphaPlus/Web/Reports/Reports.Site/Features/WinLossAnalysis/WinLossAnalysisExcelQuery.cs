using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.WinLossAnalysis;

public class WinLossAnalysisExcelQuery : WinLossAnalysisQuery, IRequest<ExcelFileViewModel>
{
}
