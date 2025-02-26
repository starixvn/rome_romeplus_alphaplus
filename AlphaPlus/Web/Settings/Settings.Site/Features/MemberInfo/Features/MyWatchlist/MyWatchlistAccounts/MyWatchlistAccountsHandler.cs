using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Models;
using AlphaPlus.Settings.MemberInfo.API.Features.MyWatchlist.MyWatchlistAccounts;
using AlphaPlus.Settings.MemberInfo.Domain.Constants;
using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Models;
using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Params;
using AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyWatchlistAccounts.Requests;
using AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyWatchlistAccounts.ViewModels;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyWatchlistAccounts
{
    public class MyWatchlistAccountsHandler :
        IRequestHandler<MyWatchlistAccountsQuery, MyWatchlistAccountsViewModel>,
        IRequestHandler<WatchlistAccountsQuery, MyWatchlistAccountsPagingViewModel>,
        IRequestHandler<PersonalPresetQuery, IEnumerable<PersonalPresetModel>>,
        IRequestHandler<AddPersonalThresholdCommand, IEnumerable<NamedResult>>,
        IRequestHandler<UpdateMultiplePersonalThresholdCommand, IEnumerable<NamedResult>>,
        IRequestHandler<DeleteMultiplePersonalThresholdCommand, IEnumerable<NamedResult>>,
        IRequestHandler<UpdateGlobalThresholdCommand, Result>,
        IRequestHandler<AddMultiplePersonalPresetCommand, IEnumerable<NamedResult>>,
        IRequestHandler<UpdatePersonalPresetCommand, Result>,
        IRequestHandler<DeletePersonalPresetCommand, Result>,
        IRequestHandler<ManageWatchlistAccountsQuery, ManageWatchlistAccountsViewModel>,
        IRequestHandler<ManageGlobalThresholdQuery, ManageGlobalThresholdViewModel>,
        IRequestHandler<ManagePersonalThresholdPresetQuery, ManagePersonalThresholdPresetViewModel>
    {
        private readonly MyWatchlistAccountsService service;
        private readonly IBrowserHelper browserHelper;

        public MyWatchlistAccountsHandler(
            MyWatchlistAccountsService service,
            IBrowserHelper browserHelper)
        {
            this.service = service;
            this.browserHelper = browserHelper;
        }

        public async Task<MyWatchlistAccountsViewModel> Handle(MyWatchlistAccountsQuery message, CancellationToken cancellationToken)
        {
            var isMobile = browserHelper.IsMobile(isOnlyTablet: false);
            var canUpdateAccount = message.CanUpdateAccount;

            var query = new WatchlistAccountsQuery()
            {
                FromDate = message.FromDate,
                ToDate = message.ToDate,
                Status = message.Status,
                ProblemAccount = message.ProblemAccount,
                Username = message.Username,
                SortType = message.SortType,
                IsDesc = message.IsDesc,
                PageIndex = message.PageIndex,
                PageSize = message.PageSize
            };
            var pagingViewModelTask = Handle(query, cancellationToken);

            var param = new SelectGlobalThresholdParam(message.ViewerId, message.Site);
            var globalThresholdTask = service.GetGlobalThreshold(param);

            await Task.WhenAll(pagingViewModelTask, globalThresholdTask);

            return new MyWatchlistAccountsViewModel(
                message,
                await globalThresholdTask,
                isMobile,
                canUpdateAccount,
                await pagingViewModelTask);
        }

        public async Task<MyWatchlistAccountsPagingViewModel> Handle(WatchlistAccountsQuery message, CancellationToken cancellationToken)
        {
            var param = new SelectWatchlistAccountsParam(
               message.ViewerId,
               message.FromDate,
               message.ToDate,
               message.Status != 0 ? ReachedThresholdStatusConstants.GetReachedThresholdStatus(message.Status) : string.Empty,
               message.ProblemAccount,
               message.Username,
               message.Site,
               message.SortType,
               message.IsDesc,
               message.PageIndex,
               message.PageSize);
            var watchlistAccountReportTask = service.GetWatchlistAccounts(param);

            var summaryParam = new SelectWatchlistSummaryParam(message.ViewerId, message.Site);
            var summaryTask = service.GetWatchlistSummary(summaryParam);

            await Task.WhenAll(watchlistAccountReportTask, summaryTask);

            return new MyWatchlistAccountsPagingViewModel(await watchlistAccountReportTask, await summaryTask);
        }

        public async Task<IEnumerable<PersonalPresetModel>> Handle(PersonalPresetQuery message, CancellationToken cancellationToken)
        {
            var param = new SelectPresetsParam(message.CustId, message.Site);

            return await service.GetPresets(param);
        }

        public async Task<IEnumerable<NamedResult>> Handle(AddPersonalThresholdCommand message, CancellationToken cancellationToken)
        {
            var param = new AddPersonalThresholdParam(
                message.Usernames,
                message.CustId,
                message.WinningThreshold,
                message.LosingThreshold,
                message.UserId,
                message.SubAccId,
                message.IP,
                message.Site);

            return await service.AddPersonalThreshold(param);
        }

        public async Task<IEnumerable<NamedResult>> Handle(UpdateMultiplePersonalThresholdCommand message, CancellationToken cancellationToken)
        {
            var tasks = new List<Task<NamedResult>>();
            var custIdArr = message.CustIds.Split(',');
            var usernameArr = message.Usernames.Split(',');

            for (int i = 0; i < custIdArr.Length; i++)
            {
                var param = new UpdatePersonalThresholdParam(
                            int.Parse(custIdArr[i]),
                            usernameArr[i],
                            message.WinningThreshold,
                            message.LosingThreshold,
                            message.UserId,
                            message.SubAccId,
                            message.IP,
                            message.Site);

                tasks.Add(service.UpdatePersonalThreshold(param));
            }

            return await Task.WhenAll(tasks);
        }

        public async Task<IEnumerable<NamedResult>> Handle(DeleteMultiplePersonalThresholdCommand message, CancellationToken cancellationToken)
        {
            var tasks = new List<Task<NamedResult>>();
            var custIdArr = message.CustIds.Split(',');
            var usernameArr = message.Usernames.Split(',');

            for (int i = 0; i < custIdArr.Length; i++)
            {
                var param = new DeletePersonalThresholdParam(
                            int.Parse(custIdArr[i]),
                            usernameArr[i],
                            message.UserId,
                            message.SubAccId,
                            message.IP,
                            message.Site);

                tasks.Add(service.DeletePersonalThreshold(param));
            }

            return await Task.WhenAll(tasks);
        }

        public async Task<Result> Handle(UpdateGlobalThresholdCommand message, CancellationToken cancellationToken)
        {
            var param = new UpdateGlobalThresholdParam(
                message.CustId,
                message.WinningThreshold,
                message.LosingThreshold,
                message.UserId,
                message.SubAccId,
                message.IP,
                message.Site);

            return await service.UpdateGlobalThreshold(param);
        }

        public async Task<IEnumerable<NamedResult>> Handle(AddMultiplePersonalPresetCommand message, CancellationToken cancellationToken)
        {
            var results = new List<NamedResult>();

            foreach (var preset in message.Presets)
            {
                var param = new AddPersonalPresetParam(
                    message.CustId,
                    preset.PresetTitle,
                    preset.WinningThreshold,
                    preset.LosingThreshold,
                    message.Site);

                results.Add(await service.AddPersonalPreset(param));
            }

            return results;
        }

        public async Task<Result> Handle(UpdatePersonalPresetCommand message, CancellationToken cancellationToken)
        {
            var param = new UpdatePersonalPresetParam(
                message.ID,
                message.CustId,
                message.PresetTitle,
                message.WinningThreshold,
                message.LosingThreshold);

            return await service.UpdatePersonalPreset(param);
        }

        public async Task<Result> Handle(DeletePersonalPresetCommand message, CancellationToken cancellationToken)
        {
            var param = new DeletePersonalPresetParam(
                message.ID,
                message.CustId,
                message.Site);

            return await service.DeletePersonalPreset(param);
        }

        public async Task<ManageWatchlistAccountsViewModel> Handle(ManageWatchlistAccountsQuery message, CancellationToken cancellationToken)
        {
            ThresholdViewModel thresholdViewModel;
            var personalPresets = await service.GetPresets(new SelectPresetsParam(message.ViewerId, message.Site));

            if (message.IsEdit)
            {
                thresholdViewModel = new ThresholdViewModel(message.WinningThreshold, message.LosingThreshold);
            }
            else if (message.MyWatchlistPage)
            {
                thresholdViewModel = new ThresholdViewModel();
            }
            else
            {
                var globalThreshold = await service.GetGlobalThreshold(new SelectGlobalThresholdParam(message.ViewerId, message.Site));
                globalThreshold ??= new GlobalThresholdModel();
                thresholdViewModel = new ThresholdViewModel(globalThreshold.WinningThreshold, globalThreshold.LosingThreshold);
            }

            return new ManageWatchlistAccountsViewModel(message, personalPresets, thresholdViewModel);
        }

        public async Task<ManageGlobalThresholdViewModel> Handle(ManageGlobalThresholdQuery message, CancellationToken cancellationToken)
        {
            var param = new SelectGlobalThresholdParam(message.CustId, message.Site);
            var globalThreshold = await service.GetGlobalThreshold(param);

            return new ManageGlobalThresholdViewModel(globalThreshold);
        }

        public async Task<ManagePersonalThresholdPresetViewModel> Handle(ManagePersonalThresholdPresetQuery message, CancellationToken cancellationToken)
        {
            var param = new SelectPresetsParam(message.CustId, message.Site);
            var personalPresets = await service.GetPresets(param);

            return new ManagePersonalThresholdPresetViewModel(personalPresets);
        }
    }
}