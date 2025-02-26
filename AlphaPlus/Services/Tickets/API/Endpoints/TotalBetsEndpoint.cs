
using AlphaPlus.BetsForecast.Infrastructure.TotalBets;
using AlphaPlus.BetsForecast.Infrastructure.TotalBets.Params;

namespace AlphaPlus.Tickets.API.Endpoints;

public class TotalBetsEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var totalBets = endpoints.MapGroup("/api/totalbets");

        totalBets.MapGet(
            "/hdpoulive",
            ([AsParameters] GetTotalBetsParam param, ITotalBetsService service) => service.GetHDPOULive(param));

        totalBets.MapGet(
            "/moneyline",
            ([AsParameters] GetTotalBetsParam param, ITotalBetsService service) => service.GetMoneyLine(param));

        totalBets.MapGet(
            "/oe1x2dnd",
            ([AsParameters] GetTotalBetsParam param, ITotalBetsService service) => service.GetOE1X2DND(param));

        totalBets.MapGet(
            "/totalgoal",
            ([AsParameters] GetTotalBetsParam param, ITotalBetsService service) => service.GetTotalGoal(param));

        totalBets.MapGet(
            "/outright",
            ([AsParameters] GetTotalBetsParam param, ITotalBetsService service) => service.GetOutright(param));

        totalBets.MapGet(
            "/htft",
            ([AsParameters] GetTotalBetsParam param, ITotalBetsService service) => service.GetHalfTimeFullTime(param));

        totalBets.MapGet(
            "/fglg",
            ([AsParameters] GetTotalBetsParam param, ITotalBetsService service) => service.GetFirstGoalLastGoal(param));

        totalBets.MapGet(
            "/homedrawawaynobet",
            ([AsParameters] GetTotalBetsParam param, ITotalBetsService service) => service.GetHomeDrawAwayNoBet(param));

        totalBets.MapGet(
            "/numbergame",
            ([AsParameters] GetTotalBetsParam param, ITotalBetsService service) => service.GetNumberGame(param));
    }
}