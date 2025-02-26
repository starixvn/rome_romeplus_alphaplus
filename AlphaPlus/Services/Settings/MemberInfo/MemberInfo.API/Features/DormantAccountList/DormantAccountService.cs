using AlphaPlus.Settings.MemberInfo.API.Features.DormantAccountList.Criteria;
using AlphaPlus.Settings.MemberInfo.Domain.Features.DormantAccountList.Models;
using AlphaPlus.Settings.MemberInfo.Domain.Features.DormantAccountList.Params;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Features.DormantAccountList
{
    public interface DormantAccountService
    {
        Task<DormantAccountReport> GetDormantAccountsAsync(DormantAccountParam param);
    }

    public class DormantAccountServiceImpl : DormantAccountService
    {
        private readonly IDynamicRepository dynamicRepository;

        public DormantAccountServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<DormantAccountReport> GetDormantAccountsAsync(DormantAccountParam param)
        {
            var criteria = new GetDormantAccountsCriteria(
                param.CustId, param.Username, param.RoleId, param.StatusId, param.PageIndex, param.PageSize);
            var rawdata = await dynamicRepository.FetchMultipleAsync<DormantAccountEntity, YesterdayBalanceEntity>(criteria);

            var dormantAccounts = new List<DormantAccount>();

            if (rawdata.Item1.Any())
            {
                var yesterdayBalanceHash = rawdata.Item2.ToDictionary(kvp => kvp.CustId, kvp => kvp.YesterdayTotalBalance);

                rawdata.Item1.ToList().ForEach(entity =>
                {
                    var account = new DormantAccount(entity, yesterdayBalanceHash.TryGetValue(entity.CustId, out var yesterdayBalance) ? yesterdayBalance : 0);
                    dormantAccounts.Add(account);
                });
            }

            return new DormantAccountReport(dormantAccounts, criteria.TotalItems);
        }
    }
}
