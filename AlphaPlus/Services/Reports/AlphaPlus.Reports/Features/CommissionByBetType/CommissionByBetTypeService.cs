using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.CommissionByBetType;

public interface ICommissionByBetTypeService
{
    Task<ReportData<CommissionByBetTypeModel>> GetCommissionByBetTypes(CommissionByBetTypeParam param);
}

public class CommissionByBetTypeService(
    IDynamicRepository repository,
    IPartialUMService partialUMService,
    IReportDateService reportDateService) : ICommissionByBetTypeService
{
    public async Task<ReportData<CommissionByBetTypeModel>> GetCommissionByBetTypes(CommissionByBetTypeParam param)
    {
        var moveBetsDay = await reportDateService.GetMoveBetsDay();
        var criteria = new CommissionByBetTypeCriteria(
            param.CustLevelId,
            param.CustId,
            param.FromDate,
            param.ToDate,
            moveBetsDay);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        var rawData = await repository.FetchAsync<CommissionByBetTypeModel>(criteria);

        return new ReportData<CommissionByBetTypeModel>(rawData, CalculateTotal(rawData));
    }

    private static CommissionByBetTypeModel CalculateTotal(IEnumerable<CommissionByBetTypeModel> rawData)
    {
        var total = new CommissionByBetTypeModel();

        foreach (var item in rawData)
        {
            total += item;
        }

        return total;
    }
}
