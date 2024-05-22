using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.Statement;

public class SMAStatementExcelQuery : SMAStatementQuery, IRequest<ExcelFileViewModel>
{
}
