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
            param.CustLevel,
            param.CustId,
            param.FromDate,
            param.ToDate,
            moveBetsDay);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        var total = new CommissionByBetTypeModel();
        var rawdata = await repository.FetchAsync<CommissionByBetTypeModel>(criteria);
        foreach (var item in rawdata)
        {
            total += item;
        }

        return new ReportData<CommissionByBetTypeModel>(rawdata, total);
    }
}
