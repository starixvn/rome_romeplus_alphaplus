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
    IParlay3rdService parlay3rdService,
    IResultEngineAdapter resultEngineAdapter) : IResultServiceFactory
{
    public IProductResultService CreateProductResultService(int sportId, int betTypeId)
    {
        if (IsMatchParlayTicket())
        {
            return new ParlayResultService(options, parlayService, resultEngineAdapter);
        }

        if (betTypeId == BetTypeConstants.MixParlayVirtuallySports)
        {
            return new Parlay3rdResultService(options, parlay3rdService, resultEngineAdapter);
        }

        return sportId switch
        {
            SportTypeConstants.Soccer => new SportsbookResultService(options, sportsbookService, resultEngineAdapter),
            SportTypeConstants.NumberGame => new NumberGameResultService(options, numberGameService, resultEngineAdapter),
            SportTypeConstants.Happy5 => new Happy5ResultService(options, numberGameService, resultEngineAdapter),
            SportTypeConstants.AllBet => new AllBetCasinoResultService(options),
            SportTypeConstants.SAGaming => new SAGamingResultService(),
            SportTypeConstants.GTCasino => new GTCasinoResultService(),
            SportTypeConstants.WM or SportTypeConstants.YeeBet => new WMResultService(),
            SportTypeConstants.LGVirtualSports => new LeapGamingResultService(resultEngineAdapter),
            SportTypeConstants.Bitcoin => new BitcoinResultService(bitcoinService, resultEngineAdapter),
            SportTypeConstants.VirtuallySports => new VirtuallySportsResultService(options, virtualSportsService, resultEngineAdapter),
            SportTypeConstants.VirtualHorsesRacing or
            SportTypeConstants.VirtualGreyhounds or
            SportTypeConstants.VirtualMotors or
            SportTypeConstants.VirtualCycling or
            SportTypeConstants.VirtualSpeedway => new VirtualRacingResultService(options, virtualRacingService, resultEngineAdapter),            
            _=> betTypeId == BetTypeConstants.BetBuilder ?
                    new BetBuilderResultService(sportsbookService, resultEngineAdapter) : new SportsbookResultService(options, sportsbookService, resultEngineAdapter)
        };

        bool IsMatchParlayTicket()
        => betTypeId == BetTypeConstants.MixParlay
            || betTypeId == BetTypeConstants.CombineMixParlay
            || betTypeId == BetTypeConstants.MixParlayBetRadarVirtualSports
            || betTypeId == BetTypeConstants.MixParlayEdgeGamingVirtualSports;
    }
}