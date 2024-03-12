using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.Statement;

public class StatementDetailExcelQuery : StatementDetailsQuery, IRequest<ExcelFileViewModel>
{
}
