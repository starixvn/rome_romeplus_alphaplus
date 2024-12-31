using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.WhiteLabel;

public class SettlementExcelQuery : SettlementQuery, IRequest<ExcelFileViewModel>
{
}