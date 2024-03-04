namespace AlphaPlus.Settings.Site.Features.MyWatchlist.MyWatchlistAccounts
{
    using AlphaPlus.Settings.Site.Features.MyWatchlist.MyWatchlistAccounts.Requests;
    using AlphaPlus.Settings.Site.Features.MyWatchlist.MyWatchlistAccounts.ViewModels;
    using AlphaPlus.Core.Helpers;
    using AlphaPlus.Settings.MemberInfo.API.Features.MyWatchlist.MyWatchlistAccounts;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Models;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Params;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class MyWatchlistAccountsHandler :
        IRequestHandler<MyWatchlistAccountsQuery, MyWatchlistAccountsViewModel>,
        IRequestHandler<WatchlistAccountsQuery, MyWatchlistAccountsPagingViewModel>,
        IRequestHandler<PersonalPresetQuery, IEnumerable<PersonalPresetModel>>,
        IRequestHandler<AddPersonalThresholdCommand, IEnumerable<NamedResult>>,
        IRequestHandler<UpdateMultiplePersonalThresholdCommand, IEnumerable<NamedResult>>,
        IRequestHandler<DeleteMultiplePersonalThresholdCommand, IEnumerable<NamedResult>>,
        // IRequestHandler<UpdateGlobalThresholdCommand, Result>, //TODO
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

        public async Task<MyWatchlistAccountsViewModel> Handle(MyWatchlistAccountsQuery message, CancellationToken cancellation)
        {
            var isMobile = browserHelper.IsMobile(isOnlyTablet: false);
            var canUpdateAccount = message.CanUpdateAccount;
            var query = new WatchlistAccountsQuery(message.Viewer)
            {
                Username = message.Username,
                SortType = message.SortType,
                IsDesc = message.IsDesc,
                PageIndex = message.PageIndex,
                PageSize = message.PageSize
            };
            var pagingViewModel = await Handle(query, cancellation);

            var param = new SelectGlobalThresholdParam(message.CustId, message.Site);
            var globalThreshold = await service.GetGlobalThreshold(param);

            return new MyWatchlistAccountsViewModel(
                message,
                globalThreshold,
                isMobile,
                canUpdateAccount,
                pagingViewModel);
        }

        public async Task<MyWatchlistAccountsPagingViewModel> Handle(WatchlistAccountsQuery message, CancellationToken cancellation)
        {
            var param = new SelectWatchlistAccountsParam(
               message.CustId,
               message.Username,
               message.Site,
               message.SortType,
               message.IsDesc,
               message.PageIndex,
               message.PageSize);

            var watchlistAccountReport = await service.GetWatchlistAccounts(param);

            return new MyWatchlistAccountsPagingViewModel(watchlistAccountReport, message);
        }

        public async Task<IEnumerable<PersonalPresetModel>> Handle(PersonalPresetQuery message, CancellationToken cancellation)
        {
            var param = new SelectPresetsParam(message.CustId, message.Site);

            return await service.GetPresets(param);
        }

        public async Task<IEnumerable<NamedResult>> Handle(AddPersonalThresholdCommand message, CancellationToken cancellation)
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

        public async Task<IEnumerable<NamedResult>> Handle(UpdateMultiplePersonalThresholdCommand message, CancellationToken cancellation)
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

        public async Task<IEnumerable<NamedResult>> Handle(DeleteMultiplePersonalThresholdCommand message, CancellationToken cancellation)
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

        public async Task<Result> Handle(UpdateGlobalThresholdCommand message, CancellationToken cancellation)
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

        public async Task<IEnumerable<NamedResult>> Handle(AddMultiplePersonalPresetCommand message, CancellationToken cancellation)
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

        public async Task<Result> Handle(UpdatePersonalPresetCommand message, CancellationToken cancellation)
        {
            var param = new UpdatePersonalPresetParam(
                message.ID,
                message.CustId,
                message.PresetTitle,
                message.WinningThreshold,
                message.LosingThreshold);

            return await service.UpdatePersonalPreset(param);
        }

        public async Task<Result> Handle(DeletePersonalPresetCommand message, CancellationToken cancellation)
        {
            var param = new DeletePersonalPresetParam(
                message.ID,
                message.CustId,
                message.Site);

            return await service.DeletePersonalPreset(param);
        }

        public async Task<ManageWatchlistAccountsViewModel> Handle(ManageWatchlistAccountsQuery message, CancellationToken cancellation)
        {
            var personalPresets = await service.GetPresets(new SelectPresetsParam(message.CustId, message.Site));
            return new ManageWatchlistAccountsViewModel(message, personalPresets);
        }

        public async Task<ManageGlobalThresholdViewModel> Handle(ManageGlobalThresholdQuery message, CancellationToken cancellation)
        {
            var param = new SelectGlobalThresholdParam(message.CustId, message.Site);
            var globalThreshold = await service.GetGlobalThreshold(param);

            return new ManageGlobalThresholdViewModel(globalThreshold);
        }

        public async Task<ManagePersonalThresholdPresetViewModel> Handle(ManagePersonalThresholdPresetQuery message, CancellationToken cancellation)
        {
            var param = new SelectPresetsParam(message.CustId, message.Site);
            var personalPresets = await service.GetPresets(param);

            return new ManagePersonalThresholdPresetViewModel(personalPresets);
        }
    }
}