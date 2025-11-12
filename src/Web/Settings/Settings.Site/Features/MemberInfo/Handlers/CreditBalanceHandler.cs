using AlphaPlus.Settings.MemberInfo.API.Services;
using AlphaPlus.Settings.MemberInfo.API.Services.CreditBalance;
using AlphaPlus.Settings.MemberInfo.API.Services.Transfer;
using AlphaPlus.Settings.MemberInfo.Domain.Params;
using MediatR;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests.CreditBalance;
using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.CreditBalance;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Handlers;

public class CreditBalanceHandler(
    CreditBalanceService creditBalanceService,
    TransferService transferService,
    IWinlossService winlossService) : IRequestHandler<CreditBalanceQuery, CreditBalanceViewModel>
{
    private readonly CreditBalanceService creditBalanceService = creditBalanceService;
    private readonly TransferService transferService = transferService;
    private readonly IWinlossService winlossService = winlossService;

    public async Task<CreditBalanceViewModel> Handle(CreditBalanceQuery message, CancellationToken cancellation)
    {
        var param = new CreditBalanceParam(
            message.CustId,
            message.CustLevel,
            message.UserName,
            message.StatusId.Value,
            message.LevelType,
            message.PageIndex,
            message.PageSize,
            message.IsAdminCanViewVNIP,
            message.AGIP);

        var pagedCreditBalancesTask = creditBalanceService.GetPagedCreditBalancesAsync(param);
        var transferConditionTask = message.ActorLevel.IsAgentLevel()
            ? transferService.GetAllowTransfer(message.ViewerId)
            : Task.FromResult<string>(null);
        var reportEnteredTask = winlossService.GetEnteredSportResultsAsync();
        var isAllDayBalTransferTask = transferService.IsAllDayBalanceTransferAsync(message.ViewerId);
        await Task.WhenAll(pagedCreditBalancesTask, transferConditionTask, reportEnteredTask, isAllDayBalTransferTask);

        return new CreditBalanceViewModel(await pagedCreditBalancesTask, await transferConditionTask, message, await reportEnteredTask, await isAllDayBalTransferTask);
    }
}