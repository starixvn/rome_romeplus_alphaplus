using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.Shared.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.Outstanding;

public interface IOutstandingService
{
    Task<ReportData<OutstandingModel>> GetOutstanding(OutstandingParam param);
}

public class OutstandingService(
    IDynamicRepository repository,
    IPartialUMService partialUMService,
    IAutoFollowBetService autoFollowBetService) : IOutstandingService
{
    private readonly IDynamicRepository repository = repository;
    private readonly IPartialUMService partialUMService = partialUMService;
    private readonly IAutoFollowBetService autoFollowBetService = autoFollowBetService;

    public async Task<ReportData<OutstandingModel>> GetOutstanding(OutstandingParam param)
    {
        var criteria = new OutstandingCriteria(param.CustLevelId, param.CustId, param.ProductIds);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        var total = new OutstandingModel();
        var rawdata = param.AutoFollowBetIsDummyAgent
            ? await GetOutstanding(param.ProductIds, param.CustId)
            : await repository.FetchAsync<OutstandingModel>(criteria);
        foreach (var item in rawdata)
        {
            total += item;
        }

        return new ReportData<OutstandingModel>(rawdata, total);
    }

    private async Task<IEnumerable<OutstandingModel>> GetOutstanding(string productIds, int custId)
    {
        var outstandingModels = new List<OutstandingModel>();
        var followBetAccounts = await autoFollowBetService.GetFollowBetAccounts(custId);

        foreach (var item in followBetAccounts)
        {
            outstandingModels.AddRange(await GetOutstanding(productIds, item));
        }

        return outstandingModels;
    }

    private async Task<IEnumerable<OutstandingModel>> GetOutstanding(
        string productIds,
        Tuple<int, IEnumerable<FollowBetAccount>> followBetAccount)
    {
        var (agentId, followBetAccountEntities) = followBetAccount;
        var outstandingModels = await repository.FetchAsync<OutstandingModel>(
            new OutstandingCriteria(RoleIdConstants.Agent, agentId, productIds));

        return outstandingModels.Join(
            followBetAccountEntities,
            outstanding => outstanding.CustId, account => account.CustId,
            (outstanding, account) =>
            {
                outstanding.CustId = account.DummyCustId;
                outstanding.CustName = account.DummyUsername;
                return outstanding;
            });
    }
}
