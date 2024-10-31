using AlphaPlus.Settings.Site.Features.MemberInfo.Requests.ProblemAccount;
using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.ProblemAccount;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Settings.MemberInfo.API.Features.MyWatchlist.MyWatchlistAccounts;
using AlphaPlus.Settings.MemberInfo.API.Services;
using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Params;
using AlphaPlus.Settings.MemberInfo.Domain.Models.ProblemAccount;
using AlphaPlus.Settings.MemberInfo.Domain.Params.ProblemAccount;
using MediatR;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Handlers
{
    public class ProblemAccountHandler :
        IRequestHandler<IndexRequest, ProblemAccountReportViewModel>,
        IRequestHandler<GetProblemAccountRequest, IEnumerable<ProblemAccountModel>>,
        IRequestHandler<UpdateProblemAccountCommand, Result>,
        IRequestHandler<UpdateProblemAccountRequest, UpdateReviewStatusViewModel>,
        IRequestHandler<ConfirmAddToWatchlistQuery, ConfirmAddToWatchlistViewModel>
    {
        private readonly IProblemAccountService problemAccountService;
        private readonly MyWatchlistAccountsService myWatchlistAccountsService;
        private readonly IBrowserHelper browserHelper;

        public ProblemAccountHandler(
            IProblemAccountService problemAccountService,
            MyWatchlistAccountsService myWatchlistAccountsService,
            IBrowserHelper browserHelper)
        {

            this.browserHelper = browserHelper;
            this.problemAccountService = problemAccountService;
            this.myWatchlistAccountsService = myWatchlistAccountsService;
        }

        public Task<UpdateReviewStatusViewModel> Handle(UpdateProblemAccountRequest message, CancellationToken cancellation)
            => Task.FromResult(new UpdateReviewStatusViewModel(message));

        public Task<Result> Handle(UpdateProblemAccountCommand message, CancellationToken cancellation)
        {
            return problemAccountService.UpdateReviewStatus(new UpdateProblemAccountParam
            {
                CustId = message.CustId,
                CustomerList = message.Ids,
                IsAffectedAllDownlines = message.IsAffectedAllDownlines,
                Language = message.Language,
                LoginIp = message.IP,
                RoleId = message.RoleId,
                SubAccId = message.SubAccId
            });
        }

        public Task<ProblemAccountReportViewModel> Handle(IndexRequest message, CancellationToken cancellation)
            => Task.FromResult(new ProblemAccountReportViewModel(message, new List<ProblemAccountModel>(), browserHelper.IsMobile(isOnlyTablet: false)));

        public async Task<IEnumerable<ProblemAccountModel>> Handle(GetProblemAccountRequest message, CancellationToken cancellation)
        {
            var models = await problemAccountService.Get(CreateParam(message));

            return models;
        }

        protected internal static GetProblemAccountParam CreateParam(ProblemAccountRequestBase message)
            => new()
            {
                FromDate = message.FromDate,
                ToDate = message.ToDate,
                ReviewStatusId = message.UserSelectedReviewedStatusId,
                CustId = message.CustId,
                SubAccId = message.CustId,
                UplineUserName = message.UplineName,
                SearchRoleId = message.SearchRoleId,
                SearchUserName = message.UserName
            };

        public async Task<ConfirmAddToWatchlistViewModel> Handle(ConfirmAddToWatchlistQuery message, CancellationToken cancellation)
        {
            var param = new SelectMultipleWatchlistParam(message.CustIds, message.Site);
            var multipleWatchlistAccountModel = await myWatchlistAccountsService.GetMultipleWatchlistAccount(param);

            return new ConfirmAddToWatchlistViewModel(multipleWatchlistAccountModel);
        }
    }
}