namespace Alpha.Tickets.API.Endpoints;

public interface IEndpoints
{
    void RegisterServices(IServiceCollection services);

    void RegisterEndpoints(IEndpointRouteBuilder endpoints);
}
