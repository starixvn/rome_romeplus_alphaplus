using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.Balance;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.CustomerStatus;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.DonwlineList;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.GivenCredit.Get;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.GivenCredit.Update;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.Models;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.OnlineList;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.Transfer.Requests;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.Transfer.Requests;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.Update.Balance;
using AlphaPlus.Core.Models;
using AlphaPlus.Utilities.IP;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount
{
    public class SearchAccountHandler(ISearchAccountService searchAccountService, IPService ipService) :
        IRequestHandler<IndexQuery, IndexViewModel>,
        IRequestHandler<CustomerInfoQuery, CustomerInfoViewModel>,
        IRequestHandler<DownlineListQuery, DownlineListViewModel>,
        IRequestHandler<GetLastTxnQuery, string>,
        IRequestHandler<OnlineListQuery, IEnumerable<OnlineUser>>,
        IRequestHandler<GivenCreditQuery, GivenCreditViewModel>,
        IRequestHandler<GivenCreditCommand, Result>,
        IRequestHandler<CustomerNonProductStatusCommand, Result>,
        IRequestHandler<MemberBalanceInfoQuery, IBalanceInfoViewModel>,
        IRequestHandler<UpdateAllDayBalanceTransferRequest, Result>
    {
        private readonly ISearchAccountService searchAccountService = searchAccountService;
        private readonly IPService ipService = ipService;

        public Task<IndexViewModel> Handle(IndexQuery request, CancellationToken cancellationToken)
            => Task.FromResult(new IndexViewModel(request));

        public async Task<CustomerInfoViewModel> Handle(CustomerInfoQuery request, CancellationToken cancellationToken)
        {
            var account = await searchAccountService.GetCustomerInfo(request.SearchAccountName, request.ViewerId, request.ViewerLevelId);
            if (account != null)
            {
                var balanceInfo = BuildBalanceInfoViewModel(request, account, await searchAccountService.GetCreditInfo(account.CustId),
                                    await searchAccountService.GetBalanceInfo(account.CustId, account.RoleId, request.AGIP),
                                    await searchAccountService.IsAllDayBalanceTransfer(request.ViewerId));

                var formatedIP = ipService.FormatIP(account.LastLoginIP, request.IsAdminCanViewVNIP);

                account.LastLoginIP = formatedIP.IP;
                account.AllowOpenInfoPopup = formatedIP.AllowOpenInfoPopup;

                return new CustomerInfoViewModel(
                    request,
                    account,
                    balanceInfo,
                    await searchAccountService.CheckFirstLoginByQRCode(account.CustId),
                    request.CanCreateAccount);
            }

            return null;
        }

        private static IBalanceInfoViewModel BuildBalanceInfoViewModel(
            CustomerInfoQuery message,
            CustomerInfoModel customerInfo,
            CreditInfo creditInfo,
            IBalanceInfoModel balanceInfoModel,
            bool isAllDayBalanceInfo)
        {
            return customerInfo.RoleId switch
            {
                3 => new MasterBalanceInfoViewModel(message, customerInfo, creditInfo, balanceInfoModel, isAllDayBalanceInfo),
                2 => new AgentBalanceInfoViewModel(message, customerInfo, creditInfo, balanceInfoModel, isAllDayBalanceInfo),
                1 => new MemberBalanceInfoViewModel(message, customerInfo, creditInfo, balanceInfoModel, isAllDayBalanceInfo),
                _ => throw new NotImplementedException()
            };
        }

        public async Task<DownlineListViewModel> Handle(DownlineListQuery request, CancellationToken cancellationToken)
        {
            var downlineList = await searchAccountService.GetDownlineList(
                request.CustId,
                request.PageIndex,
                request.RoleId,
                request.StatusId,
                request.TotalPages);

            var totalDownlinesSummarys = request.TotalPages.Equals(0)
                ? await searchAccountService.GetTotalDownlinesSummary(request.CustId, request.RoleId)
                : [];

            return new DownlineListViewModel(downlineList, totalDownlinesSummarys);
        }

        public async Task<string> Handle(GetLastTxnQuery request, CancellationToken cancellationToken)
        {
            var lastTxn = await searchAccountService.GetLastTxn(request.Username, request.UplineId, request.IsInternal);

            return DateTime.MinValue == lastTxn ? string.Empty : lastTxn.ToString();
        }

        public Task<IEnumerable<OnlineUser>> Handle(OnlineListQuery request, CancellationToken cancellationToken)
            => searchAccountService.GetOnlineList(request.SearchAccountId, request.IsAdminCanViewVNIP);

        public async Task<GivenCreditViewModel> Handle(GivenCreditQuery request, CancellationToken cancellationToken)
        {
            decimal memberCreditMin = 0;
            if (Enumeration.FromValue<UserLevel>(request.SearchAccountRoleId).IsMemberLevel())
            {
                var balanceInfo = (MemberBalanceInfoModel)await searchAccountService.GetBalanceInfo(request.SearchAccountId, request.SearchAccountRoleId, request.AGIP);

                memberCreditMin = Math.Max(0, balanceInfo.CustomerOutstanding - balanceInfo.CustomerCash + balanceInfo.PendingTransfer3rd);
            }

            return new GivenCreditViewModel(
                request,
                await searchAccountService.GetCreditInfo(request.SearchAccountId),
                memberCreditMin);
        }

        public Task<Result> Handle(GivenCreditCommand request, CancellationToken cancellationToken)
            => searchAccountService.UpdateGivenCredit(request);

        public Task<Result> Handle(CustomerNonProductStatusCommand request, CancellationToken cancellationToken)
            => searchAccountService.UpdateCustomerStatus(new UpdateCustomerStatusParam(
                     request.SearchAccountId,
                     request.ViewerId,
                     request.SubAccountId,
                     request.Language,
                     request.IP,
                     request.StatusValue,
                     request.StatusName,
                     request.ViewerName));

        public async Task<IBalanceInfoViewModel> Handle(MemberBalanceInfoQuery message, CancellationToken cancellationToken)
        {
            var customerInfoQuery = new CustomerInfoQuery
            {
                SearchAccountName = message.CustName
            };
            var customerInfo = await searchAccountService.GetCustomerInfo(
                customerInfoQuery.SearchAccountName,
                customerInfoQuery.ActorId,
                customerInfoQuery.ActorRoleId);

            if (customerInfo != null && Enumeration.FromValue<UserLevel>(customerInfo.RoleId).IsMemberLevel())
            {
                var creditInfoTask = searchAccountService.GetCreditInfo(customerInfo.CustId);
                var balanceInfoTask = searchAccountService.GetBalanceInfo(customerInfo.CustId, UserLevel.Member.Value, message.AGIP);
                var isAllDayBalanceTransferTask = searchAccountService.IsAllDayBalanceTransfer(customerInfoQuery.ActorId);
                await Task.WhenAll(creditInfoTask, balanceInfoTask, isAllDayBalanceTransferTask);

                return BuildMemberBalanceInfoViewModel(customerInfoQuery, customerInfo, await creditInfoTask, await balanceInfoTask, await isAllDayBalanceTransferTask);
            }

            return null;
        }

        public Task<Result> Handle(UpdateAllDayBalanceTransferRequest message, CancellationToken cancellationToken)
            => searchAccountService.UpdateAllDayBalanceTransfer(
               new UpdateAllDayBalanceTransferParam(
                   message.CustId,
                   message.ActorId,
                   message.Language,
                   message.IP,
                   message.StatusValue,
                   message.Currency,
                   message.AGIP));

        internal static IBalanceInfoViewModel BuildMemberBalanceInfoViewModel(
            CustomerInfoQuery message,
            CustomerInfoModel customerInfo,
            CreditInfo creditInfo,
            IBalanceInfoModel balanceInfoModel,
            bool isAllDayBalanceInfo)
            => new MemberBalanceInfoViewModel(message, customerInfo, creditInfo, balanceInfoModel, isAllDayBalanceInfo);
    }
}