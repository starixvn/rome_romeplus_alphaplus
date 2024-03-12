using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.Statement;

public class SMAStatementExcelQuery : SMAStatementQuery, IRequest<ExcelFileViewModel>
{
}
