using AlphaPlus.Core.Models;
using AlphaPlus.Shared.AppResources.Settings.Transfer;
using AlphaPlus.Settings.Site.Features.Transfer.Constants;
using AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferYesterdayBalance;
using AlphaPlus.Settings.Site.Features.Transfer.ViewModels.TransferYesterdayBalance;
using AlphaPlus.Settings.Transfer.API.Params.TransferList;
using AlphaPlus.Settings.Transfer.API.Services;
using MediatR;
using AlphaPlus.Settings.Transfer.API.Params.TransferYesterdayBalance;
using AlphaPlus.Settings.Transfer.API.Configurations;
using AlphaPlus.Settings.Transfer.API.Models;

namespace AlphaPlus.Settings.Site.Features.Transfer.Handlers;

public class TransferYesterdayBalanceHandler(
    ITransferService transferService,
    ITransferListService transferListService,
    ITransferYesterdayBalanceService transferYesterdayBalanceService) :
        IRequestHandler<SingleQuery, SingleViewModel>,
        IRequestHandler<SingleCommand, Result>,
        IRequestHandler<MultipleCommand, Result>,
        IRequestHandler<MemberQuery, MemberViewModel>,
        IRequestHandler<MemberCommand, Result>
{
    private const string AgentTransferMemberListCache = "AgentMultipleTransfer.{0}.{1}";
    private const string ConcurrentSingleTransferResultMessage = "<span style=\"color: red\">The transfer has been proceeding by {0}<br/>";
    private readonly ITransferService transferService = transferService;
    private readonly ITransferListService transferListService = transferListService;
    private readonly ITransferYesterdayBalanceService transferYesterdayBalanceService = transferYesterdayBalanceService;

    public async Task<SingleViewModel> Handle(SingleQuery message, CancellationToken cancellationToken)
    {
        var param = new TransferListParam(
            message.ViewerId,
            message.ViewerLevelId,
            message.CustName,
            StatusConstant.All,
            AutoTransferConstant.All,
            false,
            message.AllowTotalBalanceTransfer,
            1,
            1,
            message.ActorLevel.IsAgentLevel() ? message.AGIP : string.Empty);
        var reportData = await transferListService.GetTransferList(param);

        if (reportData.TotalPages == 1)
        {
            var customer = reportData.Customers.First();

            if (customer.CustId == message.CustId)
            {
                return new SingleViewModel(
                    message.CustId,
                    message.AllowTotalBalanceTransfer ? customer.Balance : customer.YesterdayBalance);
            }
        }

        return null;
    }

    public async Task<Result> Handle(SingleCommand message, CancellationToken cancellationToken)
    {
        var isAllDayBalanceTransfer = await transferService.IsAllDayBalanceTransferAsync(message.ViewerId);

        if (isAllDayBalanceTransfer)
        {
            return Result.CreateErrorResult(Resources.AllowedAllDayBalanceTransferError, 6001);
        }

        var param = new TransferYesterdayBalanceParam(
            message.ViewerId,
            message.ViewerLevelId,
            AppSettings.AllowedListSuperAccounts,
            message.CustId.ToString(),
            false,
            -message.Amount,
            string.Empty,
            AutoTransferConstant.All,
            message.LangKey,
            message.IP,
            message.SubAccId,
            message.AGIP);
        var result = await transferYesterdayBalanceService.Transfer(param);

        if (result.IsSuccessful)
        {
            result.Message = Resources.TransferSuccessfully;
        }

        return result;
    }

    public async Task<Result> Handle(MultipleCommand message, CancellationToken cancellationToken)
    {
        var isAllDayBalanceTransfer = await transferService.IsAllDayBalanceTransferAsync(message.ViewerId);

        if (isAllDayBalanceTransfer)
        {
            return Result.CreateErrorResult(Resources.AllowedAllDayBalanceTransferError, 6001);
        }

        var custIds = message.CustIds;
        if (string.IsNullOrWhiteSpace(custIds))
        {
            if (string.IsNullOrWhiteSpace(message.Username))
            {
                custIds = "0";
            }
            else
            {
                var transferListParam = new TransferListParam(
                    message.ViewerId,
                    message.ViewerLevelId,
                    message.Username,
                    message.Status,
                    message.AutoTransfer,
                    !message.AllYesterdayBalance,
                    message.AllowTotalBalanceTransfer,
                    1,
                    int.MaxValue - 1,
                    message.ActorLevel.IsAgentLevel() ? message.AGIP : string.Empty);
                var reportData = await transferListService.GetTransferList(transferListParam);
                custIds = string.Join(',', reportData.Customers.Select(t => t.CustId));
            }
        }

        var param = new TransferYesterdayBalanceParam(
            message.ViewerId,
            message.ViewerLevelId,
            AppSettings.AllowedListSuperAccounts,
            custIds,
            true,
            0,
            string.Empty,
            AutoTransferConstant.All,
            message.LangKey,
            message.IP,
            message.SubAccId,
            message.AGIP);
        var result = await transferYesterdayBalanceService.Transfer(param);

        if (result.IsSuccessful)
        {
            result.Message = Resources.TransferSuccessfully;
        }

        return result;
    }

    public async Task<MemberViewModel> Handle(MemberQuery message, CancellationToken cancellationToken)
    {
        var member = await transferYesterdayBalanceService.GetMemberTransfer(message.CustId, message.AllowTotalBalanceTransfer, message.AGIP);

        return new MemberViewModel(message.ActorName, member);
    }

    public async Task<Result> Handle(MemberCommand message, CancellationToken cancellationToken)
    {
        var isAllDayBalanceTransfer = await transferService.IsAllDayBalanceTransferAsync(message.ViewerId);

        if (isAllDayBalanceTransfer)
        {
            return Result.CreateErrorResult(Resources.AllowedAllDayBalanceTransferError, 6001);
        }

        int actorId;
        string actorName;
        if (message.SubAccId == 0)
        {
            actorId = message.ViewerId;
            actorName = message.ActorName;
        }
        else
        {
            actorId = message.SubAccId;
            actorName = message.SubAccName;
        }

        ITransferCacheService transferCacheService = new TransferCacheService(actorId, actorName);
        var cacheKey = string.Format(AgentTransferMemberListCache, AppSettings.SiteLabel, message.ViewerId);
        var custIdCache = transferCacheService.Get(cacheKey) ?? new List<TransferCacheModel>();
        var concurrentTransfer = transferCacheService.CacheContains(custIdCache, message.CustId);
        if (concurrentTransfer != null)
        {
            return Result.CreateErrorResult(string.Format(ConcurrentSingleTransferResultMessage, concurrentTransfer.AdminName), 9999);
        }

        transferCacheService.CacheAdd(custIdCache, message.CustId, actorId, actorName);
        transferCacheService.Set(cacheKey, custIdCache);
        var param = new AgentTransferYesterdayBalanceParam(
            message.ViewerId,
            message.ActorName,
            AppSettings.AllowedListSuperAccounts,
            message.CustId,
            false,
            message.Amount,
            isAllDayBalanceTransfer,
            message.LangKey,
            message.IP,
            message.SubAccId,
            message.AGIP);
        var result = await transferYesterdayBalanceService.MemberTransfer(param);
        transferCacheService.CacheRemoveSuccessTransferCustIds(cacheKey, [message.CustId]);

        if (result.IsSuccessful)
        {
            result.Message = Resources.TransferSuccessfully;
        }

        return result;
    }
}