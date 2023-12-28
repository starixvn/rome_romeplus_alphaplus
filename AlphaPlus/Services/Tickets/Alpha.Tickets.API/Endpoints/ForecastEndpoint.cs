using Alpha.Tickets.Infrastructure.Repositories;

namespace Alpha.Tickets.API.Endpoints;

public class ForecastEndpoint : IEndpoints
{
    public void RegisterServices(IServiceCollection services)
        => services.AddSingleton<IForecastRepository, ForecastRepository>();

    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
    {
        var products = endpoints.MapGroup("/api/forecast");

        products.MapGet(
            "/BetTypesSupportScoreMap",
            (IForecastRepository repository) => repository.GetBetTypeIdsSupportScoreMap());
    }
}
