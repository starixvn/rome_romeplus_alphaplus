using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.IPMatching;

public class IPMatchingExcelQuery : IPMatchingQuery, IRequest<ExcelFileViewModel>
{
}
