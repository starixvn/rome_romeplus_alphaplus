using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.Consolidation;

public class ConsolidationExcelQuery : ConsolidationQuery, IRequest<ExcelFileViewModel>
{
}
