
using AlphaPlus.BetsForecast.Infrastructure.Forecast;

namespace AlphaPlus.Tickets.API.Endpoints;

public class ForecastEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var forecast = endpoints.MapGroup("/api/forecast");

        forecast.MapGet(
            "/soccerleagues",
            (bool isLive, bool isShowAll, string language, ILeagueService service)
            => service.GetSoccerLeagues(isLive, isShowAll, language));

        forecast.MapGet(
            "/soccermatches",
            (int leagueId, bool isLive, string language, ILeagueService service)
            => service.GetSoccerMatches(leagueId, isLive, language));

        forecast.MapGet(
            "/soccer",
            async (int custId, string matchIds, bool isLive, string language, IForecastService service) =>
            {
                var (matches, betTypes, winloss) = await service.GetSoccer(custId, matchIds, isLive);

                return Tuple.Create(matches, betTypes, winloss);
            });

        forecast.MapGet(
            "/runningleagues",
            (byte custLevel, int custId, string betTypes, string language, ILeagueService service)
            => service.GetRunningLeagues(custLevel, custId, betTypes, language));

        forecast.MapGet(
            "/hdpoulive",
            (byte custLevel, int custId, int leagueId, string language, IForecastService service)
            => service.GetHDPOULive(custLevel, custId, leagueId, language));

        forecast.MapGet(
            "/1x2",
            (byte custLevel, int custId, int leagueId, string language, IForecastService service)
            => service.Get1X2(custLevel, custId, leagueId, language));

        forecast.MapGet(
            "/1x2bymatch",
            (byte custLevel, int custId, int matchId, int betType, string language, IForecastService service)
            => service.Get1X2(custLevel, custId, matchId, betType, language));
    }
}
