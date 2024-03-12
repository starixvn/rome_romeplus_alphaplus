using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.Shared;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.CommissionByBetType;

public interface ICommissionByBetTypeService
{
    Task<IEnumerable<CommissionByBetTypeModel>> GetCommissionByBetTypes(CommissionByBetTypeParam param);
}

public class CommissionByBetTypeService(
    IDynamicRepository repository,
    IPartialUMService partialUMService,
    IReportDateService reportDateService) : ICommissionByBetTypeService
{
    public async Task<IEnumerable<CommissionByBetTypeModel>> GetCommissionByBetTypes(CommissionByBetTypeParam param)
    {
        var moveBetsDay = await reportDateService.GetMoveBetsDay();
        var criteria = new CommissionByBetTypeCriteria(
            param.CustLevel,
            param.CustId,
            param.FromDate,
            param.ToDate,
            moveBetsDay);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        return await repository.FetchAsync<CommissionByBetTypeModel>(criteria);
    }
}
