using AlphaPlus.Settings.MemberInfo.Domain.Extensions;
using AlphaPlus.Settings.MemberInfo.API.Features.MyWatchlist.ReachedThreshold.Criteria;
using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.ReachedThreshold.Models;
using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.ReachedThreshold.Params;
using AlphaPlus.PartialUM.Services;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Features.MyWatchlist.ReachedThreshold
{
    public interface ReachedThresholdService
    {
        Task<ReachlistAccountReport> GetReachlistAccounts(SelectReachListParam param);

        Task<IEnumerable<ReachHistoryModel>> GetReachDetail(SelectReachDetailParam param);
    }

    public class ReachedThresholdServiceImpl : ReachedThresholdService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly IPartialUMService partialUMService;

        public ReachedThresholdServiceImpl(
            IDynamicRepository dynamicRepository,
            IPartialUMService partialUMService)
        {
            this.dynamicRepository = dynamicRepository;
            this.partialUMService = partialUMService;
        }

        public async Task<ReachlistAccountReport> GetReachlistAccounts(SelectReachListParam param)
        {
            var selectReachListByAgCriteria = new SelectReachListByAgCriteria(
                param.AgentId,
                param.FromDate.GetGreaterDate(param.LimitedDateToShowInfo),
                param.ToDate,
                param.LimitType);
            await partialUMService.ThrowIfUM(selectReachListByAgCriteria.GetSettingKey(), param.SiteName);

            await dynamicRepository.ExecuteAsync(selectReachListByAgCriteria);

            var selectReachListCriteria = new SelectReachListCriteria(
                param.AgentId,
                param.Username,
                param.Status,
                param.SortType,
                param.IsDesc,
                param.PageSize,
                param.PageIndex,
                selectReachListByAgCriteria.XMLReachList ?? string.Empty);
            var reachlistAccounts = await dynamicRepository.FetchAsync<ReachlistAccountModel>(selectReachListCriteria);

            return new ReachlistAccountReport(reachlistAccounts, selectReachListCriteria.TotalItems);
        }

        public async Task<IEnumerable<ReachHistoryModel>> GetReachDetail(SelectReachDetailParam param)
        {
            var criteria = new SelectReachDetailCriteria(
                param.CustId,
                param.FromDate.GetGreaterDate(param.LimitedDateToShowInfo),
                param.ToDate,
                param.LimitType,
                param.SortType,
                param.IsDesc);
            await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

            return await dynamicRepository.FetchAsync<ReachHistoryModel>(criteria);
        }
    }
}
