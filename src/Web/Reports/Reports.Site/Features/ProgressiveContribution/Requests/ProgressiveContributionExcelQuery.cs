using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.ProgressiveContribution;

public class ProgressiveContributionExcelQuery : ProgressiveContributionQuery, IRequest<ExcelFileViewModel>
{
}
