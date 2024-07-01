using AlphaPlus.Core.Utils;
using AlphaPlus.Tickets.Site.Features.BetLists.Models;
using AlphaPlus.Tickets.Site.Features.Shared.Models;

namespace AlphaPlus.Tickets.Site.Features.Shared.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        => services
        .Configure<AppSettings>(configuration)
        .AddSingleton<IUser, User>()
        .AddSingleton<Fanex.Caching.ICacheService, Fanex.Caching.CacheService>()
        .AddSingleton<ICacheRefService, CacheRefService>()
        .AddSingleton<IEncryption, Encryption>();
}
