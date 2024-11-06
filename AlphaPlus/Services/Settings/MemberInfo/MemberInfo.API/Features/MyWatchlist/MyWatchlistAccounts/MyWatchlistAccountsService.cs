using AlphaPlus.Settings.MemberInfo.API.Features.MyWatchlist.MyWatchlistAccounts.Criteria;
using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Models;
using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Params;
using AlphaPlus.PartialUM.Services;
using Fanex.Data.Repository;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.MemberInfo.API.Features.MyWatchlist.MyWatchlistAccounts
{
    public interface MyWatchlistAccountsService
    {
        Task<WatchlistAccountReport> GetWatchlistAccounts(SelectWatchlistAccountsParam param);

        Task<IEnumerable<PersonalPresetModel>> GetPresets(SelectPresetsParam param);

        Task<IEnumerable<NamedResult>> AddPersonalThreshold(AddPersonalThresholdParam param);

        Task<NamedResult> UpdatePersonalThreshold(UpdatePersonalThresholdParam param);

        Task<NamedResult> DeletePersonalThreshold(DeletePersonalThresholdParam param);

        Task<GlobalThresholdModel> GetGlobalThreshold(SelectGlobalThresholdParam param);

        Task<Result> UpdateGlobalThreshold(UpdateGlobalThresholdParam param);

        Task<NamedResult> AddPersonalPreset(AddPersonalPresetParam param);

        Task<Result> UpdatePersonalPreset(UpdatePersonalPresetParam param);

        Task<Result> DeletePersonalPreset(DeletePersonalPresetParam param);

        Task<WatchlistSummaryModel> GetWatchlistSummary(SelectWatchlistSummaryParam param);
    }

    public class MyWatchlistAccountsServiceImpl : MyWatchlistAccountsService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly IPartialUMService partialUMService;

        public MyWatchlistAccountsServiceImpl(
            IDynamicRepository dynamicRepository,
            IPartialUMService partialUMService)
        {
            this.dynamicRepository = dynamicRepository;
            this.partialUMService = partialUMService;
        }

        public async Task<WatchlistAccountReport> GetWatchlistAccounts(SelectWatchlistAccountsParam param)
        {
            var criteria = new SelectWatchlistAccountsCriteria(
                param.AgentId,
                param.FromDate,
                param.ToDate,
                param.Status,
                param.ProblemAccount,
                param.Username,
                param.SortType,
                param.IsDesc,
                param.PageIndex,
                param.PageSize);
            await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

            var watchlistAccounts = await dynamicRepository.FetchAsync<WatchlistAccountModel>(criteria);

            return new WatchlistAccountReport(watchlistAccounts, criteria.TotalItems);
        }

        public async Task<IEnumerable<PersonalPresetModel>> GetPresets(SelectPresetsParam param)
        {
            var criteria = new SelectPresetsCriteria(param.AgentId);
            await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

            return await dynamicRepository.FetchAsync<PersonalPresetModel>(criteria);
        }

        public async Task<IEnumerable<NamedResult>> AddPersonalThreshold(AddPersonalThresholdParam param)
        {
            var criteria = new InsertMultiplePersonalThresholdCriteria(
                param.Usernames,
                param.AgentId,
                param.WinningThreshold,
                param.LosingThreshold,
                param.UserId,
                param.SubAccId,
                param.IP);
            await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

            return await dynamicRepository.FetchAsync<NamedResult>(criteria);
        }

        public async Task<NamedResult> UpdatePersonalThreshold(UpdatePersonalThresholdParam param)
        {
            var criteria = new UpdatePersonalThresholdCommand(
                param.CustId,
                param.WinningThreshold,
                param.LosingThreshold,
                param.UserId,
                param.SubAccId,
                param.IP);
            await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

            await dynamicRepository.ExecuteAsync(criteria);

            return new NamedResult { Name = param.Username, Code = criteria.ErrCode, Message = criteria.ErrMsg ?? string.Empty };
        }

        public async Task<NamedResult> DeletePersonalThreshold(DeletePersonalThresholdParam param)
        {
            var command = new DeletePersonalThresholdCommand(
               param.CustId,
               param.UserId,
               param.SubAccId,
               param.IP);
            await partialUMService.ThrowIfUM(command.GetSettingKey(), param.SiteName);

            await dynamicRepository.ExecuteAsync(command);

            return new NamedResult { Name = param.Username, Code = command.ErrCode, Message = command.ErrMsg ?? string.Empty };
        }

        public async Task<GlobalThresholdModel> GetGlobalThreshold(SelectGlobalThresholdParam param)
        {
            var criteria = new SelectGlobalThresholdCriteria(param.AgentId);
            await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

            return await dynamicRepository.GetAsync<GlobalThresholdModel>(criteria);
        }

        public async Task<Result> UpdateGlobalThreshold(UpdateGlobalThresholdParam param)
        {
            var command = new UpdateGlobalThresholdCommand(
                param.AgentId,
                param.WinningThreshold,
                param.LosingThreshold,
                param.UserId,
                param.SubAccId,
                param.IP);
            await partialUMService.ThrowIfUM(command.GetSettingKey(), param.SiteName);

            await dynamicRepository.ExecuteAsync(command);

            return new Result { Code = command.ErrCode, Message = command.ErrMsg ?? string.Empty };
        }

        public async Task<NamedResult> AddPersonalPreset(AddPersonalPresetParam param)
        {
            var command = new AddPersonalPresetCommand(
                param.AgentId,
                param.PresetTitle,
                param.WinningThreshold,
                param.LosingThreshold);
            await partialUMService.ThrowIfUM(command.GetSettingKey(), param.SiteName);

            await dynamicRepository.ExecuteAsync(command);

            return new NamedResult { Name = param.PresetTitle, Code = command.ErrCode, Message = command.ErrMsg ?? string.Empty };
        }

        public async Task<Result> UpdatePersonalPreset(UpdatePersonalPresetParam param)
        {
            var command = new UpdatePersonalPresetCommand(
                param.AgentId,
                param.ID,
                param.PresetTitle,
                param.WinningThreshold,
                param.LosingThreshold);

            await dynamicRepository.ExecuteAsync(command);

            return new Result { Code = command.ErrCode, Message = command.ErrMsg ?? string.Empty };
        }

        public async Task<Result> DeletePersonalPreset(DeletePersonalPresetParam param)
        {
            var command = new DeletePersonalPresetCommand(
                param.AgentId,
                param.ID);
            await partialUMService.ThrowIfUM(command.GetSettingKey(), param.SiteName);

            await dynamicRepository.ExecuteAsync(command);

            return new Result { Code = command.ErrCode, Message = command.ErrMsg ?? string.Empty };
        }

        public async Task<WatchlistSummaryModel> GetWatchlistSummary(SelectWatchlistSummaryParam param)
        {
            var reachedMemberCountCriteria = new SelectReachedMemberCountCriteria(
                param.AgentId, param.SportType, param.FromDate, param.ToDate, param.LimitType);
            await partialUMService.ThrowIfUM(reachedMemberCountCriteria.GetSettingKey(), param.SiteName);
            await dynamicRepository.ExecuteAsync(reachedMemberCountCriteria);

            var summaryCriteria = new SelectWatchlistSummaryCriteria(
                param.AgentId, reachedMemberCountCriteria.ReachedDailyLimitCount, param.SportType);
            await partialUMService.ThrowIfUM(reachedMemberCountCriteria.GetSettingKey(), param.SiteName);

            return await dynamicRepository.GetAsync<WatchlistSummaryModel>(summaryCriteria);
        }
    }
}
