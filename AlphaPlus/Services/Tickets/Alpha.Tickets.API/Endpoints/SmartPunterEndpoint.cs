using Alpha.Tickets.Infrastructure.Repositories;

namespace Alpha.Tickets.API.Endpoints;

public class SmartPunterEndpoint : IEndpoints
{
    public void RegisterServices(IServiceCollection services)
        => services.AddSingleton<ISmartPunterRepository, SmartPunterRepository>();

    public void RegisterEndpoints(IEndpointRouteBuilder endpoints)
    {
        var smartPunter = endpoints.MapGroup("/api/SmartPunter");

        smartPunter.MapGet(
            "/IsInternalAdmin",
            (int adminId, ISmartPunterRepository repository) => repository.IsInternalAdmin(adminId));

        smartPunter.MapGet(
            "/IsSmartPunter",
            (int? custId, string username, ISmartPunterRepository repository) => repository.IsSmartPunter(custId, username));

        smartPunter.MapGet(
            "/HasPermission",
            (int adminId, ISmartPunterRepository repository) => repository.HasSmartPunterPermission(adminId));

        smartPunter.MapPost(
            "/WriteLogNoData",
            (int? custId, bool isRestricted, int adminId, string ip, ISmartPunterRepository repository)
                => repository.WriteLogNoData(custId, isRestricted, adminId, ip));

        smartPunter.MapPost(
            "/WriteLog",
            (string usernames, bool isRestricted, int userId, string ip, ISmartPunterRepository repository)
                => repository.WriteLog(usernames, isRestricted, userId, ip));

        smartPunter.MapPost(
            "/CheckAndWriteLog",
            (string usernames, bool isRestricted, int userId, string ip, ISmartPunterRepository repository)
                => repository.CheckSmartPunterAndWriteLog(usernames, isRestricted, userId, ip));
    }
}
