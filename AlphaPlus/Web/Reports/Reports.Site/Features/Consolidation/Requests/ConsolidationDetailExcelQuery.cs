using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.Consolidation;

public class ConsolidationDetailExcelQuery : ConsolidationQuery, IRequest<ExcelFileViewModel>
{
}