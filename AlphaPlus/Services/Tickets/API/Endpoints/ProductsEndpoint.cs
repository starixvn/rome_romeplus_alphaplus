// using Alpha.Tickets.Infrastructure.Repositories;

// namespace AlphaPlus.Tickets.API.Endpoints;

// public class ProductsEndpoint : IEndpoints
// {
//     public void RegisterServices(IServiceCollection services)
//         => services.AddSingleton<IProductRepository, ProductRepository>();

//     public void MapEndpoints(IEndpointRouteBuilder endpoints)
//     {
//         var products = endpoints.MapGroup("/api/products");

//         products.MapGet(
//             "/get",
//             (string site, int currency, byte settingStatus, bool isHidden, IProductRepository repository) => repository.GetProducts(site, currency, settingStatus, isHidden));
//     }
// }
