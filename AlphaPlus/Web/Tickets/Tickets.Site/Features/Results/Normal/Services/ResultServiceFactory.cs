using AlphaPlus.Results.Features.Normal;
using AlphaPlus.Tickets.Site.Features.Shared.Constants;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Tickets.Site.Features.Results.Normal;

public interface IResultServiceFactory
{
    IProductResultService CreateProductResultService(int sportId, int betTypeId);
}

public class ResultServiceFactory(
    IOptions<AppSettings> options,
    IParlayService parlayService,
    ISportsbookService sportsbookService,
    INumberGameService numberGameService,
    IVirtualRacingService virtualRacingService,
    IVirtualSportsService virtualSportsService,
    IBitcoinService bitcoinService,
    IResultEngineAdapter resultEngineAdapter) : IResultServiceFactory
{
    public IProductResultService CreateProductResultService(int sportId, int betTypeId)
    {
        if (betTypeId == BetTypeConstants.MixParlay
            || betTypeId == BetTypeConstants.CombineMixParlay
            || betTypeId == BetTypeConstants.MixParlayBetRadarVirtualSports
            || betTypeId == BetTypeConstants.MixParlayEdgeGamingVirtualSports)
        {
            return new ParlayResultService(options, parlayService, resultEngineAdapter);
        }

        switch (sportId)
        {
            case SportTypeConstants.Soccer:
                return new SportsbookResultService(options, sportsbookService, resultEngineAdapter);

            case SportTypeConstants.NumberGame:
                return new NumberGameResultService(options, numberGameService, resultEngineAdapter);

            case SportTypeConstants.Happy5:
                return new Happy5ResultService(options, numberGameService, resultEngineAdapter);

            case SportTypeConstants.AllBet:
                return new AllBetCasinoResultService(options);

            case SportTypeConstants.SAGaming:
                return new SAGamingResultService();

            case SportTypeConstants.GTCasino:
                return new GTCasinoResultService();

            case SportTypeConstants.WM:
            case SportTypeConstants.YeeBet:
                return new WMResultService();

            case SportTypeConstants.VirtualHorsesRacing:
            case SportTypeConstants.VirtualGreyhounds:
            case SportTypeConstants.VirtualMotors:
            case SportTypeConstants.VirtualCycling:
            case SportTypeConstants.VirtualSpeedway:
                return new VirtualRacingResultService(options, virtualRacingService, resultEngineAdapter);

            case SportTypeConstants.LGVirtualSports:
                return new LeapGamingResultService(resultEngineAdapter);

            case SportTypeConstants.Bitcoin:
                return new BitcoinResultService(bitcoinService, resultEngineAdapter);

             case SportTypeConstants.VirtuallySports:
                    return new VirtuallySportsResultService(options, virtualSportsService, resultEngineAdapter);
            default:
                if (betTypeId == BetTypeConstants.BetBuilder)
                {
                    return new BetBuilderResultService(sportsbookService, resultEngineAdapter);
                }

                return new SportsbookResultService(options, sportsbookService, resultEngineAdapter);
        }
    }
}