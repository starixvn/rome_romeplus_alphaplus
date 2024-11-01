using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Shared.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.BonusDetail;

public interface IBonusDetailService
{
    Task<PagedReportData<GroupBonus>> GetBonusDetail(BonusDetailParam param);

    Task<Result> ConfirmPayment(ConfirmPaymentParam param);
}

public class BonusDetailService(IDynamicRepository repository) : IBonusDetailService
{
    public async Task<PagedReportData<GroupBonus>> GetBonusDetail(BonusDetailParam param)
    {
        GetBonusDetailCriteria criteria = null;
        Func<string, IEnumerable<BonusEntity>, GroupBonus> getLevelBonus;

        if (RoleIdConstants.IsSuperLevel(param.ViewerLevelId))
        {
            criteria = new GetSuperBonusCriteria(
                param.Username,
                param.FromDate,
                param.ToDate,
                param.StatusId,
                param.IsComplained,
                param.PageIndex,
                param.PageSize,
                param.ViewerId);
            getLevelBonus = GetSuperBonus;
        }
        else if (RoleIdConstants.IsMasterLevel(param.ViewerLevelId))
        {
            criteria = new GetMasterBonusCriteria(
                param.Username,
                param.FromDate,
                param.ToDate,
                param.StatusId,
                param.IsComplained,
                param.PageIndex,
                param.PageSize,
                param.ViewerId);
            getLevelBonus = GetMasterBonus;
        }
        else
        {
            criteria = new GetAgentBonusCriteria(
                param.Username,
                param.FromDate,
                param.ToDate,
                param.StatusId,
                param.IsComplained,
                param.PageIndex,
                param.PageSize,
                param.ViewerId);
            getLevelBonus = GetAgentBonus;
        }

        var rawData = await repository.FetchAsync<BonusEntity>(criteria);
        return GetPagedBonusDetail(param, criteria.TotalRecords, rawData, getLevelBonus);
    }

    private static GroupBonus GetSuperBonus(string groupBy, IEnumerable<BonusEntity> entities)
    {
        var colspan = 0;
        var totalAmount = 0M;
        var masterBonusList = entities.GroupBy(
            b => b.MasterName,
            (masterName, grp) =>
            {
                var masterBonus = GetMasterBonus(masterName, grp);
                totalAmount += masterBonus.TotalAmount;
                colspan += masterBonus.ColSpan;

                return masterBonus;
            }).ToList();

        return new GroupBonus(groupBy, totalAmount, masterBonusList, colspan);
    }

    private static GroupBonus GetMasterBonus(string groupBy, IEnumerable<BonusEntity> entities)
    {
        var colspan = 0;
        var totalAmount = 0M;
        var agentBonusList = entities.GroupBy(
            b => b.AgentName,
            (agentName, grp) =>
            {
                var agentBonus = GetAgentBonus(agentName, grp);
                totalAmount += agentBonus.TotalAmount;
                colspan += agentBonus.Bonus.Count;

                return agentBonus;
            }).ToList();

        return new GroupBonus(groupBy, totalAmount, agentBonusList, colspan);
    }

    private static GroupBonus GetAgentBonus(string groupBy, IEnumerable<BonusEntity> entities)
    {
        var totalAmount = 0M;
        var bonusList = entities.Select(bonus =>
        {
            if (!bonus.IsCancelled)
            {
                totalAmount += bonus.Amount;
            }

            return bonus;
        }).ToList();

        return new GroupBonus(groupBy, totalAmount, bonusList);
    }

    private static PagedReportData<GroupBonus> GetPagedBonusDetail(
        BonusDetailParam param,
        int totalRecords,
        IEnumerable<BonusEntity> rawData,
        Func<string, IEnumerable<BonusEntity>, GroupBonus> getLevelBonus)
    {
        var totalAmount = 0M;
        var bonusList = rawData.GroupBy(
            b => b.CreatedDate.Date,
            (date, group) =>
            {
                var bonus = getLevelBonus(date.ToShortDateWithSlash(), group);
                totalAmount += bonus.TotalAmount;

                return bonus;
            }).ToList();

        return new PagedReportData<GroupBonus>(bonusList, new GroupBonus(totalAmount), param.PageIndex, param.PageSize, totalRecords);
    }

    public async Task<Result> ConfirmPayment(ConfirmPaymentParam param)
    {
        var criteria = new ConfirmPaymentCriteria(
            param.CustId,
            param.SubAccId,
            param.TransId,
            param.PaymentDate,
            param.ViewerId,
            param.LoginIP,
            param.Language);
        await repository.ExecuteAsync(criteria);

        return new Result(criteria.ErrCode, criteria.ErrMsg);
    }
}