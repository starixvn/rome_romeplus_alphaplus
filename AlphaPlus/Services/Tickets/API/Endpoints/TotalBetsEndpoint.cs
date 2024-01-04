
using AlphaPlus.BetsForecast.Infrastructure.TotalBets;

namespace AlphaPlus.Tickets.API.Endpoints;

public class TotalBetsEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var totalBets = endpoints.MapGroup("/api/totalbets");

        totalBets.MapGet(
            "/hdpoulive",
            (byte custLevel,
            int custId,
            ITotalBetsRepository repository) => repository.GetHDPOULive(custLevel, custId));

        totalBets.MapGet(
            "/moneyline",
            (byte custLevel,
            int custId,
            string? language,
            ITotalBetsRepository repository) => repository.GetMoneyLine(custLevel, custId, language));

        totalBets.MapGet(
            "/oe1x2dnd",
            (byte custLevel,
            int custId,
            string? language,
            ITotalBetsRepository repository) => repository.GetOE1x2DND(custLevel, custId, language));

        totalBets.MapGet(
            "/totalgoal",
            (byte custLevel,
            int custId,
            string? language,
            ITotalBetsRepository repository) => repository.GetTotalGoal(custLevel, custId, language));

        totalBets.MapGet(
            "/outright",
            (byte custLevel,
            int custId,
            string? language,
            ITotalBetsRepository repository) => repository.GetOutright(custLevel, custId, language));

        totalBets.MapGet(
            "/htft",
            (byte custLevel,
            int custId,
            string? language,
            ITotalBetsRepository repository) => repository.GetHalfTimeFullTime(custLevel, custId, language));

        totalBets.MapGet(
            "/fglg",
            (byte custLevel,
            int custId,
            string? language,
            ITotalBetsRepository repository) => repository.GetFirstGoalLastGoal(custLevel, custId, language));

        totalBets.MapGet(
            "/homedrawawaynobet",
            (byte custLevel,
            int custId,
            string? language,
            ITotalBetsRepository repository) => repository.GetHomeDrawAwayNoBet(custLevel, custId, language));

        totalBets.MapGet(
            "/numbergame",
            (byte custLevel,
            int custId,
            string? language,
            ITotalBetsRepository repository) => repository.GetNumberGame(custLevel, custId, language));
    }
}