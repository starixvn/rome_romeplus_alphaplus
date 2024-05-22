using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.Statement;

public class MemberStatementExcelQuery : MemberStatementQuery, IRequest<ExcelFileViewModel>
{
}
