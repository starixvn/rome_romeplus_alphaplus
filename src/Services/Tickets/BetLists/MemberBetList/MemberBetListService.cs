using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.BetLists.Shared.Services;
using AlphaPlus.IPInfo.Services;
using AlphaPlus.PartialUM.Services;
using AlphaPlus.Utilities.IP;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.MemberBetList;

public interface IMemberBetListService
{
    Task<BetListData> GetBetList(MemberBetListParam param);
}

public class MemberBetListService(
    IPartialUMService partialUMService,
    IDynamicRepository repository,
    IIPInfoService ipInfoService,
    IPService ipService) : BetListServiceBase(repository, ipInfoService, ipService), IMemberBetListService
{
    public async Task<BetListData> GetBetList(MemberBetListParam param)
    {
        var criteria = new GetBetListAllByDateCriteria(
            param.CustId,
            param.ProductIds,
            param.FromDate,
            param.ToDate,
            param.UserName);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        return await GetBetList(criteria, param.DisplayVNIP, param.SuperId);
    }
}