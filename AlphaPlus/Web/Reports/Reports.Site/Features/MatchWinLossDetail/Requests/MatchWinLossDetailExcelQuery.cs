using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.MatchWinLossDetail;

public class MatchWinLossDetailExcelQuery : MatchWinLossDetailQuery, IRequest<ExcelFileViewModel>
{
}