namespace AlphaPlus.Settings.MemberInfo.API.Services.WinLossLimit;

using System.Globalization;
using AlphaPlus.Settings.MemberInfo.API.Criteria.WinLossLimit;
using AlphaPlus.Settings.MemberInfo.Domain.Models.WinLossLimit;
using Core.Models;
using Domain.Params;
using Fanex.Data.Repository;

public interface IPotentialWinLimitService
{
    Task<PotentialWinLimitPage> GetPotentialWinLimitPage(PotentialWinLimitParam param, bool isGetAll = false);

    Task<Result> ResetCurrentSportsbookWinAmount(ResetCurrentSportsbookWinAmountParam param);
}

public class PotentialWinLimitService(IDynamicRepository repository) : IPotentialWinLimitService
{
    public async Task<PotentialWinLimitPage> GetPotentialWinLimitPage(PotentialWinLimitParam param, bool isGetAll = false)
    {
        var criteria = new GetPotentialWinLimitCriteria
        {
            CustId = param.CustId,
            RoleId = param.RoleId,
            Username = param.Username,
            Status = param.Status,
            ProblemAccount = param.ProblemAccount,
            PageSize = param.PageSize,
            PageIndex = param.PageIndex,
            IsGetAll = isGetAll,
            AccountType = param.AccountType,
        };
        var potentialLimitDetail = await repository.FetchAsync<PotentialWinLimitDetail>(criteria);
        var custIds = string.Join(",", potentialLimitDetail.Select(x => x.CustId.ToString()));

        var potentialWinLimitAmountCriteria = new GetPotentialWinLimitAmountCriteria(custIds, param.SiteDbGroupId);
        var potentialWinLimitAmount = await repository.FetchAsync<PotentialWinLimitDetail>(potentialWinLimitAmountCriteria);

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

    public async Task<Result> ResetCurrentSportsbookWinAmount(ResetCurrentSportsbookWinAmountParam param)
    {
        var checkResetCurrentSportsbookWinAmountCriteria = new CheckResetCurrentSportsbookWinAmountCriteria(param);
        await repository.ExecuteAsync(checkResetCurrentSportsbookWinAmountCriteria);
        if (checkResetCurrentSportsbookWinAmountCriteria.ErrCode != 0)
        {
            return new Result
            {
                Code = checkResetCurrentSportsbookWinAmountCriteria.ErrCode,
                Message = Shared.AppResources.Common.CommonResx.ResourceManager.GetString("ErrorMessage_" + checkResetCurrentSportsbookWinAmountCriteria.ErrCode, CultureInfo.CurrentCulture)
            };
        }
        var criteria = new ResetCurrentSportsbookWinAmountCriteria(
                    param.CustId,
                    param.SubAccId,
                    param.SubAccName,
                    param.UserId,
                    param.IP,
                    param.SettingStatus,
                    param.SiteDB);

        await repository.ExecuteAsync(criteria);

        return new Result { Code = 0, Message = string.Empty };
    }
}