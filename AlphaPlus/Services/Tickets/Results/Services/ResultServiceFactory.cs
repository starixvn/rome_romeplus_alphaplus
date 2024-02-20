namespace AlphaPlus.Results.Services;

public interface IResultServiceFactory
{
    IProductResultService CreateProductResultService(int sportId, int betTypeId);
}

public class ResultServiceFactory : IResultServiceFactory
{
    private const int MetricGaming60SecondOfKick = 801;
    private const int MetricGamingEndBetType = 899;

    public IProductResultService CreateProductResultService(int sportId, int betTypeId)
    {
        if (betTypeId == BetTypeConstants.MixParlay
        || betTypeId == BetTypeConstants.CombineMixParlay
        || betTypeId == BetTypeConstants.MixParlayBetRadarVirtualSports)
        {
            return new ParlayResultService();
        }

        switch (sportId)
        {
            case SportTypeConstants.Soccer:
                return MetricGaming60SecondOfKick <= betTypeId && betTypeId <= MetricGamingEndBetType
                    ? new MetricGamingResultService()
                    : new SportsbookResultService();

            case SportTypeConstants.NumberGame:
                return new NumberGameResultService();

            case SportTypeConstants.Happy5:
                return new Happy5ResultService();

            case SportTypeConstants.AllBet:
                return new AllBetCasinoResultService();

            case SportTypeConstants.SAGaming:
                return new SAGamingResultService();

            case SportTypeConstants.SexyGaming:
                return new SexyGamingResultService();

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
                return new VirtualRacingResultService();

            case SportTypeConstants.LGVirtualSports:
                return new LeapGamingResultService();

            case SportTypeConstants.Bitcoin:
                return new BitcoinResultService();

            default:
                return betTypeId == BetTypeConstants.BetBuilder
                    ? new BetBuilderResultService()
                    : new SportsbookResultService();
        }
    }
}