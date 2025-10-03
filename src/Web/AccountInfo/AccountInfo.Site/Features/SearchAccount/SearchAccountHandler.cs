namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount;

using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.GivenCredit.Update;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.Transfer.Requests;
using Core.Models;
using GetAccount;
using GetAccount.Balance;
using GetAccount.CustomerStatus;
using GetAccount.DonwlineList;
using GetAccount.GivenCredit.Get;
using GetAccount.Models;
using GetAccount.OnlineList;
using GetAccount.RemarkInfo.Get;
using GetAccount.RemarkInfo.Update;
using GetAccount.ViewModels;
using MediatR;
using Shared;
using Transfer.Requests;
using Update.Balance;
using Utilities.IP;

public class SearchAccountHandler(ISearchAccountService searchAccountService, IPService ipService, IKickOutCustomerService kickOutCustomerService) :
    IRequestHandler<IndexQuery, IndexViewModel>,
    IRequestHandler<CustomerInfoQuery, CustomerInfoViewModel>,
    IRequestHandler<DownlineListQuery, DownlineListViewModel>,
    IRequestHandler<DirectMemberListQuery, DownlineListViewModel>,
    IRequestHandler<GetLastTxnQuery, string>,
    IRequestHandler<OnlineListQuery, IEnumerable<OnlineUser>>,
    IRequestHandler<GivenCreditQuery, GivenCreditViewModel>,
    IRequestHandler<GivenCreditCommand, Result>,
    IRequestHandler<CustomerNonProductStatusCommand, Result>,
    IRequestHandler<MemberBalanceInfoQuery, IBalanceInfoViewModel>,
    IRequestHandler<UpdateAllDayBalanceTransferRequest, Result>,
    IRequestHandler<TotalDownlineQuery, IEnumerable<TotalDownlinesSummary>>,
    IRequestHandler<RemarkInfoQuery, RemarkInfoViewModel>,
    IRequestHandler<UpdateRemarkInfoQuery, Result>
{
    public Task<IndexViewModel> Handle(IndexQuery request, CancellationToken cancellationToken)
        => Task.FromResult(new IndexViewModel(request));

    public async Task<CustomerInfoViewModel> Handle(CustomerInfoQuery request, CancellationToken cancellationToken)
    {
        var account = await searchAccountService.GetCustomerInfo(request.SearchAccountName, request.ViewerId, request.ViewerLevelId);
        if (account != null)
        {
            var balanceInfo = BuildBalanceInfoViewModel(request, account, await searchAccountService.GetCreditInfo(account.CustId),
                await searchAccountService.GetBalanceInfo(account.CustId, account.RoleId, request.SiteDbGroupId),
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

        return new DownlineListViewModel(downlineList);
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
            var balanceInfoObj = await searchAccountService.GetBalanceInfo(request.SearchAccountId,
                request.SearchAccountRoleId, request.SiteDbGroupId);

            if (balanceInfoObj is MemberBalanceInfoModel balanceInfo)
            {
                memberCreditMin = Math.Max(0, balanceInfo.CustomerOutstanding - balanceInfo.CustomerCash + balanceInfo.PendingTransfer3rd);
            }
        }

        return new GivenCreditViewModel(
            request,
            await searchAccountService.GetCreditInfo(request.SearchAccountId),
            memberCreditMin);
    }

    public Task<Result> Handle(GivenCreditCommand request, CancellationToken cancellationToken)
        => searchAccountService.UpdateGivenCredit(request);

    public async Task<Result> Handle(CustomerNonProductStatusCommand request, CancellationToken cancellationToken)
    {
        var result = await searchAccountService.UpdateCustomerStatus(new UpdateCustomerStatusParam(
            request.SearchAccountId,
            request.ViewerId,
            request.SubAccountId,
            request.Language,
            request.IP,
            request.StatusValue,
            request.StatusName,
            request.ViewerName));

        if (result.IsSuccessful && request.StatusValue && request.StatusName.Equals("IsProtectionMode", StringComparison.OrdinalIgnoreCase))
        {
            await kickOutCustomerService.KickOutDueToProtectedStatus(
                request.SearchAccountId,
                request.SearchAccountRoleId,
                request.ServerId,
                request.IP);
        }

        return result;
    }

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
            var balanceInfoTask = searchAccountService.GetBalanceInfo(customerInfo.CustId, UserLevel.Member.Value, message.SiteDbGroupId);
            var isAllDayBalanceTransferTask = searchAccountService.IsAllDayBalanceTransfer(customerInfoQuery.ActorId);
            await Task.WhenAll(creditInfoTask, balanceInfoTask, isAllDayBalanceTransferTask);

            return BuildMemberBalanceInfoViewModel(customerInfoQuery, customerInfo, await creditInfoTask, await balanceInfoTask, await isAllDayBalanceTransferTask);
        }

        return null;
    }

    public Task<Result> Handle(UpdateAllDayBalanceTransferRequest message, CancellationToken cancellationToken)
        => searchAccountService.UpdateAllDayBalanceTransfer(
            new UpdateAllDayBalanceTransferParam(
                message.SearchAccountId,
                message.ActorId,
                message.LangKey,
                message.IP,
                message.StatusValue,
                message.Currency,
                message.SiteDbGroupId));

    internal static IBalanceInfoViewModel BuildMemberBalanceInfoViewModel(
        CustomerInfoQuery message,
        CustomerInfoModel customerInfo,
        CreditInfo creditInfo,
        IBalanceInfoModel balanceInfoModel,
        bool isAllDayBalanceInfo)
        => new MemberBalanceInfoViewModel(message, customerInfo, creditInfo, balanceInfoModel, isAllDayBalanceInfo);

    public Task<IEnumerable<TotalDownlinesSummary>> Handle(TotalDownlineQuery message, CancellationToken cancellationToken)
        => searchAccountService.GetTotalDownlinesSummary(message.CustId, message.RoleId);

    public async Task<DownlineListViewModel> Handle(DirectMemberListQuery message, CancellationToken cancellationToken)
    {
        var directMembers = await searchAccountService.GetDownlineList(
            message.CustId,
            message.PageIndex,
            message.RoleId,
            message.StatusId,
            message.TotalPages,
            true);

        return new DownlineListViewModel(directMembers);
    }

    public async Task<RemarkInfoViewModel> Handle(RemarkInfoQuery request, CancellationToken cancellationToken)
    {
        var remarkInfo = await searchAccountService.GetRemarkInfo(request.SearchAccountId, request.ActorId);

        if (remarkInfo == null)
        {
            return new RemarkInfoViewModel(
                request.SearchAccountId,
                request.SearchAccountName,
                string.Empty,
                request.IsShowCancelButton
            );
        }

        return new RemarkInfoViewModel(
            request.SearchAccountId,
            request.SearchAccountName,
            remarkInfo.Remark,
            request.IsShowCancelButton
        );
    }

    public Task<Result> Handle(UpdateRemarkInfoQuery request, CancellationToken cancellationToken)
        => searchAccountService.UpdateRemarkInfo(request);
}