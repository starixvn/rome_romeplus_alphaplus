namespace AlphaPlus.Settings.Site.Features.Transfer.Handlers
{
    using System.Threading.Tasks;
    using AlphaPlus.Shared.AppResources.Settings.Transfer;
    using AlphaPlus.Settings.Transfer.API.Models.AutoTransfer;
    using AlphaPlus.Settings.Transfer.API.Params.AutoTransfer;
    using AlphaPlus.Settings.Transfer.API.Services;
    using AlphaPlus.Settings.Site.Features.Transfer.Requests.AutoTransfer;
    using AlphaPlus.Settings.Site.Features.Transfer.ViewModels.AutoTransfer;
    using MediatR;
    using Settings.Transfer.API.Models;

    public class AutoTransferHandler :
        IRequestHandler<GetTransferConditionQuery, GetTransferConditionViewModel>,
        IRequestHandler<UpdateQuery, Result>,
        IRequestHandler<GetTodayTransfersQuery, AutoTransferMonitoringPageModel>,
        IRequestHandler<GetHistoryTransfersQuery, AutoTransferMonitoringPageModel>,
        IRequestHandler<CancelJobQuery, Result>,
        IRequestHandler<CancelAllJobsQuery, Result>,
        IRequestHandler<PostUpdateScheduleQuery, Result>,
        IRequestHandler<PostUpdateMultipleAutoTransferQuery, UpdateMultipleAutoTransferResultModel>
    {
        private readonly AutoTransferService service;
        private readonly CashBalanceTransferService cashBalanceTransferService;

        public AutoTransferHandler(
            AutoTransferService service,
            CashBalanceTransferService cashBalanceTransferService)
        {
            this.service = service;
            this.cashBalanceTransferService = cashBalanceTransferService;
        }

        public async Task<GetTransferConditionViewModel> Handle(GetTransferConditionQuery message, CancellationToken cancellation)
        {
            var tuple = (await cashBalanceTransferService.GetTransferCondition(message.UplineId));
            var transferCondition = tuple.Item1 ? Resources.Daily : string.Join(", ", tuple.Item2);

            return new GetTransferConditionViewModel
            {
                ViewerLevel = message.ViewerLevel,
                SearchAccountIds = message.SearchAccountIds,
                Usernames = message.Usernames,
                TransferCondition = transferCondition
            };
        }

        public async Task<Result> Handle(UpdateQuery message, CancellationToken cancellation)
        {
            var param = new UpdateSingleParam
            {
                CustId = message.CustId,
                UplineId = message.UplineId,
                IP = message.IP,
                Language = message.Language,
                State = message.State,
                Schedule = message.DateSchedule,
                SubAccId = message.SubAccId,
                Old_TransferOption = message.Old_TransferOption,
                AGIP = message.AGIP
            };

            var result = await service.UpdateAsync(param);

            var successMessage =
                string.Equals("on", message.State, System.StringComparison.OrdinalIgnoreCase)
                ? Resources.AutoTransfer_UpdateSuccessOn
                : Resources.AutoTransfer_UpdateSuccessOff;

            return new Result
            {
                Code = result.Code == 0 && !string.IsNullOrWhiteSpace(result.Message) ? -1 : result.Code,
                Message = string.IsNullOrWhiteSpace(result.Message)
                    ? string.Format(successMessage, message.CustName)
                    : result.Message
            };
        }

        public async Task<AutoTransferMonitoringPageModel> Handle(GetTodayTransfersQuery message, CancellationToken cancellation)
        {
            var param = new GetTodayTransfersParam
            {
                CustId = message.CustId,
                Language = message.Language,
                Username = message.Username,
                Status = message.Status,
                PageSize = message.PageSize,
                PageIndex = message.PageIndex,
                SortedBy = message.SortedBy,
                Direction = message.Direction,
                AGIP = message.AGIP
            };

            return await service.GetTodayTransfersAsync(param);
        }

        public async Task<AutoTransferMonitoringPageModel> Handle(GetHistoryTransfersQuery message, CancellationToken cancellation)
        {
            var param = new GetHistoryTransfersParam
            {
                CustId = message.CustId,
                Language = message.Language,
                Username = message.Username,
                Status = message.Status,
                PageSize = message.PageSize,
                PageIndex = message.PageIndex,
                SortedBy = message.SortedBy,
                Direction = message.Direction,
                FromDate = message.FromDate,
                ToDate = message.ToDate,
                LimitedDateToShowInfo = message.LimitedDateToShowInfo
            };

            return await service.GetHistoryTransfersAsync(param);
        }

        public async Task<Result> Handle(CancelJobQuery message, CancellationToken cancellation)
        {
            var param = new CancelJobParam
            {
                FromCustId = message.UplineId,
                ToCustId = message.CustId,
                ViewStatus = message.ViewStatus,
                Language = message.LanguageKey,
                IP = message.IP,
                AGIP = message.AGIP
            };

            return await service.CancelJobAsync(param);
        }

        public async Task<Result> Handle(CancelAllJobsQuery message, CancellationToken cancellation)
        {
            var param = new CancelAllJobParam
            {
                FromCustId = message.UplineId,
                Language = message.LanguageKey,
                IP = message.IP,
                AGIP = message.AGIP
            };

            var result = await service.CancelAllJobAsync(param);

            return new Result
            {
                Code = result.Code,
                Message = result.Code == 0 ? Resources.AutoTransfer_CancelAllSuccess : result.Message
            };
        }

        public async Task<Result> Handle(PostUpdateScheduleQuery message, CancellationToken cancellation)
        {
            var param = new UpdateScheduleParam
            {
                CustId = message.CustId,
                CustName = message.CustName,
                Schedule = message.Schedule,

                UplineId = message.UplineId,
                SubAccId = message.SubAccId,
                IP = message.IP,
                Language = message.Language,
                Old_TransferOption = message.Old_TransferOption,
                AGIP = message.AGIP
            };

            var result = await service.UpdateScheduleAsync(param);

            return new Result
            {
                Code = result.Code,
                Message = result.Code == 0 ? string.Format(Resources.AutoTransfer_UpdateScheduleSuccess, message.CustName) : result.Message
            };
        }

        public async Task<UpdateMultipleAutoTransferResultModel> Handle(PostUpdateMultipleAutoTransferQuery message, CancellationToken cancellation)
        {
            var param = new UpdateMultipleAutoTransferParam
            {
                UplineId = message.UplineId,
                SubAccId = message.SubAccId,
                IP = message.IP,
                Language = message.Language,

                OnIds = message.OnIds,
                OffIds = message.OffIds,
                OnStatus = message.OnStatus,
                OffStatus = message.OffStatus,
                OnSchedule = message.OnSchedule,
                OffSchedule = message.OffSchedule,
                Old_TransferOption = message.Old_TransferOption,
                OnNames = message.OnNames,
                OffNames = message.OffNames,
                AGIP = message.AGIP
            };

            return await service.UpdateMultipleAutoTransferAsync(param);
        }
    }
}