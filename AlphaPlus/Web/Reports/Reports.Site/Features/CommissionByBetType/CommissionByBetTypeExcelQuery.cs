using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.CommissionByBetType;

public class CommissionByBetTypeExcelQuery : CommissionByBetTypeQuery, IRequest<ExcelFileViewModel>
{
}