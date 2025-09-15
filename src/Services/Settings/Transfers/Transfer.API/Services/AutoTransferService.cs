using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Transfer.API.Constants;
using AlphaPlus.Settings.Transfer.API.Criteria.AutoTransfer;
using AlphaPlus.Settings.Transfer.API.Extensions;
using AlphaPlus.Settings.Transfer.API.Helpers;
using AlphaPlus.Settings.Transfer.API.Models.AutoTransfer;
using AlphaPlus.Settings.Transfer.API.Params.AutoTransfer;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Transfer.API.Services
{
    public interface AutoTransferService
    {
        Task<Result> UpdateAsync(UpdateSingleParam param);

        Task<AutoTransferMonitoringPageModel> GetTodayTransfersAsync(GetTodayTransfersParam param);

        Task<AutoTransferMonitoringPageModel> GetHistoryTransfersAsync(GetHistoryTransfersParam param);

        Task<Result> CancelJobAsync(CancelJobParam param);

        Task<Result> CancelAllJobAsync(CancelAllJobParam param);

        Task<Result> UpdateScheduleAsync(UpdateScheduleParam param);

        Task<UpdateMultipleAutoTransferResultModel> UpdateMultipleAutoTransferAsync(UpdateMultipleAutoTransferParam param);
    }

    public class AutoTransferServiceImpl(IDynamicRepository repository) : AutoTransferService
    {
        public async Task<Result> UpdateAsync(UpdateSingleParam param)
        {
            var isAutoJobRunning = await GetJobStatus(param.CustId, param.UplineId, param.State ?? string.Empty, param.AGIP ?? string.Empty, param.SiteDbGroupId);
            var criteria = new UpdateStatusCriteria(
                param.UplineId,
                param.CustId,
                TransferConditionHelper.GetAutoTransferScheduleFromDates(param.Schedule ?? [""]),
                isAutoJobRunning,
                string.Equals(AutoTransferConstant.On, param.State, StringComparison.InvariantCultureIgnoreCase),
                param.Language ?? string.Empty,
                param.IP ?? string.Empty,
                param.Old_TransferOption ?? string.Empty,
                param.SubAccId);

            await repository.ExecuteAsync(criteria);

            return new Result
            {
                Code = criteria.ErrCode,
                Message = criteria.ErrMsg
            };
        }

        public async Task<AutoTransferMonitoringPageModel> GetTodayTransfersAsync(GetTodayTransfersParam param)
        {
            var criteria = new GetTodayTransfersCriteria(param.SiteDbGroupId)
            {
                FromCustId = param.CustId,
                Language = param.Language,
                Searchname = param.Username,
                ViewStatus = param.Status,
                SortBy = param.SortedBy,
                SortType = param.Direction,
                PageSize = param.PageSize,
                PageIndex = param.PageIndex,
                LevelType = param.LevelType
            };

            var transfers = await repository.FetchAsync<AutoTransferMonitoringModel>(criteria, param.AGIP);

            return new AutoTransferMonitoringPageModel
            {
                Transfers = transfers,
                TotalItems = criteria.TotalRow,
                PageSize = param.PageSize,
                PageIndex = param.PageIndex,
                SortedBy = param.SortedBy,
                Direction = param.Direction,

                TotalCancelAll = criteria.TotalCancelAll,
                TotalPending = criteria.TotalPending,
                TotalProcessing = criteria.TotalProcessing,
                TotalFailed = criteria.TotalFailed,
            };
        }

        public async Task<AutoTransferMonitoringPageModel> GetHistoryTransfersAsync(GetHistoryTransfersParam param)
        {
            var criteria = new GetHistoryTransfersCriteria
            {
                FromCustId = param.CustId,
                Language = param.Language,
                Searchname = param.Username,
                ViewStatus = param.Status,
                SortBy = param.SortedBy,
                SortType = param.Direction,
                PageSize = param.PageSize,
                PageIndex = param.PageIndex,
                FromDate = param.FromDate.GetGreaterDate(param.LimitedDateToShowInfo),
                ToDate = param.ToDate.GetGreaterDate(param.LimitedDateToShowInfo),
                LevelType = param.LevelType
            };

            var transfers = await repository.FetchAsync<AutoTransferMonitoringModel>(criteria);

            return new AutoTransferMonitoringPageModel
            {
                Transfers = transfers,
                TotalItems = criteria.TotalRow,
                PageSize = param.PageSize,
                PageIndex = param.PageIndex,
                SortedBy = param.SortedBy,
                Direction = param.Direction
            };
        }

        public async Task<Result> CancelJobAsync(CancelJobParam param)
        {
            var criteria = new CancelJobCriteria(
                param.FromCustId, param.ToCustId, param.ViewStatus ?? string.Empty, param.Language ?? string.Empty, param.IP ?? string.Empty, param.SiteDbGroupId);
            await repository.ExecuteAsync(criteria);

            return new Result
            {
                Code = criteria.ErrCode,
                Message = criteria.ErrCode == 0 ? string.Empty : criteria.ErrMsg
            };
        }

        public async Task<Result> CancelAllJobAsync(CancelAllJobParam param)
        {
            var criteria = new CancelAllJobCriteria(param.FromCustId, param.Language, param.IP, param.SiteDbGroupId);
            await repository.ExecuteAsync(criteria);

            return new Result
            {
                Code = criteria.ErrCode,
                Message = criteria.ErrCode == 0 ? string.Empty : criteria.ErrMsg
            };
        }

        private async Task<bool> GetJobStatus(int custId, int creator, string state, string agIP, int siteDbGroupId)
        {
            var criteria = new GetJobStatusCriteria(
                creator,
                custId,
                string.Equals(AutoTransferConstant.On, state, StringComparison.InvariantCultureIgnoreCase),
                siteDbGroupId);
            await repository.ExecuteAsync(criteria);

            return criteria.IsAutoJobRunning;
        }

        public async Task<Result> UpdateScheduleAsync(UpdateScheduleParam param)
        {
            var isAutoJobRunning = await GetJobStatus(param.CustId, param.UplineId, "on", param.AGIP ?? string.Empty, param.SiteDbGroupId);
            var criteria = new UpdateScheduleCriteria(
                param.CustId,
                param.CustName ?? string.Empty,
                TransferConditionHelper.GetAutoTransferScheduleFromDates(param.Schedule ?? string.Empty),
                param.UplineId,
                param.SubAccId,
                param.IP ?? string.Empty,
                param.Language ?? string.Empty,
                param.Old_TransferOption ?? string.Empty,
                isAutoJobRunning);
            await repository.ExecuteAsync(criteria);

            var warningCase = string.IsNullOrWhiteSpace(criteria.ErrMsg) ? 0 : -1;

            return new Result
            {
                Code = criteria.ErrCode == 0 ? warningCase : criteria.ErrCode,
                Message = criteria.ErrMsg ?? string.Empty
            };
        }

        public async Task<UpdateMultipleAutoTransferResultModel> UpdateMultipleAutoTransferAsync(UpdateMultipleAutoTransferParam param)
        {
            var result = new UpdateMultipleAutoTransferResultModel();
            await UpdateMultipleAutoTransferToStatusAsync(param, param.OffStatus ?? string.Empty, param.OffIds ?? string.Empty, param.OffNames ?? string.Empty, param.OffSchedule ?? string.Empty, result);

            if (string.IsNullOrWhiteSpace(result.StopMessage))
            {
                await UpdateMultipleAutoTransferToStatusAsync(param, param.OnStatus ?? string.Empty, param.OnIds ?? string.Empty, param.OnNames ?? string.Empty, param.OnSchedule ?? string.Empty, result);
            }

            return result;
        }

        public async Task UpdateMultipleAutoTransferToStatusAsync(
            UpdateMultipleAutoTransferParam param,
            string status,
            string pIds,
            string pNames,
            string schedule,
            UpdateMultipleAutoTransferResultModel result)
        {
            var stopCodes = new[] { 105, 108 };
            var ids = (pIds ?? string.Empty).Split([','], StringSplitOptions.RemoveEmptyEntries);
            var names = (pNames ?? string.Empty).Split([','], StringSplitOptions.RemoveEmptyEntries);
            var dates = (schedule ?? string.Empty).Split([','], StringSplitOptions.RemoveEmptyEntries);

            for (var i = 0; i < ids.Length; i++)
            {
                var id = ids[i];
                var name = names[i].Trim();

                var updateSingleParam = new UpdateSingleParam
                {
                    CustId = int.Parse(id),
                    UplineId = param.UplineId,
                    IP = param.IP ?? string.Empty,
                    Language = param.Language ?? string.Empty,
                    State = status,
                    SubAccId = param.SubAccId,
                    Schedule = dates,
                    Old_TransferOption = param.Old_TransferOption ?? string.Empty,
                    AGIP = param.AGIP ?? string.Empty,
                    SiteDbGroupId = param.SiteDbGroupId
                };

                var singleResult = await UpdateAsync(updateSingleParam);

                if (singleResult.Code == 0)
                {
                    if (!string.IsNullOrWhiteSpace(singleResult.Message))
                    {
                        result.WarningCusts.Add(name);
                    }

                    continue;
                }

                if (stopCodes.Any(s => s == singleResult.Code))
                {
                    result.Code = singleResult.Code;
                    result.StopMessage = singleResult.Message;
                    return;
                }

                var code = singleResult.Code.ToString();
                if (result.ErrMessages.ContainsKey(code))
                {
                    result.ErrCusts[code].Add(name);
                    continue;
                }

                result.ErrMessages.Add(code, singleResult.Message ?? string.Empty);
                result.ErrCusts.Add(code, [name]);
            }
        }
    }
}