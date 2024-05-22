using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.CommissionByBetType;

public class CommissionByBetTypeExcelQuery : CommissionByBetTypeQuery, IRequest<ExcelFileViewModel>
{
}