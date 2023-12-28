
using Alpha.Tickets.Infrastructure.Repositories;

namespace Alpha.Tickets.API.Endpoints;

public class AutoFollowBetEndpoint : IEndpoints
{
    public void RegisterServices(IServiceCollection services)
        => services.AddSingleton<IAutoFollowBetRepository, AutoFollowBetRepository>();

    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
        => endpoints.MapGet(
            "/api/AutoFollowBet/RealCustId",
            (int agentId, int dummyCustId, IAutoFollowBetRepository repository) => repository.GetRealCustId(agentId, dummyCustId));
}
