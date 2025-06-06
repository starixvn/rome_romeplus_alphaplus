using AlphaPlus.Core.Extensions;
using AlphaPlus.Settings.MemberInfo.API.Criteria.WinLossLimit;
using AlphaPlus.Settings.MemberInfo.Domain.Models.WinLossLimit;
using AlphaPlus.Settings.MemberInfo.Domain.Params;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Services.WinLossLimit;

public interface IPotentialWinLimitService
{
    Task<PotentialWinLimitPage> GetPotentialWinLimitPage(PotentialWinLimitParam param, bool isGetAll = false);
}

public class PotentialWinLimitService(IDynamicRepository repository) : IPotentialWinLimitService
{
    private readonly IDynamicRepository _repository = repository;

    public async Task<PotentialWinLimitPage> GetPotentialWinLimitPage(PotentialWinLimitParam param, bool isGetAll = false)
    {
        var criteria = new GetPotentialWinLimitCriteria
        {
            AgentId = param.CustId,
            Username = param.Username,
            Status = param.Status,
            ProblemAccount = param.ProblemAccount,
            PageSize = param.PageSize,
            PageIndex = param.PageIndex,
            IsGetAll = isGetAll,
        };
        var potentialLimitDetail = await _repository.FetchAsync<PotentialWinLimitDetail>(criteria);
        var custIds = string.Join(",", potentialLimitDetail.Select(x => x.CustId.ToString()));

        var potentialWinLimitAmountCriteria = new GetPotentialWinLimitAmountCriteria
        {
            CustIds = custIds,
        };
        var potentialWinLimitAmount = await _repository.FetchAsync<PotentialWinLimitDetail>(potentialWinLimitAmountCriteria, param.AGIP);
        
        var potentialWinLimitDetail = (from x in potentialLimitDetail
                                     join y in potentialWinLimitAmount on x.CustId equals y.CustId into joined
                                     from y in joined.DefaultIfEmpty()
                                     select new
                                     {
                                         x,
                                         AccumWinLoss = y?.AccumWinLoss ?? 0,
                                         AccumToWin = y?.AccumToWin ?? 0
                                     })
                                     .Select(result =>
                                     {
                                         result.x.AccumWinLoss = result.AccumWinLoss;
                                         result.x.AccumToWin = result.AccumToWin;
                                         return result.x;
                                     })
                                     .ToList();

        return new PotentialWinLimitPage(criteria.TotalRows, potentialWinLimitDetail);
    }
}
