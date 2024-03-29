using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.MatchWinLossDetail;

public class MatchWinLossDetailExcelQuery : MatchWinLossDetailQuery, IRequest<ExcelFileViewModel>
{
}