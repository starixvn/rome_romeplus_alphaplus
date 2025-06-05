using AlphaPlus.Core.Extensions;
using AlphaPlus.Settings.MemberInfo.API.Criteria.WinLossLimit;
using AlphaPlus.Settings.MemberInfo.Domain.Models.WinLossLimit;
using AlphaPlus.Settings.MemberInfo.Domain.Params;
using Fanex.Data.Repository;
using System.Data.SqlTypes;

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
            MinDate = SqlDateTime.MinValue.Value.GetGreaterDate(param.MinDate),
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
            .GroupJoin(potentialWinLimitAmount,
                x => x.CustId,
                y => y.CustId,
                (x, y) => new { x, y })
            .SelectMany(
                xy => xy.y.DefaultIfEmpty(),
                (xy, y) =>
                {
                    xy.x.AccumWinLoss = y?.AccumWinLoss ?? 0;
                    xy.x.AccumToWin = y?.AccumToWin ?? 0;
                    return xy.x;
                })
            .ToList();

        return new PotentialWinLimitPage(criteria.TotalRows, potentialWinLimitDetail);
    }
}
