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
    private readonly IDynamicRepository repository = repository;
    private readonly IPartialUMService partialUMService = partialUMService;
    private readonly IReportDateService reportDateService = reportDateService;

    public async Task<ReportData<CommissionByBetTypeModel>> GetCommissionByBetTypes(CommissionByBetTypeParam param)
    {
        var criteria = await GetCommissionByBettypeCriteria(param);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        var rawData = await repository.FetchAsync<CommissionByBetTypeModel>(criteria);

        return new ReportData<CommissionByBetTypeModel>(rawData, CalculateTotal(rawData));
    }

    private async Task<ICriteria> GetCommissionByBettypeCriteria(CommissionByBetTypeParam param)
    {
        var moveBetsDay = await reportDateService.GetMoveBetsDay();

        if (RoleIdConstants.IsAgentLevel(param.CustLevelId))
        {
            return new CommissionByBetTypeMemberCriteria(
                param.CustId,
                param.FromDate,
                param.ToDate,
                moveBetsDay);
        }

        if (RoleIdConstants.IsMasterLevel(param.CustLevelId))
        {
            return new CommissionByBetTypeAgentCriteria(
                param.CustId,
                param.FromDate,
                param.ToDate,
                moveBetsDay,
                param.LevelType);
        }

        return new CommissionByBetTypeMasterCriteria(
            param.CustId,
            param.FromDate,
            param.ToDate,
            moveBetsDay,
            param.LevelType);
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
