using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.CommissionByBetType;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared;
using MediatR;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Reports.Site.Features.CommissionByBetType;

public class CommissionByBetTypeHandler(
    IProductService productService,
    IOptions<AppSettings> options,
    ICommissionByBetTypeService commissionByBetTypeService,
    ICommissionByBetTypeWorkbook workbook) :
        BaseHandler(productService, options),
        IRequestHandler<CommissionByBetTypeQuery, CommissionByBetTypeViewModel>,
        IRequestHandler<CommissionByBetTypeExcelQuery, ExcelFileViewModel>
{
    public async Task<CommissionByBetTypeViewModel> Handle(CommissionByBetTypeQuery message, CancellationToken cancellationToken)
    {
        var reportData = await GetCommissionByBetTypes(message);

        return new CommissionByBetTypeViewModel(message, reportData, BuildBreadcrumbViewModel(message));
    }

    public async Task<ExcelFileViewModel> Handle(CommissionByBetTypeExcelQuery message, CancellationToken cancellationToken)
    {
        var reportData = await GetCommissionByBetTypes(message);

        return workbook.BuildExcelFile(message, reportData);
    }

    private Task<ReportData<CommissionByBetTypeModel>> GetCommissionByBetTypes(CommissionByBetTypeQuery message)
    {
        var param = new CommissionByBetTypeParam(
            message.CustLevel,
            message.CustId,
            message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
            message.ToDate,
            message.Site);

        return commissionByBetTypeService.GetCommissionByBetTypes(param);
    }
}
