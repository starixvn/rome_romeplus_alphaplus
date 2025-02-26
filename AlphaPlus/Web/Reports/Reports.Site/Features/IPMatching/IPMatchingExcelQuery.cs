using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.IPMatching;

public class IPMatchingExcelQuery : IPMatchingQuery, IRequest<ExcelFileViewModel>
{
}
