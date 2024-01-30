
using AlphaPlus.BetsForecast.Infrastructure.Forecast;
using AlphaPlus.BetsForecast.Infrastructure.Forecast.Params;

namespace AlphaPlus.Tickets.API.Endpoints;

public class ForecastEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var forecast = endpoints.MapGroup("/api/forecast");

        forecast.MapGet(
            "/soccer/leagues",
            (bool isLive, bool isShowAll, string language, ISoccerService service)
            => service.GetLeagues(isLive, isShowAll, language));

        forecast.MapGet(
            "/soccer/matches",
            (int leagueId, bool isLive, string language, ISoccerService service)
            => service.GetMatches(leagueId, isLive, language));

        forecast.MapGet(
            "/soccer/results",
            (int custId, string matchIds, bool isLive, string language, ISoccerService service)
            => service.GetResults(custId, matchIds, isLive, language));

        forecast.MapGet(
            "/runningleagues",
            ([AsParameters] GetRunningLeaguesParam param, IForecastService service) => service.GetRunningLeagues(param));

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
