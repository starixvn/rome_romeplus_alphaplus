using AlphaPlus.Settings.MemberInfo.API.Services.WinLossLimit;
using AlphaPlus.Settings.MemberInfo.Domain.Params;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests.WinLossLimit;
using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.WinLossLimit;
using MediatR;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Handlers;

public class SportsbookPotentialWinLimitHandler(IPotentialWinLimitService potentialWinLimitService) : 
    IRequestHandler<SportsbookPotentialWinLimitQuery, SportsbookPotentialWinLimitViewModel>,
    IRequestHandler<ResetCurrentPotentialWinAmountCommand, Result>
{
    private readonly IPotentialWinLimitService potentialWinLimitService = potentialWinLimitService;

    public async Task<SportsbookPotentialWinLimitViewModel> Handle(SportsbookPotentialWinLimitQuery message, CancellationToken cancellation)
    {
        var param = new PotentialWinLimitParam(
            message.CustId,
            message.CustLevelId,
            message.Username,
            message.Status,
            message.ProblemAccount,
            message.PageSize,
            message.PageIndex,
            message.AGIP,
            message.AccountType);

        var potentialWinLimitPage = await potentialWinLimitService.GetPotentialWinLimitPage(param);

        var isOfferedCashService = AlphaPlus.Core.Configurations.AppSettings.IsOfferedCashService(message.Site);
        return new SportsbookPotentialWinLimitViewModel(isOfferedCashService, message, potentialWinLimitPage, param);
    }

    public async Task<Result> Handle(ResetCurrentPotentialWinAmountCommand request, CancellationToken cancellation)
    {
        var result = await potentialWinLimitService.ResetCurrentPotentialWinAmount(
            new ResetCurrentPotentialWinAmountParam(
                request.CustId,
                request.Username,
                request.ProductId,
                request.SportType,
                request.BetType,
                request.Language,
                request.SettingStatus
            )
        );
        return result;
    }
} 