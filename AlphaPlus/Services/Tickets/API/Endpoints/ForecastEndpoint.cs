
using AlphaPlus.BetsForecast.Infrastructure.Forecast;

namespace AlphaPlus.Tickets.API.Endpoints;

public class ForecastEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var forecast = endpoints.MapGroup("/api/forecast");

        forecast.MapGet(
            "/soccerleagues",
            (bool isLive, bool isShowAll, ILeagueRepository repository) => repository.GetSoccerLeagues(isLive, isShowAll));

        forecast.MapGet(
            "/soccermatches",
            (int leagueId, bool isLive, ILeagueRepository repository) => repository.GetSoccerMatches(leagueId, isLive));

        forecast.MapGet(
            "/soccer",
            async (int custId,
            string matchIds,
            bool isLive,
            IForecastRepository repository) =>
            {
                var (matches, betTypes, winloss) = await repository.GetSoccer(custId, matchIds, isLive);

                return Tuple.Create(matches, betTypes, winloss);
            });
    }
}
