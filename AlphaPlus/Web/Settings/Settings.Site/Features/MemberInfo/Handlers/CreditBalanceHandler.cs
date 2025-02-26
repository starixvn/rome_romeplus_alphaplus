using AlphaPlus.Settings.MemberInfo.API.Services;
using AlphaPlus.Settings.MemberInfo.API.Services.CreditBalance;
using AlphaPlus.Settings.MemberInfo.API.Services.Transfer;
using AlphaPlus.Settings.MemberInfo.Domain.Params;
using MediatR;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests.CreditBalance;
using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.CreditBalance;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Handlers
{
    public class CreditBalanceHandler
        : IRequestHandler<CreditBalanceQuery, CreditBalanceViewModel>
    {
        private readonly CreditBalanceService creditBalanceService;
        private readonly TransferService transferService;
        private readonly IWinlossService winlossService;

        public CreditBalanceHandler(
            CreditBalanceService creditBalanceService,
            TransferService transferService,
            IWinlossService winlossService)
        {
            this.creditBalanceService = creditBalanceService;
            this.transferService = transferService;
            this.winlossService = winlossService;
        }

        public async Task<CreditBalanceViewModel> Handle(CreditBalanceQuery message, CancellationToken cancellation)
        {
            var param = new CreditBalanceParam(
                message.CustId,
                message.CustLevel,
                message.UserName,
                message.StatusId.Value,
                message.PageIndex,
                message.PageSize,
                message.IsAdminCanViewVNIP,
                message.AGIP);

            var pagedCreditBalances = await creditBalanceService.GetPagedCreditBalancesAsync(param);

            var transferCondition = message.ActorLevel.IsAgentLevel()
                ? await transferService.GetAllowTransfer(message.ActorId)
                : null;

            var reportEntered = winlossService.GetEnteredSportResults();
            var isAllDayBalTransfer = await transferService.IsAllDayBalanceTransferAsync(message.ActorId);

            return new CreditBalanceViewModel(pagedCreditBalances, transferCondition, message, reportEntered, isAllDayBalTransfer);
        }
    }
}