namespace AlphaPlus.Settings.Site.Features.MemberInfo.Handlers
{
    using System.Linq;
    using AlphaPlus.Settings.MemberInfo.API.Services.WinLossLimit;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using AlphaPlus.Settings.MemberInfo.Domain.Models.WinLossLimit;
    using AlphaPlus.Settings.MemberInfo.Domain.Params;
    using MediatR;
    using Requests.WinLossLimit;
    using ViewModels.WinLossLimit;

    public class WinLossLimitHandler :
        IRequestHandler<WinLossLimitQuery, WinLossLimitViewModel>
    {
        private readonly WinLossLimitService service;

        public WinLossLimitHandler(WinLossLimitService service)
        {
            this.service = service;
        }

        public async Task<WinLossLimitViewModel> Handle(WinLossLimitQuery message, CancellationToken cancellation)
        {
            var products = service.GetWinLossLimitProducts(message.CustId).ToList();
            var firstProduct = products?.FirstOrDefault()?.Products?.FirstOrDefault();

            var masterList = message.ActorLevel.IsSuperLevel() ? service.GetMasterList(message.CustId) : Enumerable.Empty<CustomerInfo>();
            var actorLevel = message.ActorLevel;
            var masterId = message.MasterId != 0 ? message.MasterId
                : actorLevel.IsSuperLevel() ? masterList.FirstOrDefault()?.CustId
                : actorLevel.IsMasterLevel() ? message.CustId
                : 0;

            var agentList = masterId == null ? Enumerable.Empty<CustomerInfo>() : service.GetAgentList(masterId.Value);
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

            var winLossLimitPage = noNeedGetData ? new WinLossLimitPage() : service.GetWinLossLimitPage(param);

            var viewModel = new WinLossLimitViewModel(message, products, masterList, agentList, winLossLimitPage, param);

            return viewModel;
        }
    }
}