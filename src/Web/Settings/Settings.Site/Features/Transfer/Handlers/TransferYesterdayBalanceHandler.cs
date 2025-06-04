using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.Transfer.Constants;
using AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferYesterdayBalance;
using AlphaPlus.Settings.Site.Features.Transfer.ViewModels.TransferYesterdayBalance;
using AlphaPlus.Settings.Transfer.API.Configurations;
using AlphaPlus.Settings.Transfer.API.Models;
using AlphaPlus.Settings.Transfer.API.Params.TransferList;
using AlphaPlus.Settings.Transfer.API.Params.TransferYesterdayBalance;
using AlphaPlus.Settings.Transfer.API.Services;
using AlphaPlus.Shared.AppResources.Settings.Transfer;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.Transfer.Handlers;

public class TransferYesterdayBalanceHandler(
    ITransferCacheService transferCacheService,
    ITransferService transferService,
    ITransferListService transferListService,
    IYesterdayBalanceTransferService yesterdayBalanceTransferService) :
        IRequestHandler<SingleQuery, SingleViewModel>,
        IRequestHandler<SingleCommand, Result>,
        IRequestHandler<MultipleCommand, Result>,
        IRequestHandler<MemberQuery, MemberViewModel>,
        IRequestHandler<MemberCommand, Result>,
        IRequestHandler<MultipleMemberCommand, Result>
{
    private const string AgentTransferMemberListCache = "AgentMultipleTransfer.{0}.{1}";
    private const string ConcurrentSingleTransferResultMessage = "<span style=\"color: red\">The transfer has been proceeding by {0}<br/>";
    private readonly ITransferService transferService = transferService;
    private readonly ITransferListService transferListService = transferListService;
    private readonly IYesterdayBalanceTransferService yesterdayBalanceTransferService = yesterdayBalanceTransferService;
    private readonly ITransferCacheService transferCacheService = transferCacheService;

    public async Task<SingleViewModel> Handle(SingleQuery message, CancellationToken cancellationToken)
    {
        var param = new TransferListParam(
            message.ViewerId,
            message.ViewerLevelId,
            message.CustName,
            StatusConstant.All,
            AutoTransferConstant.All,
            0,
            false,
            message.AllowTotalBalanceTransfer,
            1,
            1,
            message.AGIP);
        var customerTransfer = await transferListService.GetCustomerTransfer(param);
        var balance = 0M;

        if (customerTransfer != null && customerTransfer.CustId == message.CustId)
        {
            balance = message.AllowTotalBalanceTransfer ? customerTransfer.Balance : customerTransfer.YesterdayBalance;
        }

        return new SingleViewModel(message.CustId, balance);
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
            message.CustId.ToString(),
            false,
            message.Amount,
            string.Empty,
            AutoTransferConstant.All,
            message.LangKey,
            message.IP,
            message.SubAccId,
            message.AGIP);
        var result = await yesterdayBalanceTransferService.Transfer(param);

        if (result.IsSuccessful)
        {
            result.Message = Resources.TransferSuccessfully;
        }

        return result;
    }

    public async Task<Result> Handle(MultipleCommand message, CancellationToken cancellationToken)
    {
        IEnumerable<int> custIds;
        if (message.IsAllPages && !string.IsNullOrWhiteSpace(message.Username))
        {
            var transferListParam = new TransferListParam(
                message.ViewerId,
                message.ViewerLevelId,
                message.Username,
                message.Status,
                message.AutoTransfer,
                message.LevelType,
                message.AllYesterdayBalance,
                message.AllowTotalBalanceTransfer,
                1,
                int.MaxValue - 1,
                message.ActorLevel.IsAgentLevel() ? message.AGIP : string.Empty);
            var reportData = await transferListService.GetTransferList(transferListParam);
            custIds = reportData.Customers.Select(c => c.CustId);
        }
        else
        {
            custIds = message.CustIds.ToListInt();

            if (custIds.Any(id => id == message.ViewerId))
            {
                return Result.CreateErrorResult(Resources.NotAuthorized, 403);
            }
        }

        var failedResults = new List<Result>();
        var custIdsBatches = custIds.Chunk(AppSettings.MaxCustIdTransfer);
        foreach (var custIdList in custIdsBatches)
        {
            var param = new TransferYesterdayBalanceParam(
                message.ViewerId,
                message.ViewerLevelId,
                string.Join(',', custIdList),
                true,
                0,
                message.Status,
                message.AutoTransfer,
                message.LangKey,
                message.IP,
                message.SubAccId,
                message.AGIP);
            var result = await yesterdayBalanceTransferService.Transfer(param);

            if (!result.IsSuccessful)
            {
                failedResults.Add(result);
            }
        }

        return failedResults.Any()
            ? Result.CreateErrorResult(string.Join("<br/>", failedResults.Select(r => r.Message)))
            : Result.CreateSuccessfulResult(Resources.TransferSuccessfully);
    }

    public async Task<MemberViewModel> Handle(MemberQuery message, CancellationToken cancellationToken)
    {
        var member = await yesterdayBalanceTransferService.GetMemberTransfer(message.CustId, message.AllowTotalBalanceTransfer, message.AGIP);

        return new MemberViewModel(message.ActorName, member);
    }

    public async Task<Result> Handle(MemberCommand message, CancellationToken cancellationToken)
    {
        var isAllDayBalanceTransfer = await transferService.IsAllDayBalanceTransferAsync(message.ViewerId);

        if (isAllDayBalanceTransfer)
        {
            return Result.CreateErrorResult(Resources.AllowedAllDayBalanceTransferError, 6001);
        }

        var cacheKey = string.Format(AgentTransferMemberListCache, AppSettings.SiteLabel, message.ViewerId);
        var custIdCache = transferCacheService.Get(cacheKey) ?? new List<TransferCacheModel>();
        var concurrentTransfer = transferCacheService.CacheContains(custIdCache, message.CustId);
        if (concurrentTransfer != null)
        {
            return Result.CreateErrorResult(string.Format(ConcurrentSingleTransferResultMessage, concurrentTransfer.AdminName), 9999);
        }

        transferCacheService.CacheAdd(custIdCache, message.CustId, message.AdminId, message.AdminName);
        transferCacheService.Set(cacheKey, custIdCache, message.AdminId, message.AdminName);
        var param = new AgentTransferYesterdayBalanceParam(
            message.ViewerId,
            message.ActorName,
            message.CustId,
            message.Amount,
            isAllDayBalanceTransfer,
            message.LangKey,
            message.IP,
            message.SubAccId,
            message.AGIP);

        var result = message.ActorLevel.IsAgentLevel()
                   ? await yesterdayBalanceTransferService.MemberTransfer(param)
                   : await yesterdayBalanceTransferService.DirectMemberTransfer(param);
        transferCacheService.CacheRemoveSuccessTransferCustIds(cacheKey, [message.CustId], message.AdminId, message.AdminName);

        if (result.IsSuccessful)
        {
            result.Message = Resources.TransferSuccessfully;
            var resultReset = await yesterdayBalanceTransferService.ResetPotentialWinLimit(message.CustId, message.AGIP);
            if (!resultReset.IsSuccessful)
            {
                result = Result.CreateErrorResult(resultReset.Message, resultReset.Code);
            }
        }

        return result;
    }

    public async Task<Result> Handle(MultipleMemberCommand message, CancellationToken cancellationToken)
    {
        if (message.CustIds.ToListInt().Any(id => id == message.ViewerId))
        {
            return Result.CreateErrorResult(Resources.NotAuthorized, 403);
        }

        var param = new AgentTransferMultipleYesterdayBalanceParam(
            message.ViewerId,
            message.ActorLevel,
            message.CustIds,
            message.Username,
            message.Status,
            message.AutoTransfer,
            message.AllYesterdayBalance,
            message.AllowTotalBalanceTransfer,
            message.IsAllPages,
            message.LangKey,
            message.IP,
            message.SubAccId,
            message.AGIP,
            message.AdminId,
            message.AdminName);
        var service = new AgentTransferMultipleYesterdayBalanceService(param, transferCacheService, transferService, transferListService, yesterdayBalanceTransferService);

        var result = await service.ProcessTransfer();
        if (result.IsSuccessful)
        {
            var resultReset = await yesterdayBalanceTransferService.ResetPotentialWinLimit(message.CustIds, message.AGIP);
            if (!resultReset.IsSuccessful)
            {
                return Result.CreateErrorResult(resultReset.Message, resultReset.Code);
            }
        }

        return Result.CreateErrorResult(result.Message, result.Code);
    }
}