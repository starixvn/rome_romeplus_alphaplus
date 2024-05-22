using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.EnteredResult;

public class EnteredResultExcelQuery : BaseQuery, IRequest<ExcelFileViewModel>
{
}