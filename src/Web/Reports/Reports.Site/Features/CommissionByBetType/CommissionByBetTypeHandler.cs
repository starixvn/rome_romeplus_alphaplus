using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.CommissionByBetType;
using AlphaPlus.Reports.Features.Shared.Models;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.CommissionByBetType;

public class CommissionByBetTypeHandler(
    ICommissionByBetTypeService commissionByBetTypeService,
    ICommissionByBetTypeWorkbook workbook) :
        IRequestHandler<CommissionByBetTypeQuery, CommissionByBetTypeViewModel>,
        IRequestHandler<CommissionByBetTypeExcelQuery, ExcelFileViewModel>
{
    private readonly ICommissionByBetTypeService commissionByBetTypeService = commissionByBetTypeService;
    private readonly ICommissionByBetTypeWorkbook workbook = workbook;

    public async Task<CommissionByBetTypeViewModel> Handle(CommissionByBetTypeQuery message, CancellationToken cancellationToken)
    {
        var reportData = await GetCommissionByBetTypes(message);

        return new CommissionByBetTypeViewModel(message, reportData);
    }

    public async Task<ExcelFileViewModel> Handle(CommissionByBetTypeExcelQuery message, CancellationToken cancellationToken)
    {
        var reportData = await GetCommissionByBetTypes(message);

        return workbook.BuildExcelFile(message, reportData);
    }

    private Task<ReportData<CommissionByBetTypeModel>> GetCommissionByBetTypes(CommissionByBetTypeQuery message)
    {
        var param = new CommissionByBetTypeParam(
            message.CustLevelId,
            message.CustId,
            message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
            message.ToDate,
            message.LevelType,
            message.Site);

        return commissionByBetTypeService.GetCommissionByBetTypes(param);
    }
}
