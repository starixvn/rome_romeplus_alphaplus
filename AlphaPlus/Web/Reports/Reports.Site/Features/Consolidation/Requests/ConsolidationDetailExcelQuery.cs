using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.Consolidation;

public class ConsolidationDetailExcelQuery : ConsolidationQuery, IRequest<ExcelFileViewModel>
{
}