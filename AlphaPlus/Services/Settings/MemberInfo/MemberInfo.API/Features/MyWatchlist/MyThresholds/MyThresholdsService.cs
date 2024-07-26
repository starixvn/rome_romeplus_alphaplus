namespace AlphaPlus.Settings.MemberInfo.API.Features.MyWatchlist.MyThresholds
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AlphaPlus.Settings.MemberInfo.API.Features.MyWatchlist.MyThresholds.Criteria;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyThresholds.Params;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Models;
    using AlphaPlus.PartialUM.Services;
    using Fanex.Data.Repository;
    using Fanex.Logging.Extensions.Web;
    using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
    using AlphaPlus.Settings.MemberInfo.Domain.Extensions;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.ReachedThreshold.Models;
    using AlphaPlus.Core.Models;

    public interface MyThresholdsService
    {
        Task<IEnumerable<ReachHistoryModel>> GetMyThresholdHistory(SelectMyThresholdHistoryParam param);

        Task<GlobalThresholdModel> GetMyThreshold(int custId, string siteName, short sportType = 0);

        Task<Result> UpdateMyThreshold(UpdateMyThresholdParam param);
    }

    public class MyThresholdsServiceImpl : MyThresholdsService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly IPartialUMService partialUMService;

        public MyThresholdsServiceImpl(
            IDynamicRepository dynamicRepository,
            IPartialUMService partialUMService)
        {
            this.dynamicRepository = dynamicRepository;
            this.partialUMService = partialUMService;
        }

        public Task<IEnumerable<ReachHistoryModel>> GetMyThresholdHistory(SelectMyThresholdHistoryParam param)
        {
            var criteria = new SelectMyThresholdHistoryCriteria(
                param.CustId,
                param.SportType,
                param.FromDate.GetGreaterDate(param.LimitedDateToShowInfo),
                param.ToDate,
                param.SortType,
                param.IsDesc);
            partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

            return dynamicRepository.FetchAsync<ReachHistoryModel>(criteria);
        }

        public Task<GlobalThresholdModel> GetMyThreshold(int custId, string siteName, short sportType = 0)
        {
            var criteria = new SelectMyThresholdCriteria(custId, sportType);
            partialUMService.ThrowIfUM(criteria.GetSettingKey(), siteName);

            return dynamicRepository.GetAsync<GlobalThresholdModel>(criteria);
        }

        public async Task<Result> UpdateMyThreshold(UpdateMyThresholdParam param)
        {
            var command = new UpdateMyThresholdCommand(
                param.CustId,
                param.SportType,
                param.WinningThreshold,
                param.LosingThreshold,
                param.UserId,
                param.SubAccId,
                param.IP);
            await partialUMService.ThrowIfUM(command.GetSettingKey(), param.SiteName);

            await dynamicRepository.ExecuteAsync(command);
            var result = new Result { Code = command.ErrCode, Message = command.ErrMsg ?? string.Empty };

            if (result.IsSuccessful)
            {
                result.Message = Resources.WatchlistUpdateSuccessfully;

                SyncUpdateMyThreshold(command.CustList ?? string.Empty);
            }

            return result;
        }

        private void SyncUpdateMyThreshold(string custList)
        {
            if (!string.IsNullOrWhiteSpace(custList))
            {
                const string DB052 = "DB052";
                Task.Run(() => SyncUpdateMyThreshold(custList, DB052)).ConfigureAwait(false);

                const string DB053 = "DB053";
                Task.Run(() => SyncUpdateMyThreshold(custList, DB053)).ConfigureAwait(false);
            }
        }

        private async Task SyncUpdateMyThreshold(string custList, string suffix)
        {
            try
            {
                await dynamicRepository.ExecuteAsync(new SyncUpdateMyThresholdCommand(custList, suffix));
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
        }
    }
}
