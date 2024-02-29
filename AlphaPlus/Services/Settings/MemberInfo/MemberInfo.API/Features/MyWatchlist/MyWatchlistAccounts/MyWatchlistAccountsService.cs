namespace AlphaPlus.Settings.MemberInfo.API.Features.MyWatchlist.MyWatchlistAccounts
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AlphaPlus.Settings.MemberInfo.API.Features.MyWatchlist.MyWatchlistAccounts.Criteria;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Models;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Params;
    using AlphaPlus.PartialUM.Services;
    using Fanex.Data.Repository;
    using AlphaPlus.Core.Models;

    public interface MyWatchlistAccountsService
    {
        Task<WatchlistAccountReport> GetWatchlistAccounts(SelectWatchlistAccountsParam param);

        Task<IEnumerable<PersonalPresetModel>> GetPresets(SelectPresetsParam param);

        Task<MultipleWatchlistAccountModel> GetMultipleWatchlistAccount(SelectMultipleWatchlistParam param);

        Task<IEnumerable<NamedResult>> AddPersonalThreshold(AddPersonalThresholdParam param);

        Task<NamedResult> UpdatePersonalThreshold(UpdatePersonalThresholdParam param);

        Task<NamedResult> DeletePersonalThreshold(DeletePersonalThresholdParam param);

        Task<GlobalThresholdModel> GetGlobalThreshold(SelectGlobalThresholdParam param);

        Task<Result> UpdateGlobalThreshold(UpdateGlobalThresholdParam param);

        Task<NamedResult> AddPersonalPreset(AddPersonalPresetParam param);

        Task<Result> UpdatePersonalPreset(UpdatePersonalPresetParam param);

        Task<Result> DeletePersonalPreset(DeletePersonalPresetParam param);
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
                param.Username,
                param.SortType,
                param.IsDesc,
                param.PageIndex,
                param.PageSize);
            partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

            var watchlistAccounts = await dynamicRepository.FetchAsync<WatchlistAccountModel>(criteria);

            return new WatchlistAccountReport(watchlistAccounts, criteria.TotalItems);
        }

        public async Task<IEnumerable<PersonalPresetModel>> GetPresets(SelectPresetsParam param)
        {
            var criteria = new SelectPresetsCriteria(param.AgentId);
            partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

            return await dynamicRepository.FetchAsync<PersonalPresetModel>(criteria);
        }

        public async Task<MultipleWatchlistAccountModel> GetMultipleWatchlistAccount(SelectMultipleWatchlistParam param)
        {
            var criteria = new SelectMultipleWatchlistCriteria(param.CustList);
            partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);
            var rawdata = await dynamicRepository.FetchMultipleAsync<WatchlistAccountModel, WatchlistAccountModel>(criteria);
            var alreadyAddedAccounts = new List<WatchlistAccountModel>();
            var newlyAddedAccounts = new List<WatchlistAccountModel>();
            if (rawdata.Item1.Any())
            {
                alreadyAddedAccounts.AddRange(rawdata.Item1);
            }
            if (rawdata.Item2.Any())
            {
                newlyAddedAccounts.AddRange(rawdata.Item2);
            }
            return new MultipleWatchlistAccountModel(alreadyAddedAccounts, newlyAddedAccounts);
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
            partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

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
            partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

            await dynamicRepository.ExecuteAsync(criteria);

            return new NamedResult { Name = param.Username, Code = criteria.ErrCode, Message = criteria.ErrMsg };
        }

        public async Task<NamedResult> DeletePersonalThreshold(DeletePersonalThresholdParam param)
        {
            var command = new DeletePersonalThresholdCommand(
               param.CustId,
               param.UserId,
               param.SubAccId,
               param.IP);
            partialUMService.ThrowIfUM(command.GetSettingKey(), param.SiteName);

            await dynamicRepository.ExecuteAsync(command);

            return new NamedResult { Name = param.Username, Code = command.ErrCode, Message = command.ErrMsg };
        }

        public async Task<GlobalThresholdModel> GetGlobalThreshold(SelectGlobalThresholdParam param)
        {
            var criteria = new SelectGlobalThresholdCriteria(param.AgentId);
            partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

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
            partialUMService.ThrowIfUM(command.GetSettingKey(), param.SiteName);

            await dynamicRepository.ExecuteAsync(command);

            return new Result { Code = command.ErrCode, Message = command.ErrMsg };
        }

        public async Task<NamedResult> AddPersonalPreset(AddPersonalPresetParam param)
        {
            var command = new AddPersonalPresetCommand(
                param.AgentId,
                param.PresetTitle,
                param.WinningThreshold,
                param.LosingThreshold);
            partialUMService.ThrowIfUM(command.GetSettingKey(), param.SiteName);

            await dynamicRepository.ExecuteAsync(command);

            return new NamedResult { Name = param.PresetTitle, Code = command.ErrCode, Message = command.ErrMsg };
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

            return new Result { Code = command.ErrCode, Message = command.ErrMsg };
        }

        public async Task<Result> DeletePersonalPreset(DeletePersonalPresetParam param)
        {
            var command = new DeletePersonalPresetCommand(
                param.AgentId,
                param.ID);
            partialUMService.ThrowIfUM(command.GetSettingKey(), param.SiteName);

            await dynamicRepository.ExecuteAsync(command);

            return new Result { Code = command.ErrCode, Message = command.ErrMsg };
        }
    }
}
