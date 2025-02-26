using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.WinLossAnalysis;

public class WinLossAnalysisExcelQuery : WinLossAnalysisQuery, IRequest<ExcelFileViewModel>
{
}
