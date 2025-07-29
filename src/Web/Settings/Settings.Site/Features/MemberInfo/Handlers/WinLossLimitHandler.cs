using AlphaPlus.Core.Models;
using AlphaPlus.Settings.MemberInfo.API.Services;
using AlphaPlus.Settings.MemberInfo.API.Services.WinLossLimit;
using AlphaPlus.Settings.MemberInfo.Domain.Features.Shared.Models;
using AlphaPlus.Settings.MemberInfo.Domain.Models.WinLossLimit;
using AlphaPlus.Settings.MemberInfo.Domain.Params;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests.WinLossLimit;
using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.WinLossLimit;
using AlphaPlus.Shared.AppResources.Common;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Handlers;

public class WinLossLimitHandler(IAccountService accountService, WinLossLimitService winLossLimitService)
    : IRequestHandler<WinLossLimitQuery, WinLossLimitViewModel>,
      IRequestHandler<ResetCurrentWinAmountCommand, Result>
{
    private readonly IAccountService accountService = accountService;
    private readonly WinLossLimitService winLossLimitService = winLossLimitService;

    public async Task<WinLossLimitViewModel> Handle(WinLossLimitQuery message, CancellationToken cancellation)
    {
        var products = await winLossLimitService.GetWinLossLimitProducts(message.CustId);
        var firstProduct = products?.FirstOrDefault()?.Products?.FirstOrDefault();

        var actorLevel = message.ActorLevel;
        var masterList = new List<CustInfo>();
        if (actorLevel.IsSuperLevel())
        {
            masterList = (await accountService.GetMasterList(message.CustId)).ToList();
            masterList.Insert(0, new CustInfo(0, CommonResx.DirectMember, 0));
        }

        var masterId = GetDefaultMasterId(message, actorLevel, masterList);
        var agentList = masterId == 0 ? new List<CustInfo>() : (await accountService.GetAgentList(masterId)).ToList();
        if (actorLevel.IsMasterLevel())
        {
            agentList.Insert(0, new CustInfo(0, CommonResx.DirectMember, 0));
        }

        var agentId = GetDefaultAgentId(message, actorLevel, agentList);
        var param = new WinLossLimitParam(
            message.UserSelectedProductId == 0 && firstProduct != null ? firstProduct.ProductId : message.UserSelectedProductId,
            message.CustId,
            message.BetType == 0 && firstProduct != null ? firstProduct.BetType : message.BetType,
            message.SportType == 0 && firstProduct != null ? firstProduct.SportType : message.SportType,
            message.Username,
            message.PlayerId,
            masterId,
            agentId,
            message.PageSize,
            message.PageIndex,
            GetIsDirectMemberParam(actorLevel, masterId, agentId),
            message.AGIP,
            message.AccountType);

        var noNeedGetData = !param.IsDirectMember && actorLevel.IsMasterOrSuperLevel() && !agentList.Any();
        var winLossLimitPage = noNeedGetData ? new WinLossLimitPage() : await winLossLimitService.GetWinLossLimitPage(param);
        var isOfferedCashService = AlphaPlus.Core.Configurations.AppSettings.IsOfferedCashService(message.Site);
        return new WinLossLimitViewModel(isOfferedCashService, message, products, masterList, agentList, winLossLimitPage, param);
    }

    public async Task<Result> Handle(ResetCurrentWinAmountCommand request, CancellationToken cancellation)
    {
        var result = await winLossLimitService.ResetCurrentWinAmount(new ResetCurrentWinAmountParam(
            request.CustId, 
            request.UserId,
            request.Username,
            request.ProductId,
            request.SportType,
            request.BetType,
            request.Language,
            request.SettingStatus));

        if (result.IsSuccessful)
        {
            return Result.CreateSuccessfulResult(AlphaPlus.Shared.AppResources.Settings.MemberInfo.Resources.ResetCurrentWinAmountSuccess);
        }

        return result;
    }

    private static int GetDefaultMasterId(WinLossLimitQuery message, UserLevel actorLevel, List<CustInfo> masterList)
    {
        if (message.MasterId == -1)
        {
            if (actorLevel.IsSuperLevel() && masterList.Count > 1)
            {
                // First item is direct member
                // Second item is the first master
                return masterList[1].CustId;
            }

            if (actorLevel.IsMasterLevel())
            {
                return message.CustId;
            }

            return 0;
        }

        return message.MasterId;
    }

    private static int GetDefaultAgentId(WinLossLimitQuery message, UserLevel actorLevel, List<CustInfo> agentList)
    {
        if (message.RecommendId == -1)
        {
            if (actorLevel.IsMasterLevel() && agentList.Count > 1)
            {
                // First item is direct member
                // Second item is the first agent
                return agentList[1].CustId;
            }

            if (actorLevel.IsAgentLevel())
            {
                return message.CustId;
            }

            return 0;
        }

        return message.RecommendId;
    }

    private static bool GetIsDirectMemberParam(UserLevel actorLevel, int masterId, int agentId)
    {
        if (actorLevel.IsSuperLevel())
        {
            return masterId == 0;
        }

        if (actorLevel.IsMasterLevel())
        {
            return agentId == 0;
        }

        return false;
    }
}