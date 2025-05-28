using AlphaPlus.Settings.MemberInfo.API.Criteria.WinLossLimit;
using AlphaPlus.Settings.MemberInfo.Domain.Models.WinLossLimit;
using AlphaPlus.Settings.MemberInfo.Domain.Params;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Services.WinLossLimit;

public interface IPotentialWinLimitService
{
    Task<PotentialWinLimitPage> GetPotentialWinLimitPage(PotentialWinLimitParam param, bool isGetAll = false);
}

public class PotentialWinLimitService : IPotentialWinLimitService
{
    private readonly IDynamicRepository _repository;

    public PotentialWinLimitService(IDynamicRepository repository)
    {
        _repository = repository;
    }

    public async Task<PotentialWinLimitPage> GetPotentialWinLimitPage(PotentialWinLimitParam param, bool isGetAll = false)
    {
        var criteria = new GetPotentialWinLimitCriteria
        {
            Username = param.Username,
            Status = param.Status,
            ProblemAccount = param.ProblemAccount,
            PageSize = param.PageSize,
            PageIndex = param.PageIndex,
            IsGetAll = isGetAll,
        };
        var winLossLimitDetail = await _repository.FetchAsync<PotentialWinLimitDetail>(criteria);
        var custIds = string.Join(",", winLossLimitDetail.Select(x => x.CustId.ToString()));

        var potentialWinLimitAmountCriteria = new GetPotentialWinLimitAmountCriteria
        {
            CustIds = custIds,
        };
        var potentialWinLimitAmount = await _repository.FetchAsync<PotentialWinLimitDetail>(potentialWinLimitAmountCriteria, param.AGIP);
        
        var potentialWinLimitDetail = winLossLimitDetail
            .Join(potentialWinLimitAmount, 
                x => x.CustId, 
                y => y.CustId, 
                (x, y) =>
                {
                    x.AccumWinLoss = y.AccumWinLoss;
                    x.AccumToWin = y.AccumToWin;
                    return x;
                })
            .ToList();

        return new PotentialWinLimitPage(criteria.TotalRows, potentialWinLimitDetail);
    }
}
