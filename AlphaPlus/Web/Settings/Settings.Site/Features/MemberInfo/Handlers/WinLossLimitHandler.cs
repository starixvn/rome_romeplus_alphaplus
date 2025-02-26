using AlphaPlus.Settings.MemberInfo.API.Services;
using AlphaPlus.Settings.MemberInfo.API.Services.WinLossLimit;
using AlphaPlus.Settings.MemberInfo.Domain.Models.WinLossLimit;
using AlphaPlus.Settings.MemberInfo.Domain.Params;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests.WinLossLimit;
using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.WinLossLimit;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Handlers;

public class WinLossLimitHandler(
    IAccountService accountService,
    WinLossLimitService winLossLimitService) : IRequestHandler<WinLossLimitQuery, WinLossLimitViewModel>
{
    private readonly IAccountService accountService = accountService;
    private readonly WinLossLimitService winLossLimitService = winLossLimitService;

    public async Task<WinLossLimitViewModel> Handle(WinLossLimitQuery message, CancellationToken cancellation)
    {
        var products = await winLossLimitService.GetWinLossLimitProducts(message.CustId);
        var firstProduct = products?.FirstOrDefault()?.Products?.FirstOrDefault();

        var masterList = message.ActorLevel.IsSuperLevel() ? await accountService.GetMasterList(message.CustId) : [];
        var actorLevel = message.ActorLevel;
        var masterId = message.MasterId != 0 ? message.MasterId
            : actorLevel.IsSuperLevel() ? masterList.FirstOrDefault()?.CustId
            : actorLevel.IsMasterLevel() ? message.CustId
            : 0;

        var agentList = masterId == null ? [] : await accountService.GetAgentList(masterId.Value);
        var agentId = message.AgentId != 0 ? message.AgentId
            : actorLevel.IsMasterLevel() ? agentList.FirstOrDefault()?.CustId
            : actorLevel.IsAgentLevel() ? message.CustId
            : 0;

        var param = new WinLossLimitParam(
            message.UserSelectedProductId == 0 && firstProduct != null ? firstProduct.ProductId : message.UserSelectedProductId,
            message.CustId,
            message.BetType == 0 && firstProduct != null ? firstProduct.BetType : message.BetType,
            message.SportType == 0 && firstProduct != null ? firstProduct.SportType : message.SportType,
            message.Username,
            message.PlayerId,
            masterId ?? 0,
            agentId ?? 0,
            message.PageSize,
            message.PageIndex,
            message.AGIP);

        var noNeedGetData = actorLevel.IsMasterOrSuperLevel() && !agentList.Any();

        var winLossLimitPage = noNeedGetData ? new WinLossLimitPage() : await winLossLimitService.GetWinLossLimitPage(param);

        var viewModel = new WinLossLimitViewModel(message, products, masterList, agentList, winLossLimitPage, param);

        return viewModel;
    }
}