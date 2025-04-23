using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
using AlphaPlus.Settings.MemberInfo.API.Services;
using AlphaPlus.Settings.MemberInfo.Domain.Constants;
using AlphaPlus.Settings.MemberInfo.Domain.Features.Commission;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Features.Commission;

public interface ICommissionService
{
    Task<CommissionReport> GetMasterComm(CommissionParam param);

    Task<CommissionReport> GetAgentComm(CommissionParam param);
}

public class CommissionService(IDynamicRepository repository, CacheRefService cacheRefService) : ICommissionService
{
    public Task<CommissionReport> GetMasterComm(CommissionParam param)
    {
        var criteria = new GetMasterCommCriteria(param.ProductId, param.CustId, param.SettingStatus, param.IsDirectMember);

        return GetCommission(criteria, param.ProductId);
    }

    public Task<CommissionReport> GetAgentComm(CommissionParam param)
    {
        var criteria = new GetAgentCommCriteria(param.ProductId, param.CustId, param.SettingStatus, param.IsDirectMember);

        return GetCommission(criteria, param.ProductId);
    }

    private async Task<CommissionReport> GetCommission(ICriteria criteria, int productId)
    {
        var rawData = await repository.FetchAsync<CommissionEntity>(criteria);

        return productId == ProductId.Sportsbook
            ? BuildCommissionModels(rawData)
            : BuildDCSCommissionModels(rawData);
    }

    private CommissionReport BuildCommissionModels(IEnumerable<CommissionEntity> commissions)
    {
        var commissionModels = new List<CommissionModel>();
        var sportTypes = new List<string> { $"{Resources.Comm} 1(%)", $"{Resources.Comm} 2(%)", $"{Resources.Comm} 3(%)" };

        foreach (var item in commissions)
        {
            var sportCommModels = new List<SportCommissionModel>
            {
                new(item.ADiscount, item.Discount, item.PlayerDiscount),
                new(item.ADiscount1X2, item.Discount1X2, item.PlayerDiscount1X2),
                new(item.ADiscountCS, item.DiscountCS, item.PlayerDiscountCS)
            };

            commissionModels.Add(BuildCommissionModel(item, sportCommModels));
        }

        return new CommissionReport(sportTypes, commissionModels);
    }

    private CommissionReport BuildDCSCommissionModels(IEnumerable<CommissionEntity> commissions)
    {
        var firstGroup = true;
        var sportTypes = new List<string>();

        var commissionModels = commissions.GroupBy(
            c => c.CustId,
            (custId, commList) =>
            {
                var firstItem = true;
                CommissionModel commissionModel = null;
                var sportCommissions = new List<SportCommissionModel>();

                foreach (var item in commList.OrderBy(c => c.SportType))
                {
                    if (firstItem)
                    {
                        commissionModel = BuildCommissionModel(item, sportCommissions);
                        firstItem = false;
                    }

                    if (firstGroup)
                    {
                        sportTypes.Add($"{cacheRefService.GetSportTypeName(item.SportType)} (%)");
                    }

                    sportCommissions.Add(BuildSportCommissionModel(item));
                }

                firstGroup = false;

                return commissionModel;
            });

        return new CommissionReport(sportTypes, commissionModels);
    }

    private static CommissionModel BuildCommissionModel(CommissionEntity entity, IEnumerable<SportCommissionModel> commissions)
        => new(
            entity.Closed,
            entity.Enabled,
            entity.Suspended,
            entity.Username,
            entity.CustId,
            entity.UGroup,
            entity.FirstName,
            entity.LastName,
            entity.Credit,
            entity.Locked,
            entity.ClosedReason,
            entity.UGroup,
            entity.DoubleCommLosing,
            commissions);

    private static SportCommissionModel BuildSportCommissionModel(CommissionEntity entity)
        => new(entity.MasterDiscount, entity.AgentDiscount, entity.PlayerDiscount);
}