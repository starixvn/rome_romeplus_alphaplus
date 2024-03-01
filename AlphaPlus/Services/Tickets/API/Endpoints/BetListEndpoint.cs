// using Alpha.Tickets.Infrastructure.Models.BetList;
// using Alpha.Tickets.Infrastructure.Params.BetList;
// using Alpha.Tickets.Infrastructure.Repositories;

// namespace AlphaPlus.Tickets.API.Endpoints;

// public class BetListEndpoint : IEndpoints
// {
//     public void RegisterServices(IServiceCollection services)
//         => services.AddSingleton<IBetListRepository, BetListRepository>();

//     public void MapEndpoints(IEndpointRouteBuilder endpoints)
//     {
//         var betList = endpoints.MapGroup("/api/betlist");

//         betList.MapGet(
//             "/{custId}/running",
//             ([AsParameters] RunningBetListParam message, IBetListRepository repository) => repository.GetBetListRunningByDate(message, CancellationToken cancellation));

//         betList.MapGet(
//             "/{custId}/ByDate",
//             ([AsParameters] RunningBetListParam message, IBetListRepository repository) => repository.GetBetListAllByDate(message, CancellationToken cancellation));
//     }
// }
