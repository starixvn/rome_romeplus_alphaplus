using AlphaPlus.Settings.MemberInfo.API.Services.WinLossLimit;
using AlphaPlus.Settings.MemberInfo.Domain.Params;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests.WinLossLimit;
using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.WinLossLimit;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Handlers;

public class SportsbookPotentialWinLimitHandler(IPotentialWinLimitService potentialWinLimitService) : 
    IRequestHandler<SportsbookPotentialWinLimitQuery, SportsbookPotentialWinLimitViewModel>
{
    private readonly IPotentialWinLimitService potentialWinLimitService = potentialWinLimitService;

    public async Task<SportsbookPotentialWinLimitViewModel> Handle(SportsbookPotentialWinLimitQuery message, CancellationToken cancellation)
    {
        var param = new PotentialWinLimitParam(
            message.CustId,
            message.LimitedDateToShowInfo,
            message.Username,
            message.Status,
            message.ProblemAccount,
            message.PageSize,
            message.PageIndex,
            message.AGIP);

        var potentialWinLimitPage = await potentialWinLimitService.GetPotentialWinLimitPage(param);

        return new SportsbookPotentialWinLimitViewModel(message, potentialWinLimitPage, param);
    }
} 