using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.Consolidation;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.Consolidation;

public class ConsolidationExcelQuery : ConsolidationQuery, IRequest<ExcelFileViewModel>
{
    public override bool IsValid()
        => ConsolidationReportType.IsValidReportType(ReportType) && base.IsValid();
}
