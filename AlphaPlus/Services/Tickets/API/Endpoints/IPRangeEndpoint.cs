
// using Alpha.Tickets.Infrastructure.Repositories;

// namespace AlphaPlus.Tickets.API.Endpoints;

// public class IPRangeEndpoint : IEndpoints
// {
//     public void RegisterServices(IServiceCollection services)
//         => services.AddSingleton<IIPRangeRepository, IPRangeRepository>();

//     public void MapEndpoints(IEndpointRouteBuilder endpoints)
//     {
//         var ipRange = endpoints.MapGroup("/api/ip");

//         ipRange.MapGet(
//             "/v4",
//             (string ipList, IIPRangeRepository repository) => repository.GetIPRangeV4(ipList));

//         ipRange.MapGet(
//             "/v4/countries",
//             (string countries, IIPRangeRepository repository) => repository.GetIPRangeV4ByCountries(countries));

//         ipRange.MapGet(
//             "/v6",
//             (string ipList, IIPRangeRepository repository) => repository.GetIPRangeV6(ipList));
//     }
// }
