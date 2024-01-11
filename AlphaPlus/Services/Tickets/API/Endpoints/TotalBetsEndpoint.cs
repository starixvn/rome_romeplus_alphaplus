
using AlphaPlus.BetsForecast.Infrastructure.TotalBets;

namespace AlphaPlus.Tickets.API.Endpoints;

public class TotalBetsEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var totalBets = endpoints.MapGroup("/api/totalbets");

        totalBets.MapGet(
            "/hdpoulive",
            (byte custLevel, int custId, string language, ITotalBetsService service)
            => service.GetHDPOULive(custLevel, custId, language));

        totalBets.MapGet(
            "/moneyline",
            (byte custLevel, int custId, string language, ITotalBetsService service)
            => service.GetMoneyLine(custLevel, custId, language));

        totalBets.MapGet(
            "/oe1x2dnd",
            (byte custLevel, int custId, string language, ITotalBetsService service)
            => service.GetOE1X2DND(custLevel, custId, language));

        totalBets.MapGet(
            "/totalgoal",
            (byte custLevel, int custId, string language, ITotalBetsService service)
            => service.GetTotalGoal(custLevel, custId, language));

        totalBets.MapGet(
            "/outright",
            (byte custLevel, int custId, string? language, ITotalBetsService service)
            => service.GetOutright(custLevel, custId, language));

        totalBets.MapGet(
            "/htft",
            (byte custLevel, int custId, string? language, ITotalBetsService service)
            => service.GetHalfTimeFullTime(custLevel, custId, language));

        totalBets.MapGet(
            "/fglg",
            (byte custLevel, int custId, string? language, ITotalBetsService service)
            => service.GetFirstGoalLastGoal(custLevel, custId, language));

        totalBets.MapGet(
            "/homedrawawaynobet",
            (byte custLevel, int custId, string? language, ITotalBetsService service)
            => service.GetHomeDrawAwayNoBet(custLevel, custId, language));

        totalBets.MapGet(
            "/numbergame",
            (byte custLevel, int custId, string? language, ITotalBetsService service)
            => service.GetNumberGame(custLevel, custId, language));
    }
}