using AlphaPlus.Core.Helpers;
using AlphaPlus.Tickets.Site.App_GlobalResources;
using AlphaPlus.Tickets.Site.Features.Shared;
using Fanex.Result.Core.ThirdParty;
using Fanex.Result.Core.ThirdParty.Models;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class ThirdPartyResultHandler(IBrowserHelper browserHelper) :
    IRequestHandler<MaxGameResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<SportsLotteryDetailResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<PPLiveCasinoResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<HabaneroResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<CGResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<VGamingResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<AdvantPlayResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<AdvantPlayMiniResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<BBINResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<RNGCasinoResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<MacauGamesResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<PPResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<SpadeGamingResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<RNGWarResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<IONCasinoResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<PGSoftResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<JokerResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<LinkedGamesResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<BGResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<CardClubResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<FGGResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<VirtualGamesDetailResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<SEALResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<MGResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<JiliResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<Live22ResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<WEResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<UUSlotsResultQuery, ThirdPartyProductViewModel>
{
    public Task<ThirdPartyProductViewModel> Handle(MaxGameResultQuery message, CancellationToken cancellationToken)
        => Handle(message, new ThirdPartyResult("MaxGameResultConfigs"));

    public Task<ThirdPartyProductViewModel> Handle(SportsLotteryDetailResultQuery message, CancellationToken cancellationToken)
        => Handle(message, new ThirdPartyResult("SportsLotteryDetailResultConfigs"));

    public Task<ThirdPartyProductViewModel> Handle(PPLiveCasinoResultQuery message, CancellationToken cancellationToken)
        => Handle(message, new ThirdPartyResult("PPLiveCasinoResultConfigs"));

    public Task<ThirdPartyProductViewModel> Handle(HabaneroResultQuery message, CancellationToken cancellationToken)
        => Handle(message, new ThirdPartyResult("HabaneroResultConfigs"));

    public Task<ThirdPartyProductViewModel> Handle(CGResultQuery message, CancellationToken cancellationToken)
        => Handle(message, new ThirdPartyResult("CGResultConfigs"));

    public Task<ThirdPartyProductViewModel> Handle(VGamingResultQuery message, CancellationToken cancellationToken)
        => Handle(message, new ThirdPartyResult("VGamingConfigs"));

    public Task<ThirdPartyProductViewModel> Handle(AdvantPlayResultQuery message, CancellationToken cancellationToken)
        => Handle(message, new ThirdPartyResult("AdvantPlayResultConfigs"));

    public Task<ThirdPartyProductViewModel> Handle(AdvantPlayMiniResultQuery message, CancellationToken cancellationToken)
        => Handle(message, new ThirdPartyResult("AdvantPlayMiniResultConfigs"));

    public Task<ThirdPartyProductViewModel> Handle(BBINResultQuery message, CancellationToken cancellationToken)
        => Handle(message, new ThirdPartyResult("BBINResultConfigs"));

    public Task<ThirdPartyProductViewModel> Handle(RNGCasinoResultQuery message, CancellationToken cancellationToken)
        => Handle(message, new ThirdPartyResult("RNGCasinoResultConfigs"));

    public Task<ThirdPartyProductViewModel> Handle(MacauGamesResultQuery message, CancellationToken cancellationToken)
        => Handle(message, new ThirdPartyResult("VoidbridgeResultConfigs"));

    public Task<ThirdPartyProductViewModel> Handle(PPResultQuery message, CancellationToken cancellationToken)
        => Handle(message, new ThirdPartyResult("PPResultConfigs"));

    public Task<ThirdPartyProductViewModel> Handle(SpadeGamingResultQuery message, CancellationToken cancellationToken)
        => Handle(message, new ThirdPartyResult("SpadegamingResultConfigs"));

    public Task<ThirdPartyProductViewModel> Handle(RNGWarResultQuery message, CancellationToken cancellationToken)
        => Handle(message, new ThirdPartyResult("RNGWarResultConfigs"));

    public Task<ThirdPartyProductViewModel> Handle(IONCasinoResultQuery message, CancellationToken cancellationToken)
        => Handle(message, new ThirdPartyResult("IONCasinoResultConfigs"));

    public Task<ThirdPartyProductViewModel> Handle(PGSoftResultQuery message, CancellationToken cancellationToken)
        => Handle(message, new ThirdPartyResult("PGSoftResultConfigs"));

    public Task<ThirdPartyProductViewModel> Handle(JokerResultQuery message, CancellationToken cancellationToken)
        => Handle(message, new ThirdPartyResult("JokerResultConfigs"));

    public Task<ThirdPartyProductViewModel> Handle(LinkedGamesResultQuery message, CancellationToken cancellationToken)
    {
        var title = $"{CacheRefServiceHelper.GetSportName(message.SportTypeId)} - {CacheRefServiceHelper.GetBetTypeName(message.BetTypeId)}";
        var request = new ResultRequest(message.CustId, message.TransId3rd, message.GameId3rd, message.CurrencyId, message.SiteId, message.Language)
        {
            IsDarkMode = message.IsDarkMode ? "1" : "0",
            Platform = browserHelper.IsMobile() ? "M" : string.Empty
        };

        var result = new ThirdPartyResult("LinkedGamesResultConfigs").GetResultGET(request);
        var viewModel = new ThirdPartyProductViewModel(title, result);

        return Task.FromResult(viewModel);
    }

    public Task<ThirdPartyProductViewModel> Handle(BGResultQuery message, CancellationToken cancellationToken)
        => Handle(message, new ThirdPartyResult("BGResultConfigs"));

    public Task<ThirdPartyProductViewModel> Handle(CardClubResultQuery message, CancellationToken cancellationToken)
        => Handle(message, new ThirdPartyResult("CardClubResultConfigs"));

    public Task<ThirdPartyProductViewModel> Handle(FGGResultQuery message, CancellationToken cancellationToken)
        => Handle(message, new ThirdPartyResult("FGGResultConfigs"));

    public Task<ThirdPartyProductViewModel> Handle(VirtualGamesDetailResultQuery message, CancellationToken cancellationToken)
        => Handle(message, new ThirdPartyResult("VirtualGamesDetailResultConfigs"));

    public Task<ThirdPartyProductViewModel> Handle(SEALResultQuery message, CancellationToken cancellationToken)
        => Handle(message, new ThirdPartyResult("SEALResultConfigs"));

    public Task<ThirdPartyProductViewModel> Handle(MGResultQuery message, CancellationToken cancellationToken)
        => Handle(message, new ThirdPartyResult("MGResultConfigs"));

    public Task<ThirdPartyProductViewModel> Handle(JiliResultQuery message, CancellationToken cancellationToken)
        => Handle(message, new ThirdPartyResult("JiliResultConfigs"));

    public Task<ThirdPartyProductViewModel> Handle(Live22ResultQuery message, CancellationToken cancellationToken)
        => Handle(message, new ThirdPartyResult("Live22ResultConfigs"));

    public Task<ThirdPartyProductViewModel> Handle(WEResultQuery message, CancellationToken cancellationToken)
        => Handle(message, new ThirdPartyResult("WEResultConfigs"));

    public Task<ThirdPartyProductViewModel> Handle(UUSlotsResultQuery message, CancellationToken cancellationToken)
        => Handle(message, new ThirdPartyResult("UUSlotsResultConfigs"));

    private Task<ThirdPartyProductViewModel> Handle(Result3rdQuery message, IThirdPartyResult resultService)
    {
        var productName = CacheRefServiceHelper.GetProductName(message.ProductId);
        var request = new ResultRequest(message.CustId, message.TransId3rd, message.GameId3rd, message.CurrencyId, message.SiteId, message.Language)
        {
            IsDarkMode = message.IsDarkMode ? "1" : "0",
            Platform = browserHelper.IsMobile() ? "M" : "D",
            BetTypeId = message.BetType,
            EndTime = message.EndTime
        };

        var result = resultService.GetResultGET(request);
        var viewModel = new ThirdPartyProductViewModel(string.Format(Resources.ResultPopupTitle, productName), result);

        return Task.FromResult(viewModel);
    }
}