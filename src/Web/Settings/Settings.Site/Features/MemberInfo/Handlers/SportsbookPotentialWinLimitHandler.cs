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

public class SportsbookPotentialWinLimitHandler(IPotentialWinLimitService potentialWinLimitService) : 
    IRequestHandler<SportsbookPotentialWinLimitQuery, SportsbookPotentialWinLimitViewModel>
{
    private readonly IPotentialWinLimitService potentialWinLimitService = potentialWinLimitService;

    public async Task<SportsbookPotentialWinLimitViewModel> Handle(SportsbookPotentialWinLimitQuery message, CancellationToken cancellation)
    {
        var param = new PotentialWinLimitParam(
            message.CustId,
            0,
            0,
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