using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.ProgressiveContribution;

public class ProgressiveContributionExcelQuery : ProgressiveContributionQuery, IRequest<ExcelFileViewModel>
{
}
