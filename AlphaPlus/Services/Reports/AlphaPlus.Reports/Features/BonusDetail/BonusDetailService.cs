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

        if (param.ViewerLevel.IsSuperLevel())
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
        else if (param.ViewerLevel.IsMasterLevel())
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
        var masterBonusList = new List<GroupBonus>();
        GroupBonus masterBonus;

        entities
            .GroupBy(b => b.MasterName)
            .ToList()
            .ForEach(grp =>
            {
                masterBonus = GetMasterBonus(grp.Key, grp);
                masterBonusList.Add(masterBonus);

                totalAmount += masterBonus.TotalAmount;
                colspan += masterBonus.ColSpan;
            });

        return new GroupBonus(groupBy, totalAmount, masterBonusList, colspan);
    }

    private static GroupBonus GetMasterBonus(string groupBy, IEnumerable<BonusEntity> entities)
    {
        var colspan = 0;
        var totalAmount = 0M;
        var agentBonusList = new List<GroupBonus>();
        GroupBonus agentBonus;

        entities
            .GroupBy(b => b.AgentName)
            .ToList()
            .ForEach(grp =>
            {
                agentBonus = GetAgentBonus(grp.Key, grp);
                agentBonusList.Add(agentBonus);

                totalAmount += agentBonus.TotalAmount;
                colspan += agentBonus.Bonus.Count;
            });

        return new GroupBonus(groupBy, totalAmount, agentBonusList, colspan);
    }

    private static GroupBonus GetAgentBonus(string groupBy, IEnumerable<BonusEntity> entities)
    {
        var totalAmount = 0M;
        var bonusList = new List<BonusEntity>();

        entities
            .ToList()
            .ForEach(bonus =>
            {
                bonusList.Add(bonus);

                if (!bonus.IsCancelled)
                {
                    totalAmount += bonus.Amount;
                }
            });

        return new GroupBonus(groupBy, totalAmount, bonusList);
    }

    private static PagedReportData<GroupBonus> GetPagedBonusDetail(
        BonusDetailParam param,
        int totalRecords,
        IEnumerable<BonusEntity> rawData,
        Func<string, IEnumerable<BonusEntity>, GroupBonus> getLevelBonus)
    {
        var totalAmount = 0M;
        var bonusList = new List<GroupBonus>();

        rawData.GroupBy(b => b.CreatedDate.Date).ToList().ForEach(group =>
        {
            var bonus = getLevelBonus(group.Key.ToShortDateWithSlash(), group);
            bonusList.Add(bonus);

            totalAmount += bonus.TotalAmount;
        });

        return new PagedReportData<GroupBonus>(bonusList, param.PageIndex, param.PageSize, totalRecords, new GroupBonus(totalAmount));
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