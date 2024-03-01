using AlphaPlus.Reports.Site.Features.Shared.Requests;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.EnteredResult;

public class EnteredResultExcelQuery : BaseQuery, IRequest<ExcelFileViewModel>
{
}