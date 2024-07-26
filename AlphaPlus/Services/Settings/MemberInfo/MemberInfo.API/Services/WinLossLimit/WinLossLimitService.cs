using AlphaPlus.Settings.MemberInfo.API.Criteria.WinLossLimit;
using AlphaPlus.Settings.MemberInfo.Domain.Models.WinLossLimit;
using AlphaPlus.Settings.MemberInfo.Domain.Params;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Services.WinLossLimit;

public interface WinLossLimitService
{
    Task<IEnumerable<WinLossLimitGroupProductModel>> GetWinLossLimitProducts(int custId);

    Task<WinLossLimitPage> GetWinLossLimitPage(WinLossLimitParam param);
}

public class WinLossLimitServiceImpl(
    IDynamicRepository repository,
    CacheRefService cacheRefService) : WinLossLimitService
{
    private readonly IDynamicRepository repository = repository;
    private readonly Func<int, string> getProductName = cacheRefService.GetProductName;

    public async Task<IEnumerable<WinLossLimitGroupProductModel>> GetWinLossLimitProducts(int custId)
    {
        var winLossLimitProducts = new List<WinLossLimitGroupProductModel>();
        var groupProducts = (await repository
            .FetchAsync<WinLossLimitProductModel>(new GetWinLossLimitProductCriteria(custId)))
            .GroupBy(product => product.Type);

        foreach (var group in groupProducts)
        {
            foreach (var product in group)
            {
                product.ProductName = getProductName(product.ProductId);
            }

            winLossLimitProducts.Add(new WinLossLimitGroupProductModel(
                group.Key == 0 ? string.Empty : getProductName(group.Key),
                group));
        }

        return winLossLimitProducts;
    }

    public async Task<WinLossLimitPage> GetWinLossLimitPage(WinLossLimitParam param)
    {
        var result = new WinLossLimitPage();
        var playerIds = !string.IsNullOrWhiteSpace(param.PlayerId)
            ? await MapPlayerIdToCustId(new MapPlayerId3rdToCustIdCriteria(param.ProductId, param.PlayerId, param.CustId, param.MasterId, param.AgentId))
            : [];

        if (!playerIds.Any() && string.IsNullOrWhiteSpace(param.PlayerId))
        {
            result += await GetWinLossLimits(new GetCustWinLossLimitCriteria(
                param.CustId,
                param.ProductId,
                param.BetType,
                param.SportType,
                param.Username,
                param.MasterId,
                param.AgentId,
                param.PageIndex,
                param.PageSize,
                null));
        }

        foreach (var playerId in playerIds)
        {
            result += await GetWinLossLimits(new GetCustWinLossLimitCriteria(
                param.CustId,
                param.ProductId,
                param.BetType,
                param.SportType,
                param.Username,
                param.MasterId,
                param.AgentId,
                param.PageIndex,
                param.PageSize,
                playerId));
        }

        if (!result.WinLossLimitDetail.Any())
        {
            return result;
        }

        var balances = await repository.FetchAsync<WinLossLimitDetail>(new GetBalanceCriteria(
            param.CustId,
            param.BetType,
            param.Username,
            param.MasterId,
            param.AgentId,
            param.PageIndex,
            param.PageSize), param.AGIP);
        var outstandings = Enumerable.Empty<WinLossLimitDetail>();

        foreach (var custWinLossLimit in result.WinLossLimitDetail)
        {
            var balance = balances.FirstOrDefault(item => item.CustId == custWinLossLimit.CustId);
            if (balance != null)
            {
                custWinLossLimit.CreditBalance = balance.CreditBalance;
                custWinLossLimit.DebitBalance = balance.DebitBalance;
                custWinLossLimit.Balance = balance.Balance;
                custWinLossLimit.Transfer3rd = balance.Transfer3rd;
            }

            var outstanding = outstandings.FirstOrDefault(item => item.CustId == custWinLossLimit.CustId);
            if (outstanding != null)
            {
                custWinLossLimit.Outstanding = outstanding.Outstanding;
            }
        }

        return result;
    }

    private Task<IEnumerable<int>> MapPlayerIdToCustId(MapPlayerId3rdToCustIdCriteria criteria)
        => repository.FetchAsync<int>(criteria);

    private async Task<WinLossLimitPage> GetWinLossLimits(GetCustWinLossLimitCriteria criteria)
    {
        var winLossLimitDetail = await repository.FetchAsync<WinLossLimitDetail>(criteria);

        return new WinLossLimitPage(criteria.TotalRows, winLossLimitDetail);
    }
}
