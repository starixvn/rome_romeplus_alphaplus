using AlphaPlus.Core.Helpers;
using AlphaPlus.Shared.AppResources.Betlist.Site;
using AlphaPlus.Tickets.Site.Features.Shared;
using Fanex.Result.Core.ThirdParty;
using Fanex.Result.Core.ThirdParty.Models;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.Results.ThirdParty;

public class ThirdPartyResultHandler(IConfiguration configuration, IBrowserHelper browserHelper, ICacheRefService cacheRefService) :
    IRequestHandler<Result3rdQuery, ThirdPartyProductViewModel>,
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
    IRequestHandler<UUSlotsResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<SabaxDResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<FastSpinResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<FACHAIResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<NextspinResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<ONCasinoResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<PlaytechResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<SexyGamingResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<GPIResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<GPILiveCasinoResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<PlayStarResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<ASKMESLOTResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<SabaKenoResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<FunkyGamesResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<HotDogResultQuery, ThirdPartyProductViewModel>,
    IRequestHandler<MicrogamingLiveResultQuery, ThirdPartyProductViewModel>
{
    private readonly List<int> _memberSiteResultSports = [202, 220, 222];
    public Task<ThirdPartyProductViewModel> Handle(Result3rdQuery message, CancellationToken cancellationToken)
    {
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(message));
        var sportName = cacheRefService.GetSportName(message.SportTypeId);
        var config = GetThirdPartyConfig(message.SportTypeId);
        if (config == null)
        {
            return Task.FromResult(new ThirdPartyProductViewModel(string.Format(Resources.ResultPopupTitle, sportName), string.Empty));
        }

        if(_memberSiteResultSports.Contains(message.SportTypeId))
        {
            message.GameId3rd += "&detail=c&app=" + (browserHelper.IsMobile() ? "S" : "W") + "&EventDate=" + GetDateTime(message.EndTime);
            if(string.IsNullOrWhiteSpace(message.TransId3rd))
            {
                message.TransId3rd = "TransId3rd";
            }
        }
        
        var request = new ResultRequest(message.CustId, message.TransId3rd, message.GameId3rd, message.CurrencyId, message.SiteId, message.Language)
        {
            IsDarkMode = message.IsDarkMode ? "1" : "0",
            Platform = browserHelper.IsMobile() ? "M" : "D",
            BetTypeId = message.BetType,
            EndTime = message.EndTime,
            OwTransId = message.RefNo
        };

        var result = config.IsPostMethod? new ThirdPartyResult(config).GetResultPOST(request): new ThirdPartyResult(config).GetResultGET(request);
        var viewModel = new ThirdPartyProductViewModel(string.Format(Resources.ResultPopupTitle, sportName), result);

        return Task.FromResult(viewModel);

        ThirdPartyConfigs GetThirdPartyConfig(int sportTypeId)
        {
            var configs = configuration.GetSection("Result3rdConfig").Get<List<ThirdPartyConfigs>>();
            return configs.FirstOrDefault(x => x.SportTypeId == sportTypeId);
        }
    }

    public Task<ThirdPartyProductViewModel> Handle(MaxGameResultQuery message, CancellationToken cancellationToken)
        => Handle(message, "MaxGameResultConfigs", "MaxGame");

    public Task<ThirdPartyProductViewModel> Handle(SportsLotteryDetailResultQuery message, CancellationToken cancellationToken)
        => Handle(message, "SportsLotteryDetailResultConfigs", "SportsLotteryDetail");

    public Task<ThirdPartyProductViewModel> Handle(PPLiveCasinoResultQuery message, CancellationToken cancellationToken)
        => Handle(message, "PPLiveCasinoResultConfigs", "PPLiveCasino");

    public Task<ThirdPartyProductViewModel> Handle(HabaneroResultQuery message, CancellationToken cancellationToken)
        => Handle(message, "HabaneroResultConfigs", "Habanero");

    public Task<ThirdPartyProductViewModel> Handle(CGResultQuery message, CancellationToken cancellationToken)
        => Handle(message, "CGResultConfigs", "CG");

    public Task<ThirdPartyProductViewModel> Handle(VGamingResultQuery message, CancellationToken cancellationToken)
        => Handle(message, "VGamingConfigs", "VGaming");

    public Task<ThirdPartyProductViewModel> Handle(AdvantPlayResultQuery message, CancellationToken cancellationToken)
        => Handle(message, "AdvantPlayResultConfigs", "AdvantPlay");

    public Task<ThirdPartyProductViewModel> Handle(AdvantPlayMiniResultQuery message, CancellationToken cancellationToken)
        => Handle(message, "AdvantPlayMiniResultConfigs", "AdvantPlayMini");

    public Task<ThirdPartyProductViewModel> Handle(BBINResultQuery message, CancellationToken cancellationToken)
        => Handle(message, "BBINResultConfigs", "BBIN");

    public Task<ThirdPartyProductViewModel> Handle(RNGCasinoResultQuery message, CancellationToken cancellationToken)
        => Handle(message, "RNGCasinoResultConfigs", "RNGCasino");

    public Task<ThirdPartyProductViewModel> Handle(MacauGamesResultQuery message, CancellationToken cancellationToken)
        => Handle(message, "VoidbridgeResultConfigs", "Voidbridge");

    public Task<ThirdPartyProductViewModel> Handle(PPResultQuery message, CancellationToken cancellationToken)
        => Handle(message, "PPResultConfigs", "PP");

    public Task<ThirdPartyProductViewModel> Handle(SpadeGamingResultQuery message, CancellationToken cancellationToken)
        => Handle(message, "SpadegamingResultConfigs", "Spadegaming");

    public Task<ThirdPartyProductViewModel> Handle(RNGWarResultQuery message, CancellationToken cancellationToken)
        => Handle(message, "RNGWarResultConfigs", "RNGWar");

    public Task<ThirdPartyProductViewModel> Handle(IONCasinoResultQuery message, CancellationToken cancellationToken)
       => Handle(message, "IONCasinoResultConfigs", "IONCasino");

    public Task<ThirdPartyProductViewModel> Handle(PlaytechResultQuery message, CancellationToken cancellationToken)
    {
        var productName = cacheRefService.GetProductName(message.ProductId);
        var request = new ResultRequest(message.CustId, message.TransId3rd, message.GameId3rd, message.CurrencyId, message.SiteId, message.Language)
        {
            IsDarkMode = message.IsDarkMode ? "1" : "0",
            Platform = browserHelper.IsMobile() ? "M" : "D",
            BetTypeId = message.BetType,
            EndTime = message.EndTime
        };

        var result = GetThirdPartyResult("PlaytechResultConfigs", "Playtech").GetResultPOST(request);
        var viewModel = new ThirdPartyProductViewModel(string.Format(Resources.ResultPopupTitle, productName), result);

        return Task.FromResult(viewModel);
    }

    public Task<ThirdPartyProductViewModel> Handle(SexyGamingResultQuery message, CancellationToken cancellationToken)
    {
        var productName = cacheRefService.GetProductName(message.ProductId);
        var request = new ResultRequest(message.CustId, message.TransId3rd, message.GameId3rd, message.CurrencyId, message.SiteId, message.Language)
        {
            Site = message.Site
        };

        var result = GetThirdPartyResult("SexyGamingResultConfigs", "SexyGaming").GetResultGET(request);
        var viewModel = new ThirdPartyProductViewModel(string.Format(Resources.ResultPopupTitle, productName), result);

        return Task.FromResult(viewModel);
    }

    public Task<ThirdPartyProductViewModel> Handle(PGSoftResultQuery message, CancellationToken cancellationToken)
        => Handle(message, "PGSoftResultConfigs", "PGSoft");

    public Task<ThirdPartyProductViewModel> Handle(JokerResultQuery message, CancellationToken cancellationToken)
        => Handle(message, "JokerResultConfigs", "Joker");

    public Task<ThirdPartyProductViewModel> Handle(LinkedGamesResultQuery message, CancellationToken cancellationToken)
    {
        var title = $"{cacheRefService.GetSportName(message.SportTypeId)} - {cacheRefService.GetBetTypeName(message.BetTypeId)}";
        var request = new ResultRequest(message.CustId, message.TransId3rd, message.GameId3rd, message.CurrencyId, message.SiteId, message.Language)
        {
            IsDarkMode = message.IsDarkMode ? "1" : "0",
            Platform = browserHelper.IsMobile() ? "M" : string.Empty
        };

        var result = GetThirdPartyResult("LinkedGamesResultConfigs", "LinkedGames").GetResultGET(request);
        var viewModel = new ThirdPartyProductViewModel(title, result);

        return Task.FromResult(viewModel);
    }

    public Task<ThirdPartyProductViewModel> Handle(BGResultQuery message, CancellationToken cancellationToken)
        => Handle(message, "BGResultConfigs", "BG");

    public Task<ThirdPartyProductViewModel> Handle(CardClubResultQuery message, CancellationToken cancellationToken)
        => Handle(message, "CardClubResultConfigs", "CardClub");

    public Task<ThirdPartyProductViewModel> Handle(FGGResultQuery message, CancellationToken cancellationToken)
        => Handle(message, "FGGResultConfigs", "FGG");

    public Task<ThirdPartyProductViewModel> Handle(VirtualGamesDetailResultQuery message, CancellationToken cancellationToken)
        => Handle(message, "VirtualGamesDetailResultConfigs", "VirtualGamesDetail");

    public Task<ThirdPartyProductViewModel> Handle(SEALResultQuery message, CancellationToken cancellationToken)
        => Handle(message, "SEALResultConfigs", "SEAL");

    public Task<ThirdPartyProductViewModel> Handle(MGResultQuery message, CancellationToken cancellationToken)
        => Handle(message, "MGResultConfigs", "MG");

    public Task<ThirdPartyProductViewModel> Handle(JiliResultQuery message, CancellationToken cancellationToken)
        => Handle(message, "JiliResultConfigs", "Jili");

    public Task<ThirdPartyProductViewModel> Handle(Live22ResultQuery message, CancellationToken cancellationToken)
        => Handle(message, "Live22ResultConfigs", "Live22");

    public Task<ThirdPartyProductViewModel> Handle(WEResultQuery message, CancellationToken cancellationToken)
        => Handle(message, "WEResultConfigs", "WE");

    public Task<ThirdPartyProductViewModel> Handle(UUSlotsResultQuery message, CancellationToken cancellationToken)
        => Handle(message, "UUSlotsResultConfigs", "UUSlots");

    public Task<ThirdPartyProductViewModel> Handle(SabaxDResultQuery message, CancellationToken cancellationToken)
        => Handle(message, "SabaxDResultConfigs", "SabaxD");

    public Task<ThirdPartyProductViewModel> Handle(FastSpinResultQuery message, CancellationToken cancellationToken)
        => Handle(message, "FastSpinResultConfigs", "FastSpin");

    public Task<ThirdPartyProductViewModel> Handle(FACHAIResultQuery message, CancellationToken cancellationToken)
        => Handle(message, "FACHAIResultConfigs", "FACHAI");

    public Task<ThirdPartyProductViewModel> Handle(NextspinResultQuery message, CancellationToken cancellationToken)
        => Handle(message, "NextspinResultConfigs", "Nextspin");

    public Task<ThirdPartyProductViewModel> Handle(ONCasinoResultQuery message, CancellationToken cancellationToken)
        => Handle(message, "ONCasinoResultConfigs", "ONCasino");

    public Task<ThirdPartyProductViewModel> Handle(GPIResultQuery message, CancellationToken cancellationToken)
        => Handle(message, "GPIRNGResultConfigs", "GPI Casino");

    public Task<ThirdPartyProductViewModel> Handle(GPILiveCasinoResultQuery message, CancellationToken cancellationToken)
        => Handle(message, "GPILiveCasinoResultConfigs", "GPI Live Casino");

    public Task<ThirdPartyProductViewModel> Handle(PlayStarResultQuery message, CancellationToken cancellationToken)
        => Handle(message, "PlayStarResultConfigs", "PLAYSTAR");

    public Task<ThirdPartyProductViewModel> Handle(ASKMESLOTResultQuery message, CancellationToken cancellationToken)
        => Handle(message, "ASKMESLOTResultConfigs", "ASKMESLOT");

    public Task<ThirdPartyProductViewModel> Handle(FunkyGamesResultQuery message, CancellationToken cancellationToken)
        => Handle(message, "FunkyGamesResultConfigs", "Funky Games");

    public Task<ThirdPartyProductViewModel> Handle(SabaKenoResultQuery message, CancellationToken cancellationToken)
        => Handle(message, "SabaKenoResultConfigs", "SABA Keno");

    public Task<ThirdPartyProductViewModel> Handle(HotDogResultQuery message, CancellationToken cancellationToken)
        => Handle(message, "HotDogResultConfigs", "HOTDOG");

    public Task<ThirdPartyProductViewModel> Handle(MicrogamingLiveResultQuery message, CancellationToken cancellationToken)
        => Handle(message, "MicrogamingLiveConfigs", "Microgaming Live");

    private Task<ThirdPartyProductViewModel> Handle(Result3rdRequest message, string sectionName, string gameName)
    {
        var productName = cacheRefService.GetProductName(message.ProductId);
        var request = new ResultRequest(message.CustId, message.TransId3rd, message.GameId3rd, message.CurrencyId, message.SiteId, message.Language)
        {
            IsDarkMode = message.IsDarkMode ? "1" : "0",
            Platform = browserHelper.IsMobile() ? "M" : "D",
            BetTypeId = message.BetType,
            EndTime = message.EndTime
        };

        var result = GetThirdPartyResult(sectionName, gameName).GetResultGET(request);
        var viewModel = new ThirdPartyProductViewModel(string.Format(Resources.ResultPopupTitle, productName), result);

        return Task.FromResult(viewModel);
    }

    private IThirdPartyResult GetThirdPartyResult(string sectionName, string gameName)
    {
        var configs = configuration.GetSection(sectionName).Get<ThirdPartyConfigs>();
        configs.Name = gameName;

        return new ThirdPartyResult(configs);
    }

    private string GetDateTime(string endTime)
    {
        if(string.IsNullOrWhiteSpace(endTime))
        {
            return string.Empty;
        }

        var dateTime = DateTime.Parse(endTime);
        return dateTime.ToString("yyyyMMdd");
    }
}