using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.Statement;

public class StatementDetailExcelQuery : StatementDetailsQuery, IRequest<ExcelFileViewModel>
{
}
