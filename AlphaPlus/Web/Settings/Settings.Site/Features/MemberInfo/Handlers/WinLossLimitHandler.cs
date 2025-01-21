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
    : IRequestHandler<WinLossLimitQuery, WinLossLimitViewModel>
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

        var masterId = message.MasterId != 0 ? message.MasterId
            : actorLevel.IsSuperLevel() ? masterList[0].CustId
            : actorLevel.IsMasterLevel() ? message.CustId
            : 0;
        var agentList = masterId == 0 ? new List<CustInfo>() : (await accountService.GetAgentList(masterId)).ToList();
        if (actorLevel.IsMasterLevel())
        {
            agentList.Insert(0, new CustInfo(0, CommonResx.DirectMember, 0));
        }

        var agentId = message.RecommendId != 0 ? message.RecommendId
            : actorLevel.IsMasterLevel() ? agentList[0].CustId
            : actorLevel.IsAgentLevel() ? message.CustId
            : 0;
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
            message.AGIP);

        var noNeedGetData = !param.IsDirectMember && actorLevel.IsMasterOrSuperLevel() && !agentList.Any();
        var winLossLimitPage = noNeedGetData ? new WinLossLimitPage() : await winLossLimitService.GetWinLossLimitPage(param);
        return new WinLossLimitViewModel(message, products, masterList, agentList, winLossLimitPage, param);
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