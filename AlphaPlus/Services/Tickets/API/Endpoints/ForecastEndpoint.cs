
using AlphaPlus.BetsForecast.Infrastructure.Forecast;
using AlphaPlus.BetsForecast.Infrastructure.Forecast.Params;

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
            ([AsParameters] GetRunningLeaguesParam param, ILeagueService service) => service.GetRunningLeagues(param));

        forecast.MapGet(
            "/hdpoulive",
            ([AsParameters] GetForecastParam param, IForecastService service) => service.GetHDPOULive(param));

        forecast.MapGet(
            "/ah",
            ([AsParameters] GetAHParam param, IForecastService service) => service.GetAH(param));

        forecast.MapGet(
            "/1x2",
            ([AsParameters] GetForecastParam param, IForecastService service) => service.Get1X2(param));

        forecast.MapGet(
            "/1x2bymatch",
            ([AsParameters] Get1X2ByMatchParam param, IForecastService service) => service.Get1X2(param));

        forecast.MapGet(
            "/scoremap/bettypes",
            (string language, IScoreMapService service) => service.GetBetTypes(language));

        forecast.MapGet(
            "/scoremap/leagues",
            (int custId, string language, IScoreMapService service) => service.GetLeagues(custId, language));

        forecast.MapGet(
            "/scoremap/matches",
            ([AsParameters] GetScoreMapMatchesParam param, IScoreMapService service) => service.GetMatches(param));

        forecast.MapGet(
            "/scoremap/results",
            ([AsParameters] GetScoreMapResultParam param, IScoreMapService service) => service.GetResults(param));
    }
}
