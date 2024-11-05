using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.Shared;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.ProgressiveContribution;

public interface IProgressiveContributionService
{
    Task<(IEnumerable<ProgressiveContributionModel>, ProgressiveContributionEntity)> GetProgressiveContributions(ProgressiveContributionParam param);
}

public class ProgressiveContributionService(
    IDynamicRepository repository,
    IPartialUMService partialUMService,
    ICacheRefService cacheRefService,
    IReportDateService reportDateService) : IProgressiveContributionService
{
    private readonly IDynamicRepository repository = repository;
    private readonly IPartialUMService partialUMService = partialUMService;
    private readonly ICacheRefService cacheRefService = cacheRefService;
    private readonly IReportDateService reportDateService = reportDateService;

    public async Task<(IEnumerable<ProgressiveContributionModel>, ProgressiveContributionEntity)> GetProgressiveContributions(ProgressiveContributionParam param)
    {
        var models = new List<ProgressiveContributionModel>();
        var grandTotal = new ProgressiveContributionEntity();
        var rawdata = await GetRawData(param);

        if (rawdata.Any())
        {
            var contributionByProducts = rawdata.GroupBy(contribution => contribution.ProductId);
            foreach (var contribution in contributionByProducts)
            {
                var total = new ProgressiveContributionEntity();
                foreach (var item in contribution)
                {
                    total += item;
                    item.BetTypeName = cacheRefService.GetBetTypeName(item.BetType);
                }

                grandTotal += total;
                models.Add(new ProgressiveContributionModel(
                    contribution.Key,
                    cacheRefService.GetProductName(contribution.Key),
                    contribution,
                    total));
            }
        }

        return (models, grandTotal);
    }

    private async Task<IEnumerable<ProgressiveContributionEntity>> GetRawData(ProgressiveContributionParam param)
    {
        var reportDate = await reportDateService.GetBalanceUpDayAndMoveBetsDay();
        var criteria = new ProgressiveContributionCriteria(
            param.CustId,
            param.FromDate,
            param.ToDate,
            reportDate.BalanceUpDay,
            reportDate.MoveBetsDay,
            param.ProductIds,
            param.ViewerLevelId);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        return await repository.FetchAsync<ProgressiveContributionEntity>(criteria);
    }
}